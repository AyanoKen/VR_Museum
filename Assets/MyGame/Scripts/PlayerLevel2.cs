using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel2 : MonoBehaviour
{
    private bool isEventActive = true;
    private int eventNumber = 0;
    public AudioClip[] audioClips;

    public GameObject[] objectPrefabs;
    public GameObject guide; 
    public float firstEventDelay = 5f;  

    public float eventDelay = 10f;

    private GameObject spawnedEvent;
    private GameObject spawnedGuide;
    private AudioSource playerAudioSource;


    private void Start()
    {
        playerAudioSource = GetComponent<AudioSource>(); 
        StartCoroutine(EnableGuideAfterDelay());
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

    
    private IEnumerator EnableGuideAfterDelay()
    {
        yield return new WaitForSeconds(firstEventDelay);  
        guide.SetActive(true);  
        StartCoroutine(DisableGuideAfterDelay());
        
    }

    private IEnumerator DisableGuideAfterDelay()
    {
        yield return new WaitForSeconds(20);  
        guide.SetActive(false);
        StartCoroutine(EnableEventsAfterDelay());
    }

    private IEnumerator EnableEventsAfterDelay()
    {
        yield return new WaitForSeconds(eventDelay);
        isEventActive = false;
        Debug.Log("Activated the events logic");
    }

    private void TriggerEvent0()
    {
        isEventActive = true;
        Vector3 spawnPosition = transform.position - transform.forward * 20f;  
        spawnedEvent = Instantiate(objectPrefabs[0], spawnPosition, Quaternion.identity);

        
        AudioSource objectAudioSource = spawnedEvent.GetComponent<AudioSource>();
        if (objectAudioSource != null)
        {
            objectAudioSource.Play();
        }

        Debug.Log("Triggered Event 0: Spawned object and played spatial SFX.");
    }

    private void CheckDespawnDistance()
    {
        float distance = Vector3.Distance(transform.position, spawnedEvent.transform.position);

        if (distance < 5)
        {
            Destroy(spawnedEvent);
            Debug.Log("Spawned object destroyed because the player got too close.");
            eventNumber++;
            StartCoroutine(EnableEventsAfterDelay());
        }
    }

    private void TriggerEvent1()
    {
        isEventActive = true;
        Vector3 spawnPosition = transform.position + transform.forward * 5f;  
        spawnedGuide = Instantiate(objectPrefabs[eventNumber], spawnPosition, Quaternion.identity);

        
        AudioSource guideAudio = spawnedGuide.GetComponent<AudioSource>();
        if (guideAudio != null)
        {
            guideAudio.Play();  
        }

        
        StartCoroutine(CheckProximityForVFX());
    }

    private IEnumerator CheckProximityForVFX()
    {
        bool vfxTriggered = false;
        while (!vfxTriggered)
        {
            float distanceToGuide = Vector3.Distance(transform.position, spawnedGuide.transform.position);

            if (distanceToGuide < 3f) 
            {
                
                Animator guideAnimator = spawnedGuide.GetComponent<Animator>();
                if (guideAnimator != null)
                {
                    guideAnimator.SetTrigger("TriggerVFX");  
                }

                
                TriggerPlaygroundTransition();
                vfxTriggered = true;
            }

            yield return null;
        }
    }

    private void TriggerPlaygroundTransition()
    {
        
        Vector3 playgroundPosition = transform.position;  
        GameObject playground = Instantiate(objectPrefabs[1], playgroundPosition, Quaternion.identity);

        
        playerAudioSource.clip = audioClips[0];  
        playerAudioSource.Play();

        
        StartCoroutine(GlitchAndTransition(playground));
    }

    private IEnumerator GlitchAndTransition(GameObject playground)
    {
        yield return new WaitForSeconds(5f);  

        
        for (int i = 0; i < 3; i++)
        {
            playground.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            playground.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        
        Destroy(playground);

        
        GameObject ruinedPlayground = Instantiate(objectPrefabs[2], transform.position, Quaternion.identity);

        
        playerAudioSource.clip = audioClips[1];
        playerAudioSource.Play();

        Debug.Log("Transitioned to the ruined playground with cries.");
    }

    private void TriggerEvent2()
    {
        return;
    }

    private void TriggerEvent3()
    {
        return;
    }
}
