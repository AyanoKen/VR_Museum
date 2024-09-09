using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ensemble : MonoBehaviour
{
    [SerializeField] private Material[] randomMaterials;  
    [SerializeField] private Material defaultMaterial; 

    private Renderer[] childRenderers;
    private AudioSource audioSource;
    private float[] spectrumData = new float[64];

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        
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

    private void Update()
    {
        
        if (audioSource != null && audioSource.isPlaying)
        {
            
            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

            
            ChangeColorsBasedOnPitch();
        }
    }

    private void ChangeColorsBasedOnPitch()
    {
        
        float averagePitch = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            averagePitch += spectrumData[i];
        }
        averagePitch /= spectrumData.Length;

        
        int materialIndex = 0;
        Debug.Log(averagePitch);
        if (averagePitch > 0.0001f && averagePitch <= 0.001f)
        {
            materialIndex = 0;
        }
        else if (averagePitch > 0.001f && averagePitch <= 0.004f)
        {
            materialIndex = 1;
        }
        else if (averagePitch > 0.004f && averagePitch <= 0.006f)
        {
            materialIndex = 2;
        }
        else if (averagePitch > 0.006f)
        {
            materialIndex = 3;
        }

        
        for (int i = 0; i < childRenderers.Length; i++)
        {
            if (childRenderers[i] != null)
            {
                childRenderers[i].material = randomMaterials[materialIndex];
            }
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         AssignRandomMaterials();
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         ResetMaterialsToDefault();
    //     }
    // }

    // private void AssignRandomMaterials()
    // {
        
    //     Material[] shuffledMaterials = ShuffleMaterials(randomMaterials);

        
    //     for (int i = 0; i < childRenderers.Length; i++)
    //     {
    //         if (childRenderers[i] != null)
    //         {
    //             childRenderers[i].material = shuffledMaterials[i];
    //         }
    //     }
    // }

    // private void ResetMaterialsToDefault()
    // {
        
    //     foreach (Renderer renderer in childRenderers)
    //     {
    //         if (renderer != null)
    //         {
    //             renderer.material = defaultMaterial;
    //         }
    //     }
    // }

    // private Material[] ShuffleMaterials(Material[] materials)
    // {
        
    //     Material[] shuffledMaterials = (Material[])materials.Clone();

        
    //     for (int i = shuffledMaterials.Length - 1; i > 0; i--)
    //     {
    //         int randomIndex = Random.Range(0, i + 1);
    //         Material temp = shuffledMaterials[i];
    //         shuffledMaterials[i] = shuffledMaterials[randomIndex];
    //         shuffledMaterials[randomIndex] = temp;
    //     }

    //     return shuffledMaterials;
    // }
}
