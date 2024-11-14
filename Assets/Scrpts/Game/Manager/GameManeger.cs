using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    #region Bubbles Pooling
    [Header("Bubbles Pooling")]
    public GameObject[] bubblesPrefabs;
    public int bubblesAmount;
    List<GameObject> bubblesInstances = new List<GameObject>();
    public List<Transform> positions = new List<Transform>();
    #endregion

    #region Fish Pooling
    [Space(30)]
    [Header("Fish Pooling")]
    public GameObject[] fishPrefabs;
    public int fishAmount;
    public List <GameObject> fishInstances = new List<GameObject>();

    #endregion

    #region Turtle and StingRay Pooling
    [Space(30)]
    [Header("Turtle and StingRay Pooling")]
    public GameObject[] tsPrefabs;
    public int tsAmount;
    public List<GameObject> tsInstances = new List<GameObject>();
    #endregion

    #region Events
    //Evnt to pause game
    public delegate void GamePause();
    public event GamePause OnGamePause;

    //Event to load new scene
    public delegate void LoadPanel();
    public event LoadPanel OnLoadPanel;


    #endregion


    [SerializeField] int phaseNumber;
    int savedValue;

    void Awake()
    {
        AddPolling();
    }

    void Start()
    {
        SpawnBubbles();
        savedValue = PlayerPrefs.GetInt("Unlocked Phase");
    }

    void Update()
    {
        PauseGame();
    }

    public void AddPolling()
    {
        for (int i = bubblesPrefabs.Length - 1; i >= 0; i--)
        {
            for(int j = bubblesAmount; j >= 0; j--)
            {
                GameObject instance = Instantiate(bubblesPrefabs[i]);
                instance.SetActive(false);
                bubblesInstances.Add(instance);
            }
        }

        for (int i = fishPrefabs.Length - 1; i >= 0; i--)
        {
            for (int j = fishAmount; j >= 0; j--)
            {
                GameObject instance = Instantiate(fishPrefabs[i]);
                instance.SetActive(false);
                fishInstances.Add(instance);
            }
        }

        for (int i = tsPrefabs.Length - 1; i >= 0; i--)
        {
            for (int j = tsAmount; j >= 0; j--)
            {
                GameObject instance = Instantiate(tsPrefabs[i]);
                instance.SetActive(false);
                tsInstances.Add(instance);
            }
        }
    }

    public void SpawnBubbles()
    {
        for(int i = bubblesInstances.Count - 1; i >= 0;i--)
        {
            bubblesInstances[i].transform.position = positions[i].position;
            bubblesInstances[i].SetActive(true);
        }
    }

    public void PauseGame()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GamePauseButton();
            //uiManager.pausePanel.SetActive(gamePause);
        }
    }

    #region Events to be called
    public void OnGameOver()
    {
        //implement the rest of the logic ---> save score??
    }

    public void OnWinGame()
    {
        //implement the rest of the logic ---> save score??

        //Unlock new fase in menu
        if (phaseNumber >= savedValue)
        {
            PlayerPrefs.SetInt("Unlocked Fase", phaseNumber);
            Debug.Log("Unlocked Fase" + phaseNumber);
        }
    }

    #endregion

    #region Buttons

    public void LoadSceneAsync(string name)
    {
        if(OnLoadPanel != null)
        {
            OnLoadPanel();
        }
        //uiManager.loadPanel.SetActive(true);
        SceneManager.LoadSceneAsync(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GamePauseButton()
    {
        if (OnGamePause != null)
        {
            OnGamePause();
        }
    }
    #endregion
}
