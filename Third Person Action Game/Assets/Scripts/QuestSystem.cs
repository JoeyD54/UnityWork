using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Xml;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class QuestSystem : MonoBehaviour
{
	[SerializeField]
	GameObject player;

	int currentStage;
	bool timerStarted;
	float timer;
	List<string> actions, targets, xps;
	List<bool> objectiveAchieved;

	string stageTitle, stageDescription, stageObjectives, startingPointForPlayer;
	bool panelDisplayed = true;
	float displayTimer;
	bool startDisplayTimer;

	int nbObjectivesAchieved = 0;
	int nbObjectivesToAchieve;
	int XPAchieved;

	public enum possibleActions
	{
		do_nothing = 0,
		talk_to = 1,
		acquire_a = 2,
		destroy_one = 3,
		enter_place_called = 4,
		exit_place_called = 5,
		collect_five = 6
	};

	List<possibleActions> actionsForQuest;
	public GameObject stagePanel, stageTitleText, stageDescriptionText, stageObjectivesText;

    // Start is called before the first frame update
    void Start()
    {
		GameObject.Find("userMessage").GetComponent<Text>().text = "";

		Init();
		MovePlayerToStartingPoint();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.H))
		{
			panelDisplayed = !panelDisplayed;
			Display(panelDisplayed);
		}

		if (startDisplayTimer)
		{
			displayTimer += Time.deltaTime;

			if (displayTimer >= 2)
			{
				displayTimer = 0.0f;
				startDisplayTimer = false;
				GameObject.Find("userMessage").GetComponent<Text>().text = "";
			}
		}
    }

	void Display(bool display)
	{
		stagePanel.SetActive(display);
	}

	public void Init()
	{
		stageTitleText = GameObject.Find("stageTitle");
		stageObjectivesText = GameObject.Find("stageObjectives");
		stageDescriptionText = GameObject.Find("stageDescription");
		stagePanel = GameObject.Find("stagePanel");

		currentStage = GetComponent<GameManager>().GetStage();

		nbObjectivesAchieved = 0;

		actions = new List<string>();
		targets = new List<string>();
		xps = new List<string>();
		objectiveAchieved = new List<bool>();
		actionsForQuest = new List<possibleActions>();
		//LoadQuest();
		LoadQuest2();
		DisplayQuestInfo();
	}

	public void LoadQuest()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("quest");
		XmlDocument doc = new XmlDocument();

		doc.LoadXml(textAsset.text);
		stageObjectives = "For this stage, you need to:\n";

		foreach(XmlNode stage in doc.SelectNodes("quest/stage"))
		{
			if(stage.Attributes.GetNamedItem("id").Value == "" + currentStage)
			{
				print("Stage: " + stage.Attributes.GetNamedItem("id").Value);
				stageTitle = stage.Attributes.GetNamedItem("name").Value;
				stageDescription = stage.Attributes.GetNamedItem("description").Value;

				foreach(XmlNode results in stage)
				{
					print("For this stage, you need to:\n");
					foreach(XmlNode result in results)
					{
						string action = result.Attributes.GetNamedItem("action").Value;
						string target = result.Attributes.GetNamedItem("target").Value;
						string xp = result.Attributes.GetNamedItem("xp").Value;

						actions.Add(action);
						targets.Add(target);
						xps.Add(xp);
						objectiveAchieved.Add(false);
						print(action + " " + target + "[" + xp + "XP]");
						stageObjectives += "\n->" + action + " " + target + "[" + xp + "XP]";
						nbObjectivesToAchieve++;
					}
				}
			}
		}
	}

	public void LoadQuest2()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("quest");
		XmlDocument doc = new XmlDocument();

		doc.LoadXml(textAsset.text);
		stageObjectives = "For this stage, you need to:\n";
		nbObjectivesToAchieve = 0;

		foreach (XmlNode stage in doc.SelectNodes("quest/stage"))
		{
			if (stage.Attributes.GetNamedItem("id").Value == "" + currentStage)
			{
				print("Stage: " + stage.Attributes.GetNamedItem("id").Value);
				stageTitle = stage.Attributes.GetNamedItem("name").Value;
				stageDescription = stage.Attributes.GetNamedItem("description").Value;

				foreach (XmlNode results in stage)
				{
					print("For this stage, you need to:\n");
					foreach (XmlNode result in results)
					{
						string action = result.Attributes.GetNamedItem("action").Value;
						string target = result.Attributes.GetNamedItem("target").Value;
						string xp = result.Attributes.GetNamedItem("xp").Value;

						possibleActions actionForQuest = possibleActions.do_nothing;
						if (action.IndexOf("Acquire") >= 0)
							actionForQuest = possibleActions.acquire_a;
						else if (action.IndexOf("Talk") >= 0)
							actionForQuest = possibleActions.talk_to;
						else if(action.IndexOf("Destroy") >= 0)
							actionForQuest = possibleActions.destroy_one;
						else if (action.IndexOf("Enter") >= 0 && action.IndexOf("place") >= 0)
							actionForQuest = possibleActions.enter_place_called;
						else if (action.IndexOf("Exit") >= 0 && action.IndexOf("place") >= 0)
							actionForQuest = possibleActions.exit_place_called;
						else if (action.IndexOf("Collect") >= 0)
							actionForQuest = possibleActions.collect_five;

						actionsForQuest.Add(actionForQuest);
						targets.Add(target);
						xps.Add(xp);
						objectiveAchieved.Add(false);
						print(action + " " + target + "[" + xp + "XP]");
						stageObjectives += "\n->" + action + " " + target + "[" + xp + "XP]";
						nbObjectivesToAchieve++;
						print("objectives to do: " + nbObjectivesToAchieve);
						print("objectives completed: " + nbObjectivesAchieved);
					}
				}
			}
		}
	}
	public void MovePlayerToStartingPoint()
	{
		GameObject p;

		if (SceneManager.GetActiveScene().name == "level1")
		{
			p = Instantiate(player);
			p.name = "Player";

			p.transform.rotation = new Quaternion(0, 180, 0, 0);
			p.transform.position = GameObject.Find("startingPoint").transform.position;
			p.transform.parent = gameObject.transform;
		}
		else if (SceneManager.GetActiveScene().name == "level2")
		{
			int width = GameObject.Find("generateMaze").GetComponent<GenerateMaze>().width;
			int height = GameObject.Find("generateMaze").GetComponent<GenerateMaze>().height;
			int wallSize = GameObject.Find("generateMaze").GetComponent<GenerateMaze>().wallSize;

			float xOffset = -(width * wallSize) / 2;
			float zOffset = -(height * wallSize) / 2;

			p = GameObject.Find("Player");
			p.transform.position = new Vector3(xOffset + wallSize * 4, 0.52f, zOffset + wallSize * 4);
		}
		else if (SceneManager.GetActiveScene().name == "level3")
		{
			p = GameObject.Find("Player");
			//p.name = "Player";
			p.transform.rotation = new Quaternion(0, 180, 0, 0);
			p.transform.position = GameObject.Find("startingPoint").transform.position;
			p.transform.parent = gameObject.transform;
		}
	}

	void DisplayQuestInfo()
	{
		stageTitleText.GetComponent<Text>().text = stageTitle;
		stageDescriptionText.GetComponent<Text>().text = stageDescription;
		stageObjectivesText.GetComponent<Text>().text = stageObjectives + "\nPress H to Hide/Display this information";
	}

	public void Notify(possibleActions action, string target)
	{
		print("Notified: Action = " + action + "Target: " + target);

		for(int i = 0; i < actionsForQuest.Count; i++)
		{
			if(action == actionsForQuest[i] && target == targets[i] && !objectiveAchieved[i])
			{
				DisplayMessage("+" + xps[i] + "XP");
				nbObjectivesAchieved++;
				XPAchieved += Int32.Parse(xps[i]);
				objectiveAchieved[i] = true;

			}
		}
		if (nbObjectivesAchieved == nbObjectivesToAchieve)
		{
			DisplayMessage("Stage Complete");
			GetComponent<GameManager>().player.XP = CalculateTotalXPForLevel();
			Invoke("StageComplete", 2);
		}
	}

	void DisplayMessage(string message)
	{
		GameObject.Find("userMessage").GetComponent<Text>().text = message;
		startDisplayTimer = true;
	}

	int CalculateTotalXPForLevel()
	{
		int totalXP = 0;

		for(int i = 0; i < actionsForQuest.Count; i++)
		{
			totalXP += Int32.Parse(xps[i]);
		}
		return totalXP;
	}

	void StageComplete()
	{
		if (SceneManager.GetActiveScene().name != "level3")
			SceneManager.LoadScene("levelComplete");
		else
			SceneManager.LoadScene("endScene");
	}

}
