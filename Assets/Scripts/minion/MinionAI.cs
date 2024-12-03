using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour

{
  Transform target;
  NavMeshAgent agent;

  public Animator animator;

  public LayerMask whatIsGround;
  // public LayerMask whatIsPlayer;
  //state
  public float sightRange = 10f;
  public float attackRange = 10f;
  bool playerInsiight, playerInAttack;


  //patroling
  public Vector3 walkPoint;
  bool walkPointSet;
  public float walkPointRange;

  //attacking
  public float timeBetweenAttack;
  bool alreadyAttacked;
  public MinionGun minionGun;
  public GameObject MiniGun;


  void Start()
  {
    target = PlayerManager.instance.player.transform;
    agent = GetComponent<NavMeshAgent>();
    minionGun = GetComponent<MinionGun>();
  }

  void Update()
  {
    float distance = Vector3.Distance(target.position, transform.position);
    if (distance >= sightRange)
    {
      playerInsiight = false;
      playerInAttack = false;
    }
    if (distance <= sightRange)
    {
      playerInsiight = true;
      playerInAttack = false;
    }
    if (distance <= attackRange)
    {
      playerInsiight = true;
      playerInAttack = true;
    }
    if (!playerInsiight && !playerInAttack)
    {
      Patroling();
    }
    if (playerInsiight && !playerInAttack)
    {
      Chasing();
    }
    if (playerInsiight && playerInAttack)
    {
      Attacking();
    }

  }
  void Patroling()//巡邏
  {
    if (!walkPointSet)
    {
      SearchWalkPoint();
    }
    if (walkPointSet)
    {
      agent.SetDestination(walkPoint);
      animator.SetBool("Walk", true);
      animator.SetBool("Attack", false);
    }
    Vector3 distanceToWalkPoint = transform.position - walkPoint;

    if (distanceToWalkPoint.magnitude < 1f)//超出範圍
    {
      walkPointSet = false;
    }
    if (MiniGun != null && MiniGun.activeSelf)
    {
      HideMiniGun();
    }

  }
  private void SearchWalkPoint()//設定巡邏範圍
  {
    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    float randomX = Random.Range(-walkPointRange, walkPointRange);
    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
    {
      walkPointSet = true;
    }
  }
  void Attacking()//攻擊
  {

    MiniGun.SetActive(true);
    agent.SetDestination(transform.position);
    Vector3 targetXZ = new(PlayerManager.instance.player.transform.position.x, 0, PlayerManager.instance.player.transform.position.z);
    
    transform.LookAt(targetXZ);
    animator.SetBool("Walk", false);
    animator.SetBool("Attack", true);
    // 顯示 Minigun
  }
  void Chasing()//追隨
  {
    agent.SetDestination(target.position);
    animator.SetBool("Walk", true);
    animator.SetBool("Attack", false);
    if (MiniGun != null && MiniGun.activeSelf)
    {
      HideMiniGun();
    }

  }
  void HideMiniGun()//隱藏子彈物件(不攻擊)
  {
    if (MiniGun != null && MiniGun.activeSelf)
    {
      MiniGun.SetActive(false);
    }
  }
  void OnDrawGizmos()//畫出視野半徑
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, sightRange);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, attackRange);
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, walkPointRange);
  }
}