using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region InputManager
    public static PlayerInput PlayerInput;

    public static bool StatusWasPressed;

    public static Vector2 Movement;
    public static bool JumpWasPressed;
    public static bool JumpIsHeld;
    public static bool JumpWasReleased;
    public static bool RunIsHeld;
    public static bool DashWasPressed;
    public static bool AttackWasPressed;
    public static bool AttackIsHolding;
    public static bool AttackWasReleased;
    public static bool Skill1WasPressed;
    public static bool Skill2WasPressed;

    public static bool CheatWasPressed;


    private InputAction _statusAction;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;
    private InputAction _dashAction;
    private InputAction _attackAction;
    private InputAction _skill1Action;
    private InputAction _skill2Action;

    private InputAction _cheatAction;

    public enum Device { PC, Mobile };
    public Device device;

    public FixedJoystick joystick;



    #endregion

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android || Application.isEditor)
        {
            device = Device.Mobile;
        }
        else
        {
            device = Device.PC;
        }

        joystick = FindObjectOfType<FixedJoystick>();

        PlayerInput = GetComponent<PlayerInput>();

        _statusAction = PlayerInput.actions["Status"];

        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
        _runAction = PlayerInput.actions["Run"];
        _dashAction = PlayerInput.actions["Dash"];
        _attackAction = PlayerInput.actions["Attack"];
        _skill1Action = PlayerInput.actions["Skill1"];
        _skill2Action = PlayerInput.actions["Skill2"];

        _cheatAction = PlayerInput.actions["Cheat"];


    }

    private void Update()
    {
        if (device == Device.PC)
        {
            Movement = _moveAction.ReadValue<Vector2>();
        }
        else if (device == Device.Mobile)
        {
            Movement = joystick.Direction;
        }

        StatusWasPressed = _statusAction.WasPressedThisFrame();

        JumpWasPressed = _jumpAction.WasPressedThisFrame();
        JumpIsHeld = _jumpAction.IsPressed();
        JumpWasReleased = _jumpAction.WasReleasedThisFrame();

        RunIsHeld = _runAction.IsPressed();

        DashWasPressed = _dashAction.WasPressedThisFrame();

        AttackWasPressed = _attackAction.WasPressedThisFrame();
        AttackIsHolding = _attackAction.IsPressed();
        AttackWasReleased = _attackAction.WasReleasedThisFrame();

        Skill1WasPressed = _skill1Action.WasPressedThisFrame();
        Skill2WasPressed = _skill2Action.WasPressedThisFrame();

        CheatWasPressed = _cheatAction.WasPressedThisFrame();

    }
}
