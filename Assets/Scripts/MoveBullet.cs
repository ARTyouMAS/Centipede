using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveBullet : MonoBehaviour
{
    float speed = 20f;
    public TilemapController tilemap;

    private void Start()
    {
        tilemap = FindObjectOfType<TilemapController>();
    }

    private void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Tilemap")
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.3f, 0); //Получаем координаты пули со здвигом.
            tilemap.HitTile(pos);
            Destroy(this.gameObject);
            
        }
        else if (collision.gameObject.name == "Centipede")
        {
            var mas = FindObjectsOfType<CentipedeController>();
            foreach (CentipedeController obj in mas) //Перебираем все существующие объекты. 
            {
                obj.Split(collision.gameObject);
            }

            FindObjectOfType<GameController>().AddScore(ScoreType.СentipedeHit); //Добавляем очки за попадание.
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.name == "Prefab_Ant(Clone)")
        {
            FindObjectOfType<GameController>().AddScore(ScoreType.AntHit); //Добавляем очки за попадание.
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
