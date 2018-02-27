using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {
    
    [SerializeField] [Range(1.0f, 50.0f)] float m_speed = 1.0f;

    public void Fire(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction * m_speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
