using UnityEngine;

public class Character : MonoBehaviour
{
    public enum State
    {
        Idle,
        RunningToEnemy,
        RunningFromEnemy,
        BeginAttack,
        Attack,
        BeginShoot,
        Shoot,
        BeginHit,
        Hit,
        BeginDead,
        Dead,
    }

    public enum Weapon
    {
        Pistol,
        Bat,
        Fist,
    }

    Animator animator;
    State state;

    public Weapon weapon;
    public Transform target;
    public float runSpeed;
    public float distanceFromEnemy;
    Vector3 originalPosition;
    Quaternion originalRotation;
    
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        state = State.Idle;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public bool IsDead()
    {
        return state == State.BeginDead || state == State.Dead;
    }

    public void Die()
    {
        if (!IsDead())
            state = State.BeginDead;
    }
    
    public void SetState(State newState)
    {
        if (!IsDead()) 
            state = newState;
    }

    [ContextMenu("Attack")]
    void AttackEnemy()
    {   
        if (IsDead()) // Если атакующий персонаж мертв, он не может атаковать
            return;
        
        if (target.GetComponent<Character>().IsDead())  // Если цель мертва, тоже ее не атакуем
            return;
        
        switch (weapon) {
            case Weapon.Bat:
                state = State.RunningToEnemy;
                break;
            case Weapon.Pistol:
                state = State.BeginShoot;
                break;
            case Weapon.Fist:
                state = State.RunningToEnemy;
                break;
        }
    }

    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        Vector3 distance = targetPosition - transform.position;
        if (distance.magnitude < 0.00001f) {
            transform.position = targetPosition;
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - transform.position);

        Vector3 step = direction * runSpeed;
        if (step.magnitude < distance.magnitude) {
            transform.position += step;
            return false;
        }

        transform.position = targetPosition;
        return true;
    }

    void FixedUpdate()
    {
        switch (state) {
            case State.Idle:
                transform.rotation = originalRotation;
                animator.SetFloat("Speed", 0.0f);
                break;

            case State.RunningToEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(target.position, distanceFromEnemy))
                    switch (weapon) {
                        case Weapon.Bat:
                            state = State.BeginAttack;
                            break;
                        case Weapon.Fist:
                            state = State.BeginHit;
                            break;
                    }
                break;

            case State.RunningFromEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                    state = State.Idle;
                break;

            case State.BeginAttack:
                animator.SetTrigger("MeleeAttack");
                state = State.Attack;
                break;

            case State.Attack:
                break;
            
            case State.BeginShoot:
                animator.SetTrigger("Shoot");
                state = State.Shoot;
                break;

            case State.Shoot:
                break;
            
            case State.BeginHit:
                animator.SetTrigger("Hit");
                state = State.Hit;
                break;
            
            case State.Hit:
                break;
            
            case State.BeginDead:
                animator.SetTrigger("IsDead");
                state = State.Dead;
                break;
            
            case State.Dead:
                break;
        }
    }
}
