using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAudio : MonoBehaviour
{
    public GameObject[] monitors;  // Array of monitors (assign in the Inspector)
    public GameObject audioSourceObject;
    public float moveSpeed = 1f;

    private int currentMonitorIndex = 0;

    private void Start()
    {
        if (monitors.Length > 0)
        {
            StartCoroutine(MoveAudioSource());
        }
        else
        {
            Debug.LogError("No monitors assigned!");
        }
    }

    private IEnumerator MoveAudioSource()
    {
        while (true)
        {
            // Get the position of the current monitor
            Vector3 targetPosition = monitors[currentMonitorIndex].transform.position;

            // Move towards the monitor smoothly
            while (Vector3.Distance(audioSourceObject.transform.position, targetPosition) > 0.1f)
            {
                audioSourceObject.transform.position = Vector3.MoveTowards(audioSourceObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;  // Wait until the next frame
            }

            // Once the audio source reaches the monitor, move to the next one
            currentMonitorIndex = (currentMonitorIndex + 1) % monitors.Length;  // Loop back to the first monitor when done

            // Wait for a short delay (e.g., let the audio source stay on a monitor for a moment)
            yield return new WaitForSeconds(1f);  // Adjust the wait time as needed
        }
    }
}
