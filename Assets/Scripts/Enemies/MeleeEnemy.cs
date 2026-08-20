using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    protected override void BasicEnemyAI(PriortySelector actions)
    {
        base.BasicEnemyAI(actions);
        Sequence AttackSettlementSeq = new Sequence("Attack Enemy", 80);

        bool isSettlementRightInFrontOfMe()
        {
            if (Target == null) return false;

            float distance = Vector3.Distance(transform.position, Target.position);
            return distance <= attackRange;
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
        AttackSettlementSeq.AddChild(new Leaf("AttackEnemy", new AttackSettlement(isSettlementRightInFrontOfMe, testSettleMent)));
        actions.AddChild(AttackSettlementSeq);
    }

    public void OnHitFrame()
    {
        if (transform != null && Target.TryGetComponent(out Settlement settlement))
        {
            settlement.DamageTower(Damage);
        }
    }
}
