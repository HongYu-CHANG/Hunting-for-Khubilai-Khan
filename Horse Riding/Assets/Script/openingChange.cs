using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class openingChange : MonoBehaviour {

	public GameObject bowModel;
	public Image imageBow;
	public Image imageHorse;
	public Image imageStart;
	public List<Sprite> spriteBowList;
	public List<Sprite> spriteHorseList;
	private bool nextImageHorse = false;
	private bool nextImageStart = false;
	private int frameSegment = 72;
	private int counter = 0;
	private float deltaPos = 0.02f;
	private int adding = 0;
	private int ImageNum = 0;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(changeImage());
		StartCoroutine(changeImageHorse());
		bowModel.SetActive(false);
		imageBow.gameObject.SetActive(true);
		imageHorse.gameObject.SetActive(false);
		imageStart.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (counter == frameSegment)
		{
			ImageNum++;
			if (ImageNum > 1)
			{
				ImageNum = 0;
			}
			counter = 0;
		}
		else
		{
			counter++;
		}
		if(!nextImageHorse) imageBow.sprite = spriteBowList[ImageNum];
		else imageHorse.sprite = spriteHorseList[ImageNum];
		if (nextImageStart)
		{
			bowModel.SetActive(true);
			imageBow.gameObject.SetActive(false);
			imageHorse.gameObject.SetActive(false);
			imageStart.gameObject.SetActive(true);
			if (bowModel.GetComponent<oneBowControl>().isShot())
			{
				StartCoroutine(VideoScene());
			}
		}
	}

	IEnumerator VideoScene()
	{
		yield return new WaitForSeconds(3f);
		float fadeTime = GameObject.Find("Canvas").GetComponent<fading>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene("IntroVideo");
	}

	IEnumerator changeImage()
	{
		yield return new WaitForSeconds(15f);
		nextImageStart = true;
	}
	IEnumerator changeImageHorse()
	{
		yield return new WaitForSeconds(7.5f);
		imageBow.gameObject.SetActive(false);
		imageHorse.gameObject.SetActive(true);
		nextImageHorse = true;
	}
}
