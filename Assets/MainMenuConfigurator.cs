using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets up the main menu when the scene begins
/// </summary>
public class MainMenuConfigurator : MonoBehaviour
{
    private void Start()
    {
        // Find the main menu object
        var main_menu_object = GameObject.Find("Main Menu");

        // Find the start button object
        var start_object = main_menu_object.transform.Find("Start");

        // Get its button component
        var start_button = start_object.GetComponent<Button>();

        // Add the event handler
        start_button.onClick.AddListener(HandleStartButtonClicked);
    }

    private void HandleStartButtonClicked()
    {
        // Find the main controller
        var main_controller = FindObjectOfType<MainController>();

        // Change the main controller to the play game state
        main_controller.CurrentState = MainController.EState.PlayGame;
    }
}