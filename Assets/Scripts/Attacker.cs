using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attacker : MonoBehaviour
{
 public int damage = 10;

 private void OnTriggerEnter(Collider other)
 {
    // if (other.gameObject.CompareTag("Bullet"))
    // {
    //     other.GetComponent<CharacterGeneral>()?.TakeDamage(this);
    // }
    other.GetComponent<CharacterGeneral>()?.TakeDamage(this);
   //  Debug.Log("Attacker: OnTriggerEnter");
 }
}
