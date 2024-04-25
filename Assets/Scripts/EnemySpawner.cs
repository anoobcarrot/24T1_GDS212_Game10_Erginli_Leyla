using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxEnemies = 100;
    public float spawnInterval = 3f;
    public NavMeshSurface[] navMeshSurfaces;

    private int currentEnemies = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (currentEnemies >= maxEnemies)
        {
            return; // Don't spawn if maximum number of enemies reached
        }

        NavMeshSurface navMesh = GetRandomNavMesh();
        if (navMesh == null)
        {
            Debug.LogError("No NavMesh assigned.");
            return;
        }

        Vector3 randomPosition = GetRandomPositionOnNavMesh(navMesh);

        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        currentEnemies++;
    }

    NavMeshSurface GetRandomNavMesh()
    {
        if (navMeshSurfaces.Length == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, navMeshSurfaces.Length);
        return navMeshSurfaces[randomIndex];
    }

    Vector3 GetRandomPositionOnNavMesh(NavMeshSurface navMesh)
    {
        NavMeshData navMeshData = navMesh.navMeshData;
        if (navMeshData == null)
        {
            Debug.LogError("NavMesh data not found.");
            return Vector3.zero;
        }

        NavMeshDataInstance navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        if (triangulation.vertices.Length == 0)
        {
            Debug.LogError("NavMesh triangulation vertices not found.");
            return Vector3.zero;
        }

        int randomIndex = Random.Range(0, triangulation.vertices.Length);
        return triangulation.vertices[randomIndex];
    }

    public void EnemyDestroyed()
    {
        currentEnemies--;
    }
}

