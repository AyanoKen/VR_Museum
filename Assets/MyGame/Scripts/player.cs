using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public AudioClip[] modelAudioClips;
    public GameObject guide;
    private AudioSource guideAudio;     // AudioSource component on the guide
    private Animator guideAnimator;
    private int spawnIndex = 0;

    private int currentIndex = 0;



    private void Start() {
        guideAudio = guide.GetComponent<AudioSource>();
        guideAnimator = guide.GetComponent<Animator>();

        
        guideAudio.Play();

        
        StartCoroutine(HandleGuideAudioAndMovement());
    }

    private IEnumerator HandleGuideAudioAndMovement()
    {
        
        yield return new WaitWhile(() => guideAudio.isPlaying);

        
        if (objectPrefabs.Length > 0)
        {
            objectPrefabs[currentIndex].SetActive(true);

            guideAnimator.SetBool("isWalking", true);

            Vector3 targetPosition = objectPrefabs[currentIndex].transform.position + new Vector3(2f, 0, 0); // Adjust the offset as needed
            StartCoroutine(MoveGuideToPosition(targetPosition));

            
            yield return new WaitForSeconds(3);
            guideAnimator.SetBool("isWalking", false);
        }
    }

    private IEnumerator MoveGuideToPosition(Vector3 targetPosition)
    {
        float speed = 2f;
        while (Vector3.Distance(guide.transform.position, targetPosition) > 0.1f)
        {
            guide.transform.position = Vector3.MoveTowards(guide.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (other.gameObject == objectPrefabs[i])
            {
                guideAudio.clip = modelAudioClips[currentIndex];
                guideAudio.Play();

                Debug.Log("Playing audio for model: " + objectPrefabs[i].name);

                currentIndex++;
            }
        }
    }

    void Update()
    {
        
    }
}
