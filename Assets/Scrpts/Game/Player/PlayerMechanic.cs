using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMechanic : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Space(20)]
    [Header("Player Speed")]
    [SerializeField] float heath;
    [SerializeField] float maxHeath;
    [SerializeField] float maxScale;
    [SerializeField] float speed;
    [SerializeField] float upSpeed = 0.01f;
    float _initialSpeed;
    float _initialUpSpeed;
    bool inPush;

    Vector2 _movement;
    Rigidbody _rb;
    bool invincible;
    bool pauseGame = false;

    [Space(20)]
    [SerializeField] float scaleChange;
    [SerializeField] AudioSource audioBubble;

    #region Events
    public delegate void PlayerDamage(float currentHealth, float maxHealth);
    public event PlayerDamage playerDamage;

    public delegate void PlayerDeath();
    public event PlayerDeath playerDeath;

    public delegate void WinGame();
    public event WinGame playerWinGame;
    #endregion
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        maxHeath = heath + 5;
        _initialSpeed = speed;
        _initialUpSpeed = upSpeed;
    }

    //In Unity6 Rigidbody.linearVelocity is new Rigidbody.velocity.
    private void Update()
    {        
        _rb.linearVelocity = new Vector3(_movement.x * speed, upSpeed, 0);
        DownScale();

        if (!pauseGame)
        {
            //this is to create an acceleration effect
            if (inPush)
            {
                upSpeed += Time.deltaTime;
            }
            else
            {
                if (upSpeed <= _initialUpSpeed)
                {
                    upSpeed = _initialUpSpeed;
                }
                else
                {
                    upSpeed -= Time.deltaTime * 3;
                }
            }
        }
    }

    public void SetInputDirection(InputAction.CallbackContext value)
    {
        _movement = value.ReadValue<Vector2>();
    }

    public void DownScale()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (heath > 0 && transform.localScale.x > 0.1)
            {
                heath--;
                speed += 0.1f;
                transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                _rb.AddForce(0, speed * 5, 0, ForceMode.Impulse);
                audioBubble.Play();
                StartCoroutine(InvincibleTime());

                //Call the event in UI
                if (playerDamage != null)
                {
                    playerDamage(heath, maxHeath);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Fish") && !invincible)
        {
            if(other.gameObject.GetComponent<FishManager>() != null)
            {
                other.gameObject.GetComponent<FishManager>().PlayerCollision();
            }
            if(heath > 0 && transform.localScale.x > 0.1)
            {
                heath--;
                audioBubble.Play();
                StartCoroutine(InvincibleTime());

                transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                speed += 0.1f;

                //Call the event in UI
                if (playerDamage != null)
                {
                    playerDamage(heath, maxHeath);
                }
            }
            else
            {
                //Call the event in GameManager and UIManager if the player life if < 0
                if(playerDeath != null)
                {
                    playerDeath();
                    GameOver();
                }
            }
        }

        if (other.CompareTag("Finish"))
        {
            if (playerWinGame != null)
            {
                playerWinGame();
            }
        }

        if (other.CompareTag("PushUP"))
        {
            //upSpeed = upSpeed * 5;
            inPush = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PushUP"))
        {
            inPush = false;
        }
    }

    IEnumerator InvincibleTime()
    {
        invincible = true;
        yield return new WaitForSeconds(2);
        invincible = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(!invincible)
        {
            heath++;
            speed -= 0.1f;
            if (heath > maxHeath) { heath = maxHeath; }

            if (transform.localScale.x <= maxScale)
            {
                transform.localScale += new Vector3(scaleChange, scaleChange, scaleChange);
            }

            //Call the event in UI
            if (playerDamage != null) { playerDamage(heath, maxHeath); }

            StartCoroutine(InvincibleTime());
        }
    }

    public void GamePause()
    {
        pauseGame = !pauseGame;
        if(pauseGame)
        {
            speed = 0;
            upSpeed = 0;
        }
        else if(!pauseGame)
        {
            speed = _initialSpeed;
            upSpeed = _initialUpSpeed;
        }
    }

    public void GameOver()
    {
        speed = 0;
        upSpeed = 0;
    }
}
