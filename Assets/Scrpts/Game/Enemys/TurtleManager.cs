using System.Collections;
using UnityEngine;

public class TurtleManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerMechanic playerMechanic;
    [SerializeField] private Rigidbody rb;
    [Space(10)]
    [SerializeField] private float speed;


    bool pauseGame;
    float _initialSpeed;
    private void Start()
    {
        _initialSpeed = speed;
        gameManager.OnGamePause += GamePause;
        playerMechanic.playerDeath += GamePause; // I use this to save line of code
        playerMechanic.playerWinGame += GamePause; // I use this to save line of code
    }

    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        rb.linearVelocity = new Vector3(1 * speed, 0, 0);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }

    public void GamePause()
    {
        pauseGame = !pauseGame;
        if (pauseGame)
        {
            speed = 0;
        }
        else if (!pauseGame)
        {
            speed = _initialSpeed;
        }
    }
}
