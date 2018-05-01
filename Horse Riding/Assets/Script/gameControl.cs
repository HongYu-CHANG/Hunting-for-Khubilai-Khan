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

	// Use this for initialization
	void Start ()
	{
		popMessagePosition = popMessage.transform.position;// new Vector3 (28.3f, 7f, 0f);
		Debug.Log(popMessage.transform.position);
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

		print(animalName);
		this.animalName = animalName;
		if (animalName.CompareTo("(Clone)") > 0)
		{
			score = 20;
			NowScore += score;
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
	
	popMessage.transform.position = new Vector3(popMessage.transform.position.x, popMessage.transform.position.y
		+ 125f * Time.deltaTime, popMessage.transform.position.z);
	/*		
	popMessage.transform.up = new Vector3(popMessage.transform.position.x, popMessage.transform.position.y
			+ 125f * Time.deltaTime, popMessage.transform.position.z);*/
		//Color color = popMessage.color;
		popMessageColor.a -= 0.025f;
		popMessage.color = popMessageColor;
		popMessage.text = animalName + " +" + score;
		if (popMessageColor.a < 0f)
		{
			popMessage.transform.position = popMessagePosition;
			popMessageColor.a = 1f;
			Debug.Log(popMessage.transform.position);
		}
		//Debug.Log(color);
		//Debug.Log(popMessageColor.a);

	}
}
