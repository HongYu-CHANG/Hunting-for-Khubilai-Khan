using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameControl : MonoBehaviour {

	public Text labelScore;
	public Text labelTime;
	public Text popMessage;
	private int NowScore = 0;
	private int time = 0;
	private float timer_f = 120f;
	private bool TimerOn = true;
	private string animalName;
	private int score = 0;
	private Vector3 popMessagePosition;
	private Color popMessageColor;
	private bool showPopMessage = false;

	// Use this for initialization
	void Start ()
	{
		popMessagePosition = popMessage.rectTransform.localPosition; 
		popMessageColor = popMessage.color; //white
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateTime(Time.deltaTime);
		UpdateUI(Time.deltaTime);
	}

	public void AddScore(string animalName)
	{
		//print(animalName);
		animalName = animalName.Replace("(Clone)", "");
		this.animalName = animalName;
		//print(String.Compare(animalName, "Terrain") + " " +animalName);
		if (String.Compare(animalName, "Terrain") != 0)
		{
			score = 20;
			NowScore += score;
			showPopMessage = true;
		}

		
		labelScore.text = string.Format("{0:D2}", NowScore);
		
	}

	private void UpdateTime(float num)
	{
		if (TimerOn)
		{
			timer_f -= num;
			time = (int)timer_f;
			labelTime.text = string.Format("{0:D2}", time);
		}
	}

	private void UpdateUI(float num)
	{
		popMessage.rectTransform.localPosition = new Vector3(popMessage.rectTransform.localPosition.x, popMessage.rectTransform.localPosition.y
		+ 125f * Time.deltaTime, popMessage.rectTransform.localPosition.z);

		popMessageColor.a -= 0.025f;
		popMessage.color = popMessageColor;
		popMessage.text = animalName + " +" + score;
		if (showPopMessage)
		{
			popMessage.rectTransform.localPosition = popMessagePosition;
			popMessageColor.a = 1f;
			showPopMessage = false;
		}

	}
}
