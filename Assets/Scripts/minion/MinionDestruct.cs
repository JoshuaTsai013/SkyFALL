using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDestruct : MonoBehaviour
{
    public GameObject destroyedMinion;
    private CharacterGeneral characterGeneral;
    public float explosionForce = 1000f;
    public float explosionRadius = 50f;
    public Vector3 explosionPositionOffset;

    private void Start()
    {
        // Get the CharacterGeneral component
        characterGeneral = GetComponent<CharacterGeneral>();

        // Subscribe to the OnDie event
        if (characterGeneral)
        {
            characterGeneral.OnDie.AddListener(HandleDie);
        }
    }

    private void HandleDie()
    {
        MinionDie();
        // Logic to handle the Die state
        Debug.Log("Handling character death in SomeComponent.");
    }
    public void MinionDie()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            if (raycastHit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                Vector3 explosionPosition = raycastHit.point + explosionPositionOffset;
                rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
        }

        Instantiate(destroyedMinion, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        if (characterGeneral != null)
        {
            characterGeneral.OnDie.RemoveListener(HandleDie);
        }
    }
}