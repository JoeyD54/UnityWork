using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "target")
		{
			//Destroy(collision.gameObject);
			collision.gameObject.GetComponent<ManageTargetHealth>().GotHit(10);
			Destroy(gameObject);
			
		}
		if(collision.gameObject.tag == "bonus")
		{
			Destroy(gameObject);
			Destroy(collision.gameObject);
		}
	}
}
