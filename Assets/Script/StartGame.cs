using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour {

    public GameObject selectpanel;
    public GameObject xiaos;

    public void StartButton()
    {
        selectpanel.SetActive(true);
        xiaos.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void FirstButton()
    {        
        SceneManager.LoadScene("First Level");
    }

    public void BackButton()
    {
        selectpanel.SetActive(false);
    }

    public void RaceSelectButton()
    {      
        SceneManager.LoadScene("Race");
        Time.timeScale = 1;
    }
    public void RaceSelectButton1()
    {
        SceneManager.LoadScene("Race 1");
        Time.timeScale = 1;
    }
    public void RaceSelectButton2()
    {
        SceneManager.LoadScene("Race 2");
        Time.timeScale = 1;
    }
}
