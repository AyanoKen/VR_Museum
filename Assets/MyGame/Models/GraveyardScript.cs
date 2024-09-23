using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class GraveyardScript : MonoBehaviour
{
    private bool letTombTrigger = true;
    private int currentTextIndex = 0;
    public string[] graveyardTexts;
    public void HandleTombstoneCollision(GameObject tombstone)
    {
        if (letTombTrigger)
        {
            TMP_Text tombstoneText = tombstone.GetComponentInChildren<TMP_Text>();

            if (tombstoneText != null && graveyardTexts.Length > 0 && currentTextIndex < graveyardTexts.Length)
            {
                tombstoneText.text = graveyardTexts[currentTextIndex];
                
                currentTextIndex++;

                letTombTrigger = false;

                // Check if we've reached the end of the array
                if (currentTextIndex >= graveyardTexts.Length)
                {
                    // StartCoroutine(DespawnTombstones());
                } 
                else 
                {
                    StartCoroutine(EnableTombsAfterDelay());
                }
            }
        }
        
    }

    private IEnumerator EnableTombsAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        letTombTrigger = true;
    }
}
