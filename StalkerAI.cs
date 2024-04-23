using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StalkerAI : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;             
    public float startWaitTime = 3;                  //  wait time until next action
    public float timeToRotate = 2;                  //  wait time when the enemy detect near the player without seeing
    public float speedWalk = 2;                     
    public float speedRun = 4;                     // speed on chasing player

    public float viewRadius = 8;                    
    public float viewAngle = 90;                    
    public LayerMask playerMask;                    
    public LayerMask obstacleMask;                  
    public float meshResolution = 1.0f;             //  How many rays will cast per degree
    public int edgeIterations = 4;                  
    public float edgeDistance = 0.5f;               


    public Transform[] waypoints;                   //  patrol route
    int CurrentWaypointIndex;                      //   waypoint where the enemy is going to

    Vector3 playerLastPosition = Vector3.zero;      //  last position when player near the enemy
    Vector3 PlayerPosition;                        //   last position of player when player seen by the enemy

    float StartWaitTime;                                 //  Variable of the wait time that makes the delay
    float TimeToRotate;                                 //  Variable of the wait time to rotate when the player is near that makes the delay
    bool playerInRange;                                //  If the player is in range of vision, state of chasing
    bool PlayerNear;                                  //  If the player is near, state of hearing
    bool IsPatrol;                                   //  If the enemy is patrol, state of patroling
    bool IsCaughtPlayer;                            //  if the enemy has caught the player

    void Start()
    {
        PlayerPosition = Vector3.zero;
        IsPatrol = true;
        IsCaughtPlayer = false;
        playerInRange = false;
        PlayerNear = false;
        StartWaitTime = startWaitTime;                 //  Set the wait time variable that will change
        TimeToRotate = timeToRotate;

        CurrentWaypointIndex = 0;                 //  Set the initial waypoint
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;             //  Set the navemesh speed with the normal speed of the enemy
        navMeshAgent.SetDestination(waypoints[CurrentWaypointIndex].position);    //  Set the destination to the first waypoint
    }

    private void Update()
    {
        EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

        if (!IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    private void Chasing()
    {
        //  The enemy is chasing the player
        PlayerNear = false;                       //  Set false that the player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

        if (!IsCaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(PlayerPosition);          //  set the destination of the enemy to the player location
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)    //  Control if the enemy arrive to the player location
        {
            if (StartWaitTime <= 0 && !IsCaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                IsPatrol = true;
                PlayerNear = false;
                Move(speedWalk);
                TimeToRotate = timeToRotate;
                StartWaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    //  Wait if the current position is not the player position
                    Stop();
                StartWaitTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (PlayerNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                Stop();
                TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            PlayerNear = false;           //  The player is no near when the enemy is platroling
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (StartWaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    StartWaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    StartWaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void OnAnimatorMove()
    {
        //Patrol walking animation
        //Stop iddle animation
        //Chasing running animation
    }

    public void NextPoint()
    {
        CurrentWaypointIndex = (CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void CaughtPlayer()
    {
        IsCaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (StartWaitTime <= 0)
            {
                PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
                StartWaitTime = startWaitTime;
                TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                StartWaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    this.playerInRange = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
                    IsPatrol = false;                     //  Change the state to chasing the player
                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    this.playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {

                //If the player is further than the view radius, then the enemy will no longer keep the player's current position or the enemy is a safe zone, the enemy will no chase
                 

                this.playerInRange = false;                //  Change the sate of chasing
            }
            if (this.playerInRange)
            {
                
                //If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 
                PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
            }
        }
    }
}
