using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(FieldOfView))]
[RequireComponent(typeof(PatrolPoints))]
public class GhostAgentController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private FieldOfView fov;
    private PatrolPoints patrolPoints;

    private Transform _currTarget;
    private Coroutine moveAndIdleCoroutine;

    private bool isChasing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        patrolPoints = GetComponent<PatrolPoints>();
        fov = GetComponent<FieldOfView>();

        moveAndIdleCoroutine = StartCoroutine(MoveAndIdle());
    }

    void Update()
    {
        if (isChasing)
        {
            agent.SetDestination(_currTarget.position);
            if (!IsFacingTarget(transform, agent.steeringTarget, 0.75f))
            {
                StartCoroutine(TurnTowards());
            }
            else
            {
                animator.SetInteger("moving", 1);
                agent.isStopped = false;
            }
        }
    }

    private IEnumerator MoveAndIdle()
    {
        while (true)
        {
            if (!isChasing)
            {
                animator.SetInteger("moving", 0);
                agent.isStopped = true;
                yield return new WaitForSeconds(3f);

                _currTarget = patrolPoints.next_point();
                agent.SetDestination(_currTarget.position);
                agent.isStopped = false;

                while (agent.remainingDistance > agent.stoppingDistance)
                {
                    if (!IsFacingTarget(transform, agent.steeringTarget, 0.75f) && !agent.isOnOffMeshLink)
                    {
                        agent.isStopped = true;
                        animator.SetInteger("moving", 0);
                        yield return StartCoroutine(TurnTowards());
                        agent.isStopped = false;
                    }
                    else
                    {
                        animator.SetInteger("moving", 1);
                        agent.isStopped = false;
                        DetectPlayer();
                        yield return null;
                    }
                }
            }
            yield return null;
        }
    }

    bool IsFacingTarget(Transform origin, Vector3 target, float threshold = 0.95f)
    {
        Vector3 direction = (target - origin.position).normalized;
        float dotProduct = Vector3.Dot(origin.forward, direction);
        return dotProduct >= threshold;
    }

    private IEnumerator TurnTowards()
    {
        while (!IsFacingTarget(transform, agent.steeringTarget, 0.95f))
        {
            Vector3 direction = (agent.steeringTarget - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
            yield return null;
        }
    }

    private void DetectPlayer()
    {
        if (fov.ContainsPlayer(out Transform player))
        {
            isChasing = true;
            _currTarget = player;
            if (moveAndIdleCoroutine != null)
            {
                StopCoroutine(moveAndIdleCoroutine);
                moveAndIdleCoroutine = null;
            }
        }
    }

    public void StartChase()
    {
        isChasing = true;
        _currTarget = GameObject.FindGameObjectWithTag("Player").transform;
        if (moveAndIdleCoroutine != null)
        {
            StopCoroutine(moveAndIdleCoroutine);
            moveAndIdleCoroutine = null;
        }
    }
}