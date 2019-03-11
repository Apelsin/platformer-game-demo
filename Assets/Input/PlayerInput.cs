using UnityEngine;

/// <summary>
/// Input class that reads from the player's input by
/// the static UnityEngine.<see cref="Input"/> class.
/// </summary>
public class PlayerInput : MonoBehaviour, IInput
{
    public float GetAxis(string axis_name)
    {
        // Use static Input class by default
        return Input.GetAxis(axis_name);
    }

    public bool GetButton(string button_name)
    {
        return Input.GetButton(button_name);
    }

    public bool GetButtonDown(string button_name)
    {
        return Input.GetButtonDown(button_name);
    }

    public bool GetButtonUp(string button_name)
    {
        return Input.GetButtonUp(button_name);
    }
}