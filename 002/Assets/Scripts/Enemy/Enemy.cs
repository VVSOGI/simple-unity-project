using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Basic Attributes")]
    [SerializeField] private float totalHealth = 3;

    private SpriteRenderer sr;
    private float hitDuration = 1;

    public void TakeDamage(float damage)
    {
        sr.color = Color.red;
        totalHealth -= damage;

        Invoke(nameof(TurnWhite), hitDuration);
    }

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (totalHealth == 0)
        {
            Debug.Log("Enemy is down");
        }
    }

    private void TurnWhite()
    {
        sr.color = Color.white;
    }
}
