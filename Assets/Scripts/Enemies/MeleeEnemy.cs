using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    protected override void BasicEnemyAI(PriortySelector actions)
    {
        //base.BasicEnemyAI(actions);
        Sequence AttackSettlementSeq = new Sequence("Attack Enemy", 80);

        bool isSettlementRightInFrontOfMe()
        {
            if (Target == null) return false;

            if (Target.TryGetComponent(out Collider2D settlementCollider))
            {
                // Gets the point on the Settlement collider closest to the enemy center
                Vector3 closestPoint = settlementCollider.ClosestPoint(transform.position);
                float distance = Vector3.Distance(transform.position, closestPoint);
                return distance <= attackRange;
            }

            // Fallback if no collider is attached
            return Vector3.Distance(transform.position, Target.position) <= attackRange;
        }

        Settlement testSettleMent()
        {
            if (Target == null) return null;

            if (Target.gameObject.TryGetComponent(out Settlement settlement))
            {
                return settlement;
            }

            else
            {
                Target = null;
                Debug.Log("no settlements");
                return null;
            }
        }

        AttackSettlementSeq.AddChild(new Leaf("IsEnemyClose", new Condition(isSettlementRightInFrontOfMe)));
        // AttackSettlementSeq.AddChild(new Leaf("AttackEnemy", new AttackSettlement(animator, isSettlementRightInFrontOfMe)));
        AttackSettlementSeq.AddChild(new Leaf("AttackEnemy", new AttackSettlement(animator, isSettlementRightInFrontOfMe, testSettleMent)));
        actions.AddChild(AttackSettlementSeq);
    }

    public virtual void OnHitFrame()
    {
        if (Target != null && Target.TryGetComponent(out Settlement settlement))
        {
            settlement.DamageTower(Damage);
        }
    }
}
