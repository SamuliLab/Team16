using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectibleLogic : MonoBehaviour
{
    public static string scorePointsKey = "ScorePoints"; // Key for PlayerPrefs
    public int itemsHeld; // Number of items held by the player
    public float rotationSpeed; // Speed of rotation in degrees per time unit

    // Door area radius where objects will be destroyed
    public float doorDestructionRadius = 5f;
    public Tilemap doorTilemap; // Viittaa Door Tilemapiin
    public TileBase doorTile; // Viittaa siihen Door-tileen, jonka haluat poistaa
    // public float destructionChance = 0.5f; // Mahdollisuus poistaa tile (0-1)

    void Start()
    {
        itemsHeld = 0; // Initialize items held to 0
    }

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
            itemsHeld++;
            Debug.Log("Item collected");

            // Update PlayerPrefs with the new score
            PlayerPrefs.SetInt(scorePointsKey, itemsHeld);

            // Destroy objects (Tiles) within a certain radius that belong to the "Door" tile
            DestroyObjectsInArea();

            // Destroy the item object
            Destroy(gameObject);
        }
    }

    // Method to destroy Door tiles in the area around the collectible
    private void DestroyObjectsInArea()
    {
        // Get the world position of the collectible
        Vector3 worldPosition = transform.position;
        Vector3Int startPosition = doorTilemap.WorldToCell(worldPosition - new Vector3(doorDestructionRadius, doorDestructionRadius, 0)); // Bottom-left corner of the area
        Vector3Int endPosition = doorTilemap.WorldToCell(worldPosition + new Vector3(doorDestructionRadius, doorDestructionRadius, 0)); // Top-right corner of the area

        // Loop through the grid cells in the defined area
        for (int x = startPosition.x; x <= endPosition.x; x++)
        {
            for (int y = startPosition.y; y <= endPosition.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                // Get the tile at this position from the Tilemap
                TileBase tile = doorTilemap.GetTile(cellPosition);

                // Check if the tile at this position is the Door tile
                if (tile == doorTile)
                {
                    doorTilemap.SetTile(cellPosition, null);
                    Debug.Log("Door tile destroyed at position: " + cellPosition);
                    // Poista tile satunnaisesti
                    //if (Random.value <= destructionChance)
                    //{
                        // Destroy the tile by setting it to null (poistetaan vain tile)
                        
                    //}
                }
            }
        }
    }
}
