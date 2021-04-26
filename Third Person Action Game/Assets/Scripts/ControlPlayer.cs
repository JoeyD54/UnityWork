using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlPlayer : MonoBehaviour {

	[Header("Health Settings")]
	[Tooltip("Health value between 0 and 100")]

	public int health = 50;

	float speed, rotationAroundY;
	bool isTalking = false;
	public bool shopIsDisplayed;

	GameObject objectToPickUp;
	bool itemToPickUpNearBy = false;
	int amountToAdd = 1;
	GameObject userMessage;
	GameObject shopUI, healthUI, skillsUI;

	GameObject weapon;
	bool weaponIsActive = false;

	Animator anim;
	CharacterController controller;
	AnimatorStateInfo info;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();

		GameObject.Find("healthBar").GetComponent<ManageBar>().SetValue(health);

		userMessage = GameObject.Find("userMessageField");
		userMessage.SetActive(false);

		controller = GetComponent<CharacterController>();

		weapon = GameObject.Find("playerWeapon").gameObject;
		weapon.SetActive(false);

		shopUI = GameObject.Find("shopUI");
		shopUI.SetActive(false);

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown("Fire1"))
		{
			if (weaponIsActive)
				anim.SetTrigger("attackWithWeapon");
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			weaponIsActive = !weaponIsActive;

			if (weaponIsActive)
				anim.SetTrigger("useWeapon");
			else
				anim.SetTrigger("putWeaponBack");
		}

		if (!shopIsDisplayed)
		{
			if (isTalking)
				return;

			info = anim.GetCurrentAnimatorStateInfo(0);

			speed = Input.GetAxis("Vertical");
			rotationAroundY = Input.GetAxis("Horizontal");

			anim.SetFloat("speed", speed);
			gameObject.transform.Rotate(0, rotationAroundY, 0);

			if (speed > 0)
			{
				controller.Move(transform.forward * speed * 8.0f * Time.deltaTime);
			}

			if (itemToPickUpNearBy)
			{
				if (Input.GetKeyDown(KeyCode.Y))
					PickUpObject1();
				if (Input.GetKeyDown(KeyCode.N))
				{
					GameObject.Find("userMessageText").GetComponent<Text>().text = "";
					userMessage.SetActive(false);
				}
			}
		}

		if (info.IsName("UseWeapon"))
		{
			if (info.normalizedTime % 1.0 >= .5)
			{
				weapon.SetActive(true);
			}
		}

		if (info.IsName("PutWeaponBack"))
		{
			if (info.normalizedTime % 1.0 >= .8)
			{
				weapon.SetActive(false);
			}
			else
				weapon.SetActive(true);
		}

	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if(hit.collider.gameObject.name == "Diana" && !isTalking || hit.collider.gameObject.name == "Mayor" && !isTalking)
		{
			hit.collider.gameObject.GetComponent<DialogueSystem>().startDialogue();
			isTalking = true;
			anim.SetFloat("speed", 0);
			hit.collider.isTrigger = true;
			hit.collider.gameObject.GetComponent<BoxCollider>().size = new Vector3(2, 1, 2);
		}
	}

	public void EndTalking()
	{
		isTalking = false;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "shop")
		{
			shopIsDisplayed = true;
			anim.SetFloat("speed", 0);
			displayShopUI();
			GameObject.Find("shopSystem").GetComponent<ShopSystem>().Init();
		}

		if (other.tag == "itemToBeCollected")
		{

			objectToPickUp = other.gameObject;
			itemToPickUpNearBy = true;
			//PickUpObject1();
			PickUpObject2();

		}
		/*
		if (other.gameObject.name == "shop")
		{

			shopIsDisplayed = true;
			anim.SetFloat("speed", 0);
			displayShopUI();
			GameObject.Find("shopSystem").GetComponent<ShopSystem>().Init(); ;

		}
		*/
	}

	[System.Obsolete]
	public void OnTriggerExit(Collider other)
	{
		itemToPickUpNearBy = false;

		if (userMessage.active)
		{
			GameObject.Find("userMessageText").GetComponent<Text>().text = "";

			userMessage.SetActive(false);
		}
	}

	public void RemoveMessage()
	{
		userMessage.SetActive(false);
	}

	void PickUpObject1()
	{
		amountToAdd = 1;
		if(objectToPickUp.GetComponent<ObjectToBeCollected>().item.name == "Gold")
		{
			amountToAdd = objectToPickUp.GetComponent<ObjectToBeCollected>().item.nb;
		}

		if (GetComponent<InventorySystem>().UpdateItem(objectToPickUp.GetComponent<ObjectToBeCollected>().type, amountToAdd))
		{
			Destroy(objectToPickUp);

			itemToPickUpNearBy = false;

			GameObject.Find("userMessageText").GetComponent<Text>().text = "Collected!";
			//userMessage.SetActive(false);
			Invoke("RemoveMessage", 2f);

			GameObject.Find("gameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.acquire_a,
				objectToPickUp.GetComponent<ObjectToBeCollected>().item.name);
		}
		else
		{
			string message = "You can't pickup this item as you have reached your max for this item";
			GameObject.Find("userMessageText").GetComponent<Text>().text = message;
		}
	}

	void PickUpObject2()
	{
		print("Collected");

		userMessage.SetActive(true);
		string article = objectToPickUp.GetComponent<ObjectToBeCollected>().item.article;
		string message = "You just found " + article + " " + objectToPickUp.GetComponent<ObjectToBeCollected>().item.name + "\n Collect? (y/n)";

		userMessage.SetActive(true);
		GameObject.Find("userMessageText").GetComponent<Text>().text = message;
	}

	public void displayShopUI()
	{
		shopUI.SetActive(true);
	}

	public void DecreaseHealth(int amount)
	{
		health -= amount;

		if (health <= 0)
			health = 0;

		if(health <= 0)
		{
			health = 0;

			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		GameObject.Find("healthBar").GetComponent<ManageBar>().SetValue(health);
	}

	public void IncreaseHealth(int amount)
	{
		health += amount;

		if (health > 100)
			health = 100;
		print("Health" + health);

		GameObject.Find("healthBar").GetComponent<ManageBar>().SetValue(health);
	}
}
