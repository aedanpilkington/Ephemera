using System.Collections.Generic;
using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    public GameObject applePrefab;

    public int maxApples = 30;
    public float spawnRadius = 40f;
    public float minDistanceFromPlayer = 8f;

    public Transform player;
    public LayerMask groundLayer;

    private List<GameObject> spawnedApples = new List<GameObject>();

    private void Start()
    {
        SpawnBatch();
    }

    private void Update()
    {
        // if player moves far away, respawn apples
        if (Vector3.Distance(transform.position, player.position) > spawnRadius * 1.5f)
        {
            ClearApples();
            transform.position = player.position;
            SpawnBatch();
        }
    }

    private void SpawnBatch()
    {
        for (int i = 0; i < maxApples; i++)
        {
            Vector3 pos = GetRandomGroundPosition();
            GameObject apple = Instantiate(applePrefab, pos, Quaternion.identity);
            spawnedApples.Add(apple);
        }
    }

    private Vector3 GetRandomGroundPosition()
    {
        Vector3 randomPos =
            player.position +
            new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                20f,
                Random.Range(-spawnRadius, spawnRadius)
            );

        if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 50f, groundLayer))
        {
            return hit.point + Vector3.up * 0.5f;
        }

        return player.position;
    }

    private void ClearApples()
    {
        foreach (GameObject apple in spawnedApples)
        {
            if (apple != null)
                Destroy(apple);
        }

        spawnedApples.Clear();
    }
}
