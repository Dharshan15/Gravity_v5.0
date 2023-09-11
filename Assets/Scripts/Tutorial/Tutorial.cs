using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    public TextMeshProUGUI startText;
    Rigidbody2D rigidBody;

    public GameObject bullet;
    public GameObject enemy;

    public bool isTop = false;
    public bool isGameOver = false;
    public bool isLeftTouched = false;

    private float halfScreenWidth; // Store the screen width here
    private bool rightTextDisplayed = false; // Flag to check if the right text has been displayed

    private float timeSinceLastShot = 1f;
    private float shootingCooldown = 1f;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        halfScreenWidth = Screen.width / 2; // Store the screen width during Start
    }

    void Update()
    {
        // Check for touch input 
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is on the left half of the screen
            if (touch.position.x < halfScreenWidth && touch.phase == TouchPhase.Began)
            {
                isLeftTouched = true;
                leftText.gameObject.SetActive(false);
                rigidBody.gravityScale *= -1;
                isTop = !isTop;
                if (!rightTextDisplayed) // Display the right text only if it hasn't been displayed yet
                {
                    rightText.gameObject.SetActive(true);
                    enemy.gameObject.SetActive(true);
                    rightTextDisplayed = true; // Set the flag to true after displaying the right text
                }
            }
            else if (touch.position.x > halfScreenWidth && touch.phase == TouchPhase.Began && isLeftTouched)
            {
                // Check if enough time has passed since the last shot
                if (timeSinceLastShot >= shootingCooldown)
                {
                    Instantiate(bullet, new Vector2(bullet.transform.position.x, transform.position.y), bullet.transform.rotation);
                    timeSinceLastShot = 0f; // Reset the time for the next shot
                    rightText.gameObject.SetActive(false);
                    startText.gameObject.SetActive(true);
                    StartCoroutine(WaitAndLoadScene());
                }
            }
        }

        if (gameObject.transform.position.x > 11)
            gameObject.transform.position = new Vector2(-6.32f, -3.28f);

        if (isTop)
            transform.eulerAngles = new Vector3(0, 180, 180);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);

    }

    IEnumerator WaitAndLoadScene()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Load the new scene with the desired scene name (replace "YourSceneName" with the actual scene name)
        SceneManager.LoadScene(2);
    }
}
