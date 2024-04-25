using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterOption
{
    public GameObject prefab; // Prefab associated with the character
    public GameObject imagePrefab; // UI Image prefab representing the character
}

public class CharacterSelection : MonoBehaviour
{
    public CharacterOption[] characterOptions; // Array of character options

    public Transform imageParent; // Parent transform to instantiate the UI Image
    private GameObject characterImagePrefab; // Instantiated UI Image prefab representing the character
    private int selectedCharacterIndex = 0; // Index of the currently selected character

    private const string SelectedCharacterKey = "SelectedCharacterIndex";
    [SerializeField] private bool spawnPlayerRequested = false; // Flag to indicate if the player character should be spawned

    private void Start()
    {
        // Load the selected character index from persistent storage
        selectedCharacterIndex = PlayerPrefs.GetInt(SelectedCharacterKey, 0);
        UpdateCharacterImage();
    }

    public void SelectCharacter()
    {
        // Find the index of the selected character image
        for (int i = 0; i < characterOptions.Length; i++)
        {
            if (characterOptions[i].imagePrefab == characterImagePrefab)
            {
                selectedCharacterIndex = i;
                break;
            }
        }

        UpdateCharacterImage();

        // Save the selected character index to persistent storage
        PlayerPrefs.SetInt(SelectedCharacterKey, selectedCharacterIndex);
        PlayerPrefs.Save();
    }

    public void PreviousCharacter()
    {
        selectedCharacterIndex = (selectedCharacterIndex - 1 + characterOptions.Length) % characterOptions.Length;
        UpdateCharacterImage();
    }

    public void NextCharacter()
    {
        selectedCharacterIndex = (selectedCharacterIndex + 1) % characterOptions.Length;
        UpdateCharacterImage();
    }

    public void RequestSpawnPlayer()
    {
        spawnPlayerRequested = true;
    }

    private void Update()
    {
        // Check if the player character should be spawned
        if (spawnPlayerRequested)
        {
            // Spawn the player character and reset the flag
            SpawnPlayerCharacter();
            spawnPlayerRequested = false;
        }
    }

    private void SpawnPlayerCharacter()
    {
        // Get the spawn point in the game scene
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (spawnPoint != null)
        {
            // Spawn the selected character prefab at the spawn point
            if (selectedCharacterIndex >= 0 && selectedCharacterIndex < characterOptions.Length)
            {
                GameObject selectedCharacterPrefab = characterOptions[selectedCharacterIndex].prefab;
                Instantiate(selectedCharacterPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            }
            else
            {
                Debug.LogWarning("Selected character index out of range.");
            }
        }
        else
        {
            Debug.LogWarning("Spawn point not found in the game scene.");
        }
    }

    private void UpdateCharacterImage()
    {
        // Destroy the previous character image prefab if it exists
        if (characterImagePrefab != null)
        {
            Destroy(characterImagePrefab);
        }

        // Instantiate the UI Image prefab for the selected character
        if (selectedCharacterIndex >= 0 && selectedCharacterIndex < characterOptions.Length)
        {
            GameObject imagePrefab = characterOptions[selectedCharacterIndex].imagePrefab;
            characterImagePrefab = Instantiate(imagePrefab, imageParent);
        }
    }
}



