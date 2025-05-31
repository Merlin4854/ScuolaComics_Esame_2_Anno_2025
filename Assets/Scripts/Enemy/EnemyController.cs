using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameEvents
{
    public static System.Action<int> OnBaseDamaged;
}

public class EnemyController : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] List<Transform> pathPoints;
    private int currentPointIndex = 0;

    [Header("Movement")]
    [SerializeField] float speed = 2f;

    [Header("Health")]
    [SerializeField] float maxHealth = 10f;
    private float currentHealth;
    [SerializeField] GameObject canvasLife;
    [SerializeField] Image lifeBar;

    [Header("Damage")]
    [SerializeField] int damageToPlayer = 1;

    [Header("Graphics")]
    [SerializeField] SpriteRenderer graphicsObject;

    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D component missing from EnemyController object.");

        if (canvasLife != null)
            canvasLife.SetActive(false);
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        currentPointIndex = 0;
        if (canvasLife != null)
        {
            canvasLife.SetActive(false);
            lifeBar.fillAmount = 1f;
        }
    }

    private void FixedUpdate()
    {
        FollowPath();
    }

    private void FollowPath()
    {
        if (pathPoints == null || pathPoints.Count == 0) return;

        Vector2 currentPosition = rb.position;
        Vector2 targetPoint = pathPoints[currentPointIndex].position;
        Vector2 direction = (targetPoint - currentPosition).normalized;

        Vector2 newPos = currentPosition + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        UpdateGraphicsRotation(direction);

        if (Vector2.Distance(newPos, targetPoint) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex >= pathPoints.Count)
                ReachExit();
        }
    }

    private void UpdateGraphicsRotation(Vector2 direction)
    {
        if (graphicsObject == null) return;

        float angle;
        bool horizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);

        if (horizontal)
        {
            if (direction.x > 0f)
            {
                angle = 90f;
                graphicsObject.flipY = true;
            }
            else
            {
                angle = -90f;
                graphicsObject.flipY = true;
            }
        }
        else
        {
            if (direction.y > 0f)
            {
                angle = 0f;
            }
            else
            {
                angle = 180f;
            }
            graphicsObject.flipY = false;
        }

        graphicsObject.transform.localEulerAngles = new Vector3(0f, 0f, angle);
    }

    private void ReachExit()
    {
        GameEvents.OnBaseDamaged?.Invoke(damageToPlayer);
        Die();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (canvasLife != null && !canvasLife.activeSelf)
            canvasLife.SetActive(true);

        if (lifeBar != null)
            lifeBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0f) Die();
    }

    private void Die()
    {
        // pooling reuse
        currentHealth = maxHealth;
        currentPointIndex = 0;

        if (canvasLife != null)
        {
            canvasLife.SetActive(false);
            lifeBar.fillAmount = 1f;
        }

        gameObject.SetActive(false);
    }
}
