using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    private PlayerController playerRef;

    [SerializeField] private string playerTag = "Player"; // Tag ของผู้เล่น

    private void Start()
    {
        playerRef = GameObject.FindWithTag(playerTag).GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            playerRef.hp += 1;
            Debug.Log("❤️ ผู้เล่นเก็บไอเทม HP!");

            // ส่งกลับ Pool แทนการทำลาย
            SpawnManagerPool.GetInstance().Return(gameObject);
        }
    }
}
