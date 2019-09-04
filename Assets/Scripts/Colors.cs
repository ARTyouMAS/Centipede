using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Colors
{
    public static Color Get() //Возвращает случайный цвет.
    {

        //byte[] Rgb = new byte[] { 0, 0, 0 };

        //var color = Random.Range(0, 3);
        //var maxIntense = Random.Range(0, 3);
       

        //Rgb[color] = System.Convert.ToByte(Random.Range(0, 256));

        //if (maxIntense != color) с/0/0
        //{
        //    Rgb[maxIntense] = 255;
        //}
        //else if (maxIntense != 2)
        //    {
        //        Rgb[++maxIntense] = 255;
        //    }
        //    else
        //    {
        //        Rgb[--maxIntense] = 255;
        //    }

        byte random = System.Convert.ToByte(Random.Range(0, 256));
        byte r = 255, g = 255, b = 255;

        switch (Random.Range(0, 3))
        {
            case 0:
                 r = random;
                if(Random.Range(0,2) == 0)
                {
                    g = 76;
                }
                else
                {
                    b = 76;
                }
                break;
            case 1:
                g = random;
                if (Random.Range(0, 2) == 0)
                {
                    r = 76;
                }
                else
                {
                    b = 76;
                }
                break;
            case 2:
                b = random;
                if (Random.Range(0, 2) == 0)
                {
                    r = 76;
                }
                else
                {
                    g = 76;
                }
                break;
        }

        Color color = new Color(r/255f, g/255f, b/255f);
        return color;
    }
}
