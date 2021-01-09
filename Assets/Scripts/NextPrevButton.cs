using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NextPrevButton : MonoBehaviour
{
	public Button prev;
	public Button next;

	public Image jungle;
	public Image desert;
	public Image winter;
	public Image arena;
	public Image boss;

	public TextMeshProUGUI difficultyText;

	private int numOfEnv = 5;
	private int currEnv = 0;

	private int numOfDifficulties = 3;
	private int currDifficulty = 0;

	void Start()
	{
		Button btn1 = prev.GetComponent<Button>();
		Button btn2 = next.GetComponent<Button>();
		btn1.onClick.AddListener(TaskOnClick1);
		btn2.onClick.AddListener(TaskOnClick2);
	}

	void TaskOnClick1()
	{
		if (currEnv == 0)
			currEnv = numOfEnv - 1;
		else
			currEnv--;

		HandleChange();
	}

	void TaskOnClick2()
	{
		currEnv = (currEnv + 1) % numOfEnv;
		HandleChange();
	}

	void HandleChange()
	{
		if (currEnv == 0) //jungle
		{
			PlayerPrefs.SetString("CurrentScene", "Game1");
			jungle.enabled = true;
			desert.enabled = false;
			winter.enabled = false;
			arena.enabled = false;
			boss.enabled = false;
		}
		else if (currEnv == 1) //desert
		{
			PlayerPrefs.SetString("CurrentScene", "GAME_TESTING"); //should be "Game2"
			jungle.enabled = false;
			desert.enabled = true;
			winter.enabled = false;
			arena.enabled = false;
			boss.enabled = false;
		}
		else if (currEnv == 2) //winter
		{
			PlayerPrefs.SetString("CurrentScene", "GAME_TESTING"); //should be "Game3"
			jungle.enabled = false;
			desert.enabled = false;
			winter.enabled = true;
			arena.enabled = false;
			boss.enabled = false;
		}
		else if (currEnv == 3) //arena
		{
			PlayerPrefs.SetString("CurrentScene", "GAME_TESTING"); //should be "Game4"
			jungle.enabled = false;
			desert.enabled = false;
			winter.enabled = false;
			arena.enabled = true;
			boss.enabled = false;
		}
		else if (currEnv == 4) //boss
		{
			PlayerPrefs.SetString("CurrentScene", "GAME_TESTING"); //should be "Game5"
			jungle.enabled = false;
			desert.enabled = false;
			winter.enabled = false;
			arena.enabled = false;
			boss.enabled = true;
		}
	}

	public void HigherDifficulty()
	{
		currDifficulty = (currDifficulty + 1) % numOfDifficulties;
		HandleDifficulty();
	}

	public void LowerDifficulty()
	{
		if (currDifficulty == 0)
			currDifficulty = numOfDifficulties - 1;
		else
			currDifficulty--;
		HandleDifficulty();
	}

	void HandleDifficulty()
	{
		if (currDifficulty == 0)
			difficultyText.SetText("EASY");
		else if (currDifficulty == 1)
			difficultyText.SetText("MEDIUM");
		else if (currDifficulty == 2)
			difficultyText.SetText("HARD");

		PlayerPrefs.SetInt("CurrentDifficulty", currDifficulty);
	}
}
