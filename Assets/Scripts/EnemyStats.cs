using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private float scoreAddAmount = 10;

    GameController gameController;
    Spawner spawn;

    public float currHealth;
    public float maxHealth;

    public bool isDead = false;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        spawn = gameController.GetComponentInChildren<Spawner>();

        maxHealth = 100;
        currHealth = maxHealth;
    }

    private void Update()
    {
        CheckHealth();
    }

    public virtual void CheckHealth()
    {
        if (currHealth >= maxHealth)
        {
            currHealth = maxHealth;
        }
        if (currHealth <= 0)
        {
            currHealth = 0;
            isDead = true;
            Die();
        }
    }

    public virtual void Die()
    {
        gameController.AddScore(scoreAddAmount);
        spawn.enemiesKilled++;
        if(spawn.enemiesKilled >= spawn.enemySpawnAmount)
        {
            spawn.NextWave();
        }
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
    }
}
