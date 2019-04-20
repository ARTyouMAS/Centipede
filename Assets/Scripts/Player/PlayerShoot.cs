using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float fireDelay;
    float cooldownTimer = 0;

    public GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        fireDelay = GetComponent<PlayerController>().fireDelay; 
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && cooldownTimer <= 0 && gameController.levelStarted)
        {
            Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z), transform.rotation);
            cooldownTimer = fireDelay; 
        }

    }
}
