using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour, IHittable {

    [Header("Nav Mehs Things")]
    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField] float damage;
    [SerializeField] GameObject objective;
    [SerializeField] NavMeshAgent navMeshAgent;

    [Header("CC Things")]
    [SerializeField] bool stunned;
    [SerializeField] float timerStunned = 0;
    [SerializeField] float maxTimerStunned;

    private void Start() {
        objective = FindObjectOfType<FPSController>().gameObject;
        
        navMeshAgent.speed = speed;
        navMeshAgent.acceleration = acceleration;
    }

    private void Update() {
        if (stunned) {
               
            
            timerStunned += Time.deltaTime;
            if(timerStunned >= maxTimerStunned) {
                timerStunned = 0;
                stunned = false;
                navMeshAgent.isStopped = false;
            }
        }

    }

    private void FixedUpdate() {
        navMeshAgent.SetDestination(objective.transform.position);
    }

    public void Hit(float damage) {
        Debug.Log("AAAAAA LA OCNCHA DE LA LORA");

        health -= damage;
        if(health <= 0) {
            health = 0;
            Destroy(this.gameObject);
        }
    }

    public void HitWithStun(float damage, float stunDuration) {
        stunned = true;
        maxTimerStunned = stunDuration;
            navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
       
        Hit(damage);
    }
}
