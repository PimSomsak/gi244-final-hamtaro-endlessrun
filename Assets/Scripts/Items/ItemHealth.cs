using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    private PlayerController playerRef;

    // กำหนด Layer ที่จะตรวจสอบ เช่น "Player" = Layer 6
    

    private void Start()
    {
        playerRef = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRef.hp += 1;
            Debug.Log("ผู้เล่นชนกับไอเทมแล้ว! ❤️");
        }
    }
}
