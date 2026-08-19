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

        AttackSettlementSeq.AddChild(new Leaf("IsEnemyClose", new Condition(isSettlementRightInFrontOfMe)));
        AttackSettlementSeq.AddChild(new Leaf("AttackEnemy", new AttackSettlement(animator, isSettlementRightInFrontOfMe)));
        actions.AddChild(AttackSettlementSeq);
    }
}
