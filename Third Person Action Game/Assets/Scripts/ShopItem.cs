using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
	string name;
	int price, quantity;
	public int index;

    // Start is called before the first frame update
    void Start()
    {
		quantity = 0;
		UpdateQuantityLabel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void IncreaseQuantity()
	{
		if (!canClick()) return;

		quantity++;
		print(quantity);
		UpdateQuantityLabel();
	}

	public void DecreaseQuantity()
	{
		quantity--;
		if (quantity < 0) quantity = 0;

		print(quantity);

		UpdateQuantityLabel();
	}

	void UpdateQuantityLabel()
	{
		transform.Find("itemQty").GetComponent<Text>().text = "" + quantity;

		GameObject.Find("shopSystem").GetComponent<ShopSystem>().UpdateTotal(index, quantity);
	}

	bool canClick()
	{
		//return true;
		
		GameObject shopSystem = GameObject.Find("shopSystem");

		return shopSystem.GetComponent<ShopSystem>().CanAddItemsToCart(this.index);
	}
}
