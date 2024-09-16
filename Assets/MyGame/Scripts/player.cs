using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public AudioClip[] modelAudioClips;
    public GameObject guide;
    public GameObject playerCamera;
    private AudioSource guideAudio;     // AudioSource component on the guide
    private Animator guideAnimator;
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
            Debug.Log(objectPrefabs[currentIndex]);
            objectPrefabs[currentIndex].SetActive(true);
            
            if (!objectPrefabs[currentIndex].activeInHierarchy)
            {
                Debug.Log("Trying to set active again");
                objectPrefabs[currentIndex].SetActive(true); 
            } else {
                Debug.Log(objectPrefabs[currentIndex] + "Is already active");
            }

            if (currentIndex == 0)
            {
                
                StartCoroutine(EnableModelScriptAfterNarration());
            }

            if (currentIndex == 4)
            {
                guideAudio.clip = modelAudioClips[i];
                guideAudio.Play();
            }

            
            guideAnimator.SetBool("isWalking", true);

            
            Vector3 targetPosition = objectPrefabs[currentIndex].transform.position + new Vector3(0, 0, -5);
            StartCoroutine(MoveGuideToPosition(targetPosition));

            
            currentIndex++;
        }
    }

    private IEnumerator EnableModelScriptAfterNarration()
    {
        
        yield return new WaitForSeconds(guideAudio.clip.length);

        
        Script_Ensemble ensembleScript = objectPrefabs[0].GetComponent<Script_Ensemble>();
        if (ensembleScript != null)
        {
            ensembleScript.enabled = true;
        }
    }

    private IEnumerator MoveGuideToPosition(Vector3 targetPosition)
    {
        float speed = 2f;
        while (Vector3.Distance(guide.transform.position, targetPosition) > 0.1f)
        {
           
            //RotateGuideTowardsTarget(objectPrefabs[currentIndex].transform.position);

            
            guide.transform.position = Vector3.MoveTowards(guide.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        
        guideAnimator.SetBool("isWalking", false);

        
        RotateGuideTowardsPlayer();
    }

    private void RotateGuideTowardsTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - guide.transform.position;
        directionToTarget.y = 0; // Keep rotation in the horizontal plane

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate the guide towards the model
        guide.transform.rotation = Quaternion.Slerp(guide.transform.rotation, targetRotation, 0.1f);
    }

    private void RotateGuideTowardsPlayer()
    {
        Vector3 directionToPlayer = playerCamera.transform.position - guide.transform.position;
        directionToPlayer.y = 0; 

        
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        
        Vector3 adjustedRotation = targetRotation.eulerAngles;
        adjustedRotation.x += 90f; 

        
        targetRotation = Quaternion.Euler(adjustedRotation);

        
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
                        if (currentIndex != 4){
                            guideAudio.clip = modelAudioClips[i];
                            guideAudio.Play();

                            StartCoroutine(WaitForAudioToEnd());
                        }else {
                            SceneManager.LoadScene("SoundMuseum"); 
                            Debug.Log("Loading Level 2");
                        }
                        
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
