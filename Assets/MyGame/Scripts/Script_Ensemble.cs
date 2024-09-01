using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ensemble : MonoBehaviour
{
    [SerializeField] private Material[] randomMaterials;  
    [SerializeField] private Material defaultMaterial; 

    private Renderer[] childRenderers;

    private void Start()
    {
        
        if (randomMaterials.Length != 4)
        {
            Debug.LogError("Please assign exactly 4 unique materials to the Random Materials array in the Inspector.");
            return;
        }

        
        childRenderers = GetComponentsInChildren<Renderer>();

        
        if (childRenderers.Length != 4)
        {
            Debug.LogError("This GameObject should have exactly 4 child objects with Renderers.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AssignRandomMaterials();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetMaterialsToDefault();
        }
    }

    private void AssignRandomMaterials()
    {
        
        Material[] shuffledMaterials = ShuffleMaterials(randomMaterials);

        
        for (int i = 0; i < childRenderers.Length; i++)
        {
            if (childRenderers[i] != null)
            {
                childRenderers[i].material = shuffledMaterials[i];
            }
        }
    }

    private void ResetMaterialsToDefault()
    {
        
        foreach (Renderer renderer in childRenderers)
        {
            if (renderer != null)
            {
                renderer.material = defaultMaterial;
            }
        }
    }

    private Material[] ShuffleMaterials(Material[] materials)
    {
        
        Material[] shuffledMaterials = (Material[])materials.Clone();

        
        for (int i = shuffledMaterials.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Material temp = shuffledMaterials[i];
            shuffledMaterials[i] = shuffledMaterials[randomIndex];
            shuffledMaterials[randomIndex] = temp;
        }

        return shuffledMaterials;
    }
}
