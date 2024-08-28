using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismaScript : MonoBehaviour
{

    [SerializeField] private GameObject[] glowableObjects; 
    public float glowDuration = 5f;
    public float switchInterval = 7f;
    public float spawnDistance = 10f;
    public Material[] emissionMaterials;

    public Material metalMaterial;

    private List<GameObject> currentGlowingObjects = new List<GameObject>();
    private Transform player; 
    private bool isFadingIn = false;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> glowableList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Cylinder"))
            {
                glowableList.Add(child.gameObject);
            }
        }
        glowableObjects = glowableList.ToArray();

        StartCoroutine(FadeInModel());

        StartCoroutine(ManageGlowEffect());

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FadeInModel()
    {
        isFadingIn = true;

        float fadeDuration = 5f;
        float elapsedTime = 0f;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

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

        isFadingIn = false;
    }

    private IEnumerator ManageGlowEffect()
    {
        while (true)
        {
            ResetGlowingObjects();

            int numObjectsToGlow = Random.Range(1, glowableObjects.Length + 1);
            for (int i = 0; i < numObjectsToGlow; i++)
            {
                GameObject obj = glowableObjects[Random.Range(0, glowableObjects.Length)];
                currentGlowingObjects.Add(obj);
                
                Material randomEmissionMaterial = emissionMaterials[Random.Range(0, emissionMaterials.Length)];
                SetMaterial(obj, randomEmissionMaterial);
            }

            yield return new WaitForSeconds(glowDuration);

            ResetGlowingObjects();

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
