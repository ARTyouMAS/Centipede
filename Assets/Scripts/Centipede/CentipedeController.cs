using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeController : MonoBehaviour
{
    public List<GameObject> centBody;
    public TilemapController tilemapController;

    private void Start()
    {
        tilemapController = FindObjectOfType<TilemapController>();
    }

    IEnumerator SplitC(GameObject _splitPoint)
    {
        yield return new WaitForEndOfFrame();

        if (centBody.Find(x => x == _splitPoint) != null)//Проверка - есть ли объект в листе.
        {
            int index = centBody.FindIndex(x => x == _splitPoint); //Индекс элемента который нужно удалить.

            if (centBody.Count >= 2)//Существует ли возможность разделится?
            {
                GameObject go = new GameObject(); //Создаем новый объект контроллер.
                go.AddComponent<CentipedeController>();
                go.GetComponent<CentipedeController>().centBody = new List<GameObject>();
               // go.GetComponent<CentipedeController>().tilemapController = tilemapController;

                GameObject newHead; //Объект новой головы.
                 
                if (centBody.Count-1 == index) //Исключение, объект в который попали находится в хвосте.
                {
                     newHead = centBody[index].gameObject;
                }
                else 
                {
                     newHead = centBody[index + 1].gameObject; 
                }
                newHead.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("head"); //Меняем спрайт на спрайт головы. 
                newHead.GetComponent<CentipedeBody>().head = true; //У новой головы отключаем скрипт тела.
                newHead.AddComponent<CentipedeHead>(); //Добавляем скрипт головы.

                newHead.GetComponent<CentipedeHead>().dictionY = centBody[0].GetComponent<CentipedeHead>().dictionY; //Получаем направление движения.

                for (int i = index + 1; i < centBody.Count; i++) 
                {
                    go.GetComponent<CentipedeController>().centBody.Add(centBody[i]);//Прикрепляем в новой контроллер вторую половину.
                }
            }

            tilemapController.SetGroundTile(centBody[index].gameObject.transform.position); //Устанавливаем землю.
            Destroy(centBody[index].gameObject); //Уничтожаем объект для удаления.
  
            centBody.RemoveRange(index, centBody.Count - index); //Удаляем вторую половину из текущей головы.

            if (centBody.Count == 0) //Если не осталось компонентов - удаляем контроллер.
            {
                FindObjectOfType<GameController>().EndLevelCheck(); //Проверка на прохождения уровня.
                Destroy(this.gameObject);
            }
        }
    }

    public void Split(GameObject _splitPoint)
    {
        StartCoroutine(SplitC(_splitPoint));
    }

    public void Destroy() //Уничтожение врага.
    {
        foreach(GameObject _gO in centBody)
        {
            Destroy(_gO.gameObject);
        }

        Destroy(this.gameObject);
    }
}
