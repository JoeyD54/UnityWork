using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
	string nameOfCharacter;
	Dialogue[] dialogues;
	int nbDialogues;
	int currentDialogueIndex = 0;
	bool waitingForUserInput = false;
	bool dialogueIsActive = false;

	GameObject dialogueBox, dialoguePanel;

    // Start is called before the first frame update
    void Start()
    {
		nameOfCharacter = gameObject.name;
		//print("name of character: " + nameOfCharacter);
		nbDialogues = calculateNbDialogues();
		dialogues = new Dialogue[nbDialogues];

		loadDialogue();
		nbDialogues = calculateNbDialogues();
		

		dialogueBox = GameObject.Find("dialogueBox");
		dialoguePanel = GameObject.Find("dialoguePanel");
		/*
		if (gameObject.name == "Diana")
			GameObject.Find("dialogueImage").GetComponent<RawImage>().texture = Resources.Load<Texture2D>("Diana") as Texture2D;
		else if(gameObject.name == "Mayor")
			GameObject.Find("dialogueImage").GetComponent<RawImage>().texture = Resources.Load<Texture2D>("Mayor") as Texture2D;
		*/
		GameObject.Find("dialogueImage").GetComponent<RawImage>().texture = Resources.Load<Texture2D>(gameObject.name) as Texture2D;
		dialoguePanel.SetActive(false);
	}

	// Update is called once per frame
	void Update()
    {
		if (dialogueIsActive)
		{
			if (!waitingForUserInput)
			{
				dialoguePanel.SetActive(true);

				if (currentDialogueIndex != -1)	
					displayDialogue2();
				else
				{
					dialogueIsActive = false;
					dialoguePanel.SetActive(false);
					waitingForUserInput = false;
					GameObject.Find("Player").GetComponent<ControlPlayer>().EndTalking();
					currentDialogueIndex = 0;

					GameObject.Find("gameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.talk_to,
						nameOfCharacter);
				}

				waitingForUserInput = true;
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.A))
				{
					currentDialogueIndex = dialogues[currentDialogueIndex].targetForResponse[0];
					waitingForUserInput = false;
				}
				else if (Input.GetKeyDown(KeyCode.B))
				{
					currentDialogueIndex = dialogues[currentDialogueIndex].targetForResponse[1];
					waitingForUserInput = false;
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					waitingForUserInput = false;
					currentDialogueIndex = -1;
				}
			}
		}
    }
	
	public void loadDialogue()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("dialogues");
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(textAsset.text);
		int dialogueIndex = 0;

		foreach(XmlNode character in doc.SelectNodes("dialogues/character"))
		{
			if(character.Attributes.GetNamedItem("name").Value == nameOfCharacter)
			{
				print(character.Attributes.GetNamedItem("name").Value);
				dialogueIndex = 0;
				foreach(XmlNode dialogueFromXML in character) //doc.SelectNodes("dialogues/character/dialogue"))
				{
					print(dialogueFromXML.Attributes.GetNamedItem("content").Value);
					dialogues[dialogueIndex] = new Dialogue();
					dialogues[dialogueIndex].message = dialogueFromXML.Attributes.GetNamedItem("content").Value;

					int choiceIndex = 0;
					dialogues[dialogueIndex].response = new string[2];
					dialogues[dialogueIndex].targetForResponse = new int[2];

					foreach(XmlNode choice in dialogueFromXML)
					{
						dialogues[dialogueIndex].response[choiceIndex] = choice.Attributes.GetNamedItem("content").Value;
						dialogues[dialogueIndex].targetForResponse[choiceIndex] = int.Parse(choice.Attributes.GetNamedItem("target").Value);

						choiceIndex++;
					}
					dialogueIndex++;
				}
			}
		}
	}

	public int calculateNbDialogues()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("dialogues");
		XmlDocument doc = new XmlDocument();

		doc.LoadXml(textAsset.text);
		int dialogueIndex = 0;

		foreach(XmlNode character in doc.SelectNodes("dialogues/character"))
		{
			if(character.Attributes.GetNamedItem("name").Value == nameOfCharacter)
			{
				foreach(XmlNode dialogueFromXML in doc.SelectNodes("dialogues/character/dialogue"))
				{
					dialogueIndex++;
				}
			}
		}
		return dialogueIndex;
	}

	public void displayDialogue1()
	{
		print("Dialogue Index = " + currentDialogueIndex);
		print("Test dialogue message: " + dialogues[0].message);
		print("[A]> " + dialogues[currentDialogueIndex].response[0]);
		print("[B]> " + dialogues[currentDialogueIndex].response[1]);
	}

	public void displayDialogue2()
	{
		string textToDisplay = "[" + gameObject.name + "]" + " " + dialogues[currentDialogueIndex].message + "\n[A]> " +
			dialogues[currentDialogueIndex].response[0] + "\n[B]> " + dialogues[currentDialogueIndex].response[1] + 
			"\nPress [ESC] to leave chat";

		GameObject.Find("dialogueBox").GetComponent<Text>().text = textToDisplay;
	}

	public void startDialogue()
	{
		waitingForUserInput = false;
		dialogueIsActive = true;
	}
}
