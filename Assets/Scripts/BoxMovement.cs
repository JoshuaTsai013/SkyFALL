using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
   public Transform startPosition;
    public Transform endPosition;
    public float duration = 5f;




    private float elapsedTime;
    private bool movingToEnd = true;

    void Start()
    {
        transform.position = startPosition.position;
        elapsedTime = 0f;
    }

    void Update()
    {
        // Increase the elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the parameter t
        float t = elapsedTime / duration;

        // Interpolate the position
        transform.position = Vector3.Lerp(startPosition.position, endPosition.position, t);

        // Check if we have reached the end
        if (t >= 1f)
        {
            // Swap start and end positions to reverse direction
            Vector3 temp = startPosition.position;
            startPosition.position = endPosition.position;
            endPosition.position = temp;

            // Reset elapsed time
            elapsedTime = 0f;
        }
    }
}

    

