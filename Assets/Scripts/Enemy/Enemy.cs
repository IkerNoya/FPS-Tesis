using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour, IHittable {

    [Header("Nav Mehs Things")]
    [SerializeField] protected float speed;
    [SerializeField] protected float acceleration;
    [SerializeField] protected GameObject objective;
    [SerializeField] protected NavMeshAgent navMeshAgent;

    [Header("CC Things")]
    [SerializeField] protected bool stunned;
    [SerializeField] protected float timerStunned = 0;
    [SerializeField] protected float maxTimerStunned;

    [Header("Attack Things")]
    [SerializeField] protected float distanceToAttack;
    [SerializeField] protected float timerAttack = 0;
    [SerializeField] protected float timeToPrepareAttack;
    [SerializeField] protected float damage;
    [SerializeField] protected bool isAtacking;

    [Header("Health Things")]
    [SerializeField] HPController hpController;
    RaycastHit hit;
    private void Awake() {
        navMeshAgent.updateRotation = false;
    }
    protected virtual void Start() {
        objective = GameObject.FindGameObjectWithTag("Player");

        navMeshAgent.speed = speed;
        navMeshAgent.acceleration = acceleration;

        timerAttack = 0;
    }

    protected virtual void Update() {
        if (stunned) {
            timerStunned += Time.deltaTime;
            if (timerStunned >= maxTimerStunned) {
                timerStunned = 0;
                stunned = false;
                navMeshAgent.isStopped = false;
            }
            return;
        }

        Physics.Raycast(transform.position, (objective.transform.position - transform.position), out hit, distanceToAttack);
        if (Vector3.Distance(transform.position, objective.transform.position) <= distanceToAttack && hit.transform.gameObject == objective) {
            navMeshAgent.isStopped = true;
            timerAttack += Time.deltaTime;
            if (timerAttack >= timeToPrepareAttack)
                Attack();
        }
        else {
            navMeshAgent.isStopped = false;
            timerAttack = 0;
        }
    }

    protected virtual void FixedUpdate() {
        navMeshAgent.SetDestination(objective.transform.position);
    }
    private void LateUpdate() {
        transform.rotation = Quaternion.LookRotation((objective.transform.position - transform.position).normalized);
    }
    protected virtual void Attack() {
        timerAttack = 0;
        if (Physics.Raycast(transform.position, (objective.transform.position - transform.position), out hit, distanceToAttack))
            if (hit.transform.gameObject == objective)
                if (hit.transform.GetComponent<IHittable>() != null) 
                    hit.transform.GetComponent<IHittable>().Hit(damage);
    }

    public virtual void Hit(float damage) {
        hpController.TakeDamage((int)damage);
        if (hpController.GetHP() <= 0) {
            Destroy(this.gameObject);
        }
    }

    public virtual void HitWithStun(float damage, float stunDuration) {
        stunned = true;
        maxTimerStunned = stunDuration;
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;

        Hit(damage);
    }
}
