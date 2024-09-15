using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : MonoBehaviour
{
    public float moveDistance = 10f;       
    public float moveSpeed = 3f;       
    public float rotationSpeed = 2f;  

    public ParticleSystem[] particleSystems; 

    private Transform playerTransform;  
    private Animator animator;
    private AudioSource audioSource;
    private bool hasReachedDestination = false; 

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        
        StartCoroutine(MoveAndTurnTowardsPlayer());
    }

    private IEnumerator MoveAndTurnTowardsPlayer()
    {
        Vector3 targetPosition = transform.position + transform.forward * moveDistance; 

        
        animator.SetBool("isWalking", true);

        
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        
        animator.SetBool("isWalking", false);
        hasReachedDestination = true;

        
        StartCoroutine(TurnTowardsPlayer());
    }

    private IEnumerator TurnTowardsPlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        
        while (Quaternion.Angle(transform.rotation, lookRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

       
        animator.SetTrigger("idle");
    }

    public void TriggerVFX()
    {
        animator.SetTrigger("TriggerVFX");

        // Trigger the particle systems simultaneously
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
    }

    void Update()
    {
        if (hasReachedDestination)
        {
            
        }
    }
}
