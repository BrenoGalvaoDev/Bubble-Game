using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NUnit.Framework;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject loadPanel;

    public GameObject[] phasesButtons;

    int unlockedPhase;

    private void Start()
    {
        unlockedPhase = PlayerPrefs.GetInt("Unlocked Phase", 0);
        for (int i = 0; i <= unlockedPhase; i++)
        {
            phasesButtons[i].SetActive(true); // if I need to add new phases, remember to change the number in the inspector
        }
    }

    public void LoadSceneAsync(string scene)
    {
        loadPanel.SetActive(true);
        SceneManager.LoadSceneAsync(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
