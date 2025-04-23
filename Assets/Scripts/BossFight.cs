using System.Collections;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    private BossState currentState;

    private Coroutine movePatterns;

    private int damage;
    private float moveSpeed = 2;


    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject handPrefab;


    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Health hp;



    private bool isLeft;
    private bool isAttacking;
    private bool isdead;
    private bool isPhase2;
    private bool isEvolving;


    private void Awake()
    {
        isdead = false;
        isAttacking = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        hp = GetComponent<Health>();
    }

    void Start()
    {
        currentState = BossState.Spawn;
        HandleState();
        movePatterns = StartCoroutine(MovePattern());
    }


    /// <summary>
    /// handle inputs
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking && !isdead && !isEvolving)
        {
            currentState = BossState.Attack;
            HandleState();
            animator.SetBool("isShootLaser", true);
            StartCoroutine(ShootLaser());
        }

        if (Input.GetKeyDown(KeyCode.S) && !isAttacking && !isdead && !isEvolving) 
        {
            currentState = BossState.Attack;
            HandleState();
            animator.SetBool("isShootHand", true);
            StartCoroutine(PillarHandAttack());
        }
    }


    /// <summary>
    /// hanlde transitions between states
    /// </summary>
    private void HandleState()
    {
        switch (currentState)
        {
            case (BossState.Idle):
                movePatterns = StartCoroutine(MovePattern());
                Debug.Log("idle state");
                break;

            case (BossState.Spawn):
                Debug.Log("spawn state");
                break;

            case (BossState.Attack):
                isAttacking = true;
                StopCoroutine(movePatterns);
                rb.velocity = Vector2.zero;
                Debug.Log("attak state");
                break;

            case (BossState.Death):
                isdead = true;
                StopAllCoroutines();
                rb.velocity = Vector2.zero;
                Debug.Log("death state");
                break;

            case (BossState.Phase2):
                isPhase2 = true;
                isEvolving = true;
                animator.speed = 2;
                StartCoroutine(Phase2());
                Debug.Log("phase2 state");
                break;
        }
    }



    /// <summary>
    /// handling laser shooting
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootLaser()
    {
        float shotInterval = 1;

        if (isPhase2)
        {
            shotInterval = 0.5f;
            damage = 20;
        }
        else
        {
            damage = 10;
        }

        yield return new WaitForSeconds(shotInterval);
        animator.SetBool("isShootLaser", false);
        var laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        if (isLeft)
        {
            sr.flipX = true;
            laser.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y + 2, transform.position.z);
            laser.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            sr.flipX = false;
            laser.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y + 2, transform.position.z);
        }

        yield return new WaitForSeconds(shotInterval);
        isAttacking = false;
        DamageTaken(damage);

        yield return new WaitForSeconds(0.4f);
        Destroy(laser);
        HandleState();
    }

    /// <summary>
    /// handling pillar hand attack
    /// </summary>
    /// <returns></returns>
    private IEnumerator PillarHandAttack()
    {
        float handInterval = 0.7f;
        float projectileSpeed = 5f;

        if (isPhase2)
        {
            projectileSpeed = 10f;
            handInterval = 0.5f;
            damage = 10;
        }
        else
        {
            
            damage = 5;
        }

        yield return new WaitForSeconds(handInterval);
        animator.SetBool("isShootHand", false);
        var hand = Instantiate(handPrefab, transform.position, Quaternion.identity);
        hand.gameObject.GetComponent<Hand>().moveSpeed = projectileSpeed;

        if (isLeft)
        {
            sr.flipX = true;
            hand.transform.position = new Vector3(transform.position.x - 1.75f, transform.position.y + 1.5f, transform.position.z);
            hand.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            sr.flipX = false;
            hand.transform.position = new Vector3(transform.position.x + 1.75f, transform.position.y + 1.5f, transform.position.z);
        }
        isAttacking = false;
        DamageTaken(damage);
        HandleState();
    }

    /// <summary>
    /// handling the movement of the boss
    /// </summary>
    /// <returns></returns>
    private IEnumerator MovePattern()
    {
        float walkIntervals = 2f;

        while (currentState == BossState.Idle)
        {
            if (isLeft)
            {
                sr.flipX = true;
                rb.velocity = Vector2.left * moveSpeed;
            }
            else
            {
                sr.flipX = false;
                rb.velocity = Vector2.right * moveSpeed;
            }
            yield return new WaitForSeconds(walkIntervals);
            isLeft = !isLeft;
        }

        yield return new WaitForSeconds(walkIntervals);
    }

    /// <summary>
    /// handling the transition to phase 2
    /// </summary>
    /// <returns></returns>
    private IEnumerator Phase2()
    {
        moveSpeed *= 3;
        damage *= 2;
        yield return new WaitForSeconds(3);
        animator.SetBool("isPhase2", true);
        isEvolving = false;
        currentState = BossState.Idle;
        HandleState();
    }
    
    /// <summary>
    /// Damage Handler + lose condition + phase2 condition
    /// </summary>
    /// <param name="damage"></param>
    private void DamageTaken(int damage)
    {
        hp.TakeDamage(damage);

        if (hp.GetCurrentHealth() <= 50 && !isPhase2)
        {
            animator.SetTrigger("isHalfHP");
            currentState = BossState.Phase2;
        }

        else if (hp.GetCurrentHealth() <= 0)
        {
            currentState = BossState.Death;
            animator.SetTrigger("isDead");
            rb.velocity = Vector2.zero;
        }

        else
        {
            currentState = BossState.Idle;
        }
    }
}
