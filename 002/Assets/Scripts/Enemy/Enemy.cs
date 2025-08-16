using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float timer;

    [Header("Enemy Basic Attributes")]
    [SerializeField] private float totalHealth = 3;

    private SpriteRenderer sr;
    private float hitDuration = 1f;

    public void TakeDamage(float damage)
    {
        sr.color = Color.red;
        timer = hitDuration;
        totalHealth -= damage;
    }

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0 && sr.color != Color.white)
        {
            TurnWhite();
        }
    }

    private void TurnWhite()
    {
        sr.color = Color.white;
    }
}
