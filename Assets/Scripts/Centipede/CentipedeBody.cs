using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeBody : MonoBehaviour
{
    public float speed;
    public GameObject targetObj;
    public Vector3 target;

    public bool head;

    private void Update()
    {
        if (!head) //Движемся за таргетом если не голова.
        {
            target = targetObj.transform.position; //Получаем текущее положение таргета.
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
        }
    }
}
