using UnityEngine;

public class ImmortalItem : MonoBehaviour
{
    private PlayerController playerRef;

    private void Start()
    {
        playerRef = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // เรียกใช้งาน Immortal แบบมีระยะเวลา
            playerRef.TriggerImmortal();

            Debug.Log("✅ Player picked up Immortal Item");
            Destroy(gameObject); // ทำลายไอเทมหลังชนแล้ว
        }
    }
}
