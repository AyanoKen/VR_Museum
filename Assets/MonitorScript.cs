using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorScript : MonoBehaviour
{
    public GameObject Monitors;
    public ParticleSystem[] MonitorVFX;
    public GameObject selfReference;
    
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {

            GetComponent<Animator>().SetTrigger("TriggerVFX");

            foreach (ParticleSystem ps in MonitorVFX)
            {
                ps.Play();
            }

            StartCoroutine(ActivateMonitors());
        }
    }

    private IEnumerator ActivateMonitors()
    {
        yield return new WaitForSeconds(4f);

        Monitors.SetActive(true); 

        yield return new WaitForSeconds(4f);
        
        Destroy(selfReference);
    }
}
