using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public GameObject explosionPrefab;
    public Vector3 expoPos = new(3, 1, 0);

    public AudioClip jumpSfx;
    public AudioClip crashSfx;

    private Rigidbody rb;
    private InputAction jumpAction;
    public InputAction sprintAction;
    public bool isSprint = false;
    private bool isOnGround = true;
    private bool isDoubleJumpable = false;

    public int hp;

    private Animator playerAnim;
    private AudioSource playerAudio;
    private UiManager uiManager;

    public bool gameOver = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        hp = 5;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // rb.AddForce(1000 * Vector3.up);
        Physics.gravity = new Vector3(0, -9.81f, 0); 
        Physics.gravity *= gravityModifier;

        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");

        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.triggered && isOnGround && !isDoubleJumpable && !gameOver)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            isOnGround = false;
            isDoubleJumpable = true;

            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSfx);
        }
        else if (jumpAction.triggered && !isOnGround && isDoubleJumpable && !gameOver)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            isOnGround = false;
            isDoubleJumpable = false;

            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSfx);
        }


        if (sprintAction.IsPressed()) 
        {
            isSprint = true;
        }
        else
        {
            isSprint = false;
        }


        if (hp <= 0)
        {
            //Debug.Log("Game Over!");
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play(); 
            dirtParticle.Stop();
            uiManager.OverScreen();
            //playerAudio.PlayOneShot(crashSfx);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            isDoubleJumpable = false;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            
            hp--;
            GameObject expoFx = Instantiate(explosionPrefab, expoPos, explosionPrefab.transform.rotation);
            Destroy(expoFx,2);
            //explosionParticle.Play();
            //Destroy(collision.gameObject);
            SpawnManagerPool.GetInstance().Return(collision.gameObject);
        }
    }

}