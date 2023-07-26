using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerControls;

public class PlayerInput : MonoBehaviour, IPlayerActions, IUIActions
{
    // Player Inputs
    public event UnityAction<Vector2> MovementEvent = delegate { };
    public event UnityAction ChangeEvent = delegate { };
    public event UnityAction PlaceEvent = delegate { };
    public event UnityAction RemoveEvent = delegate { };
    public event UnityAction ZoomInEvent = delegate { };
    public event UnityAction ZoomOutEvent = delegate { };
    public event UnityAction OpenMenuEvent = delegate { };

    // UI Inputs
    public event UnityAction SubmitEvent = delegate {  };
    public event UnityAction ClickEvent = delegate {  };
    public event UnityAction CancelEvent = delegate {  };
    
    private PlayerControls gameInput;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new PlayerControls();
            gameInput.Player.SetCallbacks(this);
            gameInput.UI.SetCallbacks(this);
        }

        EnableGameplayInput();
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void DisableAllInput()
    {
        gameInput.Player.Disable();
        gameInput.UI.Disable();
    }

    public void EnableGameplayInput()
    {
        if (gameInput.Player.enabled) return;

        gameInput.UI.Disable();

        gameInput.Player.Enable();
    }
    
    public void EnableUIInput()
    {
        if (gameInput.UI.enabled) return;

        gameInput.UI.Enable();

        gameInput.Player.Disable();
    }

    #region Player Events
    
    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnChange(InputAction.CallbackContext context)
    {
        if (context.performed)
            ChangeEvent?.Invoke();
    }

    public void OnPlace(InputAction.CallbackContext context)
    {
        if (context.performed)
            PlaceEvent?.Invoke();
    }

    public void OnRemove(InputAction.CallbackContext context)
    {
        if (context.performed)
            RemoveEvent?.Invoke();
    }

    public void OnZoomIn(InputAction.CallbackContext context)
    {
        if (context.performed)
            ZoomInEvent?.Invoke();
    }

    public void OnZoomOut(InputAction.CallbackContext context)
    {
        if (context.performed)
            ZoomOutEvent?.Invoke();
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
            OpenMenuEvent?.Invoke();
    }
    
    #endregion

    #region UI Events

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if(context.performed)
            SubmitEvent?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if(context.performed)
            CancelEvent?.Invoke();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if(context.performed)
            ClickEvent?.Invoke();
    }
    
    #endregion
}
