using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item
{
	public enum ItemType
	{
		GOLD = 0,
		PURSE = 1,
		RUBY = 2,
		BLUE_DIAMOND = 3,
		RED_DIAMOND = 4,
		MEAT = 5,
		APPLE = 6,
		HOT_DOG = 7,
		SWORD = 8,
		BATON = 9,
		SPEAR = 10
	}

	public enum ItemFamilyType { LOOT = 0, FOOD = 1, WEAPON = 3}


	public string name, description;
	public int price, healthBenefits, dammage, nb, maxNb;
	public ItemType type;
	public string article;
	public ItemFamilyType familyType;

	public Item(ItemType type)
	{
		switch (type)
		{
			case ItemType.GOLD:
				name = "Gold";
				price = 100;
				nb = 100;
				maxNb = 10000;
				description = "A pile of gold";
				familyType = ItemFamilyType.LOOT;
				article = "a";
				break;

			case ItemType.PURSE:
				name = "Purse";
				price = 50;
				nb = 1;
				maxNb = 5;
				description = "A purse";
				familyType = ItemFamilyType.LOOT;
				article = "a";
				break;
			/*
		case ItemType.YELLOW_DIAMOND:
			name = "Yellow diamond";
			price = 200;
			nb = 1;
			maxNb = 5;
			description = "a yellow diamond";
			familyType = ItemFamilyType.LOOT;
			article = "a";
			break;
			*/

			case ItemType.RUBY:
				name = "Ruby";
				price = 200;
				nb = 1;
				maxNb = 5;
				description = "A red ruby";
				familyType = ItemFamilyType.LOOT;
				article = "a";
				break;

			case ItemType.BLUE_DIAMOND:
				name = "Blue diamond";
				price = 200;
				nb = 1;
				maxNb = 5;
				description = "A blue diamond";
				familyType = ItemFamilyType.LOOT;
				article = "a";
				break;

			case ItemType.RED_DIAMOND:
				name = "Red diamond";
				price = 200;
				nb = 1;
				maxNb = 5;
				description = "A red diamond";
				familyType = ItemFamilyType.LOOT;
				article = "a";
				break;

			case ItemType.MEAT:
				name = "Meat";
				price = 50;
				nb = 1;
				maxNb = 10;
				healthBenefits = 20;
				description = "A piece of meat";
				familyType = ItemFamilyType.FOOD;
				article = "an";
				break;

			case ItemType.APPLE:
				name = "Apple";
				price = 50;
				nb = 1;
				maxNb = 5;
				healthBenefits = 10;
				description = "An apple";
				familyType = ItemFamilyType.FOOD;
				article = "an";
				break;

			case ItemType.HOT_DOG:
				name = "Hot dog";
				price = 50;
				nb = 1;
				maxNb = 5;
				healthBenefits = 15;
				description = "A hot dog";
				familyType = ItemFamilyType.FOOD;
				article = "an";
				break;

			case ItemType.SWORD:
				name = "Sword";
				price = 100;
				nb = 1;
				maxNb = 5;
				description = "A sword";
				familyType = ItemFamilyType.WEAPON;
				article = "a";
				break;

			case ItemType.BATON:
				name = "Baton";
				price = 100;
				nb = 1;
				maxNb = 5;
				description = "A baton";
				familyType = ItemFamilyType.WEAPON;
				article = "a";
				break;

			case ItemType.SPEAR:
				name = "Spear";
				price = 100;
				nb = 1;
				maxNb = 5;
				description = "A spear";
				familyType = ItemFamilyType.WEAPON;
				article = "an";
				break;


		}
		this.type = type;
	}

	public Texture GetTexture()
	{

		//Texture2D tx;
		if (this.familyType == Item.ItemFamilyType.LOOT) return (Resources.Load<Texture2D>("loot/" + this.name.Replace(" ", "_")));
		else if (this.familyType == Item.ItemFamilyType.FOOD) return (Resources.Load<Texture2D>("food/" + this.name.Replace(" ", "_")));
		else if (this.familyType == Item.ItemFamilyType.WEAPON) return (Resources.Load<Texture2D>("weapons/" + this.name.Replace(" ", "_")));
		else return null;

	}

	public string ItemInfo()
	{

		string info = "name:" + this.name + ", damage" + this.dammage + ", nb" + this.nb + ", type" + this.type;
		return info;

	}

}

