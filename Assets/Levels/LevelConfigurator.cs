using UnityEngine;

/// <summary>
/// Sets up a game level when the scene begins
/// </summary>
public class LevelConfigurator : MonoBehaviour
{
    private Vector3 _ProtagonistStartPosition;

    /// <summary>
    /// Finds the protagonist object
    /// </summary>
    /// <param name="game_object">Output protagonist <see cref="GameObject"/></param>
    /// <param name="motion">Output <see cref="ProtagonistMotion"/></param>
    public static void FindProtagonist(out GameObject game_object, out ProtagonistMotion motion)
    {
        game_object = GameObject.FindGameObjectWithTag("Protagonist");
        motion = game_object.GetComponent<ProtagonistMotion>();
    }

    private void Start()
    {
        // Find the player input object
        var input = FindObjectOfType<PlayerInput>();

        // Find the protagonist
        FindProtagonist(
            out GameObject protagonist_object,
            out ProtagonistMotion protagonist);

        // Assign the protagonist input
        protagonist.Input = input;

        // Record the initial position of the protagonist
        _ProtagonistStartPosition = protagonist.transform.position;

        // Set up input button event handler
        var input_events = input.GetComponent<InputEvents>();
        input_events.ButtonDown += HandlePlayerButtonDownEvent;
    }

    private void HandlePlayerButtonDownEvent(string name)
    {
        // Reset when the Cancel button is pressed
        if (name == "Cancel")
            OnPlayerReset();
    }

    private void OnPlayerReset()
    {
        // Reset the position
        var protagonist = FindObjectOfType<ProtagonistMotion>();
        protagonist.transform.position = _ProtagonistStartPosition;
    }
}