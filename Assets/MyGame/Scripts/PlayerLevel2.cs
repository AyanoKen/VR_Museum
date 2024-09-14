using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel2 : MonoBehaviour
{
    private bool isEventActive = true;
    private int eventNumber = 0;
    public AudioClip[] audioClips;

    public GameObject[] objectPrefabs;
    public GameObject playgroundPrefab;
    public GameObject ruinedPlaygroundPrefab;
    public GameObject guide; 
    public float firstEventDelay = 5f;  

    public float eventDelay = 10f;
    public ParticleSystem[] event2VFX; 

    private GameObject spawnedEvent;
    private GameObject spawnedGuide;
    private AudioSource playerAudioSource;


    private void Start()
    {
        playerAudioSource = GetComponent<AudioSource>(); 
        StartCoroutine(EnableGuideAfterDelay()); // Start the intro scene of the Guide moving around the player, explaining the room
    }

    private void Update() 
    {
        if (!isEventActive){
            TriggerEvent(eventNumber);
        }

        if (spawnedEvent != null)
        {
            CheckDespawnDistance();
        }
        
    }

    private void TriggerEvent(int eventNumber)
    {
        switch (eventNumber){
            case 0:
                TriggerEvent0();
                break;
            case 1:
                TriggerEvent1();
                break;
            case 2:
                TriggerEvent2();
                break;
            case 3:
                TriggerEvent3();
                break;
            default:
                break;
        }
    }

    
    private IEnumerator EnableGuideAfterDelay() //Take 5 seconds to start the whole experience. This is to count in any lag when the level loads
    {
        yield return new WaitForSeconds(firstEventDelay);  
        guide.SetActive(true);  //Guide is always present on the player, just not enabled, enable it here
        StartCoroutine(DisableGuideAfterDelay()); 
        
    }

    private IEnumerator DisableGuideAfterDelay() //Wait for the first voiceover to end, to disable the guide and enable the first event
    {
        yield return new WaitForSeconds(20);   // Change the number here based on the length of the voiceover
        guide.SetActive(false);
        StartCoroutine(EnableEventsAfterDelay()); 
    }

    private IEnumerator EnableEventsAfterDelay() //This is the function which determines whether we start any events or not
    {
        yield return new WaitForSeconds(eventDelay);
        isEventActive = false; //This is true when the game starts, so no event will run during the prologue scene.
        Debug.Log("Activated the events logic");
    }

    //Act 1: The scene with the mother and the child
    private void TriggerEvent0()
    {
        isEventActive = true; //Trigger this to disable any other events from starting
        Vector3 spawnPosition = transform.position - transform.forward * 20f;  //Spawn the event behind the player
        spawnedEvent = Instantiate(objectPrefabs[0], spawnPosition, Quaternion.identity);

        
        AudioSource objectAudioSource = spawnedEvent.GetComponent<AudioSource>(); //Enable the audio on the event
        if (objectAudioSource != null)
        {
            objectAudioSource.Play();
        }

        Debug.Log("Triggered Event 0: Spawned object and played spatial SFX.");
    }

    private void CheckDespawnDistance()  //This is only for Act 1, we despawn the event if the player is too close
    {
        float distance = Vector3.Distance(transform.position, spawnedEvent.transform.position);

        if (distance < 5)
        {
            Destroy(spawnedEvent);
            Debug.Log("Spawned object destroyed because the player got too close.");
            eventNumber++;
            StartCoroutine(EnableEventsAfterDelay()); //Enable events again after incrementing eventNumber :)
        }
    }

    //Act 2: The Playground Scene
    private void TriggerEvent1()
    {
        isEventActive = true;
        Vector3 spawnPosition = transform.position + transform.forward * 5f;  
        spawnedGuide = Instantiate(objectPrefabs[eventNumber], spawnPosition, Quaternion.identity); //Note that we are using a new variable here, if we did not, the guide would despawn when player too close

        
        // AudioSource guideAudio = spawnedGuide.GetComponent<AudioSource>(); //Trigger the voice over of the guide, Mostly will just be a small "Follow me, traveller"
        // if (guideAudio != null)
        // {
        //     guideAudio.Play();  
        // }

        
        StartCoroutine(CheckProximityForVFX());
    }

    private IEnumerator CheckProximityForVFX() //This is where we check if the player is too close to the guide
    {
        bool vfxTriggered = false; //check so that we dont keep triggering the vfx
        while (!vfxTriggered) //Loop to keep checking if the player is close enough or not
        {
            float distanceToGuide = Vector3.Distance(transform.position, spawnedGuide.transform.position);

            if (distanceToGuide < 3f) 
            {
                
                Event1 guideScript = spawnedGuide.GetComponent<Event1>();
                if (guideScript != null)
                {
                    guideScript.TriggerVFX();  //When player too close, we trigger this spell VFX
                }

                
                TriggerPlaygroundTransition(); //During the Animation
                vfxTriggered = true;
            }

            yield return null;
        }
    }

    private void TriggerPlaygroundTransition() //This is how we are changing the scene to the playground scene while the spell from the guide covers the player's view
    {
        
        Vector3 playgroundPosition = transform.position;  
        GameObject playground = Instantiate(playgroundPrefab, playgroundPosition, Quaternion.identity);
        GameObject ruinedPlayground = Instantiate(ruinedPlaygroundPrefab, playgroundPosition, Quaternion.identity);
        ruinedPlayground.SetActive(false);
        playground.SetActive(false);
        
        StartCoroutine(GlitchAndTransition(playground, ruinedPlayground));
    }

    private IEnumerator GlitchAndTransition(GameObject playground, GameObject ruinedPlayground) //This is where we turn on and off the playground scene as if its glitching in the existence 
    {
        yield return new WaitForSeconds(5f);  //Wait before starting the glitch

        playground.SetActive(true);
        Destroy(spawnedGuide);

        yield return new WaitForSeconds(10f);

        
        for (int i = 0; i < 3; i++)  //Glitch the playground scene. 5 seconds breathing time
        {
            playground.SetActive(false);
            ruinedPlayground.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            ruinedPlayground.SetActive(false);
            playground.SetActive(true);
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



        Debug.Log("Transitioned to the ruined playground with cries.");

        yield return new WaitForSeconds(20f);
        Destroy(ruinedPlayground);
        eventNumber++;
        StartCoroutine(EnableEventsAfterDelay()); //Enable events again after incrementing eventNumber :)
    }

    private void TriggerEvent2()
    {
        foreach (ParticleSystem ps in event2VFX)
        {
            ps.Play();
        }
    }

    private void TriggerEvent3()
    {
        return;
    }
}
