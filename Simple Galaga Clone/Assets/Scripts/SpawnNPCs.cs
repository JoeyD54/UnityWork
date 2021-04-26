using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNPCs : MonoBehaviour
{
	public GameObject npc1;
	private float timer, respawnTime;

	// Start is called before the first frame update
	void Start()
    {
		        
    }

    // Update is called once per frame
    void Update()
    {
		timer += Time.deltaTime;

		float respawnTime = 5 / GameObject.Find("gameManager").GetComponent<ManageShooterGame>().difficulty;

		//if (timer >= 1)
		if (timer >= respawnTime)
		{
			timer = 0;
			SpawnNPC(npc1);
		}
    }

	void SpawnNPC(GameObject typerOfNPC)
	{
		float range = Random.Range(-10, 10); // screen width
		Vector3 newPosition = new Vector3(GameObject.Find("player").transform.position.x + range, transform.position.y, 0);
		GameObject newNPC = (GameObject)(Instantiate(npc1, newPosition, Quaternion.identity));
		newNPC.transform.Rotate(new Vector3(0, 0, 180));
		newNPC.name = "npc1";
	}
}
