using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject
        camera,
        deathHeadParticle,
        deathWeaponParticle,
        deathBloodParticle;

    private float currentHealth;

    public HealthBar healthBar;

    private void Start()
    {

        healthBar.GetComponent<HealthBar>();
        currentHealth = maxHealth;
        Debug.Log("BossHealth is " + currentHealth);
        healthBar.SetMaxHealth(maxHealth);

    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        
        healthBar.SetHealth(currentHealth);
        Debug.Log("BossHealth after attack is " + currentHealth);

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
         Instantiate(deathHeadParticle, transform.position, deathHeadParticle.transform.rotation);
        Instantiate(deathWeaponParticle, transform.position, deathWeaponParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
         Instantiate(camera, transform.position, camera.transform.rotation);
    }
}
