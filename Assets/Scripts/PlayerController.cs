using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
    public AudioClip heartSfx;
    public AudioClip immortalSfx;


    private Rigidbody rb;
    private InputAction jumpAction;
    public InputAction sprintAction;
    public InputAction slowAction;
    public bool isSprint = false;
    public bool isSlow = false;
    private bool isOnGround = true;
    private bool isDoubleJumpable = false;

    public int hp;

    private Animator playerAnim;
    private AudioSource playerAudio;
    private UiManager uiManager;

    public bool gameOver = false;
    public bool isImmortal = false;
    [SerializeField] private float TimeImmortal = 3f;
    private float ImmortalCount;


    private float distanceTravelled = 0f;
    public float visualRunSpeed = 6f;
    public float visualSprintSpeed = 9f;


    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI TimeText;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        hp = 5;
    }

    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
        Physics.gravity *= gravityModifier;
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Fast");
        slowAction = InputSystem.actions.FindAction("Slow");

        gameOver = false;
        ImmortalCount = 0f;
        TimeText.text = "";
    }

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

        if (slowAction.IsPressed())
        {
            isSlow = true;

        }
        else
        {
            isSlow = false;

        }


        if (hp <= 0 && !gameOver)
        {
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            uiManager.OverScreen();
            
        }


        if (!gameOver)
        {
            float currentVisualSpeed = isSprint ? visualSprintSpeed : visualRunSpeed;
            distanceTravelled += currentVisualSpeed * Time.deltaTime;
            distanceText.text = "distance: " + distanceTravelled.ToString("F0") + "M";
            HPText.text = "Health : " + hp.ToString();
        }



        if (isImmortal)
        {
            ImmortalCount -= Time.deltaTime;
            TimeText.text = ImmortalCount.ToString("F1") + "S";

            if (ImmortalCount <= 0f)
            {
                isImmortal = false;
                TimeText.text = "";
            }
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
            //SpawnManagerPool.GetInstance().Return(collision.gameObject);
            GameObject expoFx = Instantiate(explosionPrefab, expoPos, explosionPrefab.transform.rotation);
            playerAudio.PlayOneShot(crashSfx);
            Destroy(expoFx, 2);
            //explosionParticle.Play();
            Destroy(collision.gameObject);
            //SpawnManagerPool.GetInstance().Return(collision.gameObject);

        }
        else if (collision.gameObject.CompareTag("Heart"))
        {
            playerAudio.PlayOneShot(heartSfx);
        }
        else if (collision.gameObject.CompareTag("Immortal"))
        {
            playerAudio.PlayOneShot(immortalSfx);
        }

    }

    public void TriggerImmortal()
    {
        StopCoroutine("ResetImmortal");
        isImmortal = true;
        StartCoroutine(ResetImmortal());
    }

    IEnumerator ResetImmortal()
    {
        ImmortalCount = TimeImmortal;
        yield return new WaitForSeconds(TimeImmortal);
        isImmortal = false;
        TimeText.text = "";
    }
}
