using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ManageXP : MonoBehaviour
{

	float initialPower, initialAccuracy, initialCommunication, initialXP;
	float currentPower, currentAccuracy, currentCommunication, currentXP;
	float previousPower, previousAccuracy, previousCommunication, previousXP;
	float XPTotal;

	static int purseCount, moneyCount;

	GameObject powerSlider, accuracySlider, communicationSlider, initialXPTextUI, gameManager, playerInventory;
	GameObject purseText, moneyText, meatText;

	float deltaPower, deltaAccuracy, deltaCommunication, deltaXP;

	// Start is called before the first frame update
	void Start()
	{
		//gameManager = GameObject.Find("gameManager");
		gameManager = GameObject.Find("gameManager");
		playerInventory = GameObject.Find("Player");

		print(playerInventory.GetComponent<InventorySystem>().PurseCount());

		print(gameManager);
		print(playerInventory);
		/*
		initialPower = 50;
		initialAccuracy = 50;
		initialCommunication = 50;
		initialXP = 50;
		*/

		purseText = GameObject.Find("purseCount");
		moneyText = GameObject.Find("moneyCount");

		if(SceneManager.GetActiveScene().name == "endScene")
			meatText = GameObject.Find("meatCount");

		if (SceneManager.GetActiveScene().name == "levelComplete")
		{
			initialPower = gameManager.GetComponent<GameManager>().player.power;
			initialAccuracy = gameManager.GetComponent<GameManager>().player.accuracy;
			initialCommunication = gameManager.GetComponent<GameManager>().player.communication;
			initialXP = gameManager.GetComponent<GameManager>().player.XP;

			XPTotal = initialXP;

			currentPower = initialPower;
			currentAccuracy = initialAccuracy;
			currentCommunication = initialAccuracy;

			powerSlider = GameObject.Find("powerSlider");
			accuracySlider = GameObject.Find("accuracySlider");
			communicationSlider = GameObject.Find("communicationSlider");
			initialXPTextUI = GameObject.Find("xpGained");

			initialXPTextUI.GetComponent<Text>().text = "" + initialXP;

			powerSlider.GetComponent<Slider>().minValue = initialPower;
			powerSlider.GetComponent<Slider>().maxValue = initialPower + initialXP;

			accuracySlider.GetComponent<Slider>().minValue = initialAccuracy;
			accuracySlider.GetComponent<Slider>().maxValue = initialAccuracy + initialXP;

			communicationSlider.GetComponent<Slider>().minValue = initialCommunication;
			communicationSlider.GetComponent<Slider>().maxValue = initialCommunication + initialXP;

		}
		//purseCount = 2;// GetComponent<InventorySystem>().PurseCount();	  PLACEHOLDER
		//moneyCount = 200;//GetComponent<InventorySystem>().GoldCount();   PLACEHOLDER

		//purseText.GetComponent<Text>().text = "" + purseCount;
		//moneyText.GetComponent<Text>().text = "" + moneyCount;

		purseText.GetComponent<Text>().text = "" + playerInventory.GetComponent<InventorySystem>().PurseCount();
		moneyText.GetComponent<Text>().text = "" + playerInventory.GetComponent<InventorySystem>().GetMoney();
		if (SceneManager.GetActiveScene().name == "endScene")
			meatText.GetComponent<Text>().text = "" + playerInventory.GetComponent<InventorySystem>().GetMeat();

		print("MANAGE XP has been initialized");
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			
			gameManager.GetComponent<GameManager>().player.power = (int)currentPower;
			gameManager.GetComponent<GameManager>().player.accuracy = (int)currentAccuracy;
			gameManager.GetComponent<GameManager>().player.communication = (int)currentCommunication;
			gameManager.GetComponent<GameManager>().player.XP = (int)currentXP;
			gameManager.GetComponent<GameManager>().IncreaseStage(1);
			gameManager.GetComponent<GameManager>().LoadNewScene();
		}
	}

	void SaveCurrentValues()
	{
		print("saving current values");
		previousPower = currentPower;
		previousAccuracy = currentAccuracy;
		previousCommunication = currentCommunication;
	}

	void GetNewValues()
	{
		print("getting new values");
		currentPower = powerSlider.GetComponent<Slider>().value;
		currentAccuracy = accuracySlider.GetComponent<Slider>().value;
		currentCommunication = communicationSlider.GetComponent<Slider>().value;
	}

	void CalculateDeltas()
	{
		print("calculating deltas");
		deltaPower = currentPower - initialPower;
		deltaAccuracy = currentAccuracy - initialAccuracy;
		deltaCommunication = currentCommunication - initialCommunication;
		deltaXP = deltaAccuracy + deltaPower + deltaCommunication;

		GameObject.Find("xpGained").GetComponent<Text>().text = "" + (int)(initialXP - deltaXP);
	}

	public void SetXP()
	{
		string currentXPAsText = initialXPTextUI.GetComponent<Text>().text;

		print("SETXP CALLED");

		currentXP = Int32.Parse(currentXPAsText);
		SaveCurrentValues();
		GetNewValues();

		if(currentXP == 0)
		{
			OutOfXP();
		}

		CalculateDeltas();
		DisplayUpdatedValues();
	}

	void DisplayUpdatedValues()
	{
		GameObject.Find("powerLabel").GetComponent<Text>().text = "Power(" + (int)currentPower + ")";
		GameObject.Find("accuracyLabel").GetComponent<Text>().text = "Accuracy(" + (int)currentAccuracy + ")";
		GameObject.Find("communicationLabel").GetComponent<Text>().text = "Communication(" + (int)currentCommunication + ")";
	}

	void OutOfXP()
	{
		if (currentPower > previousPower)
			currentPower = previousPower;
		if (currentAccuracy > previousAccuracy)
			currentAccuracy = previousAccuracy;
		if (currentCommunication > previousCommunication)
			currentCommunication = previousCommunication;

		powerSlider.GetComponent<Slider>().value = currentPower;
		accuracySlider.GetComponent<Slider>().value = currentAccuracy;
		communicationSlider.GetComponent<Slider>().value = currentCommunication;
	}

	public void UpButtonClicked()
	{
		string buttonName = EventSystem.current.currentSelectedGameObject.name;
		print(buttonName);
		string currentXPAsText = initialXPTextUI.GetComponent<Text>().text;
		currentXP = Int32.Parse(currentXPAsText);
		SaveCurrentValues();

		if (buttonName == "powerUpButton")
		{
			print("Raising power by 1");
			if (currentXP != 0)
				powerSlider.GetComponent<Slider>().value += 1;
			else
				OutOfXP();
			SaveCurrentValues();
			GetNewValues();
		}
		else if (buttonName == "accuracyUpButton")
		{
			print("Raising accuracy by 1");
			if (currentXP != 0)
				accuracySlider.GetComponent<Slider>().value += 1;
			else
				OutOfXP();
			SaveCurrentValues();
			GetNewValues();
		}
		else if (buttonName == "communicationUpButton")
		{
			print("Raising communication by 1");
			if (currentXP != 0)
				communicationSlider.GetComponent<Slider>().value += 1;
			else
				OutOfXP();
			SaveCurrentValues();
			GetNewValues();
		}
		CalculateDeltas();
		DisplayUpdatedValues();
	}

	public void DownButtonClicked()
	{
		string buttonName = EventSystem.current.currentSelectedGameObject.name;
		print(buttonName);
		string currentXPAsText = initialXPTextUI.GetComponent<Text>().text;
		currentXP = Int32.Parse(currentXPAsText);
		SaveCurrentValues();

		if (buttonName == "powerDownButton")
		{
			print("Lowering power by 1");
			if (currentXP < XPTotal)
				powerSlider.GetComponent<Slider>().value -= 1;
			SaveCurrentValues();
			GetNewValues();
		}
		else if (buttonName == "accuracyDownButton")
		{
			print("Lowering accuracy by 1");
			if (currentXP < XPTotal)
				accuracySlider.GetComponent<Slider>().value -= 1;
			SaveCurrentValues();
			GetNewValues();
		}
		else if (buttonName == "communicationDownButton")
		{
			print("Lowering communication by 1");
			if (currentXP < XPTotal)
				communicationSlider.GetComponent<Slider>().value -= 1;
			SaveCurrentValues();
			GetNewValues();
		}
		CalculateDeltas();
		DisplayUpdatedValues();
	}
}
