using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundScript : MonoBehaviour
{
    public GameObject ruinedPlaygroundPrefab;
    public GameObject playground;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 playgroundPosition = playground.transform.position;
            GameObject ruinedPlayground = Instantiate(ruinedPlaygroundPrefab, playgroundPosition, Quaternion.identity);
            ruinedPlayground.SetActive(false);

            StartCoroutine(GlitchAndTransition(playground, ruinedPlayground));
        }
    }

    private IEnumerator GlitchAndTransition(GameObject playground, GameObject ruinedPlayground) //This is where we turn on and off the playground scene as if its glitching in the existence 
    {
        
        for (int i = 0; i < 3; i++)  //Glitch the playground scene. 5 seconds breathing time
        {
            // playground.SetActive(false);
            ruinedPlayground.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            ruinedPlayground.SetActive(false);
            //playground.SetActive(true);
            yield return new WaitForSeconds(5f);
        }

        
        Destroy(playground);

        
        ruinedPlayground.SetActive(true);
        
        foreach (Transform child in ruinedPlayground.transform)
        {
            AudioSource audioSource = child.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();  // Play the AudioSource
            }

        }
    }
}
