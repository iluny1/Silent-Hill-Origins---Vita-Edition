using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StraightJacket : MonoBehaviour, IEnemyAnimation, IHealth, IEnemy, IInteractable {

    [SerializeField] private NavPoint m_NavPoint;
    private NavPoint m_NavPointLast;

    [SerializeField] private Vector3 m_TargetPoint;
    private Transform m_PlayerTransform;

    [SerializeField] private bool m_DontThink;
    private bool m_Laying;
    
    [SerializeField] private bool m_Attacked = false;
    [SerializeField] private bool m_PlayerSeen = false;

    [SerializeField] private float m_IdleTimer = 0f;
    private const int IDLE_TIME = 4;

    [SerializeField] private float m_AttackTimer = 0f;
    private const int ATTACK_TIME = 7;

    private float m_QTETimer = 0f;
    private const int QTE_TIME = 20;

    private Animator m_AnimatorMain;
    private EnemyAudio m_Audio;
    
    private SphereCollider m_SphereInteract;
    [SerializeField] private Collider m_SphereAware;
    [SerializeField] private CharacterController m_Controller;

    [SerializeField] ParticleSystem m_SpitEffect;

    private const float SPEED_WALK = 1.5f;
    private const float SPEED_RUN = 3f;

    Vector3 m_WorldDeltaPosition;

    public Enums.ThinkStates ThinkState { get { return m_ThinkState; } }
    [SerializeField] private Enums.ThinkStates m_ThinkState = Enums.ThinkStates.Idle;
    private Enums.ThinkStates m_ThinkStatePrevious;

    public Enums.Enemies Type
    {
        get { return Enums.Enemies.StraightJacket; }
    }
    
    private NavMeshAgent m_Agent;

    [SerializeField] private GameObject m_BloodStain;

    private void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_AnimatorMain = GetComponent<Animator>();
        m_SphereInteract = GetComponent<SphereCollider>();
        m_Audio = GetComponent<EnemyAudio>();
        m_IdleTimer = IDLE_TIME;
        m_Agent.updatePosition = false;
        m_Health = MAX_HEALTH;
    }

    private void OnAnimatorMove()
    {
        Vector3 position = m_AnimatorMain.rootPosition;
        position.y = m_Agent.nextPosition.y;
        transform.position = position;
    }

    private void Update()
    {
        Think();
    }

    public void OnAwareEnter(Collider other)
    {
        if (m_ThinkState == Enums.ThinkStates.Death ||
            m_ThinkState == Enums.ThinkStates.OnGround ||
            !other.CompareTag("Player")) return;
        m_ThinkState = Enums.ThinkStates.Chase;
        m_PlayerTransform = other.transform;
        m_Audio.PlayFX(Enums.EnemySFXTypes.Spot);
        m_PlayerSeen = true;
        m_AttackTimer = ATTACK_TIME;
    }

    public void OnAwareExit(Collider other)
    {
        if (m_ThinkState == Enums.ThinkStates.Death ||
            m_ThinkState == Enums.ThinkStates.OnGround ||
            !other.CompareTag("Player")) return;
        m_PlayerSeen = false;
        m_PlayerTransform = null;
        ToWalkState(true);
    }

    private void Think()
    {
        if (m_DontThink) return;

        switch (m_ThinkState)
        {
            case Enums.ThinkStates.Idle: Idle(); break;
            case Enums.ThinkStates.Walk: Walk(); break;
            case Enums.ThinkStates.Chase: Chase(); break;
            case Enums.ThinkStates.Attack: Attack(); break;
            case Enums.ThinkStates.AttackQTE: AttackQTE(); break;
            default: return;
        }

        if (m_AttackTimer > 0) m_AttackTimer -= Time.deltaTime;
        if (m_QTETimer > 0) m_QTETimer -= Time.deltaTime;
    }

    private void Idle()
    {
        if (m_IdleTimer > 0f) m_IdleTimer -= Time.deltaTime;
        else ToWalkState(false);
    }

    private void Walk()
    {
        if (!m_Agent.pathPending && m_Agent.remainingDistance < 0.2f)
        {
            m_Agent.isStopped = true;
            ToIdleState();            
        }

        m_WorldDeltaPosition = m_Agent.nextPosition - transform.position;
        if (m_WorldDeltaPosition.magnitude > m_Agent.radius)
            m_Agent.nextPosition = transform.position + 0.1f * m_WorldDeltaPosition;
    }

    private void Chase()
    {
        if (!m_PlayerSeen) { m_Agent.isStopped = true; ToIdleState(); }

        if (m_TargetPoint.x != m_PlayerTransform.position.x && m_TargetPoint.z != m_PlayerTransform.position.z)
        {
            m_TargetPoint = m_PlayerTransform.position;
            m_Agent.isStopped = false;
            m_Agent.SetDestination(m_TargetPoint);
            m_Agent.speed = SPEED_RUN;            
            SetRunAnimation();
        }

        float distance = Vector3.Distance(m_TargetPoint, transform.position);

        if (distance < 1.5f && m_QTETimer <= 0)
        {
            m_ThinkState = Enums.ThinkStates.AttackQTE;
            return;
        }
        else if (distance < 10f && m_AttackTimer <= 0)
        {
            m_ThinkState = Enums.ThinkStates.Attack;
            return;
        }
        else if (m_AttackTimer > 0)
            SetFastRunAnimation();
        else
            SetRunAnimation();

        m_WorldDeltaPosition = m_Agent.nextPosition - transform.position;
        if (m_WorldDeltaPosition.magnitude > m_Agent.radius)
            m_Agent.nextPosition = transform.position + 0.1f * m_WorldDeltaPosition;
    }

    private void Attack()
    {
        if (m_AttackTimer > 0 || m_Attacked) return;

        transform.forward = m_PlayerTransform.position - transform.position;
        m_Agent.isStopped = m_Attacked = true;
        m_SpitEffect.Play();
        TriggerAnimation("Attack");
        m_Audio.PlayFX(Enums.EnemySFXTypes.Attack);
    }

    public void Spit()
    {
        Vector3 targetPosition = Travis_ScriptHandle.Instance.transform.position + Vector3.up * 1.5f;
        AcidBallPool.Instance.GetAcidBall(m_SpitEffect.transform.position, targetPosition, gameObject);
    }

    public void FromAttack()
    {
        m_ThinkState = Enums.ThinkStates.Chase;
        m_Agent.isStopped = m_Attacked = false;
        m_AttackTimer = ATTACK_TIME;
    }

    private void AttackQTE()
    {
        if (m_Attacked) return;

        m_Attacked = true;
        m_Agent.updatePosition = m_Agent.updateRotation = m_Controller.enabled = false;
        transform.position = m_PlayerTransform.position + (m_PlayerTransform.forward.normalized * 0.87f);
        transform.forward = m_PlayerTransform.position - transform.position;
        m_PlayerTransform.GetComponent<Travis_QTE>().BeginQTE(Type, gameObject);
        m_AnimatorMain.SetTrigger("QTE");
    }

    private void FromQTE()
    {
        m_Attacked = false;
        m_Agent.updateRotation = m_Controller.enabled = true;
        m_QTETimer = QTE_TIME;
        m_AttackTimer = ATTACK_TIME / 2f;
        m_ThinkState = Enums.ThinkStates.Chase;
    }

    private void ToWalkState(bool fromChase)
    {
        if (m_DontThink) { ToIdleState(); return; }

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
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << 30);

        while (colliders.Length < 1 && radius <= 100)
        {
            radius += 10f;
            colliders = Physics.OverlapSphere(transform.position, radius, 1 << 30);
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

    public void SpawnBloodFX()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.5f,  1 << 0))
            Instantiate(m_BloodStain, hit.point + new Vector3(0, 0.001f, 0), Quaternion.identity);
    }

    #region Animation

    public void SetWalkAnimation()
    {
        m_AnimatorMain.SetFloat("Speed", 1);
    }

    public void SetIdleAnimation()
    {
        m_AnimatorMain.SetFloat("Speed", 0);
    }

    public void SetRunAnimation()
    {
        m_AnimatorMain.SetFloat("Speed", 2);
    }

    public void SetFastRunAnimation()
    {
        m_AnimatorMain.SetFloat("Speed", 3);
    }

    public void TriggerAnimation(string triggerName)
    {
        m_AnimatorMain.SetTrigger(triggerName);
    }

    public void TriggerQTEAnimation()
    {
        m_AnimatorMain.SetTrigger("QTE");
    }

    public void TriggerQTEHitAnimation()
    {
        m_AnimatorMain.SetTrigger("QTE_Hit");
    }

    public void TriggerQTEMissAnimation()
    {
        m_AnimatorMain.SetTrigger("QTE_Miss");
    }    

    public void ActivateAttackCollider() { }
    public void DeactivateAttackCollider() { }

    #endregion

    #region Health

    private float m_Health;
    private const float MAX_HEALTH = 45f;

    public float Health
    {
        get
        {
            return m_Health;
        }
    }

    public void GetDamage(float damage, Vector3 hitPos)
    {
        m_Health -= damage;
        if (m_Health <= 0) LayDown();
        else
            ReactToDamage(transform.position - hitPos);
    }

    public void GetDamage(float damage)
    {
        m_Health -= damage;
        if (m_Health <= 0) LayDown();
        else
            ReactToDamage(transform.position - Travis_ScriptHandle.Instance.transform.position);
    }

    public void ReactToDamage(Vector3 hitDir)
    {
        m_Agent.isStopped = true;
        m_Audio.PlayFX(Enums.EnemySFXTypes.Impact);
        m_ThinkStatePrevious = m_ThinkState;
        m_ThinkState = Enums.ThinkStates.Knocked;
        m_AnimatorMain.SetFloat("KnockX", hitDir.normalized.x);
        m_AnimatorMain.SetFloat("KnockY", hitDir.normalized.z);
        m_AnimatorMain.SetTrigger("Hitted");
    }

    public void ReactionAftermath()
    {
        m_ThinkState = Enums.ThinkStates.Chase;
        if (m_ThinkStatePrevious != Enums.ThinkStates.Chase)
        {
            m_PlayerTransform = Travis_ScriptHandle.Instance.transform;
            m_Audio.PlayFX(Enums.EnemySFXTypes.Spot);
            m_PlayerSeen = true;            
        }
    }

    public void LayDown()
    {
        m_ThinkState = Enums.ThinkStates.OnGround;
        m_AnimatorMain.SetBool("OnGround", true);
        m_Audio.PlayFX(Enums.EnemySFXTypes.Impact);
        m_Agent.updatePosition = m_Agent.updateRotation = m_SphereAware.enabled = false;
        m_Agent.isStopped = true;        
    }

    public void OnGround()
    {
        m_Audio.PlayFX(Enums.EnemySFXTypes.Writhe);
        m_Audio.SetLoop(true);
        m_Controller.enabled = false;
        m_SphereInteract.enabled = true;
    }

    public void Death()
    {
        m_ThinkState = Enums.ThinkStates.Death;
        m_Audio.PlayFX(Enums.EnemySFXTypes.Death);
        m_Agent.enabled = m_Controller.enabled = m_SphereInteract.enabled = m_SphereAware.enabled = false;
        SpawnBloodFX();
    }

    public int GetStateID()
    {
        return (int)m_ThinkState;
    }

    public void Interact()
    {
        Death();
        Travis_ScriptHandle.Instance.TravisInteraction.FinishEnemy(transform);
    }

    #endregion

}
