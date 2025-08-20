using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isKnockBack;

    [Header("Enemy Basic Attributes")]
    [SerializeField] private string enemyName;
    [SerializeField] private float totalHealth = 3;
    [SerializeField] private float timer;
    protected private bool isDeath = false;

    [SerializeField] private float hitDuration = 1f;
    [SerializeField] private SpriteRenderer sr;

    private EnemyPhysicsEffect enemyPhysicsEffect;

    IEnumerator KnockbackProcess(Direction direction)
    {
        isKnockBack = true;
        yield return StartCoroutine(enemyPhysicsEffect.SmoothKnockback(direction));
        isKnockBack = false;
    }

    public void TakeDamage(float damage, Direction direction)
    {
        if (isDeath) return;

        sr.color = Color.red;
        timer = hitDuration;
        totalHealth -= damage;

        if (direction == Direction.Left)
        {
            StartCoroutine(KnockbackProcess(direction));
        }

        if (direction == Direction.Right)
        {
            StartCoroutine(KnockbackProcess(direction));
        }
    }

    protected virtual void Start()
    {
        enemyPhysicsEffect = GetComponent<EnemyPhysicsEffect>();
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
