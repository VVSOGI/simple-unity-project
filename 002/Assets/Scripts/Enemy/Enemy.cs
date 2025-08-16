using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Basic Attributes")]
    [SerializeField] private float totalHealth = 3;

    public void TakeDamage(float damage)
    {
        totalHealth -= damage;
    }

    private void Update()
    {

    }
}
