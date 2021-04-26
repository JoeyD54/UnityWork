using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
	List<Item> playerInventory;

	int currentInventoryIndex = 0;
	bool isVisible = false;

	GameObject inventoryText, inventoryImage, inventoryDescription, inventoryPanel;

    // Start is called before the first frame update
    void Start()
    {
		inventoryText = GameObject.Find("inventoryText");
		inventoryImage = GameObject.Find("inventoryImage");
		inventoryDescription = GameObject.Find("inventoryDescription");
		inventoryPanel = GameObject.Find("inventoryPanel");

		DisplayUI(false);

		playerInventory = new List<Item>();
		playerInventory.Add(new Item(Item.ItemType.GOLD));
		playerInventory.Add(new Item(Item.ItemType.PURSE));

		playerInventory[0].nb = 300;

		checkInventory();
	}

    // Update is called once per frame
    void Update()
    {
		if (isVisible)
		{
			DisplayUI(true);

			Item currentItem = playerInventory[currentInventoryIndex];

			GameObject.Find("inventoryText").GetComponent<Text>().text = currentItem.name + "[" + currentItem.nb + "]";
			GameObject.Find("inventoryDescription").GetComponent<Text>().text = currentItem.description + "\n\nPress [U] to Select";

			if(currentItem.healthBenefits != 0)
			{
				GameObject.Find("inventoryDescription").GetComponent<Text>().text = currentItem.description + 
					" " + "\nHeals: " + currentItem.healthBenefits + "\nPress [U] to Select";
			}

			GameObject.Find("inventoryImage").GetComponent<RawImage>().texture = currentItem.GetTexture();

			if (Input.GetKeyDown(KeyCode.I))
				currentInventoryIndex++;

			

			if(currentInventoryIndex >= playerInventory.Count)
			{
				currentInventoryIndex = 0;
				isVisible = false;
				DisplayUI(false);
			}
			if (Input.GetKeyDown(KeyCode.U))
			{
				if(playerInventory[currentInventoryIndex].familyType == Item.ItemFamilyType.FOOD)
				{
					GetComponent<ControlPlayer>().IncreaseHealth(playerInventory[currentInventoryIndex].healthBenefits);
					SubtractFood();
					currentInventoryIndex = 0;
					isVisible = false;
					DisplayUI(false);
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.I))
			isVisible = true;
		
    }

	void checkInventory()
	{
		for(int i = 0; i < playerInventory.Count; i++)
		{
			print(playerInventory[i].ItemInfo());
		}
	}

	void DisplayUI(bool toggle)
	{
		inventoryText.SetActive(toggle);
		inventoryPanel.SetActive(toggle);
		inventoryImage.SetActive(toggle);
		inventoryDescription.SetActive(toggle);

	}

	public bool UpdateItem(Item.ItemType type, int nbItemsToAdd)
	{
		bool foundSimilarItem = false;

		for(int i = 0; i < playerInventory.Count; i++)
		{
			if(playerInventory[i].type == type)
			{
				if(playerInventory[i].nb + nbItemsToAdd <= playerInventory[i].maxNb)
				{
					playerInventory[i].nb += nbItemsToAdd;
					foundSimilarItem = true;
					if(playerInventory[i].name == "Meat" && playerInventory[i].nb == 5)
					{
						GameObject.Find("gameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.collect_five,
							playerInventory[i].name);
					}
					break;
				}
				else
				{
					return false;
				}
			}
		}
		if (!foundSimilarItem)
		{
			playerInventory.Add(new Item(type));
			playerInventory[playerInventory.Count - 1].nb = nbItemsToAdd;
		}
		return true;
	}
	
	public int GetMoney()
	{
		for(int i = 0; i < playerInventory.Count; i++)
		{
			if(playerInventory[i].type == Item.ItemType.GOLD)
			{
				return (playerInventory[i].nb);
			}
		}
		return 0;
	}

	public int GetMeat()
	{
		for (int i = 0; i < playerInventory.Count; i++)
		{
			if (playerInventory[i].type == Item.ItemType.MEAT)
			{
				return (playerInventory[i].nb);
			}
		}
		return 0;
	}

	public void AddPurchasedItems(List<Item> purchasedItems)
	{
		bool t;

		for(int i = 0; i < purchasedItems.Count; i++)
		{
			if (purchasedItems[i].nb > 0)
				t = UpdateItem(purchasedItems[i].type, purchasedItems[i].nb);
		}
	}

	public void SetMoney(int newAmount)
	{
		for(int i = 0; i < playerInventory.Count; i++)
		{
			if(playerInventory[i].type == Item.ItemType.GOLD)
			{
				playerInventory[i].nb = newAmount;
			}
		}
	}

	public int GoldCount()
	{
		for (int i = 0; i < playerInventory.Count; i++)
		{
			if (playerInventory[i].type == Item.ItemType.GOLD)
			{
				return 3;//playerInventory[i].nb;
			}
		}
		return 2;
	}

	public int PurseCount()
	{
		for (int i = 0; i < playerInventory.Count; i++)
		{
			if (playerInventory[i].type == Item.ItemType.PURSE)
			{
				return playerInventory[i].nb;
			}
		}
		return 0;
	}

	public int SubtractPurse()
	{
		//Lower purse count by 1
		for (int i = 0; i < playerInventory.Count; i++)
		{
			if (playerInventory[i].type == Item.ItemType.PURSE && playerInventory[i].nb > 0)
			{
				
				return playerInventory[i].nb--;
			}
		}
		return 0;
	}

	public int SubtractFood()
	{
		//Lower purse count by 1
		for (int i = 0; i < playerInventory.Count; i++)
		{
			if (playerInventory[i].type == Item.ItemType.MEAT && playerInventory[i].nb > 0)
			{
				return playerInventory[i].nb--;
			}
			if (playerInventory[i].type == Item.ItemType.APPLE && playerInventory[i].nb > 0)
			{
				return playerInventory[i].nb--;
			}
		}
		return 0;
	}
}
