using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CharacterSelection characterSelection;

    private void Start()
    {
        // Find the GameObject with the CharacterSelection script attached
        GameObject characterSelectionObject = GameObject.Find("CHARACTER SELECTION");

        // Check if the GameObject was found
        if (characterSelectionObject != null)
        {
            // Get the CharacterSelection component
            characterSelection = characterSelectionObject.GetComponent<CharacterSelection>();
            if (characterSelection != null)
            {
                // Request the spawn of the player character
                characterSelection.RequestSpawnPlayer();
            }
            else
            {
                Debug.LogWarning("CharacterSelection component not found.");
            }
        }
        else
        {
            Debug.LogWarning("CharacterSelectionObject not found.");
        }
    }
}
