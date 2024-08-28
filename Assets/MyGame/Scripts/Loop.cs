using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{

    public Material activateMaterial;
    public Material deactiveMaterial;

    // Start is called before the first frame update
    void Start()
    {

        // Get the Renderer component attached to the GameObject
        Renderer renderer = GetComponent<Renderer>();

        // Check if the Renderer component exists
        if (renderer == null){
            Debug.LogError("Renderer component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        GetComponent<Renderer>().material = activateMaterial;
    }

    private void OnTriggerExit(Collider other) {
        GetComponent<Renderer>().material = deactiveMaterial;
    }

}
