using UnityEngine;

public class ImmortalItem : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerRef = collision.gameObject.GetComponent<PlayerController>();
            if (playerRef != null)
            {
                playerRef.TriggerImmortal();
                Debug.Log("✅ Player picked up Immortal Item");
            }

            SpawnManagerPool.GetInstance().Return(gameObject); // คืนเข้า Pool
        }
    }
}
