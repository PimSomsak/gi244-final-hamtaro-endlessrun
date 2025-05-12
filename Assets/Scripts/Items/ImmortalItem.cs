//using UnityEngine;

//public class ImmortalItem : MonoBehaviour
//{
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            PlayerController playerRef = collision.gameObject.GetComponent<PlayerController>();
//            if (playerRef != null)
//            {
//                playerRef.TriggerImmortal();
//                Debug.Log("✅ Player picked up Immortal Item");
//            }

//            FindObjectOfType<ItemPoolManager>().ReturnToPool(gameObject, "Immortal");
//        }
//    }
//}
using UnityEngine;
using System.Collections;

public class ImmortalItem : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    private bool isPickedUp = false;
    private Coroutine timeoutRoutine;

    private void OnEnable()
    {
        isPickedUp = false;
        timeoutRoutine = StartCoroutine(AutoReturnAfterDelay(lifeTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPickedUp) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isPickedUp = true;

            if (timeoutRoutine != null)
                StopCoroutine(timeoutRoutine);

            PlayerController playerRef = collision.gameObject.GetComponent<PlayerController>();
            if (playerRef != null)
            {
                playerRef.TriggerImmortal();
                Debug.Log("✅ Player picked up Immortal Item");
            }

            ReturnNow();
        }
    }

    private IEnumerator AutoReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isPickedUp)
        {
            Debug.Log("⌛ Immortal Item timeout → Return to Pool");
            ReturnNow();
        }
    }

    private void ReturnNow()
    {
        gameObject.SetActive(false);
        FindObjectOfType<ItemPoolManager>().ReturnToPool(gameObject, "Immortal");
        isPickedUp = false;
    }
}

