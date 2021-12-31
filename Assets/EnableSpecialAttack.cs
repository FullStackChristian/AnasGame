using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSpecialAttack : MonoBehaviour
{
    public GameObject player;
    private SpecialAttack specialAttackTrigger;

    // Update is called once per frame
    void Update()
    {
       specialAttackTrigger =  FindObjectOfType<SpecialAttack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        specialAttackTrigger.combatEnabled = true;
    }
}
