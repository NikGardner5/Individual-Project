using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;
    private float moveInput;
    public Text score;
    private int scoreValue = 0;

    public GameObject textDisplay;
    public int secondsLeft = 13;
    public bool takingAway = false;

    public Text gameOverText;
    private bool gameOver;

    public Text start;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public AudioSource audioSource;
    public AudioClip pickUp;
    public AudioClip gameMusic;
    public AudioClip loseMusic;
    public AudioClip winMusic;


    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score:" + scoreValue.ToString();

        textDisplay.GetComponent<Text>().text= "Time: "+secondsLeft;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameMusic;
        audioSource.Play();
        audioSource.loop = true;


        gameOverText.text = "";
        gameOver = false;
    }

        public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rd2d.velocity = new Vector2(moveInput * speed, rd2d.velocity.y);

        isGrounded = Physics2D.OverlapCircle(feetPos.position,checkRadius, whatIsGround);
         
            if(isGrounded == true && Input.GetKeyDown(KeyCode.W))
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rd2d.velocity = Vector2.up * jumpForce;
            }

            if(Input.GetKey(KeyCode.W) && isJumping == true)
            {
                 if(jumpTimeCounter > 0)
                 {
                     rd2d.velocity = Vector2.up * jumpForce;
                     jumpTimeCounter -= Time.deltaTime;

                 }
                 else
                 {
                     isJumping = false;
                 }
            }

            if(Input.GetKeyUp(KeyCode.W))
            {
                isJumping = false;
            }

        if(takingAway==false && secondsLeft > 0)
        {
            StartCoroutine(TimerTake());
        }

        if (scoreValue >= 3 && gameOver == false)
            {
            gameOver = true;
            gameOverText.text = "You Win! Game by Nik Gardner!";

            audioSource.clip = gameMusic;
            audioSource.Stop();

            audioSource.clip = winMusic;
            audioSource.Play();
            audioSource.loop = false;

            }

        else if (scoreValue <= 2 && secondsLeft <=0 && gameOver ==false)
            {
            gameOver = true;
            gameOverText.text = "You Lose! Game by Nik Gardner!";

            audioSource.clip = gameMusic;
            audioSource.Stop();

            audioSource.clip = loseMusic;
            audioSource.Play();
            audioSource.loop = false;
            }

        if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
                {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            PlaySound(pickUp);
        }
    }

    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft-= 1;
            if (secondsLeft <10)
            {
                textDisplay.GetComponent<Text>().text= "Time: 0"+secondsLeft;
            }
            else
            {
                textDisplay.GetComponent<Text>().text= "Time: "+secondsLeft;
            }
        takingAway = false;
    }
}