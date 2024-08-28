using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismaScript : MonoBehaviour
{

    [SerializeField] private GameObject[] glowableObjects; 

    public float playerDisplacement = 50f;
    public float glowDuration = 5f;
    public float switchInterval = 7f;
    public Material[] emissionMaterials;

    public Material metalMaterial;

    private List<GameObject> currentGlowingObjects = new List<GameObject>();
    private Transform player; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 

        List<GameObject> glowableList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Cylinder"))
            {
                glowableList.Add(child.gameObject);
            }
        }
        glowableObjects = glowableList.ToArray();

        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, Vector3.zero);

        if (distance >= playerDisplacement){
            StartCoroutine(FadeInModel());
        }
    }

    private IEnumerator FadeInModel()
    {
        float fadeDuration = 5f;
        float elapsedTime = 0f;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        StartCoroutine(ManageGlowEffect());

        // Fade-in effect
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            foreach (Renderer renderer in renderers)
            {
                Color color = renderer.material.color;
                color.a = alpha;
                renderer.material.color = color;
            }

            yield return null;
        }
    }

    private IEnumerator ManageGlowEffect()
    {
        while (true)
        {
            // Clear the list of currently glowing objects
            ResetGlowingObjects();

            // Randomly pick objects to glow
            int numObjectsToGlow = Random.Range(1, glowableObjects.Length + 1);
            for (int i = 0; i < numObjectsToGlow; i++)
            {
                GameObject obj = glowableObjects[Random.Range(0, glowableObjects.Length)];
                currentGlowingObjects.Add(obj);
                
                // Randomly pick an emission material from the array
                Material randomEmissionMaterial = emissionMaterials[Random.Range(0, emissionMaterials.Length)];
                SetMaterial(obj, randomEmissionMaterial);
            }

            // Wait for the glow duration
            yield return new WaitForSeconds(glowDuration);

            // Switch back to metal material
            ResetGlowingObjects();

            // Wait for the switch interval before repeating
            yield return new WaitForSeconds(switchInterval);
        }
    }

    private void SetMaterial(GameObject obj, Material mat)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = mat;
        }
    }

    private void ResetGlowingObjects()
    {
        foreach (GameObject obj in currentGlowingObjects)
        {
            SetMaterial(obj, metalMaterial);
        }

        currentGlowingObjects.Clear();
    }
}
