using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] [Range(.5f, 5000.0f)] float m_hitPoints = 1.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_deathTimer = 5.0f;
    [SerializeField] Animator m_animator;
    
    public float hitPoints { get { return m_hitPoints; } }
    public bool isAlive = true;

    private void OnTriggerEnter(Collider other)
    {
        if(isAlive)
        {
            if ((gameObject.tag == "Player" && other.gameObject.tag == "WeaponEnemy") ||
                (gameObject.tag == "Enemy" && other.gameObject.tag == "WeaponPlayer"))
            {               
                Weapon weapon = other.gameObject.GetComponent<Weapon>();

                m_animator.SetTrigger("IsHit");

                m_hitPoints -= weapon.damage;

                if (m_hitPoints <= 0.0f)
                {
                    isAlive = false;
                    m_animator.SetBool("Dead", true);

                    if (gameObject.tag != "Player")
                    {
                        Destroy(gameObject, m_deathTimer);
                    }                  
                }
            }
        }        
    }    
}
