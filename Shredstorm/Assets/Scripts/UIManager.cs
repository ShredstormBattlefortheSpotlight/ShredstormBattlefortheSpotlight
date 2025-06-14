using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float gameDuration = 300f; // 5 minutes in seconds

    private float timeRemaining;
    private bool timerRunning = true;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI enemyCountText;

    private Player player;
    
    void Start()
    {
        timeRemaining = gameDuration;
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (player == null) return;

        healthText.text = $"Health: {player.CurrentHealth}/{player.MaximumHealth}";
        damageText.text = $"Damage: {player.Damage}";
        xpText.text = $"XP: {player.Experience}/{player.ExperienceRequired}";
        enemyCountText.text = $"Enemies: {GameObject.FindGameObjectsWithTag("Enemy").Length}";
        
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerRunning = false;
            timerText.text = "Time's up!";
            // TODO: Trigger boss or final phase
        }
        else
        {
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }
}