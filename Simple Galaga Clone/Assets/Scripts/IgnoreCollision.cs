using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		//Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GameObject.Find("npc1").GetComponent<PolygonCollider2D>());
		int layer1 = GameObject.Find("npc1").layer;
		int layer2 = gameObject.layer;
		Physics2D.IgnoreLayerCollision(layer1, layer2, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
