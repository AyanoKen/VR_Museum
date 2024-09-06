using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel2 : MonoBehaviour
{
    private bool isEventActive = true;
    private int eventNumber = 0;

    public GameObject[] objectPrefabs;
    public GameObject guide; 
    public float firstEventDelay = 5f;  

    public float eventDelay = 10f;

    private GameObject spawnedEvent;


    private void Start()
    {
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
            StartCoroutine(EnableEventsAfterDelay());
        }
    }

    private void TriggerEvent1()
    {
        return;
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
