using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JianCeDian : MonoBehaviour
{
    public int index;
    private float angleVector;

    private float i;
    // Use this for initialization
    void Start()
    {
       
        index = int.Parse(name);
    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 9)
        {
                    
            Car car = c.gameObject.GetComponent<Car>();
            if (index == car.jianceNum - 1 && car.nextIndex == index)
            {
                car.nextIndex = 0;
                car.moveDis += car.jianceDis[index];
                car.quanshu++;
                if (car.quanshu >= 3)
                {
                     car.gameObject.SetActive(false);                    
                }
            }
            else if (index == car.nextIndex)
            {
                car.moveDis += car.jianceDis[index];
                car.nextIndex++;
            }
        }
    }


}
