using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public AudioClip[] modelAudioClips;
    public GameObject guide;
    public GameObject playerCamera;
    private AudioSource guideAudio;     // AudioSource component on the guide
    private Animator guideAnimator;
    private int spawnIndex = 0;
    private bool isTriggerActive = false;
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
            ActivateNextModel();
        }
    }

    private void ActivateNextModel()
    {
        if (currentIndex < objectPrefabs.Length)
        {
            objectPrefabs[currentIndex].SetActive(true);

            
            guideAnimator.SetBool("isWalking", true);

            
            Vector3 targetPosition = objectPrefabs[currentIndex].transform.position + new Vector3(2f, 0, 0);
            StartCoroutine(MoveGuideToPosition(targetPosition));

            
            currentIndex++;
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

        guideAnimator.SetBool("isWalking", false);
        RotateGuideTowardsPlayer();
    }

    private void RotateGuideTowardsPlayer()
    {
        Vector3 directionToPlayer = playerCamera.transform.position - guide.transform.position;
        directionToPlayer.y = 0; 

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        guide.transform.rotation = Quaternion.Slerp(guide.transform.rotation, targetRotation, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggerActive)
        {
            for (int i = 0; i < objectPrefabs.Length; i++)
            {
                if (other.gameObject == objectPrefabs[i])
                {
                    isTriggerActive = true;

                    if (modelAudioClips.Length > i && guideAudio != null)
                    {
                        guideAudio.clip = modelAudioClips[i];
                        guideAudio.Play();

                        StartCoroutine(WaitForAudioToEnd());
                    }
                    break;
                }
            }
        }
    }

    private IEnumerator WaitForAudioToEnd()
    {
        yield return new WaitWhile(() => guideAudio.isPlaying);

        if (currentIndex < objectPrefabs.Length)
        {
            ActivateNextModel();
        }

        isTriggerActive = false;
    }

    void Update()
    {
        
    }
}
