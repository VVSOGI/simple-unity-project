using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Basic Attributes")]
    [SerializeField] private string enemyName;
    [SerializeField] private float totalHealth = 3;
    [SerializeField] private float timer;

    private float hitDuration = 1f;
    [SerializeField] private SpriteRenderer sr;

    public void TakeDamage(float damage)
    {
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
        Debug.Log(sr.name);
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            TurnWhite();
        }
    }

    private void TurnWhite()
    {
        sr.color = Color.white;
    }
}
