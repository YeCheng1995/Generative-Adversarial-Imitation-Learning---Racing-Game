using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour {

    public Button resumebutton;
    public Button restartbutton;
    public Button backmainbutton;
    public void ResumeButton()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;

    }

    public void BackMainButton()
    {
        SceneManager.LoadScene("Start");
    }
}
