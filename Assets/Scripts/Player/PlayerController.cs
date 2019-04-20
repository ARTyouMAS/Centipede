using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed; 
    public float fireDelay;

    GameController gameController;

    int life;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        life = 3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Centipede")
        {
            life--;
            gameController.ChangeLifeIcons(life, false);
            if (life == 0)
            {
                GameOver();
                return;
            }
            MinusLife();
        }else if(collision.name == "Prefab_Ant(Clone)")
        {
            life--;
            gameController.ChangeLifeIcons(life, false);
            Destroy(collision.gameObject);

            if (life == 0)
            {
                GameOver();
                return;
            }
            MinusLife();
        }
    }

    void MinusLife()
    {
        FindObjectOfType<GameController>().LevelControll(false); 
    }

    void GameOver()
    {
        FindObjectOfType<GameController>().GameOver();
    }

    public void Bonus()
    {
        if(Random.Range(0,5) == Random.Range(0, 5))
        {
            switch (Random.Range(0, 5)) //Шанс получить жизнь меньше.
            {
                case 0:
                    if (life == 3)
                        goto case 1;
                    BonusLife();
                    break;
                case 1:
                case 2:
                    IncreaseShootingSpeed();
                    break;
                case 3:
                case 4:
                    IncreaseSpeed();
                    break;
            }
        }
    }

    public void IncreaseSpeed()
    {
        if (speed < 15)
        {
            speed += 0.7f;
            GetComponent<PlayerMove>().speed = speed;
        }
    }

    public void IncreaseShootingSpeed()
    {
        if (fireDelay < 0.15)
        {
            fireDelay -= 0.02f;
            GetComponent<PlayerShoot>().fireDelay = fireDelay;
        }
    }

    public void BonusLife()
    {
        life += 1;
        gameController.ChangeLifeIcons(life, true);
    }
}
