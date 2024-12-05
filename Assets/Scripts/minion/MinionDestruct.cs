using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDestruct : MonoBehaviour
{
    public GameObject destroyed;

    public CharacterGeneral getdestruct;
    public float explosionForce = 1000f;
    public float explosionRadius = 50f; 
    public Vector3 explosionPositionOffset; 

    public void SelfDestroy()
    { 
        getdestruct = GetComponent<CharacterGeneral>();
        Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out RaycastHit raycastHit , 999f))
        {
            if(raycastHit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                Vector3 explosionPosition = raycastHit.point + explosionPositionOffset;
                rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
        }
        if(getdestruct == true)
        {
            Instantiate(destroyed,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }
}