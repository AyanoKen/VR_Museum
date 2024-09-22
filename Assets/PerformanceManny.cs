using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceManny : MonoBehaviour
{
    public GameObject Event;
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Event.SetActive(true);
        } 
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Event.SetActive(false);
        } 
    }
}
