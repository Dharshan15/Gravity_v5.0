using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    public Button restartButton;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI winText;
    Rigidbody2D rigidBody;
    public GameObject bullet;
    public GameObject indicator;
    public int coins = 0;
    public List<GameObject> lifeObjects;

    float[] initialSpeeds = new float[3] { 2.5f, 3.0f, 3.6f };

    public bool isTop = false;
    public bool isGameOver = false;

    private float halfScreenWidth; // Store the screen width here

    private float timeSinceLastShot = 7f;
    private float shootingCooldown = 7f; // Cooldown time between shots in seconds
    private int lives = 3;

    private void Awake()
    {
        // Reset the static variable to its initial value when a new scene is loaded
        Enemy3.speed = initialSpeeds[0];
        Enemy1.speed = initialSpeeds[1];
        Enemy2.speed = initialSpeeds[2];
    }
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        halfScreenWidth = Screen.width / 2; // Store the screen width during Start
    }

    void Update()
    {
        // Check for touch input 
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is on the left half of the screen
            if (touch.position.x < halfScreenWidth && touch.phase == TouchPhase.Began && !isGameOver)
            {
                //audioSource.Play();
                rigidBody.gravityScale *= -1;
                isTop = !isTop;
            }
            else if (timeSinceLastShot >= shootingCooldown)
            {
                indicator.gameObject.SetActive(true);// Check if enough time has passed since the last shot  
                if (touch.position.x > halfScreenWidth && touch.phase == TouchPhase.Began && !isGameOver)
                {
                    Instantiate(bullet, new Vector2(bullet.transform.position.x, transform.position.y), bullet.transform.rotation);
                    timeSinceLastShot = 0f; // Reset the time for the next shot
                    indicator.gameObject.SetActive(false);
                }
            }
        }

        // Check if enough time has passed since the last shot
        timeSinceLastShot += Time.deltaTime;

        if (isTop)
        {
            transform.eulerAngles = new Vector3(0, 180, 180);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (gameManager.time >= 360)
        {
            winText.gameObject.SetActive(true);
            GameOver();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "enemybullet")
        {           
            lives -= 1;
            lifeObjects[lives].gameObject.SetActive(false);
            Destroy(collision.gameObject);
            if (lives == 0)
            {
                isGameOver = !isGameOver;
                GameOver();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "coin")
        {
            Destroy(collision.gameObject);
            coins += 1;
        }
    }
}
