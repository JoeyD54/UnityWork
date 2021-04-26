using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlNPC : MonoBehaviour
{
	public List<GameObject> wayPoints = new List<GameObject>();

	public enum GUARD_TYPE
	{
		IDLE = 0,
		PATROLLER = 1,
		CHASER = 2,
		SEARCHER = 3
	};
	[SerializeField]
	public GUARD_TYPE guardType;

	[SerializeField]
	public GameObject player, objectToCollect;

	[SerializeField]
	bool canHearPlayer;

	int wayPointIndex = 0;
	[HideInInspector]
	public Animator anim;
	AnimatorStateInfo info;

	private Ray rayFromNPC;
	private RaycastHit hit;
	private string objectInSight;

	public Vector3 direction;
	public bool isInTheFieldOfView;
	public bool noObjectBetweenNPCAndPlayer = false;
	public float distance;

	bool playerActivated;

    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();

		if (guardType == GUARD_TYPE.IDLE)
			anim.SetBool("isIdleGuard", true);
		else if (guardType == GUARD_TYPE.PATROLLER)
			anim.SetBool("isPatrolGuard", true);
		else if (guardType == GUARD_TYPE.CHASER)
			anim.SetBool("isChaserGuard", true);
		else if (guardType == GUARD_TYPE.SEARCHER)
			anim.SetBool("isSearching", true);
	}

    // Update is called once per frame
    void Update()
    {
		if (!playerActivated)
		{
			player = GameObject.Find("Player");
			playerActivated = true;
		}


		//print("My name is " + gameObject.name + "\nI am currently " + guardType);
		info = anim.GetCurrentAnimatorStateInfo(0);

		if(info.IsName("Idle") || info.IsName("Patrol"))
		{
			if (isNearPlayer() && canHearPlayer)
				SetGuardType(GUARD_TYPE.CHASER);
		}

		if (isNearPlayer())
			anim.SetBool("isWithinAttackRange", true);
		else
			anim.SetBool("isWithinAttackRange", false);

		if (info.IsName("Patrol"))
		{
			GetComponent<NavMeshAgent>().isStopped = false;

			if (Vector3.Distance(transform.position, wayPoints[wayPointIndex].transform.position) < 1.5f)
				wayPointIndex++;
			if (wayPointIndex > wayPoints.Count)
				wayPointIndex = 0;

			GetComponent<NavMeshAgent>().SetDestination(wayPoints[wayPointIndex].transform.position);
			print("moving toward: " + wayPoints[wayPointIndex]);


			Debug.DrawRay(transform.position + Vector3.up * .3f, transform.forward * 100, Color.blue);
			if(Physics.Raycast(transform.position + Vector3.up * .3f, transform.forward * 100, out hit))
			{
				objectInSight = hit.collider.gameObject.tag;
				print("Patroller sees: " + objectInSight);
				if(objectInSight == "itemToBeCollected")
				{
					print("Collecting garbage");
					GetComponent<NavMeshAgent>().SetDestination(objectToCollect.transform.position);

					if (Vector3.Distance(objectToCollect.transform.position, transform.position) < 2.0f)
						Destroy(objectToCollect);
				}
			}


			/*
			if (anim.GetBool("canSeeItem") == true)
			{
				rayFromNPC.origin = transform.position;
				rayFromNPC.direction = transform.forward;
				objectInSight = "";

				direction = (GameObject.Find("objectToCollect").transform.position - transform.position).normalized;
				print(direction);
				isInTheFieldOfView = (Vector3.Dot(transform.forward.normalized, direction) > .7);

				Debug.DrawRay(transform.position, direction * 100, Color.green);
				Debug.DrawRay(transform.position, transform.forward * 100, Color.blue);

				if (Physics.Raycast(transform.position, direction * 100, out hit))
				{
					objectInSight = hit.collider.gameObject.tag;
					print(objectInSight);

					if (hit.collider.gameObject.tag == "itemToBeCollected")
						noObjectBetweenNPCAndPlayer = true;
					else
						noObjectBetweenNPCAndPlayer = false;

					if (noObjectBetweenNPCAndPlayer && isInTheFieldOfView)
					{
						anim.SetBool("playerFound", true);
					}
				}
				else
					anim.SetBool("playerFound", false);
			}
			*/
		}

		if (info.IsName("Chase"))
		{
			GetComponent<NavMeshAgent>().speed = 2.5f;
			GetComponent<NavMeshAgent>().isStopped = false;
			GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
		}
		else if (info.IsName("Attack"))
		{
			transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
			GetComponent<NavMeshAgent>().isStopped = true;

			if(info.normalizedTime % 1.0 >= .98 && isVeryNearPlayer())
			{
				player.GetComponent<ControlPlayer>().DecreaseHealth(10);
			}
		}
		else if (info.IsName("Search"))
		{
			Debug.DrawRay(transform.position + Vector3.up * .3f, transform.forward * 100, Color.red);

			if(Physics.Raycast(transform.position + Vector3.up * .3f, transform.forward * 100, out hit))
			{
				objectInSight = hit.collider.gameObject.tag;
				//print("Saw " + objectInSight);
				if(objectInSight == "Player")
				{
					print("Player found. Chasing");
					anim.SetBool("playerFound", true);

				}
			}
		}
		/*
		else if (info.IsName("Idle"))
		{
			if (isNearPlayer())
				SetGuardType(GUARD_TYPE.CHASER);
		}
		*/
		
    }

	bool isNearPlayer()
	{
		if (Vector3.Distance(transform.position, player.transform.position) < 2.0f)
			return true;
		else
			return false;
	}

	public void Dies()
	{
		anim.SetTrigger("isDying");
	}

	bool isVeryNearPlayer()
	{
		if (Vector3.Distance(transform.position, player.transform.position) < 1.5f)
			return true;
		else
			return false;
	}

	public void SetGuardType(GUARD_TYPE newType)
	{
		guardType = newType;
		anim.SetBool("isIdleGuard", false);
		anim.SetBool("isPatrolGuard", false);
		anim.SetBool("isChaserGuard", false);

		switch (guardType)
		{
			case GUARD_TYPE.IDLE: anim.SetBool("isIdleGuard", true); break;
			case GUARD_TYPE.PATROLLER: anim.SetBool("isPatrolGuard", true); break;
			case GUARD_TYPE.CHASER: anim.SetBool("isChaserGuard", true); break;
			default:anim.SetBool("isIdleGuard", true); break;
		}
	}
}
