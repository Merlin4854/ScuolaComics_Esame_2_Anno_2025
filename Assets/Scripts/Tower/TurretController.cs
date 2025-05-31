using System.Linq;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private float range = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int level = 1;
    [SerializeField] private int cost = 50;
    [SerializeField] private int upgradeCost = 30;

    [Header("Damage")]
    [SerializeField] private float damageAmount = 1f;

    [Header("Object/Other")]
    [SerializeField] Transform cannonGraphics;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;

    private float fireCooldown = 0f;

    
    public float Range => range;
    public float FireRate => fireRate;
    public float DamageAmount => damageAmount;
    public int Level => level;
    public int Cost => cost;
    public int UpgradeCost => upgradeCost;

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        var enemies = System.Linq.Enumerable.Where(hits, h => h.GetComponent<EnemyController>() != null)
                                            .Select(h => h.transform)
                                            .ToList();

        if (enemies.Count == 0)
            return;

        Transform target = enemies.OrderBy(t => Vector2.Distance(transform.position, t.position)).First();

        Vector2 dir = (target.position - cannonGraphics.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cannonGraphics.rotation = Quaternion.Euler(0f, 0f, angle);

        if (fireCooldown <= 0f)
        {
            Shoot(dir);
            fireCooldown = 1f / fireRate;
        }
    }

    private void Shoot(Vector2 direction)
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        BaseProjectile proj = projObj.GetComponent<BaseProjectile>();
        if (proj != null)
        {
            proj.OnDestroyProjectile += () =>
            {
                Debug.Log("Projectile destroyed!");
            };
        }
    }

    public bool Upgrade()
    {
        if (GameManager.Instance.CurrentCoins >= upgradeCost)
        {
            if (GameManager.Instance.SpendCoins(upgradeCost))
            {
                level++;
                damageAmount *= 1.5f;
                fireRate *= 1.1f;
                range *= 1.1f;
                upgradeCost += 20;
                return true;
            }
        }
        return false;
    }


    public void Sell()
    {
        Debug.Log("Sellig Turret" + name);
        GameManager.Instance.AddCoins(cost / 2); // refund half
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
