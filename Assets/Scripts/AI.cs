using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 10.0f)] float m_walkSpeed = 1.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_runSpeed = 2.0f;
    [SerializeField] [Range(0.0f, 2.0f)] float m_perceptionTime = 1.0f;
    [SerializeField] [Range(1.0f, 100.0f)] float m_rotateSpeed = 1.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_attackRadius = 2.0f;
    [SerializeField] GameObject m_attackCollider;
    Animator m_animator = null;

    [Header("SFX")]
    [SerializeField] AudioClip m_footstepSFX = null;
    [SerializeField] AudioClip m_attack1SFX = null;
    [SerializeField] AudioClip m_hurtSFX = null;
    [SerializeField] AudioClip m_deadSFX = null;

    StackStateMachine<AI> m_stackStateMachine = new StackStateMachine<AI>();
    PerceptionSphere m_perception = null;
    GameObject m_targetGameObject = null;
    Waypoint m_waypoint = null;
    Destructable m_destructable = null;

    float m_perceptionTimer = 0.0f;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_perception = GetComponent<PerceptionSphere>();
        m_destructable = GetComponent<Destructable>();

        m_stackStateMachine.AddState("alert", new AlertState<AI>(this));
        m_stackStateMachine.AddState("attack", new AttackState<AI>(this));
        m_stackStateMachine.AddState("wander", new WanderState<AI>(this));
        m_stackStateMachine.PushState("wander");

        GameObject waypointContainer = GameObject.FindGameObjectWithTag("WaypointContainer");
        Waypoint[] waypoints = waypointContainer.GetComponentsInChildren<Waypoint>();

        int num = (int)Random.Range(0.0f, waypoints.Length - 1);
        Waypoint nextWayPoint = (Waypoint)waypoints[num];

        SetWaypoint(nextWayPoint);
    }

    void Update()
    {
        if (m_destructable.isAlive)
        {
            m_stackStateMachine.Update();
        }
    }

    public void SetWaypoint(Waypoint waypoint)
    {
        m_waypoint = waypoint;
    }

    public void ActivateAttackCollider()
    {
        m_attackCollider.SetActive(true);
    }

    public void DeacivateAttackCollider()
    {
        m_attackCollider.SetActive(false);
    }

    class AttackState<T> : State<T> where T : AI
    {
        public AttackState(T owner) : base(owner)
        {

        }

        public override void Update()
        {
            m_owner.m_animator.SetTrigger("Attack");
            m_owner.m_animator.SetBool("Walking", false);
            m_owner.m_animator.SetBool("Running", false);

            Vector3 direction = m_owner.m_targetGameObject.transform.position - m_owner.transform.position;

            m_owner.transform.rotation = Quaternion.Slerp(m_owner.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * m_owner.m_rotateSpeed);

            Vector3 velocity = Vector3.zero;
            m_owner.transform.position = m_owner.transform.position + (velocity * Time.deltaTime);

            m_owner.m_perceptionTimer = m_owner.m_perceptionTimer - Time.deltaTime;

            m_owner.m_targetGameObject = m_owner.m_perception.GetGameObjectWithTagInRadius("Player", m_owner.m_attackRadius);
            if (m_owner.m_targetGameObject == null)
            {
                m_owner.m_stackStateMachine.PopState();
            }   
        }
    }

    class AlertState<T> : State<T> where T : AI
    {
        public AlertState(T owner) : base(owner)
        {

        }

        public override void Update()
        {
            m_owner.m_animator.SetBool("Walking", true);
            m_owner.m_animator.SetBool("Running", true);

            m_owner.m_targetGameObject = m_owner.m_perception.GetGameObjectWithTag("Player");

            if (m_owner.m_targetGameObject)
            {
                Vector3 direction = m_owner.m_targetGameObject.transform.position - m_owner.transform.position;

                m_owner.transform.rotation = Quaternion.Slerp(m_owner.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * m_owner.m_rotateSpeed);

                Vector3 velocity = m_owner.transform.rotation * (Vector3.forward * m_owner.m_runSpeed);
                m_owner.transform.position = m_owner.transform.position + (velocity * Time.deltaTime);
            }

            m_owner.m_perceptionTimer = m_owner.m_perceptionTimer - Time.deltaTime;

            if (m_owner.m_perceptionTimer <= 0.0f)
            {
                m_owner.m_perceptionTimer = m_owner.m_perceptionTime;

                m_owner.m_targetGameObject = m_owner.m_perception.GetGameObjectWithTag("Player");

                if (m_owner.m_targetGameObject == null)
                {
                    m_owner.m_stackStateMachine.PopState();
                    m_owner.m_animator.SetBool("Running", false);
                }
                else
                {
                    m_owner.m_targetGameObject = m_owner.m_perception.GetGameObjectWithTagInRadius("Player", m_owner.m_attackRadius);

                    if (m_owner.m_targetGameObject)
                    {
                        m_owner.m_stackStateMachine.PushState("attack");
                    }
                }
            }
        }
    }

    class WanderState<T> : State<T> where T : AI
    {
        public WanderState(T owner) : base(owner)
        {

        }

        public override void Update()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 direction = Vector3.zero;

            if (m_owner.m_waypoint != null)
            {
                direction = m_owner.m_waypoint.transform.position - m_owner.transform.position;
                velocity.z = m_owner.m_walkSpeed;
            }

            bool isWalking = !Mathf.Approximately(velocity.magnitude, 0.0f);
            m_owner.m_animator.SetBool("Walking", isWalking);

            m_owner.transform.rotation = Quaternion.Slerp(m_owner.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * m_owner.m_rotateSpeed);

            velocity = m_owner.transform.rotation * velocity;
            m_owner.transform.position = m_owner.transform.position + (velocity * Time.deltaTime);

            m_owner.m_targetGameObject = m_owner.m_perception.GetGameObjectWithTag("Player");
            
            if (m_owner.m_targetGameObject)
            {
                m_owner.m_stackStateMachine.PushState("alert");
            }
        }
    }
    void PlayAttackSound()
    {
        GetComponent<AudioSource>().clip = m_attack1SFX;
        GetComponent<AudioSource>().Play();
    }

    void PlayFootStepSound()
    {
        GetComponent<AudioSource>().clip = m_footstepSFX;
        GetComponent<AudioSource>().Play();
    }

    void PlayHurtSound()
    {
        GetComponent<AudioSource>().clip = m_hurtSFX;
        GetComponent<AudioSource>().Play();
    }

    void PlayDeadSound()
    {
        GetComponent<AudioSource>().clip = m_deadSFX;
        GetComponent<AudioSource>().Play();
    }
}
