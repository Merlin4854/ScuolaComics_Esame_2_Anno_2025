using System;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 1f;
    [SerializeField] private float lifetime = 5f;

    public Action OnDestroyProjectile;

    private float timer;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * transform.up;

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            DestroyProjectile();
        }
    }

    public void DestroyProjectile()
    {
        OnDestroyProjectile?.Invoke();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyProjectile();
    }
}
