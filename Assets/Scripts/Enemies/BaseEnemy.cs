
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : MonoBehaviour
{
    [Header("LayerMask of Target")]
    [SerializeField] private LayerMask targetLayerMask;

    [Header("Animations")]
    [SerializeField] protected Animator animator;

    [Header("HealthUI")]
    [SerializeField] protected Image HealthBar;
    [SerializeField] protected Transform Target;

    [Header("Stats")]
    public float EnemyHealth = 100f;
    public float Speed = 5f;
    public float Damage = 60f;

    [Header("Attack Range")]
    [SerializeField] protected float attackRange = 0.5f;

    [Header("PopUpUI")]
    [SerializeField] private GameObject PopUp;

    protected bool SettleMentFound = false;
    protected BehaviourTree tree;
    private bool isReady = false;
    private bool isDead = false;
    private float MaxEnemyHealth = 0;

    public void DamageEnemy(float Damage)
    {
        if (isDead) return;
        EnemyHealth -= Damage;

        if (!HealthBar.gameObject.activeSelf && EnemyHealth != 0)
        {
            HealthBar.gameObject.SetActive(true);
        }

        HealthBar.fillAmount = EnemyHealth / MaxEnemyHealth;
        DamagePopUp.CreatePopUp(PopUp, transform, (int)Damage);

        if (EnemyHealth <= 0)
        {
            isDead = true;
            EnemyHealth = 0;
            HealthBar.gameObject.SetActive(false);

            EventBus.Act(new EnemiesKilledEvent(1));
            StartCoroutine(KillEnemy());

        }
    }

    public IEnumerator KillEnemy()
    {
        isReady = false;

        yield return new WaitForSeconds(1f);

        tree?.Reset();
        Destroy(gameObject);
    }

    public virtual void Initialise(Transform targetTransform)
    {
        InstatiateTarget(targetTransform);

        MaxEnemyHealth = EnemyHealth;
        if (HealthBar != null)
        {
            HealthBar.gameObject.SetActive(false);
            HealthBar.fillAmount = EnemyHealth / MaxEnemyHealth;
        }

        SetUpTree();

    }

    protected virtual void SetUpTree()
    {
        tree = new BehaviourTree("Enemy");
        PriortySelector actions = new PriortySelector("Enemy Logic");

        Sequence MovingToTarget = new Sequence("finding target", 50);

        bool SettlementisSeen()
        {
            if (Target != null) return true;
            return FindNewSettlement(); // Returns true if a new target was successfully found
        }

        MovingToTarget.AddChild(new Leaf("IsEnemyClose", new Condition(SettlementisSeen)));
        MovingToTarget.AddChild(new Leaf("Patrol", new MoveTowards(transform, () => Target, Speed)));
        // Leaf Patrol = new Leaf("Patrol", new MoveTowards(transform, Target, 6f));
        actions.AddChild(MovingToTarget);

        BasicEnemyAI(actions);

        // actions.AddChild(actions);
        tree.AddChild(actions);
        isReady = true;
    }

    protected virtual void BasicEnemyAI(PriortySelector actions)
    {
        Sequence DeathSequnce = new Sequence("Death Logic", 100);

        bool HasEnemyDied()
        {
            if (EnemyHealth <= 0)
            {
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

    private bool FindNewSettlement()
    {
        // Layer mask search (e.g., Physics2D.OverlapCircleAll or Physics.OverlapSphere)
        Collider2D[] settlements = Physics2D.OverlapCircleAll(transform.position, 100f, targetLayerMask);

        if (settlements.Length > 0)
        {
            // Pick the closest settlement or first found
        Transform nearest = null;
        float shortestDistanceSqr = Mathf.Infinity;

         for (int i = 0; i < settlements.Length; i++)
         {
            if (settlements[i] == null) continue;

            float distanceSqr = (transform.position - settlements[i].gameObject.transform.position).sqrMagnitude;
            if (distanceSqr < shortestDistanceSqr)
            {
                shortestDistanceSqr = distanceSqr;
                nearest = settlements[i].gameObject.transform;
            }
         }
            
            if(nearest == null) return false;
            Target = nearest;
            return true;
        }

        Target = null;
        return false;

    }

    private void InstatiateTarget(Transform transform) => Target = transform;
    // Update is called once per frame
    private void Update() { if (!isReady) return; tree.Process(); }
}
