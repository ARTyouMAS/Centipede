using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public GameObject bonusLife, bonusSpeed, bonusShootingSpeed;
    int bonusType;

    public float speed = 4.5f;

    void Start()
    {
        switch (Random.Range(0, 5)) //Шанс получить жизнь меньше.
        {
            case 0:
                bonusType = 1;
                bonusLife.SetActive(true);
                break;
            case 1:
            case 2:
                bonusType = 2;
                bonusShootingSpeed.SetActive(true);
                break;
            case 3:
            case 4:
                bonusType = 3;
                bonusSpeed.SetActive(true);
                break;
        }
    }


    private void Update()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Player")
        {
            switch (bonusType)
            {
                case 1:
                    BonusLife(col.GetComponent<PlayerController>());
                    break;
                case 2:
                    IncreaseShootingSpeed(col.GetComponent<PlayerShoot>());
                    break;
                case 3:
                    IncreaseSpeed(col.GetComponent<PlayerMove>());
                    break;
            }
        }
    }

    public void IncreaseSpeed(PlayerMove PM)
    {
        if (PM.speed < 15)
        {
            PM.speed += 0.7f;
        }
        OnBecameInvisible();
    }

    public void IncreaseShootingSpeed(PlayerShoot PS)
    {
        if (PS.fireDelay > 0.15)
        {
            PS.fireDelay -= 0.015f;
        }
        OnBecameInvisible();
    }

    public void BonusLife(PlayerController PC)
    {
        if (FindObjectOfType<PlayerController>().life != 3)
        {
            PC.life += 1;
            FindObjectOfType<GameController>().ChangeLifeIcons(PC.life, true);
        }
        OnBecameInvisible();
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
