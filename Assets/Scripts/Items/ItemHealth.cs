using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    private PlayerController playerRef;

    private void Start()
    {
        playerRef = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            playerRef.hp += 1;
            Debug.Log("ผู้เล่นเข้าสู่โซน!");

        }
    }
}
