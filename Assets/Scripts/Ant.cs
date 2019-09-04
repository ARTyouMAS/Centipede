using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public float yPos = 10.5f;
    public float xPos;

    TilemapController tilemapController;

    int speed = 4;

    void Start()
    {      
        tilemapController = FindObjectOfType<TilemapController>();

        xPos = Random.Range(-12.5f, 12.5f); //Получаем случайную позицию по оси X.
        transform.position = new Vector2(xPos, yPos);

        StartCoroutine(SpawnTile());
    }

    void Update()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);        
    }

    IEnumerator SpawnTile()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            if(Random.Range(0,2) == Random.Range(0, 2) && transform.position.y > -7.5f) //Случайная генерация объектов земли в допустимой зоне.                                                                                               
            {
                tilemapController.SetGroundTile(transform.position);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
