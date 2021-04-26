using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagePlayerHealth : MonoBehaviour
{
	public float timerForShield;
	public bool startInvincibility;
	public int score;
	public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
		score = 0;
		GameObject.Find("shield").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("scoreUI").GetComponent<Text>().text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
		if (startInvincibility)
		{
			timerForShield += Time.deltaTime;
			print("Invincible for " + timerForShield + " more seconds");
			if (timerForShield >= 20)
			{
				print("Turning invincibility off");
				timerForShield = 0;
				startInvincibility = false;
				GameObject.Find("shield").GetComponent<SpriteRenderer>().enabled = false;
			}
		}
    }


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "bonus")
		{
			Destroy(collision.gameObject);
			startInvincibility = true;
			GameObject.Find("shield").GetComponent<SpriteRenderer>().enabled = true;
		}
		if ((collision.gameObject.tag == "target" || collision.gameObject.tag == "bullet") && !startInvincibility)
		{
			Destroy(collision.gameObject);
			print("Restarting");
			GetComponent<AudioSource>().clip = hitSound;
			GetComponent<AudioSource>().Play();
			Invoke("DestroyPlayer", 1f);
		}
	}

	void DestroyPlayer()
	{

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void inscreaseScore(int addedPoints)
	{
		score += addedPoints;
		GameObject.Find("scoreUI").GetComponent<Text>().text = "Score: " + score;
	}
}
