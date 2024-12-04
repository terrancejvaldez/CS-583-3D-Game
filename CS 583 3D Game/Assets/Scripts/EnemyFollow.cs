using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 0.5f;      // Movement speed
    public float triggerRadius = 0.5f; // Radius around the ball to trigger the timer
    public float loseTime = 3f;   // Time the enemy needs to stay near the ball to trigger a loss
    private float timer = 0f;      // Timer to track time spent near the ball

    private GameObject targetBall; // Reference to the ball

    void Update()
    {

        targetBall = GameObject.FindGameObjectWithTag("Basketball");
        // Follow the ball
        transform.position = Vector3.MoveTowards(transform.position, targetBall.transform.position, speed * Time.deltaTime);

        // Check if the enemy is near the ball
        float distanceToBall = Vector3.Distance(transform.position, targetBall.transform.position);

        if (distanceToBall <= triggerRadius)
        {
            // Increment the timer
            timer += Time.deltaTime;

            if (timer >= loseTime)
            {
                TriggerLoss();
            }
        }
        else
        {
             // Reset the timer if the enemy moves away
            timer = 0f;
        }
        
    }

    void TriggerLoss()
    {
        Debug.Log("You Lose!"); // Replace this with actual loss logic
        Time.timeScale = 0f;    // Pause the game (optional)
    }
}
