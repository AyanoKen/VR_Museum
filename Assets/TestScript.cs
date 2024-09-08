using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private float targetPositionX;
    void Start()
    {
        targetPositionX = transform.position.x + 1000000;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPositionX, transform.position.y, transform.position.z), 2f * Time.deltaTime);
    }
}
