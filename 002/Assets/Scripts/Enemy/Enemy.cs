using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Basic Attributes")]
    [SerializeField] private string enemyName;
    [SerializeField] private float totalHealth = 3;
    [SerializeField] private float timer;
    protected private bool isDeath = false;

    private float hitDuration = 1f;
    [SerializeField] private SpriteRenderer sr;

    public void TakeDamage(float damage)
    {
        if (isDeath) return;

        sr.color = Color.red;
        timer = hitDuration;
        totalHealth -= damage;
    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            TurnWhite();
        }

        if (totalHealth == 0)
        {
            isDeath = true;
        }
    }

    private void TurnWhite()
    {
        sr.color = Color.white;
    }
}
