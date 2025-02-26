using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountDown : MonoBehaviour {

    private int timeleft = 4;
    private int sumtime = 360;
    private Text timelefttext;
    private Text gametimetext;
    public GameObject resultpanel;
    public Text resulttext;
    private GameObject finish;

    void Start()
    {       
        timelefttext = GameObject.Find("TimeLeftText").GetComponent<Text>();
        gametimetext = GameObject.Find("GameTimeText").GetComponent<Text>();
        //finish = GameObject.Find("Finish");
        // finish.SetActive(false);
        AudioListener.pause = false;
        //开启协程
        StartCoroutine(TimeCount());
    }
    
    //倒计时
    public IEnumerator TimeCount()
    {
        while (timeleft >= 0)
        {
            //print("还剩：" + timeleft + "秒");
            yield return new WaitForSeconds(1);
            timeleft--;
            timelefttext.text = timeleft.ToString();
            if (timeleft == 0)
                timelefttext.text = "Start";
            else if (timeleft == -1)
                 {
                    timelefttext.text = "";
                    GameObject[] CarTag = GameObject.FindGameObjectsWithTag("Car");
                    foreach (GameObject cartag in CarTag)
                    {
                   
                    cartag.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                 }
                 GameObject AlCarTag = GameObject.FindGameObjectWithTag("AICarParent");
                 AlCarTag.GetComponent<Evolution1>().enabled = true;
                 StartCoroutine(GameTimeCount());
                 }            
        }
    }
  
    public IEnumerator GameTimeCount()
    {
        while (sumtime >= 0)
        {
            sumtime--;
            //print("总时间还剩" + sumtime / 60 + "分:" + sumtime % 60 + "秒");
            gametimetext.text = sumtime / 60 + ":" + sumtime % 60 ;
            if (sumtime == 0)
            {
                resultpanel.SetActive(true);
                resulttext.text = "Time Out";
                Time.timeScale = 0;
                AudioListener.pause = true;
            }
            /*
            if (sumtime > 160)
            {
                finish.SetActive(true);
            }
            */
            yield return new WaitForSeconds(1);
        }
    }
   
}
