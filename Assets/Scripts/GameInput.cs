using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PlayerPrefsBindings = "InputBindings";
    public static GameInput Instance { get; private set; }
    public event EventHandler OnUseAction;
    public event EventHandler OnUseAlternativeAction;
    public event EventHandler OnPausedAction;
    public event EventHandler OnBindingRebind;
    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Use,
        UseAlternative,
        Pause
    }
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PlayerPrefsBindings))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PlayerPrefsBindings));
        }
        playerInputActions.Player.Enable();
        playerInputActions.Player.Use.performed += Use_performed;
        playerInputActions.Player.UseAlternative.performed += UseAlternative_performed; 
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Use.performed -= Use_performed;
        playerInputActions.Player.UseAlternative.performed -= UseAlternative_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPausedAction?.Invoke(this, EventArgs.Empty);
    }

    private void UseAlternative_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnUseAlternativeAction?.Invoke(this, EventArgs.Empty);
    }

    private void Use_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnUseAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            case Binding.MoveUp:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveDown:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveLeft:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.MoveRight:
                return playerInputActions.Player.Move.bindings[5].ToDisplayString();
            case Binding.Use:
                return playerInputActions.Player.Use.bindings[0].ToDisplayString();
            case Binding.UseAlternative:
                return playerInputActions.Player.UseAlternative.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            default:
                return "";
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        InputAction inputAction;
        int bindingIndex;
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveDown:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveLeft:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.MoveRight:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 5;
                break;
            case Binding.Use:
                inputAction = playerInputActions.Player.Use;
                bindingIndex = 0;
                break;
            case Binding.UseAlternative:
                inputAction = playerInputActions.Player.UseAlternative;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }
        playerInputActions.Player.Disable();
        playerInputActions.Player.Move.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound();
            PlayerPrefs.SetString(PlayerPrefsBindings, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        });
    }
}
