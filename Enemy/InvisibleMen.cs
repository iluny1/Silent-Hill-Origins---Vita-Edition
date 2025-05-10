using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class InvisibleMen : MonoBehaviour, IEnemy, IEnemyAnimation, IHealth {

    public Enums.Enemies Type { get; } = Enums.Enemies.InvisibleMan;
    public Enums.ThinkStates ThinkState { get { return m_ThinkState; } }

    public float Health
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }

    private NavPoint m_NavPoint;
    private NavPoint m_NavPointLast;

    [SerializeField] private Vector3 m_TargetPoint;

    [SerializeField] private bool m_Idle;    

    private bool m_StopThink = false;
    [SerializeField] private bool m_Attacked = false;
    [SerializeField] private bool m_PlayerSeen = false;
    [SerializeField] private bool m_PlayerNearToAttack = false;

    [SerializeField] private float m_IdleTimer = 0f;
    private const int IDLE_TIME = 4;

    [SerializeField] private float m_AttackTimer = 0f;
    private const int ATTACK_TIME = 3;

    private Animator m_AnimatorMain;
    [SerializeField] private Animator m_AnimatorShadow;
    private EnemyAudio m_Audio;

    private SphereCollider m_SphereInteract;

    private Transform m_PlayerTransform;
    [SerializeField] private Transform m_ShadowPosition;
    [SerializeField] private InvisibleAttack m_Attack;

    private Enums.ThinkStates m_ThinkState = Enums.ThinkStates.Idle;

    

    private NavMeshAgent m_Agent;

    private void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_AnimatorMain = GetComponent<Animator>();
        m_Audio = GetComponent<EnemyAudio>();
        m_IdleTimer = IDLE_TIME;
    }

    private void Update()
    {
        Think();
    }

    private void FixedUpdate()
    {
        //Debug.Log($"{gameObject.name} - Current State: {m_ThinkState.ToString()}");
    }

    private void OnTriggerEnter(Collider other)
    {
        OnAwareEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnAwareExit(other);
    }

    public void OnAwareEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        m_ThinkState = Enums.ThinkStates.Chase;
        m_PlayerTransform = other.transform;
        m_Audio.PlayFX(Enums.EnemySFXTypes.Spot);
        m_PlayerSeen = true;
    }

    public void OnAwareExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        m_PlayerSeen = false;
        m_PlayerTransform = null;
        ToWalkState(true);
    }

    private void Think()
    {
        switch (m_ThinkState)
        {
            case Enums.ThinkStates.Idle: Idle(); break;
            case Enums.ThinkStates.Walk: Walk(); break;
            case Enums.ThinkStates.Chase: Chase(); break;
            case Enums.ThinkStates.Attack: Attack(); break;
        }

        if (m_AttackTimer > 0) m_AttackTimer -= Time.deltaTime;
    }

    private void Idle()
    {
        if (m_IdleTimer > 0f) m_IdleTimer -= Time.deltaTime;
        else ToWalkState(false);
    }

    private void Walk()
    {
        if (!m_Agent.pathPending && m_Agent.remainingDistance < 0.1f)
        {
            m_Agent.isStopped = true;
            ToIdleState();            
        }
    }

    private void Chase()
    {
        if (!m_PlayerSeen) { m_Agent.isStopped = true; ToIdleState(); }
        if (m_TargetPoint.x != m_PlayerTransform.position.x && m_TargetPoint.z != m_PlayerTransform.position.z)
        {
            m_TargetPoint = m_PlayerTransform.position;
            m_Agent.isStopped = false;
            m_Agent.SetDestination(m_TargetPoint);
            SetWalkAnimation();
        }
        
        if (Vector3.Distance(m_TargetPoint, transform.position) < 1.5f && m_AttackTimer <= 0)
        {
            m_ThinkState = Enums.ThinkStates.Attack;
            SetIdleAnimation();
        }
    }

    private void Attack()
    {
        Vector3 fwd = (m_PlayerTransform.position - transform.position).normalized;
        fwd.y = 0;
        transform.forward = fwd;

        if (!m_Attacked)
        {            
            m_Agent.isStopped = true;
            TriggerAnimation("Attack");
            m_Audio.PlayFX(Enums.EnemySFXTypes.Attack);
            m_Attacked = true;
        }
    }

    public void FromAttack()
    {
        m_ThinkState = Enums.ThinkStates.Chase;
        m_Attacked = false;
        m_AttackTimer = ATTACK_TIME;
        //transform.position = m_ShadowPosition.position;
    }

    private void ToWalkState(bool fromChase)
    {
        if (m_Idle) { ToIdleState(); return; }

        if (fromChase)
        {
            m_ThinkState = Enums.ThinkStates.Walk;
            m_Agent.SetDestination(m_TargetPoint);
            m_Agent.isStopped = false;
        }
        else if (m_NavPoint == null)
        {
            if (FindNavPoint())
            {
                m_ThinkState = Enums.ThinkStates.Walk;
                m_TargetPoint = m_NavPoint.GetCoordinates();
                m_Agent.SetDestination(m_NavPoint.transform.position);
                m_Agent.isStopped = false;
            }
            else
            {
                ToIdleState(); 
                return;
            }
                
        }
        else
        {
            m_ThinkState = Enums.ThinkStates.Walk;
            m_NavPoint = m_NavPoint.GetNextNeighbour();
            m_TargetPoint = m_NavPoint.GetCoordinates();
            m_Agent.SetDestination(m_NavPoint.transform.position);
            m_Agent.isStopped = false;
        }

        SetWalkAnimation();
    }


    private void ToIdleState()
    {
        m_IdleTimer = IDLE_TIME;
        m_ThinkState = Enums.ThinkStates.Idle;
        SetIdleAnimation();
    }

    private bool FindNavPoint()
    {
        float radius = 10f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << 10);

        while (colliders.Length < 1 && radius <= 100)
        {
            radius += 10f;
            colliders = Physics.OverlapSphere(transform.position, radius, 1 << 10);
        }

        if (radius > 100 && colliders.Length < 1) return false;
        else
        {
            if (colliders.Length == 1)
            {
                m_NavPoint = colliders[0].GetComponent<NavPoint>();
                return true;
            }
            else
            {
                float distance = float.MaxValue;
                int index = -1;
                Vector2 position = new Vector2(transform.position.x, transform.position.z);
                
                for(int i = 0; i < colliders.Length; i++)
                {
                    Vector2 colPos = new Vector2(colliders[i].transform.position.x, colliders[i].transform.position.z);
                    float distanceTemp = Vector2.Distance(position, colPos);
                    if (distanceTemp < distance)
                    {
                        index = i;
                        distance = distanceTemp;                        
                    }
                }

                m_NavPoint = colliders[index].GetComponent<NavPoint>();
                return true;
            }
        }
    }

    public int GetStateID()
    {
        return 0;
    }

    #region Animation

    public void SetWalkAnimation()
    {
        m_AnimatorMain.SetFloat("Speed", 1);
        m_AnimatorShadow.SetFloat("Speed", 1);
    }

    public void SetIdleAnimation()
    {
        m_AnimatorMain.SetFloat("Speed", 0);
        m_AnimatorShadow.SetFloat("Speed", 0);
    }

    public void TriggerAnimation(string triggerName)
    {
        m_AnimatorMain.SetTrigger(triggerName);
        m_AnimatorShadow.SetTrigger(triggerName);
    }   

    public void ActivateAttackCollider() { m_Attack.ActivateCollider(); }
    public void DeactivateAttackCollider() { m_Attack.DeactivateCollider(); }

    public void SetRunAnimation()
    {
        throw new System.NotImplementedException();
    }

    public void SetFastRunAnimation()
    {
        throw new System.NotImplementedException();
    }

    public void TriggerQTEAnimation()
    {
        throw new System.NotImplementedException();
    }

    public void TriggerQTEHitAnimation()
    {
        throw new System.NotImplementedException();
    }

    public void TriggerQTEMissAnimation()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Health

    public void GetDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void GetDamage(float damage, Vector3 hitDir)
    {
        m_Agent.isStopped = true;
        m_Audio.PlayFX(Enums.EnemySFXTypes.Impact);
        m_ThinkState = Enums.ThinkStates.Knocked;
        m_AnimatorMain.SetFloat("KnockX", hitDir.normalized.x);
        m_AnimatorMain.SetFloat("KnockY", hitDir.normalized.z);
        m_AnimatorMain.SetTrigger("Hitted");
    }

    public void Death()
    {
        throw new System.NotImplementedException();
    }

    public void ReactToDamage(Vector3 hitDir)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
