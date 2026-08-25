
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputBinding : MonoBehaviour
{
    /*
    
    class made to enable binding for all player controls only 
    Manages Player input and enables and disables based on certain envents oor if not needed.    
    
    */
    protected GameController map = null;
    // makes new inputmap for gamecontroller
    protected virtual void Awake() => map = new GameController();

    // enables the map and evokes the methods
    protected virtual void OnEnable()
    {
        map.Enable();
        EnableActions();
        EventBus.Subscribe<GameStateEvent>(RetrieveData);
    }

    protected virtual void OnDisable()
    {
        DisableActions();
        map.Disable();
        EventBus.Unsubscribe<GameStateEvent>(RetrieveData);
    }

    private void RetrieveData(GameStateEvent state)
    {
        if (state.gameState == GameState.Ongoing || state.gameState == GameState.StartWave)
        {
            setControls(true);
        }

        else
        {
            setControls(false);
        }
    }

    protected virtual void Start() => setControls(false);
    // disables the map and the methods
    protected void setControls(bool state)
    {
        if (state && !map.PlayerController.enabled)
        {
            map.Enable();
        }

        else if (!state)
        {
            map.Disable();
        }

        Debug.Log(state);
    }

    // enables all playercontroller input actions once scene starts
    private void EnableActions()
    {
        map.PlayerController.Movement.performed += MoveSatelite;
        map.PlayerController.Movement.canceled += CancelMovment;
        map.PlayerController.Shoot.performed += ShootBullet;
        map.PlayerController.Pause.performed += PauseGame;
        map.PlayerController.ZoomIn.performed += ZoomCamera;
        map.PlayerController.ZoomOut.performed += ZoomOutCamera;
    }

    // disables all playercontroller input actions once scene ends 
    private void DisableActions()
    {
        map.PlayerController.Movement.performed -= MoveSatelite;
        map.PlayerController.Movement.canceled -= CancelMovment;
        map.PlayerController.Shoot.performed -= ShootBullet;
        map.PlayerController.Pause.performed -= PauseGame;
        map.PlayerController.ZoomIn.performed -= ZoomCamera;
        map.PlayerController.ZoomOut.performed -= ZoomOutCamera;
    }

    protected virtual void ZoomOutCamera(InputAction.CallbackContext value) { }

    protected virtual void ZoomCamera(InputAction.CallbackContext value) { }
    // virtual methods used to evoke the gameplay input, will be used by child class
    protected virtual void MoveSatelite(InputAction.CallbackContext value) { }

    protected virtual void CancelMovment(InputAction.CallbackContext value) { }

    protected virtual void ShootBullet(InputAction.CallbackContext value) { }

    protected virtual void PauseGame(InputAction.CallbackContext value) { }
}
