using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTower : TowerObject
{
    public GameObject bulletPrefab;
    public Bullet bulletReference;
    public float attackDelay = 0.1f; // Delay between attacks in seconds

    private Queue<bool> attackQueue = new Queue<bool>();
    private bool isAttacking = false;

    public override void Attack()
    {
        attackQueue.Enqueue(true);
        if (!isAttacking)
        {
            StartCoroutine(ProcessAttackQueue());
        }
    }

    private IEnumerator ProcessAttackQueue()
    {
        isAttacking = true;

        while (attackQueue.Count > 0)
        {
            attackQueue.Dequeue();

            // Perform the actual attack
            bulletReference = Instantiate(bulletPrefab, transform).GetComponent<Bullet>();
            bulletReference.damage = Power + bonusPower;

            // Wait for the specified delay
            yield return new WaitForSeconds(attackDelay);
        }

        isAttacking = false;
    }
}
