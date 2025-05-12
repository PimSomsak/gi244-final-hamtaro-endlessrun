﻿using UnityEngine;
using TMPro;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10f;

    private float leftBound = -15;
    

    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    
    void Update()
    {
        if (!playerController.gameOver && !playerController.isSprint)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        else if (!playerController.gameOver && playerController.isSprint)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed * 2);
        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
//<<<<<<< HEAD
//<<<<<<< HEAD
            
            SpawnManagerPool.GetInstance().Return(gameObject);
//=======
            Destroy(gameObject);
            //SpawnManagerPool.GetInstance().Return(gameObject);
//>>>>>>> Premiere
//=======
            Destroy(gameObject);
            //SpawnManagerPool.GetInstance().Return(gameObject);
//>>>>>>> Premiere
        }

        
    }
}
