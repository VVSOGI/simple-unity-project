using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    [Header("Enemy Basic Attributes")]
    [SerializeField] private string enemyName;
    [SerializeField] private float totalHealth = 3;
    [SerializeField] private float timer;
    [SerializeField] private float hitDuration = 1f;
    [SerializeField] private SpriteRenderer sr;

    public bool isCanMove = true;
    protected private bool isDeath = false;
    protected private bool isKnockBack = false;
    protected private Animator animator;

    private EnemyPhysicsEffect enemyPhysicsEffect;

    IEnumerator KnockbackProcess(Direction direction)
    {
        isKnockBack = true;
        animator.SetBool("IsKnockBack", true);
        yield return StartCoroutine(enemyPhysicsEffect.SmoothKnockback(direction));

        if (!isCanMove)
        {
            StartMove();
        }

        isKnockBack = false;
        animator.SetBool("IsKnockBack", false);
    }

    public abstract void StartMove();
    public abstract void StopMove();

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
        animator = GetComponentInChildren<Animator>();
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
