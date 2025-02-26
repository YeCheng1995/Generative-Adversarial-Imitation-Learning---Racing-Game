using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishCheck : MonoBehaviour {

    private int checkfinishcar;
    private int checkAI;
    public GameObject resultpanel;
    public Text resulttext;

    public GameObject AICarParent;


    private void Start()
    {
        checkfinishcar = 0;
        checkAI = 0;
    }

    // Update is called once per frame
    void Update () {
        if (checkfinishcar == 4)
        {
            resultpanel.SetActive(true);
            resulttext.text = "You Win";
            Time.timeScale = 0;
            AudioListener.pause = true;
        }

        if (checkAI == 1)
        {
            resultpanel.SetActive(true);
            resulttext.text = "AIcar Win\nYou Defeat!";
            Evolution1 ev1 = AICarParent.gameObject.GetComponent<Evolution1>();
            
            Time.timeScale = 0;
            AudioListener.pause = true;

        }
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 11)
        {
            checkfinishcar ++;
          //  print("Car");
            
        }
        
        if (c.transform.tag == "AI")
        {
            Car car1 = c.gameObject.GetComponent<Car>();

            if (car1.quanshu == 2)
            {
                checkAI = 1;
               // print("AI");
            }            
        }
    }
}
