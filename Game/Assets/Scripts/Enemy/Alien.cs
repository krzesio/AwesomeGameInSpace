﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Alien : CharacterAI
{
    protected NavMeshAgent navMeshAgent;
    protected RagdollAnimation ragdollAnimation;
    public float maxHitPoints;
    public AudioClip[] scream;
    internal float currentHitPoints;

    public override bool isDead { get; set; }

    public override Vector3 velocity { get { return navMeshAgent.velocity; } }

    private bool disposeBody;

    protected void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdollAnimation = GetComponent<RagdollAnimation>();
        currentHitPoints = maxHitPoints;
        disposeBody = false;
    }

    public virtual void Kill()
    {
        if (!isDead)
        {
            isDead = true;
            navMeshAgent.enabled = false;
            ragdollAnimation.Kill();
            StartCoroutine(DelayDie(60.0f));
            audio.PlayOneShot(scream [Random.Range(0, scream.Length)], 10.0f);
            GameMaster.instance.activeEnemies--;
        }
    }

    IEnumerator DelayDie(float delay)
    {
        yield return new WaitForSeconds(delay);
        disposeBody = true;
    }

    public override void DealDamage(float damage)
    {
        if (GetComponent<BaseFSM>().currentState == BaseFSM.State.Patrol)
            GetComponent<BaseFSM>().currentState = BaseFSM.State.Chase;
        currentHitPoints -= damage;
        if (currentHitPoints <= 0.0f)
            Kill();
    }

    void OnBecameInvisible()
    {
        if (disposeBody)
        {
            Destroy(this.gameObject);
        }
    }

    /*void OnBecameVisible()
    {
        if (!isDead)
        {

        }
    }*/

    public virtual void MineHit()
    {
        Kill();
    }

    public void Untrap()
    {
        StartCoroutine(untrap());
    }

    private IEnumerator untrap()
    {       
        yield return new WaitForSeconds(5.0f);
        transform.position = new Vector3(gameObject.transform.FindChild("Bip001").FindChild("Bip001 Pelvis").position.x, 0.0f, gameObject.transform.FindChild("Bip001").FindChild("Bip001 Pelvis").position.z);
      if(!isDead) gameObject.GetComponent<AlienAnimation>().isRagdoll = false;
        yield return new WaitForSeconds(0.5f);

        if (!isDead) navMeshAgent.enabled = true;
        if (!isDead) gameObject.GetComponent<BaseFSM>().enabled = true;
        yield return null;
            
    }

}
