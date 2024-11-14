using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image lifeBar;
    public GameObject pausePanel;
    public GameObject loadPanel;
    public GameObject GameOverPanel;
    public GameObject WinPanel;


    bool gamePause;

    public void OnPlayerDamage(float currentHealth, float maxHealth)
    {
        lifeBar.fillAmount = currentHealth / maxHealth;
    }

    public void OnPauseGame()
    {
        gamePause = !gamePause;
        pausePanel.SetActive(gamePause);
    }

    public void OnLoadPanelActive()
    {
        loadPanel.SetActive(true);
    }

    public void OnGameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void OnWinGame()
    {
        WinPanel.SetActive(true);
    }
}
