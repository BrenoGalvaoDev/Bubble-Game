using UnityEngine;

public class EventManager : MonoBehaviour
{
    public PlayerMechanic playerMechanic;
    public UIManager uiManager;
    public GameManager gameManager;
    public SpawnPrefabs spawnPrefabs;

    void Start()
    {
        playerMechanic.playerDamage += uiManager.OnPlayerDamage;
        playerMechanic.playerDeath += gameManager.OnGameOver;
        playerMechanic.playerDeath += uiManager.OnGameOver;
        playerMechanic.playerWinGame += gameManager.OnWinGame;
        playerMechanic.playerWinGame += uiManager.OnWinGame;

        gameManager.OnGamePause += uiManager.OnPauseGame;
        gameManager.OnGamePause += playerMechanic.GamePause;
        gameManager.OnLoadPanel += uiManager.OnLoadPanelActive;

        gameManager.OnGamePause += spawnPrefabs.OnGamePause;
    }
}
