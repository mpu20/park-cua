using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{
    [SerializeField]
    Text txtTime;
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject btnResume;
    [SerializeField]
    GameObject btnRestart;
    [SerializeField]
    GameObject btnSetting;
    [SerializeField]
    GameObject btnMenu;
    float time = 0;
    bool isPaused;

    void Start()
    {
        txtTime.text = "Time: 0:00";
        menu.SetActive(false);
        btnMenu.SetActive(false);
        btnResume.SetActive(false);
        btnRestart.SetActive(false);
        btnSetting.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        if (!isPaused)
        {
            time += Time.deltaTime;
            int minutes = (int)time / 60;
            int seconds = (int)time % 60;
            txtTime.text = "Time: " + minutes.ToString() + ":" + seconds.ToString("00");
        }
    }

    public void PauseButton_Click()
    {
        Debug.Log("An nut Pause");
        if (isPaused)
        {
            isPaused = false;
            menu.SetActive(false);
            btnMenu.SetActive(false);
            btnResume.SetActive(false);
            btnRestart.SetActive(false);
            btnSetting.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            isPaused = true;
            menu.SetActive(true);
            btnMenu.SetActive(true);
            btnResume.SetActive(true);
            btnRestart.SetActive(true);
            btnSetting.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void ResumeButton_Click()
    {
        isPaused = false;
        menu.SetActive(false);
        btnMenu.SetActive(false);
        btnResume.SetActive(false);
        btnRestart.SetActive(false);
        btnSetting.SetActive(false);
        Time.timeScale = 1;
    }
}