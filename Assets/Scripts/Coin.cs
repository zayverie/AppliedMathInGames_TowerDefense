using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 100f; // Spin animation
    public float duration = 0.6f;       // Flight duration

    private Vector3 startPos;
    private Transform uiTarget;
    private UIManager uiManager;
    private int coinValue = 1;
    private float elapsedTime = 0f;
    private bool isFlying = false;

    public void Setup(Transform target, UIManager manager, int value)
    {
        startPos = transform.position;
        uiTarget = target;
        uiManager = manager;
        coinValue = value;
        isFlying = true;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (!isFlying || uiTarget == null) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        float easedT = Ease.EaseInQuad(t);

        transform.position = startPos + (uiTarget.position - startPos) * easedT;

        if (t >= 1f)
        {
            if (uiManager != null)
            {
                uiManager.OnCoinArrived(coinValue);
            }
            Destroy(gameObject);
        }
    }
}