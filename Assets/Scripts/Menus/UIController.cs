using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected GameController map = null;
    private Vector2 moveInput;
    public float deadZone;
    public float moveRepeatDelay = 0.4f;
    public float moveRepeatRate = 0.1f;
    private Coroutine navRoutine; 

    protected GameObject lastValidSelected;

    protected virtual void OnEnable()
    {
       StartCoroutine(SetMap());
    }

    protected void BindObject(GameObject gameObject)
    {
        lastValidSelected = gameObject;
    }

    private IEnumerator SetMap()
    {
        yield return new WaitForSeconds(0.01f);
        map = UIInput.getMap();

        if (map != null)
        {
            map.Enable(); 
            EnableActions();
        }
    }

    protected virtual void OnDisable()
    {
        if (map != null)
        {
          DisableActions();
          map.Disable();    
        }
        
    }

    protected virtual void EnableActions()
    {
      map.UIController.Movement.started += Movement;  
      map.UIController.Movement.canceled += CancelMovemnt;
    }

    protected virtual void DisableActions()
    {
      map.UIController.Movement.started -= Movement;
      map.UIController.Movement.canceled -= CancelMovemnt;
    }

    private void Movement(InputAction.CallbackContext value)
    {
         moveInput = value.ReadValue<Vector2>();

         bool hasInput = moveInput.magnitude >= deadZone;

         if (hasInput && navRoutine == null)
         {
            navRoutine = StartCoroutine(NavigationRoutine());
         }

         else if(!hasInput &&  navRoutine != null)
         {
            StopCoroutine(navRoutine);
            navRoutine = null;
         }
    }

    private void CancelMovemnt(InputAction.CallbackContext value)
    {
        moveInput = Vector2.zero;
    }
     private MoveDirection getDirectionInput()
    {
    
        if (moveInput.magnitude < deadZone) return MoveDirection.None;
       

            float absX = Mathf.Abs(moveInput.x);
            float absY = Mathf.Abs(moveInput.y);

            if (absX > absY * 1.5f)
                return moveInput.x > 0 ? MoveDirection.Right : MoveDirection.Left;
            else if(absY > absX * 1.5f)
                return moveInput.y > 0 ? MoveDirection.Up : MoveDirection.Down;

            return MoveDirection.None;
    }

    private void ConfirmSelection()
    {
         if (EventSystem.current.currentSelectedGameObject != null)
            return;

        if (lastValidSelected != null && lastValidSelected.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(lastValidSelected);
            return;
        }

        if (EventSystem.current.firstSelectedGameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(
                EventSystem.current.firstSelectedGameObject);
            return;
        }

        Selectable anySelectable = FindAnyObjectByType<Selectable>();
        if (anySelectable != null)
        {
            EventSystem.current.SetSelectedGameObject(anySelectable.gameObject);
        }
                       
    }

    private void Update()
{
   if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject != null)
    {
        lastValidSelected = EventSystem.current.currentSelectedGameObject;
    }
    // 2. Only if the mouse cleared selection, restore the cached objec
}

    private IEnumerator NavigationRoutine()
    {
        
        ConfirmSelection();

        MoveDirection dir = getDirectionInput();
        //Debug.Log(dir);

        if (dir == MoveDirection.None)
        {
             navRoutine = null;
             yield break;
        }
 
        Execute(dir);
        
        yield return new WaitForSecondsRealtime(moveRepeatDelay);

        while (moveInput.magnitude >= deadZone)
        {
            dir = getDirectionInput();
            if (dir != MoveDirection.None)
            {
                // Ensure target hasn't been cleared mid-hold
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    ConfirmSelection();
                    Execute(dir);
                }
                
            }
            yield return new WaitForSecondsRealtime(moveRepeatRate);

           
            
        }

       navRoutine = null;
    }

    void Execute(MoveDirection dir)
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if(current == null) return;

        AxisEventData data = new AxisEventData(EventSystem.current)
        {
          moveDir = dir  
        };

        ExecuteEvents.Execute(current, data, ExecuteEvents.moveHandler);
      //  Debug.Log(EventSystem.current.currentSelectedGameObject.name);
    }

}