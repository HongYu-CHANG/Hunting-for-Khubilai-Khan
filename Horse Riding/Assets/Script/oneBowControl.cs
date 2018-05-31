using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System.Threading;

public class oneBowControl : MonoBehaviour
{

	//arrow
	public GameObject arrow;
	public GameObject bowMiddle;
	public GameObject bowTop;
	public GameObject bowBot;
	private GameObject arrowClone;

	public float arrowShootCoefficient = 850;

	//rotary encoder
	private float previousData;
	private float nowData;

	// Arduino connection
	private CommunicateWithArduino Uno = new CommunicateWithArduino();

	private LineRenderer lineRenderer;
	private int numberOfPointsOnBow = 3;

	private Vector3[] bowPositions = new Vector3[3];
	private bool hasArrow;

	public bool isFirstShot = false;

	void Start()
	{
		new Thread(Uno.connectToArdunio).Start();
		previousData = 0;
		hasArrow = false;
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.startColor = Color.white;
		lineRenderer.endColor = Color.white;
		lineRenderer.startWidth = 0.01f;
		lineRenderer.endWidth = 0.01f;
		lineRenderer.positionCount = numberOfPointsOnBow;
	}

	void Update()
	{
		
		nowData = Uno.ReceiveData();
		//Debug.Log("nowData = " + nowData);
		float twoDiff = nowData - previousData;
		//Debug.Log("twoDiff = " + twoDiff);
		previousData = nowData;
		lineRenderer = GetComponent<LineRenderer>();

		if (twoDiff == 0)//沒拉弓
		{
			if (!hasArrow)
				bowPositions[1] = bowMiddle.transform.position;
		}
		else if (twoDiff > 20)//拉弓
		{
			if (!hasArrow)
			{

				//Debug.Log("bowMiddle = " + bowMiddle.transform.position);
				Vector3 arrowPosition = new Vector3(bowMiddle.transform.position.x, bowMiddle.transform.position.y, bowMiddle.transform.position.z);
				arrowPosition += transform.forward.normalized * 1f;
				arrowClone = Instantiate(arrow, arrowPosition, bowMiddle.transform.rotation);
				//Debug.Log("arrowClone = " + arrowClone.transform.position);
				arrowClone.transform.parent = gameObject.transform;
				arrowClone.transform.up = bowMiddle.transform.forward;
				arrowClone.active = true;
				hasArrow = true;
				
			}
			
			arrowClone.transform.position -= bowMiddle.transform.forward.normalized * ConvertToPullBackCoefficient(twoDiff) * Time.deltaTime;
			bowPositions[1] = arrowClone.transform.Find("tail").position;
		}
		else if (twoDiff < 0 && twoDiff > -80)//緩緩鬆弓
		{
			if (hasArrow)
			{
				
				arrowClone.transform.position += bowMiddle.transform.forward.normalized * ConvertToPullBackCoefficient(twoDiff) * Time.deltaTime;
				bowPositions[1] = arrowClone.transform.Find("tail").position;
				if (nowData > -100 && nowData < 100)
				{
					Destroy(arrowClone);
					hasArrow = false;
				}
			}
			
		}
		else if( twoDiff < -100 && hasArrow)//射箭
		{
			//Debug.LogWarning("isFirstShot = " + isFirstShot);
			arrowClone.transform.parent = GameObject.FindWithTag("horse").transform;
			arrowClone.GetComponent<Rigidbody>().AddForce(bowMiddle.transform.forward * arrowShootCoefficient);
			hasArrow = false;
			bowPositions[1] = bowMiddle.transform.position;
			isFirstShot = true;
			//Debug.LogWarning("isFirstShot = " + isFirstShot);
		}
		bowPositions[0] = bowTop.transform.position;
		bowPositions[2] = bowBot.transform.position;

		// Render the bowstring
		lineRenderer.SetPositions(bowPositions);
	}
	public bool isShot()
	{
		return isFirstShot;
	}

	public void OnDestroy()
	{
		Uno.closeSerial();
	}

	private float ConvertToPullBackCoefficient(float rotaryEncoderData)
	{
		return Mathf.Abs((rotaryEncoderData * 5) / 1600 );
	}

	class CommunicateWithArduino
	{
		public bool connected = true;
		public bool mac = false;
		public string choice = "cu.usbmodem1421";
		private SerialPort arduinoController;

		public void connectToArdunio()
		{

			if (connected)
			{
				string portChoice = "COM7";
				if (mac)
				{
					int p = (int)Environment.OSVersion.Platform;
					// Are we on Unix?
					if (p == 4 || p == 128 || p == 6)
					{
						List<string> serial_ports = new List<string>();
						string[] ttys = Directory.GetFiles("/dev/", "cu.*");
						foreach (string dev in ttys)
						{
							if (dev.StartsWith("/dev/tty."))
							{
								serial_ports.Add(dev);
								Debug.Log(String.Format(dev));
							}
						}
					}
					portChoice = "/dev/" + choice;
				}
				arduinoController = new SerialPort(portChoice, 9600, Parity.None, 8, StopBits.One);
				arduinoController.Handshake = Handshake.None;
				arduinoController.RtsEnable = true;
				arduinoController.Open();
				//Debug.LogWarning(arduinoController);
			}

		}
		public void SendData(object obj)
		{
			string data = obj as string;
			Debug.Log(data);
			if (connected)
			{
				if (arduinoController != null)
				{
					arduinoController.Write(data);
					arduinoController.Write("\n");
				}
				else
				{
					Debug.Log(arduinoController);
					Debug.Log("nullport");
				}
			}
			else
			{
				Debug.Log("not connected");
			}
			Thread.Sleep(500);
		}

		public float ReceiveData()
		{
			return float.Parse(arduinoController.ReadLine());
		}

		public void closeSerial()
		{
			arduinoController.Close();
		}
	}
}

