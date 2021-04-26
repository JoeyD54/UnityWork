using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToBeCollected : MonoBehaviour
{
	public GameObject gold, purse, apple, meat;
	public Item.ItemType type;
	public Item item;

	void Start()
	{

		item = new Item(type);
		GameObject g = new GameObject();

		switch (type)
		{
			case Item.ItemType.GOLD: g = gold; break;
			case Item.ItemType.PURSE: g = purse; break;
			case Item.ItemType.APPLE: g = apple; break;
			case Item.ItemType.MEAT: g = meat; break;
			default: break;
		}

		GameObject g1 = Instantiate(g, transform.position, Quaternion.identity);
		g1.transform.parent = transform;

	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
