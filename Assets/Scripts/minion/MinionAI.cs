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
  public CharacterGeneral TakeDamage;
  //state
  public float sightRange = 10f;
  public float attackRange = 10f;
  bool playerInsight, playerInAttack;


  //patrolling
  public Vector3 walkPoint;
  bool walkPointSet;
  public float walkPointRange;
  public Transform patrolCenter;    // 巡邏中心點
  public float patrolRadius = 10f;  // 巡邏半徑
  //attacking
  public float timeBetweenAttack;
  bool alreadyAttacked;
  // public MinionGun minionGun;
  public GameObject MiniGun;


  void Start()
  {
    target = PlayerManager.instance.player.transform;
    agent = GetComponent<NavMeshAgent>();
    // minionGun = GetComponent<MinionGun>();
    Patroling();
    MiniGun.SetActive(false);
  }

  void Update()//狀態
  {
    float distance = Vector3.Distance(target.position, transform.position);
    // if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
    // {
    //   Patroling();
    //   Debug.Log("Patroling");
    // }
    if (distance >= sightRange)
    {
      playerInsight = false;
      playerInAttack = false;
    }
    if (distance <= sightRange)
    {
      playerInsight = true;
      playerInAttack = false;
    }
    if (distance <= attackRange)
    {
      playerInsight = true;
      playerInAttack = true;
    }
    if (!playerInsight && !playerInAttack)
    {
      Patroling();
      // Debug.Log("Patroling");
    }
    if (playerInsight && !playerInAttack)
    {
      Chasing();
      // Debug.Log("Chasing");
    }
    if (playerInsight && playerInAttack)
    {
      Attacking();
      // Debug.Log("Attacking");
    }
  }
  void Patroling()
  {
    Vector3 randomPoint = RandomPointAroundCenter();
    NavMeshHit hit;
    MiniGun.SetActive(false);
    animator.SetBool("Walk", true);
    animator.SetBool("Attack", false);

    // 確保目標點位於有效的 NavMesh 範圍內
    if (NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, NavMesh.AllAreas))
    {
      agent.SetDestination(hit.position);
    }
  }

  Vector3 RandomPointAroundCenter()
  {
    // 在圓周上隨機生成一個點
    float angle = Random.Range(0f, Mathf.PI * 2); // 隨機角度
    Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * patrolRadius;
    return patrolCenter.position + offset;       // 偏移量加到中心點
  }



  void Attacking()//攻擊
  {
    agent.SetDestination(transform.position);
    Vector3 targetXZ = new(PlayerManager.instance.player.transform.position.x, transform.position.y, PlayerManager.instance.player.transform.position.z);
    transform.LookAt(targetXZ);
    animator.SetBool("Walk", false);
    animator.SetBool("Attack", true);
    MiniGun.SetActive(true);
    // 顯示 Minigun
  }
  void Chasing()//追隨
  {
    agent.SetDestination(target.position);
    animator.SetBool("Walk", true);
    animator.SetBool("Attack", false);
    // if (MiniGun != null && MiniGun.activeSelf)
    // {
    //   HideMiniGun();
    // }
    MiniGun.SetActive(false);
  }
  // void HideMiniGun()
  // {
  //   if (MiniGun != null && MiniGun.activeSelf)
  //   {
  //     MiniGun.SetActive(false);
  //   }
  // }
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