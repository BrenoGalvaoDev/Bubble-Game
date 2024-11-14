using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPrefabs : MonoBehaviour
{
    public GameManager gameManager;
    public Vector2 spawnLimits;
    public float fishInterval;
    public float tsInterval;

    bool gamePause = false;
    float counterFish = 0;
    float counterTs = 0;
    private Queue<GameObject> inactiveFish = new Queue<GameObject>();
    private Queue<GameObject> inactiveTS = new Queue<GameObject>();

    private void Start()
    {
        // Inicializa a fila de objetos inativos
        foreach (GameObject fish in gameManager.fishInstances)
        {
            inactiveFish.Enqueue(fish);
            fish.SetActive(false);
        }

        foreach (GameObject tS in gameManager.tsInstances)
        {
            inactiveTS.Enqueue(tS);
            tS.SetActive(false);
        }
    }

    private void Update()
    {
        if (!gamePause)
        {
            counterFish += Time.deltaTime;
            counterTs += Time.deltaTime;

        }
        if (counterFish >= fishInterval)
        {
            counterFish = 0;
            SpawnFish();
        }

        if (counterTs >= tsInterval)
        {
            counterTs = 0;
            SpawnTS();
        }
    }

    private void SpawnFish()
    {
        if (inactiveFish.Count > 0)
        {
            GameObject fish = inactiveFish.Dequeue();
            float posX = Random.Range(spawnLimits.x, spawnLimits.y);
            fish.transform.position = new Vector3(posX, transform.position.y, transform.position.z);
            fish.SetActive(true);
            inactiveFish.Enqueue(fish); // Retorna o objeto à fila após uso
        }
    }

    private void SpawnTS()
    {
        if (inactiveTS.Count > 0)
        {
            GameObject tS = inactiveTS.Dequeue();
            float posX = Random.Range(spawnLimits.x, spawnLimits.y);
            tS.transform.position = new Vector3(posX, transform.position.y - 2, transform.position.z);
            tS.SetActive(true);
            inactiveTS.Enqueue(tS);
        }
    }

    public void OnGamePause()
    {
        gamePause = !gamePause;
    }
}
