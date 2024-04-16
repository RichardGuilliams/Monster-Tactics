using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float yOffset;
    public GameObject healthBarPrefab; // Reference to the health bar prefab
    public Transform canvasTransform; // Reference to the Canvas transform
    public Transform target; // Reference to the creature's Transform (head position)
    private bool active = false;
    private Slider healthSlider; // Reference to the slider UI component

    void Start()
    {
        // Ensure the healthBarPrefab reference is set
        if (healthBarPrefab == null)
        {
            Debug.LogError("HealthBarPrefab reference is not set!");
            enabled = false; // Disable the script if the reference is not set
            return;
        }

        // Create a new health bar prefab instance
        GameObject healthBarInstance = Instantiate(healthBarPrefab, canvasTransform);

        // Get the Slider component from the health bar instance
        healthSlider = healthBarInstance.GetComponent<Slider>();

        // Set the initial position of the health bar above the creature's head
        UpdatePosition();
    }

    void Update()
    {
        //Update the visibility of the health bar.
        UpdateVisible();
        // Update the position of the health bar every frame to follow the creature's head
        UpdatePosition();
    }

    void UpdateVisible(){
        if(!active){
            healthBarPrefab.SetActive(false);
        }
    }

    void UpdatePosition()
    {
        // Ensure the target reference is set
        if (target != null && healthSlider != null)
        {
            // Get the position of the creature's head in world space
            Vector3 targetPosition = target.position;

            // Offset the position to display the health bar above the creature's head
            targetPosition.y += yOffset; // Adjust this value as needed for proper positioning

            // Set the position of the health bar
            healthSlider.transform.position = Camera.main.WorldToScreenPoint(targetPosition);
        }
    }

    // Method to update the health value displayed on the health bar
    public void SetHealth(float health)
    {
        // Ensure the healthSlider reference is set
        if (healthSlider != null)
        {
            // Update the value of the slider (health)
            healthSlider.value = health;
        }
    }
}
