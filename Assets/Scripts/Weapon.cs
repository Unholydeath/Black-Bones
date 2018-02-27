using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] [Range(1.0f, 200.0f)] float m_damage = 5.0f;

    public float damage { get { return m_damage; } }
}
