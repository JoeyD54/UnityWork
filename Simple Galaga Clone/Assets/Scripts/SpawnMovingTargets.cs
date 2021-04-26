using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMovingTargets : MonoBehaviour
{
	float timer = 0;
	public GameObject boulder;
	public GameObject bonus;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;

		float range = Random.Range(-10, 10);
		Vector3 newPosition = new Vector3(GameObject.Find("player").transform.position.x + range, transform.position.y, 0);

		float respawnTime = 5 / GameObject.Find("gameManager").GetComponent<ManageShooterGame>().difficulty;

		//if (timer >= 1)
		if(timer >= respawnTime)
		{
			float typeOfObjectSpawn = Random.Range(0, 99);
			GameObject t;

			if (typeOfObjectSpawn >= 40)
			{
				float randomType = Random.Range(0, 20);
				if (randomType <= 10)
				{
					boulder.GetComponent<SpriteRenderer>().color = Color.red;
					t = (GameObject)(Instantiate(boulder, newPosition, Quaternion.identity));
					t.GetComponent<ManageTargetHealth>().type = ManageTargetHealth.TARGET_BOULDER_0;
					t.GetComponent<MovingTarget>().type = MovingTarget.TARGET_BOULDER_SPEED_0;
					timer = 0;
					print("spawning BOULDER 0");
				}
				if (randomType > 10 && randomType < 16)
				{
					boulder.GetComponent<SpriteRenderer>().color = new Color(255, 0, 255, 255);
					t = (GameObject)(Instantiate(boulder, newPosition, Quaternion.identity));
					t.GetComponent<ManageTargetHealth>().type = ManageTargetHealth.TARGET_BOULDER_1;
					t.GetComponent<MovingTarget>().type = MovingTarget.TARGET_BOULDER_SPEED_1;
					timer = 0;
					print("spawning BOULDER 1");
				}
				if (randomType >= 16)
				{
					boulder.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255, 255);
					t = (GameObject)(Instantiate(boulder, newPosition, Quaternion.identity));
					t.GetComponent<ManageTargetHealth>().type = ManageTargetHealth.TARGET_BOULDER_2;
					t.GetComponent<MovingTarget>().type = MovingTarget.TARGET_BOULDER_SPEED_2;
					timer = 0;
					print("spawning BOULDER 2");
				}
			}
			else
			{
				t = (GameObject)(Instantiate(bonus, newPosition, Quaternion.identity));
				t.GetComponent<MovingTarget>().type = MovingTarget.TARGET_BOULDER_SPEED_1;
				print("spawning BONUS");
			}
			
		}
	}
}
