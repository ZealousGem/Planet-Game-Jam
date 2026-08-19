using System;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{

    [SerializeField] protected Transform Target;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] protected Animator animator;
    public float EnemyHealth = 100f;
    public float Speed = 5f;
    public float Damage = 60f;
    protected float attackRange = 0.5f;
    protected bool SettleMentFound = false;
    protected BehaviourTree tree;
    private bool isReady = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // protected virtual void Awake()
    // {
    //     SetUpTree();
    // }

    public virtual void Initialise(Transform targetTransform)
    {
        InstatiateTarget(targetTransform);
        SetUpTree();

    }

    protected virtual void SetUpTree()
    {
        isReady = true;

        tree = new BehaviourTree("Enemy");
        PriortySelector actions = new PriortySelector("Enemy Logic");

        BasicEnemyAI(actions);

        Sequence MovingToTarget = new Sequence("finding target", 50);

        bool SettlementisSeen()
        {
            if (Target != null) return true;

            // Target is null, attempt to re-acquire right here
            FindNewSettlement();

            return Target != null; // Returns true if a new target was successfully found
        }

        MovingToTarget.AddChild(new Leaf("IsEnemyClose", new Condition(SettlementisSeen)));
        MovingToTarget.AddChild(new Leaf("Patrol", new MoveTowards(transform, Target, 6f)));
        // Leaf Patrol = new Leaf("Patrol", new MoveTowards(transform, Target, 6f));
        actions.AddChild(MovingToTarget);

        // actions.AddChild(actions);
        tree.AddChild(actions);
    }

    protected virtual void BasicEnemyAI(PriortySelector actions)
    {
        Sequence DeathSequnce = new Sequence("Death Logic", 100);

        bool HasEnemyDied()
        {
            if (EnemyHealth <= 0)
            {
                EnemyHealth = 0;
                return true;
            }

            else
            {
                return false;
            }

        }

        DeathSequnce.AddChild(new Leaf("IsEnemyClose", new Condition(HasEnemyDied)));
        DeathSequnce.AddChild(new Leaf("AttackEnemy", new DeathAnimation(animator, HasEnemyDied)));
        actions.AddChild(DeathSequnce);
    }

    private void FindNewSettlement()
    {
        // Layer mask search (e.g., Physics2D.OverlapCircleAll or Physics.OverlapSphere)
        Collider[] settlements = Physics.OverlapSphere(transform.position, 100f, targetLayerMask);

        if (settlements.Length > 0)
        {
            // Pick the closest settlement or first found
            Target = settlements[0].transform;
        }
    }

    private void InstatiateTarget(Transform transform) => Target = transform;
    // Update is called once per frame
    private void Update() { if (!isReady) return; tree.Process(); }
}
