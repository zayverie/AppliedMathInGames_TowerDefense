using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum CurveType
    {
        Quadratic,
        Cubic
    }
    public CurveType curveType;
    public Vector3 initialPosition, p1, p2, p3; // Control points for the curve
    public float velocity = 5f;
    public EnemySpawner enemySpawner;
    public Player playerScript;
    public float duration = 10f; // Duration to traverse the curve
    public float elapsedTime = 0f; // Time elapsed since the start of the movement

    public GameObject coinPrefab; 

    public int coinsReward = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(enemySpawner == null)
        {
            enemySpawner = FindAnyObjectByType<EnemySpawner>();
        }

        if(playerScript == null)
        {
            playerScript = FindAnyObjectByType<Player>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration); 

        transform.position = curveType == CurveType.Quadratic
            ? Ease.QuadraticBezier(initialPosition, p1 * 1.8f, p2, t)
            : Ease.CubicBezier(initialPosition, p1 * 3.6f, p2 * 1.8f, p3, t);

        bool reachedGoal = t >= 1f;

        if (!reachedGoal && enemySpawner != null && enemySpawner.sharedPoint != null)
        {
            reachedGoal = (transform.position - enemySpawner.sharedPoint.position).sqrMagnitude < 0.25f; 
        }

        if (reachedGoal)
        {
            if (playerScript != null)
            {
                playerScript.TakeDamage(1f);
            }
            Destroy(gameObject);
        }
    }

    public void EnemyDies()
    {
        if (coinPrefab != null)
        {
            GameObject coinObj = Instantiate(coinPrefab, transform.position, transform.rotation);
            Coin coin = coinObj.GetComponent<Coin>();
            UIManager ui = FindAnyObjectByType<UIManager>();

            if (coin != null && ui != null && ui.coinIcon != null)
            {
                coin.Setup(ui.coinIcon, ui, coinsReward);
            }
            else if (ui == null || ui.coinIcon == null)
            {
                Debug.LogWarning("Coin could not start because UIManager.coinIcon is not assigned.", this);
            }
        }
        Destroy(gameObject);
    }
    public void OnDrawGizmos()
    {
        var previousLine = initialPosition;
        for(int i = 0; i <= 10; i++)
        {
            var t = i / 10f;
            var currentLine = curveType == CurveType.Quadratic
                ? Ease.QuadraticBezier(initialPosition, p1 * 1.8f, p2, t)
                : Ease.CubicBezier(initialPosition, p1 * 3.6f, p2 * 1.8f, p3, t);
            Gizmos.DrawLine(previousLine, currentLine);
            previousLine = currentLine;
        }
    }
}