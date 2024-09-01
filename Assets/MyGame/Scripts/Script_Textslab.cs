using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_Textslab : MonoBehaviour
{
    [SerializeField] private string displayText = "Enter your text here";

    private TextMeshPro textObject;
    

    private void Start()
    {

        textObject = GetComponentInChildren<TextMeshPro>();

        if (textObject != null)
        {
            textObject.text = "";
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component is not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (textObject != null)
            {
                textObject.text = displayText;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (textObject != null)
            {
                textObject.text = ""; 
            }
        }
    }
}
