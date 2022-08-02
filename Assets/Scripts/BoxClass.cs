using UnityEngine;
using System.Collections.Generic;
using Toggle = UnityEngine.UI.Toggle;

// Class to run the game itself
public class BoxClass : MonoBehaviour
{
    // Cardinal directions used for raycasting
    private readonly Vector2[] _cardinalDirections = {Vector2.right, Vector2.down, Vector2.left, Vector2.up};
    
    // Keeping references for self position and the Toggle
    private Vector3 _selfPosition;
    private Toggle _selfToggle;
    
    // Keeping track of the active neighbors (up to 4, for each cardinal direction)
    private List<BoxClass> _neighbors;

    private void Start()
    {
        // Initialize the values
        _selfPosition = gameObject.transform.position;
        _selfToggle = gameObject.GetComponent<Toggle>();
        _neighbors = new List<BoxClass>();
    }

    // The function that is called by the pointer click event trigger of the box
    public void TriggerSelf()
    {
        // If the box is inactive after the click
        if (!_selfToggle.isOn)
        {
            // Remove the box from the neighbors list of each of its active neighbors
            foreach (var neighbor in _neighbors)
            {
                neighbor.RemoveNeighbor(this);
            }
            
            // Clear the self neighbors list and return
            _neighbors.Clear();
            return;
        }
        // If the box is active after the click, for each cardinal direction
        foreach (var direction in _cardinalDirections)
        {
            // Call GetActiveNeighbor for the direction
            var neighbor = GetActiveNeighbor(direction);
            
            // If there is an active neighbor in that direction
            if (neighbor)
            {
                // Add the neighbor to the neighbors list and self to neighbor's neighbors list
                _neighbors.Add(neighbor);
                neighbor.AddNeighbor(this);
            }
        }
        
        // Check if self or any neighboring box has at least two active neighbors, if so, deactivate the neighboring boxes
        if(CheckNeighborCounts())
            DeactivateNeighbors();
    }
    
    // Method to get the active neighbor (if any) towards the given direction
    private BoxClass GetActiveNeighbor(Vector2 direction)
    {
        // Get all the raycast hits starting from the self position and going into the given direction, I used the screen height as the maximum distance it should travel
        var hits = Physics2D.RaycastAll(_selfPosition, direction, Screen.height);
        
        // The hits array has a length of at least 1 since it also hits itself, so if the length is less than 2 that means raycast didn't hit any boxes, meaning the box is on the side of the grid
        // In this case just return null
        if (hits.Length < 2) return null;
        
        // Get the second element of the hit array, which means the one after self, which is the direct neighbor
        var hit = hits[1];
        
        // If the collider exists, and the neighbor is active return the BoxClass component of the neighbor, else return null
        if (hit.collider != null && hit.collider.gameObject.GetComponent<Toggle>().isOn)
            return hit.collider.gameObject.GetComponent<BoxClass>();
        return null;
    }

    // Method to remove a given box from the neighbors list
    // Param: (BoxClass) neighbor to be removed
    public void RemoveNeighbor(BoxClass neighbor)
    {
        if (!_neighbors.Contains(neighbor))
            return;
        _neighbors.Remove(neighbor);
    }

    // Method to add a given box to the neighbors list
    // Param: (BoxClass) neighbor to be added
    public void AddNeighbor(BoxClass neighbor)
    {
        if (_neighbors.Contains(neighbor))
            return;
        _neighbors.Add(neighbor);
    }

    // Method to return the private _neighbors' count
    public int GetNeighborsCount()
    {
        return _neighbors.Count;
    }

    // Method to check if self or any direct neighbors have at least two neighbors in their neighbors list, meaning at least three of the boxes touch each other
    private bool CheckNeighborCounts()
    {
        // If self has more than one neighbor, return true
        if (GetNeighborsCount() > 1) return true;
        
        // Keep track of the neighbors
        var foundThree = false;
        
        // For each neighbor
        foreach (var neighbor in _neighbors)
        {
            // Check if it has more than 1 neighbor, make foundThree true if so.
            if (neighbor.GetNeighborsCount() > 1) foundThree = true;
        }
        
        // Return the result of the search on the neighbors
        return foundThree;
    }

    // Recursive method to deactivate self and neighbors. Used in case CheckNeighborCounts() returns true after a click
    private void DeactivateNeighbors()
    {
        // Turn self off
        _selfToggle.isOn = false;
        
        // For each neighbor
        foreach (var neighbor in _neighbors)
        {
            // If the neighbor is on, continue the recursion on it
            if(neighbor.GetComponent<Toggle>().isOn)
                neighbor.DeactivateNeighbors();
        }
        
        // Clear the neighbors list
        _neighbors.Clear();
    }
}