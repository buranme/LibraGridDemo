using UnityEngine;
using UnityEngine.UI;

// Class to handle the main menu
public class MenuHandler : MonoBehaviour
{
    // Keeping instances for the grid that is already inside the canvas but is inactive, the input text field, and the text left to it
    public GridClass grid;
    public Text inputText;
    public Text helperText;

    // Method which initializes the grid, called by the "Start" button
    public void InitializeGrid()
    {
        // Turn the input text into an integer
        var inputInt = int.Parse(inputText.text);
        
        // Check if the integer is valid
        if (inputInt < 2)
        {
            // If not, change the text to the left to help the user out
            helperText.text = "Size should be bigger than 1";
            return;
        }
        
        helperText.text = "Please enter the size:";
        
        // Get all the boxClasses that the grid has, if any
        var children = grid.GetComponentsInChildren<BoxClass>();
        
        // Destroy every Box
        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }
        
        // Change the view to the game view and initialize the grid with the given size
        gameObject.SetActive(false);
        grid.gameObject.SetActive(true);
        grid.Initialize(inputInt);
    }
}
