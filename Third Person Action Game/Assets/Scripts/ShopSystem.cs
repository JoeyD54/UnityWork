using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
	public List<Item> shopItems;
	public GameObject shopItemComponent;
	GameObject[] shopItemComponents;

	int totalPurchase = 0;
	int initialMoney = 0;
	public int moneyLeft;
	float topLeftX, topLeftY;
	bool discountApplied = false;


	// Start is called before the first frame update
	void Start()
	{
		//Init();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Init()
	{
		//initialMoney = 1000;

		initialMoney = GameObject.Find("Player").GetComponent<InventorySystem>().GetMoney();
		moneyLeft = initialMoney;
		topLeftX = 0;
		topLeftY = 400;

		shopItems = new List<Item>();

		shopItems.Add(new Item(Item.ItemType.RUBY));
		shopItems.Add(new Item(Item.ItemType.BLUE_DIAMOND));
		shopItems.Add(new Item(Item.ItemType.RED_DIAMOND));
		shopItems.Add(new Item(Item.ItemType.MEAT));
		shopItems.Add(new Item(Item.ItemType.APPLE));
		shopItems.Add(new Item(Item.ItemType.SWORD));
		shopItems.Add(new Item(Item.ItemType.BATON));
		shopItems.Add(new Item(Item.ItemType.HOT_DOG));
		shopItems.Add(new Item(Item.ItemType.SPEAR));

		shopItemComponents = new GameObject[shopItems.Count];

		GameObject.Find("shopMoneyLeftValue").GetComponent<Text>().text = "" + initialMoney;
		int purseCount = GameObject.Find("Player").GetComponent<InventorySystem>().PurseCount();

		GameObject.Find("shopDiscountValue").GetComponent<Text>().text = "" + purseCount;

		//print("set shop");
		for (int i = 0; i < shopItems.Count; i++)
		{
			SetupShopItemCompenent(i);
		}
	}

	void SetupShopItemCompenent(int index)
	{
		//print("setting up shop: " + index);

		shopItems[index].nb = 0;

		shopItemComponents[index] = Instantiate(shopItemComponent, transform.position, Quaternion.identity);

		shopItemComponents[index].GetComponent<ShopItem>().index = index;

		float width = shopItemComponents[index].transform.Find("itemBg").GetComponent<RectTransform>().sizeDelta.x;
		float borderAroundEachItem = 1.05f;

		shopItemComponents[index].name = "shopItem_" + index + shopItems[index].name;
		shopItemComponents[index].transform.Find("itemLabel").GetComponent<Text>().text = shopItems[index].name + "($" + shopItems[index].price + ")";
		shopItemComponents[index].transform.Find("itemQty").GetComponent<Text>().text = "" + shopItems[index].nb;

		shopItemComponents[index].transform.parent = GameObject.Find("shopUI").transform;
		shopItemComponents[index].transform.localPosition = new Vector3(topLeftX + (index % 3) * (width * borderAroundEachItem),
			topLeftY - (index / 3) * width * borderAroundEachItem, 0.0f);
		shopItemComponents[index].transform.Find("itemImage").GetComponent<RawImage>().texture = shopItems[index].GetTexture();
	}

	public void UpdateTotal(int itemIndex, int itemAmount)
	{
		shopItems[itemIndex].nb = itemAmount;
		int tempTotal;

		tempTotal = CalculateTotal();

		GameObject.Find("shopTotalValue").GetComponent<Text>().text = "" + tempTotal;

		totalPurchase = tempTotal;

		moneyLeft = initialMoney - tempTotal;

		GameObject.Find("shopMoneyLeftValue").GetComponent<Text>().text = "" + moneyLeft;
	}

	int CalculateTotal()
	{
		int temp = 0;

		for (int i = 0; i < shopItems.Count; i++)
		{
			temp += shopItems[i].nb * shopItems[i].price;
		}

		return temp;
	}

	public bool CanAddItemsToCart(int index)
	{
		if (moneyLeft >= shopItems[index].price && shopItems[index].nb < shopItems[index].maxNb)
			return true;
		else
			return false;
	}
	public void ApplyDiscount()
	{
		int purseCount = GameObject.Find("player").GetComponent<InventorySystem>().PurseCount();
		if (purseCount > 0 && discountApplied == false)
		{
			for(int i = 0; i < shopItems.Count; i++)
			{
				discountApplied = true;
				purseCount = GameObject.Find("player").GetComponent<InventorySystem>().SubtractPurse();
				GameObject.Find("shopDiscountValue").GetComponent<Text>().text = "" + purseCount;

				var temp = shopItems[i].price;
				temp /= 2;
				shopItems[i].price = temp;

				//totalPurchase /= 2;
				GameObject.Find("shopDiscountAppliedMsg").GetComponent<Text>().text = "Discount Applied!";

				SetupShopItemCompenent(i);
			}
		}
	}
}
