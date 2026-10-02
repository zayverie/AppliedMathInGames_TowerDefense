using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Health Bar Fill")]
    public Image healthFillImage;
    public Image ghostHpFillImage;

    [Header("Ghost Bar")]
    [SerializeField] private float ghostHpFillDuration = 0.6f; 
    [SerializeField] private float ghostHpDelay = 0.35f;

    private float startGhostFill = 1f;
    private float targetFill = 1f;
    private float elapsedTime = 0f;
    private float delayTimer = 0f;
    public TextMeshProUGUI healthText; // Optional: shows "15 / 20"

    public void Update()
    {
        if (delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;
            return; // Exit so the ghost bar stays completely still
        }
        
        if(ghostHpFillImage != null && elapsedTime < ghostHpFillDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / ghostHpFillDuration); 

            float easedT = Ease.EaseOutCubic(t);

            ghostHpFillImage.fillAmount = startGhostFill + (targetFill - startGhostFill) * easedT;

        }
    }
    public void UpdateHealthBar(float currentHp, float maxHp)
    {
        targetFill = currentHp / maxHp;
        if (healthFillImage != null)
        {
            // fillAmount expects a 0.0 to 1.0 range
            healthFillImage.fillAmount = Mathf.Clamp01(currentHp / maxHp);
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.Max(0, currentHp)} / {maxHp}";
        }

        if (ghostHpFillImage != null)
        {
            startGhostFill = ghostHpFillImage.fillAmount;
            elapsedTime = 0f;
            delayTimer = ghostHpDelay; // Start the pause countdown
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
        }
    }
}
