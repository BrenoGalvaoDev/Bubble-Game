using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerMechanic playerMechanic;

    Rigidbody rb;
    [Space(50)]
    [SerializeField] float speed = 1f;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float maxHeight = 2f;

    Vector3 targetPosition;
    bool pauseGame;
    float _initialSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetNewTargetPosition();

        _initialSpeed = speed;
        gameManager.OnGamePause += GamePause;
        playerMechanic.playerDeath += GamePause; // I use this to save line of code
        playerMechanic.playerWinGame += GamePause; // I use this to save line of code
    }

    private void Update()
    {
        MoveToTarget();
    }

    void MoveToTarget()
    {
        // Calcula a direção para o alvo e aplica velocidade
        Vector3 direction = (targetPosition - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        // Checa se o peixe alcançou o alvo
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetNewTargetPosition();
        }
        if(targetPosition.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void SetNewTargetPosition()
    {
        // Define uma nova posição aleatória dentro do limite de distância
        float randomX = Random.Range(-maxDistance, maxDistance);
        float randomY = Random.Range(-maxHeight, maxHeight);
        targetPosition = new Vector3(randomX, transform.position.y + randomY, 0);
    }

    public void PlayerCollision()
    {
        rb.AddForce(speed * 5, 0, 0, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish") && other.gameObject != gameObject)
        {
            PlayerCollision();
        }
        if (other.CompareTag("Respawn"))
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
