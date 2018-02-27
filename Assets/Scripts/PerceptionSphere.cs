using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionSphere : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 270.0f)] float m_fov = 70.0f;
    [SerializeField] [Range(1.0f, 90.0f)] float m_radius = 5.0f;
    [SerializeField] LayerMask m_layers;

    public GameObject GetGameObjectWithTag(string tag)
    {
        GameObject targetGameObject = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, m_radius, m_layers);

        foreach (Collider collider in colliders)
        {
            if(collider.CompareTag(tag))
            {
                Vector3 direction = collider.gameObject.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                if(angle < m_fov)
                {
                    Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + direction.normalized * m_radius);

                    RaycastHit raycastHit;
                                    
                    if(Physics.Raycast(transform.position + Vector3.up, direction.normalized, out raycastHit, m_radius, m_layers))
                    {
                        if(raycastHit.collider.gameObject == collider.gameObject)
                        {
                            targetGameObject = collider.gameObject;
                            break;
                        }
                    }                   
                }
            }            
        }

        return targetGameObject;
    }

    public GameObject GetGameObjectWithTagInRadius(string tag, float radius)
    {
        GameObject targetGameObject = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, m_layers);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                Vector3 direction = collider.gameObject.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle < m_fov)
                {
                    Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + direction.normalized * radius);

                    RaycastHit raycastHit;

                    if (Physics.Raycast(transform.position + Vector3.up, direction.normalized, out raycastHit, radius, m_layers))
                    {
                        if (raycastHit.collider.gameObject == collider.gameObject)
                        {
                            targetGameObject = collider.gameObject;
                            break;
                        }
                    }
                }
            }
        }

        return targetGameObject;
    }
}
