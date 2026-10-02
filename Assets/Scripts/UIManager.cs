using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject gameplayUI;

    [Header("Health Bar Fill")]
    public Image healthFillImage;
    public Image ghostHpFillImage;

    [Header("Ghost Bar")]
    [SerializeField] private float ghostHpFillDuration = 0.6f; 
    [SerializeField] private float ghostHpDelay = 0.2f;

    private float startGhostFill = 1f;
    private float targetFill = 1f;
    private float elapsedTime = 0f;
    private float delayTimer = 0f;

    // public TextMeshProUGUI healthText; 

    [Header("Coins")]
    public RectTransform coinIcon;     
    public TextMeshProUGUI coinsText;  

    private float displayedCoins = 0f;
    private float targetCoins = 0f;
    private float punchTimer = 0f;

    public void Update()
    {
        if (delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;
        }
        else if (ghostHpFillImage != null && elapsedTime < ghostHpFillDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / ghostHpFillDuration); 

            float easedT = Ease.EaseOutCubic(t);

            ghostHpFillImage.fillAmount = startGhostFill + (targetFill - startGhostFill) * easedT;
        }

        if (displayedCoins < targetCoins)
        {
            displayedCoins += 25f * Time.deltaTime;
            if (displayedCoins > targetCoins) displayedCoins = targetCoins;

            if (coinsText != null)
            {
                coinsText.text = Mathf.RoundToInt(displayedCoins).ToString();
            }
        }

        if (punchTimer > 0f)
        {
            punchTimer -= Time.deltaTime;
            if (coinIcon != null) coinIcon.localScale = Vector3.one * 1.35f;
        }
        else
        {
            if (coinIcon != null) coinIcon.localScale = Vector3.one;
        }
    }

    public void OnCoinArrived(int addedValue)
    {
        targetCoins += addedValue;
        punchTimer = 0.2f;

        Player player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            player.coins = (int)targetCoins;
        }
    }

    public void UpdateHealthBar(float currentHp, float maxHp)
    {
        targetFill = currentHp / maxHp;
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = Mathf.Clamp01(currentHp / maxHp);
        }

        // if (healthText != null)
        // {
        //     healthText.text = $"{Mathf.Max(0, currentHp)} / {maxHp}";
        // }

        if (ghostHpFillImage != null)
        {
            startGhostFill = ghostHpFillImage.fillAmount;
            elapsedTime = 0f;
            delayTimer = ghostHpDelay; 
        }
    }

    public void WinGame()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    public void LoseGame()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            gameplayUI.SetActive(false);
            Time.timeScale = 0f;
        }
    }
    
    public void UpdateCoinDisplay(int currentCoins)
    {
        if (coinsText != null)
        {
            coinsText.text = currentCoins.ToString();
        }   
    }
}