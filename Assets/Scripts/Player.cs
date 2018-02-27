using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_hitPointCount = null;
    [SerializeField] [Range(1.0f, 10.0f)] float m_walkSpeed = 1;
    [SerializeField] [Range(1.0f, 10.0f)] float m_runSpeed = 1;
    [SerializeField] [Range(1.0f, 100.0f)] float m_rotateSpeed = 75;
    [SerializeField] GameObject m_axeObject = null;

    [Header("Magic")]
    [SerializeField] Spell m_spell = null;
    [SerializeField] Transform m_emitter = null;

    [Header("SFX")]
    [SerializeField] AudioClip m_footstepSFX = null;
    [SerializeField] AudioClip m_attack1SFX = null;
    [SerializeField] AudioClip m_attack2SFX = null;
    [SerializeField] AudioClip m_deadSFX = null;

    Animator m_animator = null;
    CapsuleCollider m_axeCollider = null;
    Destructable m_destructable = null;
    
    bool isRunning = false;
    bool isDead = false;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_axeCollider = m_axeObject.GetComponent<CapsuleCollider>();
        m_destructable = GetComponent<Destructable>();
    }

    void Update()
    {
        if(m_destructable.hitPoints <= 0.0f)
        {
            isDead = true;
            m_hitPointCount.text = "Health: 0";
        }

        if (!isDead)
        {
            Vector3 velocity = Vector3.zero;
            Vector3 rotate = Vector3.zero;

            velocity.z = Input.GetAxis("Vertical");
            velocity.x = Input.GetAxis("Horizontal");

            velocity = velocity * m_walkSpeed;

            rotate.y = Input.GetAxis("Rotate") * m_rotateSpeed;

            bool isWalking = !Mathf.Approximately(velocity.magnitude, 0.0f);
            m_animator.SetBool("Walking", isWalking);
            m_animator.SetFloat("WalkSpeed", velocity.magnitude);

            if (Input.GetButtonDown("Run"))
            {
                isRunning = true;
            }

            if (Input.GetButtonUp("Run"))
            {
                isRunning = false;
            }

            m_animator.SetBool("Running", isRunning);
            m_animator.SetFloat("RunSpeed", velocity.magnitude * m_runSpeed);

            if (isRunning)
            {
                velocity = velocity * m_runSpeed;
            }

            transform.rotation = transform.rotation * Quaternion.Euler(rotate * Time.deltaTime);

            velocity = transform.rotation * velocity;
            transform.position += (velocity * Time.deltaTime);

            m_hitPointCount.text = "Health: " + m_destructable.hitPoints;

            if (Input.GetButtonDown("Attack1"))
            {
                m_animator.SetTrigger("Attack1");
            }

            if (Input.GetButtonDown("Attack2"))
            {
                m_animator.SetTrigger("Attack2");
            }

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    m_animator.SetBool("Walking", true);
                }
                else if(touch.phase == TouchPhase.Ended)
                {
                    m_animator.SetBool("Walking", false);
                }
            }            
        }        
    }

    void PlayFootStep()
    {
        GetComponent<AudioSource>().clip = m_footstepSFX;
        GetComponent<AudioSource>().Play();
    }

    void SendFireBall()
    {
        Spell spell = Instantiate<Spell>(m_spell, m_emitter.position + (Vector3.up / 2), Quaternion.identity);

        Vector3 direction = transform.rotation * Vector3.forward;
        spell.Fire(direction);
    }

    void PlayAttack1Sound()
    {
        GetComponent<AudioSource>().clip = m_attack1SFX;
        GetComponent<AudioSource>().Play();
    }

    void PlayAttack2Sound()
    {
        GetComponent<AudioSource>().clip = m_attack2SFX;
        GetComponent<AudioSource>().Play();
    }

    void DisableAxeCollider()
    {
        m_axeCollider.enabled = false;
    }

    void EnableAxeCollider()
    {
        m_axeCollider.enabled = true;
    }

    void PlayDeadSound()
    {
        GetComponent<AudioSource>().clip = m_deadSFX;
        GetComponent<AudioSource>().Play();
    }
}
