using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
	public static int TARGET_BOULDER_SPEED_0 = 5;
	public static int TARGET_BOULDER_SPEED_1 = 2;
	public static int TARGET_BOULDER_SPEED_2 = 1;

	public int type;
	// Start is called before the first frame update
	void Start()
    {
		if (type == TARGET_BOULDER_SPEED_0)
			GetComponent<Rigidbody2D>().velocity = Vector2.down * TARGET_BOULDER_SPEED_0;
		if (type == TARGET_BOULDER_SPEED_1)
			GetComponent<Rigidbody2D>().velocity = Vector2.down * TARGET_BOULDER_SPEED_1;
		if (type == TARGET_BOULDER_SPEED_2)
			GetComponent<Rigidbody2D>().velocity = Vector2.down * TARGET_BOULDER_SPEED_2;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
