using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject melee;
    bool isAttacking = false;
    public float attackDuration = 0.3f;
    public float attackCooldown = 0.1f;
    public float damage = 5;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy hit");
    }

    public void Attack(Vector2 direction)
    {
        StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        if(!isAttacking)
        {
            melee.SetActive(true);
            isAttacking = true;
            yield return new WaitForSeconds(attackDuration);
            melee.SetActive(false);
            isAttacking = false;
            yield return new WaitForSeconds(attackCooldown);
        }
    }
}
