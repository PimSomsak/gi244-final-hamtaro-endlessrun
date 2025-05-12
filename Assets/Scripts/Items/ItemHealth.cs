//using UnityEngine;

//public class ItemHealth : MonoBehaviour
//{
//    private PlayerController playerRef;
//    private AudioSource playerAudio;
//    public AudioClip crashSfx;


//    [SerializeField] private string playerTag = "Player"; // Tag ของผู้เล่น

//    private void Start()
//    {
//        playerAudio = GetComponent<AudioSource>();
//        playerRef = GameObject.FindWithTag(playerTag).GetComponent<PlayerController>();
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag(playerTag))
//        {
//            playerRef.hp += 1;
//            Debug.Log("❤️ ผู้เล่นเก็บไอเทม HP!");
//            playerAudio.PlayOneShot(crashSfx);
//            // ส่งกลับ Pool แทนการทำลาย
//            FindObjectOfType<ItemPoolManager>().ReturnToPool(gameObject, "Heart");
//        }
//    }
//}
using UnityEngine;
using System.Collections;

public class ItemHealth : MonoBehaviour
{
    private PlayerController playerRef;

    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float lifeTime = 5f;

    private bool isPickedUp = false;
    private Coroutine timeoutRoutine;

    private void OnEnable()
    {
        isPickedUp = false;
        timeoutRoutine = StartCoroutine(AutoReturnAfterDelay(lifeTime));
    }

    private void Start()
    {
        playerRef = GameObject.FindWithTag(playerTag).GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPickedUp) return;

        if (collision.gameObject.CompareTag(playerTag))
        {
            isPickedUp = true;

            if (timeoutRoutine != null)
                StopCoroutine(timeoutRoutine);

            playerRef.hp += 1;
            Debug.Log("❤️ ผู้เล่นเก็บไอเทม HP!");

            ReturnNow();
        }
    }

    private IEnumerator AutoReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isPickedUp)
        {
            Debug.Log("⌛ HP Item timeout → Return to Pool");
            ReturnNow();
        }
    }

    private void ReturnNow()
    {
        gameObject.SetActive(false);
        FindObjectOfType<ItemPoolManager>().ReturnToPool(gameObject, "Heart");
        isPickedUp = false;
    }
}

