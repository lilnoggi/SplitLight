using UnityEngine;

public class PillarSmall : MonoBehaviour
{
    public float detectionRadius = 3f;
    public float attackCooldown = 2f;
    public Transform player;
    public LayerMask playerLayer;
    public Transform attackCheck;
    public float attackRange = 1.5f;

    private Animator animator;
    private bool isAttacking = false;
    private float attackTimer = 0f;

    private enum State { Asleep, Awakening, Awake, Sleeping }
    private State currentState = State.Asleep;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Asleep:
                if (distance < detectionRadius)
                {
                    animator.Play("Pillar_Small_Awaken");
                    currentState = State.Awakening;
                }
                break;

            case State.Awakening:
                // Let animation event or transition handle this
                break;

            case State.Awake:
                if (distance > detectionRadius)
                {
                    animator.Play("Pillar_Small_Sleep");
                    currentState = State.Sleeping;
                }
                else
                {
                    TryAttackPlayer();
                }
                break;

            case State.Sleeping:
                if (distance < detectionRadius)
                {
                    animator.Play("Pillar_Small_Awaken");
                    currentState = State.Awakening;
                }
                break;
        }

        attackTimer -= Time.deltaTime;

        if (currentState == State.Awake || currentState == State.Awakening)
        {
            FlipTowardsPlayer();
        }

    }

    // Called via animation event or transition
    public void OnAwakened()
    {
        animator.Play("Pillar_Small_Idle_Awake");
        currentState = State.Awake;
    }

    public void OnSleepFinished()
    {
        animator.Play("Pillar_Small_Idle_Asleep");
        currentState = State.Asleep;
    }

    private void TryAttackPlayer()
    {
        if (attackTimer > 0f || isAttacking)
            return;

        // Check if player is in front (not on top!)
        Collider2D hit = Physics2D.OverlapCircle(attackCheck.position, attackRange, playerLayer);
        if (hit != null)
        {
            isAttacking = true;
            animator.Play("Pillar_Small_Attack");
            animator.SetTrigger("Attack");
            attackTimer = attackCooldown;
        }
    }

    // Called via animation event at the moment of damage
    public void DamagePlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackCheck.position, attackRange, playerLayer);
        if (hit != null)
        {
            PlayerLantern playerLantern = hit.GetComponent<PlayerLantern>();
            if (playerLantern != null)
            {
                playerLantern.TakeDamage(20f);
                Debug.Log("PillarSmall dealt 20 damage to the player!");
            }
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
        animator.Play("Pillar_Small_Idle_Awake");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCheck.position, attackRange);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    
    private void FlipTowardsPlayer()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;

        if (player.position.x < transform.position.x)
        {
            // Player is to the left
            scale.x = Mathf.Abs(scale.x); // make sure it's facing left
        }
        else
        {
            // Player is to the right
            scale.x = -Mathf.Abs(scale.x); // face right
        }

    transform.localScale = scale;
}

}
