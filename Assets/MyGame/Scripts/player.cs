using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public float prismaTrigger = 10f;
    private float initialPrismaTrigger;
    public float prismaRange = 25f;
    public float spawnDistance = 20f;
    [SerializeField] private float leftOffset = 5f;
    [SerializeField] private float yOffset = 0.447942f;

    private bool artifactSpawned = false; 
    private GameObject spawnedPrisma;

    private int spawnIndex = 0;



    private void Start() {
        initialPrismaTrigger = prismaTrigger;
    }

    void Update()
    {
        if (!artifactSpawned){
            float distance = Vector3.Distance(transform.position, Vector3.zero); 

            if (distance > prismaTrigger && !artifactSpawned){
                SpawnPrisma();
                artifactSpawned = true;
                Debug.Log("Spawned");
            }
        }

        if (artifactSpawned && spawnedPrisma != null)
        {
            float distanceToPrisma = Vector3.Distance(transform.position, spawnedPrisma.transform.position);

            if (distanceToPrisma > prismaRange)
            {
                DespawnPrisma();
                artifactSpawned = false;
                spawnIndex++;
                prismaTrigger += initialPrismaTrigger + spawnDistance + prismaRange;
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

            if (spawnIndex == 0){
                spawnPosition.y += yOffset;
            }

            if (spawnIndex == 1){
                spawnPosition.y += 3f;
                Quaternion correctRotation = Quaternion.Euler(0, -60, 90);
                spawnedPrisma = Instantiate(objectPrefabs[spawnIndex], spawnPosition, correctRotation);
            }else{
                spawnedPrisma = Instantiate(objectPrefabs[spawnIndex], spawnPosition, Quaternion.identity);
            }

            
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
