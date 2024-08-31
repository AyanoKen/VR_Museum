using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public float prismaTrigger = 10f;
    public float prismaRange = 100f;
    public float spawnDistance = 40f;
    [SerializeField] private float leftOffset = 5f;
    [SerializeField] private float yOffset = 0.447942f;

    private bool prismaSpawned = false; 
    private GameObject spawnedPrisma;

    void Update()
    {
        if (!prismaSpawned){
            float distance = Vector3.Distance(transform.position, Vector3.zero); 

            if (distance > prismaTrigger && !prismaSpawned){
                SpawnPrisma();
                prismaSpawned = true; 
            }
        }

        if (prismaSpawned && spawnedPrisma != null)
        {
            float distanceToPrisma = Vector3.Distance(transform.position, spawnedPrisma.transform.position);

            if (distanceToPrisma > prismaRange)
            {
                DespawnPrisma();
            }
        }
        
    }

    private void SpawnPrisma()
    {
        if (objectPrefabs.Length > 0)
        {
            Vector3 forwardSpawnPosition = transform.position + transform.forward * spawnDistance;

            Vector3 leftDirection = -transform.right; 

            Vector3 leftOffsetPosition = leftDirection * leftOffset;

            Vector3 spawnPosition = forwardSpawnPosition + leftOffsetPosition;

            spawnPosition.y += yOffset;

            spawnedPrisma = Instantiate(objectPrefabs[0], spawnPosition, Quaternion.identity);
        }
    }

    private void DespawnPrisma()
    {
        if (spawnedPrisma != null)
        {
            Destroy(spawnedPrisma); // Destroy the Prisma object
            spawnedPrisma = null; // Clear the reference
        }
    }
}
