using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIInput : Singleton<UIInput>
{
      private static GameController customInputMap = null;
    public override void Awake()
    {
        base.Awake();
        if (Instance != this) return;
        customInputMap ??= new GameController();
    }
    
    public static GameController getMap()
    {
        return customInputMap; 
    }

    private void OnEnable()
    {
        if (Instance != null && Instance != this) return;
        // Ensures input is enabled if the object was toggled off/on
        customInputMap?.Enable();
        InputSystem.onActionChange += OnActionTriggered;
    }

    private void OnDisable()
    {
        if (Instance != null && Instance != this) return;
       InputSystem.onActionChange -= OnActionTriggered;
        // Prevents input actions from firing while UIInput is disabled
        customInputMap?.Disable();
    }

    private void OnActionTriggered(object obj, InputActionChange change)
    {
       if (change != InputActionChange.ActionPerformed && change != InputActionChange.ActionStarted)
            return;
         
         if (obj is InputAction action && action.activeControl != null)
        {
            InputDevice lastDevice = action.activeControl.device;
           // Debug.Log(lastDevice.name);

        if (lastDevice is Mouse)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        else 
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
                {
                    // Restore selection to first valid UI item if mouse click cleared it
                    if (EventSystem.current.firstSelectedGameObject != null)
                    {
                        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
                    }
                }
        }
        }
        
        
    }
}
