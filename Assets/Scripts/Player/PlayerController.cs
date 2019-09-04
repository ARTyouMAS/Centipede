using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed; 
    public float fireDelay;

    GameController gameController;

    public int life;

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

}
