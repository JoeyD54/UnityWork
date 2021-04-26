using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTargetHealth : MonoBehaviour
{
	[Range(100, 1000)]
	public int health = 100;

	float hiTimer;
	bool hitFlash;
	float alpha;

    // Start is called before the first frame update
    void Start()
    {
		alpha = 0.0f;

		gameObject.transform.Find("Sphere").GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, alpha);
    }

    // Update is called once per frame
    void Update()
    {
		if (hitFlash)
		{
			alpha -= Time.deltaTime;

			gameObject.transform.Find("Sphere").GetComponent<Renderer>().material.color = new Color(1, 0, 0, alpha);

			if(alpha <= 0)
			{
				hitFlash = false;
				alpha = 0;
			}
		}
    }

	public void SetHealth(int health)
	{
		this.health = health;

		if(health <= 0)
		{
			this.health = 0;
			DestroyTarget();

		}
	}

	public int GetHealth()
	{
		return (this.health);
	}

	private void DestroyTarget()
	{
		GetComponent<ControlNPC>().Dies();

		Destroy(gameObject, 5);

		GameObject.Find("gameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.destroy_one,
				gameObject.name);
	}
	
	public void DecreaseHealth(int increment)
	{
		SetHealth(this.health - increment);

		hitFlash = true;
		alpha = 0.5f;

		gameObject.GetComponent<ControlNPC>().SetGuardType(ControlNPC.GUARD_TYPE.CHASER);

		AlertOtherGuards();
	}

	void AlertOtherGuards()
	{
		GameObject[] otherGuards;

		otherGuards = GameObject.FindGameObjectsWithTag("target");

		for(int i = 0; i < otherGuards.Length; i++)
		{
			otherGuards[i].GetComponent<ControlNPC>().SetGuardType(ControlNPC.GUARD_TYPE.CHASER);
		}
	}

}
