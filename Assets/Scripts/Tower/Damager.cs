using UnityEngine;

public class Damager : MonoBehaviour
{
    private float damageAmount = 1f;

    public void SetDamage(float amount)
    {
        damageAmount = amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.TakeDamage(damageAmount);
        }
    }
}
