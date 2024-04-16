        
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float minMoveInterval; // Minimum interval between movements
    public float maxMoveInterval; // Maximum interval between movements
    public Stats stats;   // Speed of movement
    public float minRestInterval;
    public float maxRestInterval;

    private float timer;    // Timer to track time elapsed
    private float rest; // Timer to track resting period.
    private bool canMove; // Switch for resting/moving phases
    private Vector2 randomDirection; // Random direction for movement

    void Start()
    {
        // Initialize timer with a random value between min and max intervals
        timer = Random.Range(minMoveInterval, maxMoveInterval);
        // Initialize rest with a random value between min and max intervals
        rest = Random.Range(minMoveInterval, maxMoveInterval);
        canMove = true;
        // Generate a random direction vector
        randomDirection = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        // Decrement timer

        // // Check if it's time to move
        // if (timer <= 0)
        // {
        //     // Generate new random direction
        //     randomDirection = Random.insideUnitCircle.normalized;
        //     // Reset timer with a new random value between min and max intervals
        //     timer = Random.Range(minMoveInterval, maxMoveInterval);
        //     timer = Random.Range(minMoveInterval, maxMoveInterval);
        // }
        if(canMove && timer >= 0){
        // Move the object in the random direction
            transform.Translate(randomDirection * stats.speed * Time.deltaTime);
            timer -= Time.deltaTime;
            if(timer <= 0) {
                canMove = false;
                rest = Random.Range(minRestInterval, maxRestInterval);
            }
        }
        else if(!canMove && rest >= 0){
            rest -= Time.deltaTime;
            if(rest <= 0){
                canMove = true;
                randomDirection = Random.insideUnitCircle.normalized;
                timer = Random.Range(minMoveInterval, maxMoveInterval);
            }
        }
        Debug.Log(timer);
        Debug.Log(rest);
    }
}
