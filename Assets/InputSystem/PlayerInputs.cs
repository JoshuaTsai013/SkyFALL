using UnityEngine;

using UnityEngine.InputSystem;




public class PlayerInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool Dash;
    public bool aim;
    public bool shoot;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;


    // public void OnMove(InputValue value)
    // {
    // 	MoveInput(value.Get<Vector2>());
    // }

    // public void OnLook(InputValue value)
    // {
    // 	if (cursorInputForLook)
    // 	{
    // 		LookInput(value.Get<Vector2>());
    // 	}
    // }

    // public void OnJump(InputValue value)
    // {
    // 	JumpInput(value.isPressed);
    // }

    // public void OnDash(InputValue value)
    // {
    // 	DashInput(value.isPressed);
    // }
    // public void OnAim(InputValue value)
    // {
    // 	AimInput(value.isPressed);
    // }
    // public void OnShoot(InputValue value)
    // {
    // 	ShootInput(value.isPressed);
    // }


    public void MoveInput(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
        // Debug.Log("Move: " + move);
    }

    public void LookInput(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }

    public void JumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            jump = true;
        }
        else{
            jump = false;
        }

        //print("Jumpppp"+jump);
    }
    public void DashInput(InputAction.CallbackContext ctx)
    {
        //Dash = ctx.ReadValue<bool>();
        if (ctx.performed)
        {
            Dash = true;
        }

        //print("Dashttt"+Dash);
    }
    public void AimInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            aim = true;
        }
        else
        {
            aim = false;
        }
        // Debug.Log("Aim: " + aim);
    }
    public void ShootInput(InputAction.CallbackContext ctx)
    {
        // shoot = ctx.ReadValue<bool>();
        if (ctx.performed)
        {
            shoot = true;
        }
        else
        {
            shoot = false;
        }
        // Debug.Log("Shoot: " + shoot);
    }


    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

