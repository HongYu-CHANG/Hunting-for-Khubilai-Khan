using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class openingChange : MonoBehaviour {

	public GameObject bowModel;
	public Image image1;
	public Image image2;
	private bool nextImage = false;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(changeImage());
		bowModel.SetActive(false);
		image1.gameObject.SetActive(true);
		image2.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (nextImage)
		{
			bowModel.SetActive(true);
			image1.gameObject.SetActive(false);
			image2.gameObject.SetActive(true);
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
		nextImage = true;
	}
}
