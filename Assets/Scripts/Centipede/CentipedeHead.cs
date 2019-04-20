using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeHead : MonoBehaviour
{
    public float speed = 5; 
    bool directionX; //Направление по оси X.

    public float offset;

    bool move = true;
    public bool dictionY = true; //Направление по оси Y.

    private void Update()
    {
        if (move) //Если не совершаем перемещение по оси Y.
        {
            if (!directionX) //Движемся влево.
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            else //Движемся вправо.
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            }
        }

        if (offset > 0) //Если нужно изменить положение по оси Y.
        {
            move = false;

            if (dictionY) //Если движемся вверх - поднимаемся.
            {
                transform.position -= new Vector3(0, 0.1f, 0);
            }
            else //Если движемся внизу - опускаемся.
            {
                transform.position += new Vector3(0, 0.1f, 0);
            }

            offset -= 0.1f;
        }
        else
        {
            if (transform.position.y % 0.5f != 0) //Проверка на правильное расположение по оси Y.
            {
                float tmp = transform.position.y - 0.4f; //Отнимаем значение от текущей позици по Y.
                tmp = (float)System.Math.Round(tmp, 0); //Округляем до целого числа.
                tmp += 0.5f; //Добавлем сдвиг для правильного расположения в клеточках.

                transform.position = new Vector3(transform.position.x, tmp, transform.position.z);
                return; //Выходим из функции.
            }
            move = true; //Сдвиг произошел, можно продолжить движение по оси X.
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ground") 
        {
            dictionY = !dictionY; //Меняем направление по оси Y.
            FindObjectOfType<GameController>().ShowRoof(true); 

        }else if(collision.gameObject.name == "Tilemap")
        {
            offset += 1; //Сдвигаемся по оси Y взависимости от направления движения.
            directionX = !directionX; //Меняем направление по оси X.
        }
    }
}
