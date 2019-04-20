using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        speed = GetComponent<PlayerController>().speed;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            pos.y += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            pos.y -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= speed * Time.deltaTime;
        }

        if(pos.y > -6.5f)
        {
            pos.y = -6.5f;
        }

        if (pos.y < -9.5f)
        {
            pos.y = -9.5f;
        }

        if (pos.x > 12.7f)
        {
            pos.x = 12.7f;
        }

        if (pos.x < -12.7f)
        {
            pos.x = -12.7f;
        }

        transform.position = pos;
    }
}
