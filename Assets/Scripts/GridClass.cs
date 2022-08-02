using UnityEngine;

// Class to deal with the initialization of the grid
public class GridClass : MonoBehaviour
{
    // Box count that is given by the player
    public int boxCount;
    
    // Size of the grid, default is 900px
    public float gridSize;
    
    // References for the main menu, continue button, box preset, and self transform 
    public GameObject menu;
    public GameObject continueButton;
    public GameObject boxReference;
    private Transform _selfTransformReference;

    // Function to instantiate the boxes.
    // Param: (int) given n for nxn grid
    public void Initialize(int givenCount)
    {
        // Save the value
        boxCount = givenCount;
        
        // Calculate how much each box should be pushed considering the screen and grid sizes
        var pushRight = (Screen.width - gridSize) / 2;
        var pushDown = (Screen.height - gridSize) / 2;
        
        // Scale of each box
        var scale = gridSize / boxCount;
        
        // Saving a reference for the transform of self
        _selfTransformReference = gameObject.transform;
        
        // Double for loop for nxn
        for (var j = 0; j < boxCount; j++)
        {
            for (var i = 0; i < boxCount; i++)
            {
                // Instantiate a box from boxReference in the correct place calculated with push values calculated before, i-j, and scale
                var box = Instantiate(boxReference, new Vector3(pushRight + i * scale, pushDown + j * scale, -0.5f), Quaternion.identity);
                
                // Scale the box
                box.transform.localScale = new Vector3(scale, scale, 0);
                
                // Name the box (I used the names for debugging, not necessary for the game, thought it is okay to keep it)
                box.name = "(" + i + ", " + j + ")";
                
                // Set the box as the grid's child
                box.transform.SetParent(_selfTransformReference);
            }
        }
    }

    // Method used by the little button on the top left corner to go back to menu
    public void GoBack()
    {
        // Switch to the menu
        gameObject.SetActive(false);
        menu.SetActive(true);
        
        // Additionally make the continue button active, this button can be used to go back to the game without changing anything
        continueButton.SetActive(true);
    }
}
