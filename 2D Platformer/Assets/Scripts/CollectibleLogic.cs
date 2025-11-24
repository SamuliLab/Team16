using UnityEngine;

public class CollectibleLogic : MonoBehaviour
{
    // public static int totalItems = 0; // Static variable for total score
    public static string scorePointsKey = "ScorePoints"; // Key for PlayerPrefs
    public int itemsHeld; // Number of items held by the player
    public float rotationSpeed; // Speed of rotation in degrees per time unit
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the item around its Y-axis
        transform.Rotate(Vector3.down, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Increment the score by 1
            itemsHeld ++;
            Debug.Log("Item collected");
            Debug.Log("Items collected: " + itemsHeld);

            // Destroy the item object
            Destroy(gameObject);
        }
    }
    
    
}