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

    public IEnumerator SplitC(GameObject splitPoint)
    {
        yield return new WaitForEndOfFrame();

        if (centBody.Find(x => x == splitPoint) != null)//Проверка - есть ли объект в листе.
        {
            int index = centBody.FindIndex(x => x == splitPoint); //Индекс элемента который нужно удалить.

            if (centBody.Count >= 2)//Существует ли возможность разделится?
            {
                CentipedeController centipedeController = new GameObject().AddComponent<CentipedeController>(); //Создаем новый объект контроллер.
                centipedeController.centBody = new List<GameObject>();

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
                    centipedeController.centBody.Add(centBody[i]);//Прикрепляем в новой контроллер вторую половину.
                }
            }

            tilemapController.SetGroundTile(centBody[index].gameObject.transform.position); //Устанавливаем землю.
            Destroy(centBody[index].gameObject); //Уничтожаем объект для удаления.
  
            centBody.RemoveRange(index, centBody.Count - index); //Удаляем вторую половину из текущей головы.

            if (centBody.Count == 0) //Если не осталось компонентов - удаляем контроллер.
            {
                var GC = FindObjectOfType<GameController>();//Проверка на прохождения уровня.
                GC.StartCoroutine(GC.EndLevelCheckCour());
                Destroy(this.gameObject);
            }
        }
    }

    public void Destroy() //Уничтожение врага.
    {
        foreach(GameObject gO in centBody)
        {
            Destroy(gO.gameObject);
        }

        Destroy(this.gameObject);
    }
}
