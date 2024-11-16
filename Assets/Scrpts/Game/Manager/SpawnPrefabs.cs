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
    public float bubblesInterval;
    [Tooltip("The Vector.x is the min, and the Vector.y is the Max interval to spawn")]public Vector2 pushInterval;
    [Tooltip("The first time to spawn in game, next is random")]public float pushTime;

    bool gamePause = false;
    float counterFish = 0;
    float counterTs = 0;
    float counterBubbles = 0;
    float counterPush = 0;
    private Queue<GameObject> inactiveFish = new Queue<GameObject>();
    private Queue<GameObject> inactiveTS = new Queue<GameObject>();
    private Queue<GameObject> inactiveBubbles = new Queue<GameObject>();
    private Queue<GameObject> inactivePushes = new Queue<GameObject>();

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

        foreach (GameObject bb in gameManager.bubblesInstances)
        {
            inactiveBubbles.Enqueue(bb);
            bb.SetActive(false);
        }

        foreach (GameObject push in gameManager.pushInstances)
        {
            inactivePushes.Enqueue(push);
            push.SetActive(false);
        }
    }

    private void Update()
    {
        if (!gamePause)
        {
            counterFish += Time.deltaTime;
            counterTs += Time.deltaTime;
            counterBubbles += Time.deltaTime;
            counterPush += Time.deltaTime;

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

        if (counterBubbles >= bubblesInterval)
        {
            counterBubbles = 0;
            SpawnBubbles();
        }

        if (counterPush >= pushTime)
        {
            counterPush = 0;
            pushTime = Random.Range(pushInterval.x, pushInterval.y);
            SpawnPush();
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

    private void SpawnBubbles()
    {
        if (inactiveBubbles.Count > 0)
        {
            GameObject bb = inactiveBubbles.Dequeue();
            float posX = Random.Range(spawnLimits.x, spawnLimits.y);
            bb.transform.position = new Vector3(posX, transform.position.y - 5, transform.position.z);
            bb.SetActive(true);
            inactiveBubbles.Enqueue(bb);
        }
    }

    private void SpawnPush()
    {
        if (inactivePushes.Count > 0)
        {
            GameObject push = inactivePushes.Dequeue();
            float posX = Random.Range(spawnLimits.x, spawnLimits.y);
            push.transform.position = new Vector3(posX, transform.position.y, transform.position.z);
            push.SetActive(true);
            inactiveBubbles.Enqueue(push);
        }
    }


    public void OnGamePause()
    {
        gamePause = !gamePause;
    }
}
