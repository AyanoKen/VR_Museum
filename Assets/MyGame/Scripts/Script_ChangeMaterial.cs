using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ChangeMaterial : MonoBehaviour
{
    [SerializeField] private Material enterMaterial;  
    [SerializeField] private Material exitMaterial;

    private Renderer objectRenderer; 

    private void Start()
    {
        
        objectRenderer = GetComponent<Renderer>();

       
        if (objectRenderer == null)
        {
            Debug.LogError("Renderer component is not found on this GameObject!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            
            if (objectRenderer != null && enterMaterial != null)
            {
                objectRenderer.material = enterMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            
            if (objectRenderer != null && exitMaterial != null)
            {
                objectRenderer.material = exitMaterial;
            }
        }
    }
}
