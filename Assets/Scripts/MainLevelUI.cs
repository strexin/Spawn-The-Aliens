using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainLevelUI : MonoBehaviour
{
    [SerializeField] Player player;

    [Header("UI Components")]
    [SerializeField] private Slider slider;
    [SerializeField] GameObject pausePanel;

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    public void SetMaxHealth(float max)
    {
        slider.maxValue = max;
        slider.value = max;
    }

    public void CurrentHealth(float health)
    {
        slider.value = health;
    }

    public void PausePanelOn()
    {
        player.isPausing = true;
        Time.timeScale = 0.0f;
        pausePanel.SetActive(true);
    }

    public void PausePanelOff()
    {
        player.isPausing = false;
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
    }

    public void ExitGame()
    {
        PausePanelOff();
        SceneManager.LoadScene(0);
    }
}
