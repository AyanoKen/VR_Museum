using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public float prismaTrigger = 50f;
    public float prismaRange = 100f;

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
            Vector3 spawnPosition = transform.position + transform.forward * prismaTrigger;

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
