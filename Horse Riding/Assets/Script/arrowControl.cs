using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class arrowControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		//Button Esc: close game
		/*if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}*/
	}

	void OnCollisionEnter(Collision collision)
	{
		//Debug.Log(gameObject.transform.parent);
		//Debug.Log((collision.gameObject.name == "Card1" || collision.gameObject.name == "Card2" || collision.gameObject.name == "Card3"));
		//Debug.Log(gameObject.transform.parent == GameObject.FindWithTag("horse").transform);
		//Debug.Log(SceneManager.GetActiveScene().name);

		if (SceneManager.GetActiveScene().name == "Hunt")
		{
			if (collision.gameObject.name != "Horse"
			&& gameObject.transform.parent == GameObject.FindWithTag("horse").transform)
			{
				Destroy(this.gameObject);
				if (collision.gameObject.name != "StartAnimal" && collision.gameObject.name != "Horse")
					SendMessageUpwards("AddScore", collision.gameObject.name);
			}

			//if (collision.gameObject.name != "StartAnimal" && collision.gameObject.name != "Horse")
			//{
			//	Destroy(this.gameObject);
			//}
		}

		if (SceneManager.GetActiveScene().name == "Yurt-V2")
		{
			if ((collision.gameObject.name == "Card1" || collision.gameObject.name == "Card2" || collision.gameObject.name == "Card3") && gameObject.transform.parent == GameObject.FindWithTag("horse").transform)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
