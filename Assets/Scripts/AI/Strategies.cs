using System;
using System.Collections.Generic;
using UnityEngine;
public interface IStrategy
{
    Node.Status Process();
    void Reset() { }
}

public class ActionStrategy : IStrategy
{
    readonly Action doSomething;

    public ActionStrategy(Action doSomething)
    {
        this.doSomething = doSomething;
    }

    public Node.Status Process()
    {
        doSomething();
        return Node.Status.Success;
    }
}

public class Condition : IStrategy
{
    readonly Func<bool> predicate;

    public Condition(Func<bool> predicate)
    {
        this.predicate = predicate;
    }

    public Node.Status Process() => predicate() ? Node.Status.Success : Node.Status.Failure;
}

public class MoveTowards : IStrategy
{

    readonly Transform entity;
    readonly float PatrolSpeed;
    Transform EnemyLocation;

    public MoveTowards(Transform entity, Transform enemyLoc, float PatrolSpeed = 2f)
    {
        this.entity = entity;
        EnemyLocation = enemyLoc;
        this.PatrolSpeed = PatrolSpeed;

    }

    public Node.Status Process()
    {

        if (entity == null || EnemyLocation == null)
            return Node.Status.Failure;

        if (EnemyLocation == null) return Node.Status.Success;

        var target = EnemyLocation;

        Vector3 targetPositionXYOnly = new Vector3(target.position.x, target.position.y, entity.position.z);

        entity.position = Vector2.MoveTowards(
        entity.position,
        targetPositionXYOnly,
        PatrolSpeed * Time.deltaTime
    );

        Vector2 direction = ((Vector2)targetPositionXYOnly - (Vector2)entity.position).normalized;

        float facingDirection = direction.x < 0 ? 1f : -1f;

        entity.localScale = new Vector3(
        Mathf.Abs(entity.localScale.x) * facingDirection,
        entity.localScale.y,
        entity.localScale.z
        );

        return Node.Status.Running;

    }


}

public class AttackSettlement : IStrategy
{
    readonly Func<bool> checkAttackHit;
    readonly float attackCooldown;
    private float lastAttackTime;
    private Animator ani;
    public AttackSettlement(Animator ani, Func<bool> checkAttackHit = null, float attackCooldown = 1.0f)
    {
        this.checkAttackHit = checkAttackHit;
        this.attackCooldown = attackCooldown;
        this.ani = ani;
    }

    public Node.Status Process()
    {
        if (ani == null) return Node.Status.Failure;

        if (checkAttackHit != null && !checkAttackHit())
        {
            // hitbox.enabled = false;
            return Node.Status.Failure;
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {

            ani.SetBool("Attack", true);
            lastAttackTime = Time.time;
        }

        return Node.Status.Running;
    }

    public void Reset()
    {
        if (ani == null) return;
        ani.SetBool("Attack", false);
        lastAttackTime = 0f;
    }
}

public class DeathAnimation : IStrategy
{
    readonly Func<bool> isDead;
    private Animator ani;
    public DeathAnimation(Animator ani, Func<bool> isDead = null)
    {
        this.ani = ani;
    }

    public Node.Status Process()
    {
        if (ani == null) return Node.Status.Failure;

        if (isDead())
        {

            ani.SetBool("Dead", true);

        }

        return Node.Status.Running;
    }
}
