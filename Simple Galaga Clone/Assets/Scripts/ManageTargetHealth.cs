using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTargetHealth : MonoBehaviour
{
	public int health, type;
	public static int TARGET_BOULDER_0 = 0;
	public static int TARGET_BOULDER_1 = 1;
	public static int TARGET_BOULDER_2 = 2;

	public int boulderPoints;

	public bool isBlinking = false;
	public float timer;
	public Color previousColor;
	public Color hitColor;
	public GameObject explosion;

	// Start is called before the first frame update
	void Start()
	{
		if (type == TARGET_BOULDER_0)
		{
			boulderPoints = 1;
			health = 10;
		}
		if (type == TARGET_BOULDER_1) {
			boulderPoints = 2;
			health = 40;
			hitColor = new Color32(80, 0, 80, 255);
		}
		if (type == TARGET_BOULDER_2)
		{
			boulderPoints = 3;
			health = 60;
			hitColor = new Color32(0, 0, 80, 255);
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (isBlinking)
		{
			timer += Time.deltaTime;

			if (timer >= .2 && health > 30)
			{
				isBlinking = false;
				GetComponent<SpriteRenderer>().color = previousColor;
				timer = 0;
			}
			if (timer >=.2 && health <= 30)
				GetComponent<SpriteRenderer>().color = previousColor;
			if (timer >= .4 && health <= 30) { 
					GetComponent<SpriteRenderer>().color = hitColor;
					timer = 0;
			}
		}
    }

	public void GotHit(int damage)
	{
		health -= damage;
		if (health <= 0)
			DestroyTarget();
		previousColor = GetComponent<SpriteRenderer>().color;
		GetComponent<SpriteRenderer>().color = hitColor;
		isBlinking = true;
	}

	public void DestroyTarget()
	{
		GameObject exp = (GameObject)(Instantiate(explosion, transform.position, Quaternion.identity));
		Destroy(exp, .5f);
		GameObject.Find("player").GetComponent<ManagePlayerHealth>().inscreaseScore(boulderPoints);
		Destroy(gameObject);
	}
}
