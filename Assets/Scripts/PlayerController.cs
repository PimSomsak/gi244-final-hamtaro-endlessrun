using System.Collections;
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

    public bool gameOver = false;
    public bool isImmortal = false;
    private float TimeImmortal = 3;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        hp = 5;

       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // rb.AddForce(1000 * Vector3.up);
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
        else if (collision.gameObject.CompareTag("Obstacle") && !isImmortal)
        {
            hp--;
            explosionParticle.Play();
            SpawnManagerPool.GetInstance().Return(collision.gameObject);

            isImmortal = true; // เปิดโหมดอมตะ
            StartCoroutine(ResetImmortal()); // เริ่มนับเวลา 3 วินาที
        }
    }

    public void TriggerImmortal()
    {
        StopCoroutine("ResetImmortal"); // กันกรณีชนหลายครั้งซ้อน
        isImmortal = true;
        StartCoroutine(ResetImmortal());
    }


    IEnumerator ResetImmortal()
    {
        yield return new WaitForSeconds(TimeImmortal);
        isImmortal = false; // กลับสู่สถานะปกติ
    }

}