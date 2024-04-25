using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    public int numberOfObjects = 10;
    public float spawnRadius = 10f;

    private void Start()
    {
        if (prefabsToSpawn.Length == 0)
        {
            Debug.LogError("No prefabs assigned to spawn.");
            return;
        }

        Collider terrainCollider = GetComponent<Collider>();
        if (terrainCollider == null)
        {
            Debug.LogError("Terrain collider not found.");
            return;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Generate random point within the collider bounds
            Vector3 randomPoint = GetRandomPointInCollider(terrainCollider);

            // Select a random prefab from the array
            GameObject randomPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

            // Instantiate the selected prefab at the random point
            Instantiate(randomPrefab, randomPoint, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 randomPoint = Vector3.zero;

        for (int attempts = 0; attempts < 10; attempts++)
        {
            // Generate a random point within the collider bounds
            randomPoint = collider.bounds.center + new Vector3(
                Random.Range(-collider.bounds.extents.x, collider.bounds.extents.x),
                0f,
                Random.Range(-collider.bounds.extents.z, collider.bounds.extents.z)
            );

            RaycastHit hit;
            // Raycast downward to find the terrain surface
            if (Physics.Raycast(randomPoint + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
            {
                randomPoint = hit.point;
                break;
            }
        }

        return randomPoint;
    }
}

