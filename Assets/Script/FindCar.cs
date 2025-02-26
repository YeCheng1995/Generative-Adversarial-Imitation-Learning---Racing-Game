using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;
public class FindCar : MonoBehaviour
{
    public FollowingCar fc;
    public Text txt;
    void Update()
    {
        Car[] carArr = FindObjectsOfType<Car>();
        Car c = null;
        float f = 0;
        foreach (var item in carArr)
        {
            if (item.fit > f)
            {
                f = item.fit;
                c = item;
            }
        }
        if (c != null)
        {
            fc.target = c.transform;
            txt.text = c.name + "   Speed:" + Mathf.RoundToInt(c.curSpeed * 3.6f / 0.6f) + "    Lap Number:" + c.quanshu + "    Fitness:" + c.fit;
        }
        else
        {
            txt.text = "";
        }
    }

}
