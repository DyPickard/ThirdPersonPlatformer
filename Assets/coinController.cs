using UnityEngine;

public class coinController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get the coin's rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // rotate the coin
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected: " + other.gameObject.tag);

        // if the coin collides with the player
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Coin Collected!");
            // destroy the coin
            Destroy(gameObject);
            // increase the score
            gameManager.IncreaseScore();
        }
    }
}
