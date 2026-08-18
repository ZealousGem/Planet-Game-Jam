using UnityEngine;

public class BaseEnemy : MonoBehaviour
{

    private Transform Target;
    public float Speed;
    protected float attackRange = 0.5f;
    protected bool EnemyFound = false;
    protected BehaviourTree tree;
    private bool isReady = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void SetUpTree()
    {

        isReady = true;

        tree = new BehaviourTree("Enemy");
        PriortySelector actions = new PriortySelector("Enemy Logic");

        BasicEnemyAI(actions);

        Leaf Patrol = new Leaf("Patrol", new MoveTowards(transform, Target, 6f));
        actions.AddChild(Patrol);

        // actions.AddChild(actions);
        tree.AddChild(actions);
    }

    protected virtual void BasicEnemyAI(PriortySelector actions) { }

    // Update is called once per frame
    void Update() { if (!isReady) return; tree.Process(); }
}
