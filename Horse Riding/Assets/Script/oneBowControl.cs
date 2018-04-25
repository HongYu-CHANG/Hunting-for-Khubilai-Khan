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
	private GameObject arrowClone;
	

	public float arrowShootCoefficient;
	public float pullBackCoefficient;

	//rotary encoder
	private float previousData;
	private float nowData;

	// Arduino connection
	private CommunicateWithArduino Uno = new CommunicateWithArduino();

	private bool hasArrow;

	void Start()
	{
		new Thread(Uno.connectToArdunio).Start();
		previousData = 0;
		hasArrow = false;
	}

	void Update()
	{
		
		nowData = Uno.ReceiveData();
		//Debug.Log("nowData = " + nowData);
		float twoDiff = nowData - previousData;
		//Debug.Log("twoDiff = " + twoDiff);
		previousData = nowData;

		if (twoDiff == 0)//沒拉弓
		{ }
		else if (twoDiff > 20)//拉弓
		{
			if (!hasArrow)
			{
				
				arrowClone = Instantiate(arrow, bowMiddle.transform.position, bowMiddle.transform.rotation);
				arrowClone.transform.parent = gameObject.transform;
				arrowClone.active = true;
				arrowClone.transform.position = bowMiddle.transform.position;
				hasArrow = true;
			}
			
			arrowClone.transform.up = bowMiddle.transform.forward;
			arrowClone.transform.position -= bowMiddle.transform.forward.normalized * ConvertToPullBackCoefficient(twoDiff) * Time.deltaTime;

		}
		else if (twoDiff < 0 && twoDiff > -1000)//緩緩鬆弓
		{
			if (hasArrow)
			{
				
				arrowClone.transform.position += bowMiddle.transform.forward.normalized * ConvertToPullBackCoefficient(twoDiff) * Time.deltaTime;
				if (nowData > -100 && nowData < 100)
				{
					Destroy(arrowClone);
					hasArrow = false;
				}
			}
			
		}
		else if( twoDiff < -1000 && hasArrow)//射箭
		{
			//Debug.LogWarning("hasArrow twoDiff = " + twoDiff);
			arrowClone.transform.parent = GameObject.FindWithTag("horse").transform;
			arrowClone.GetComponent<Rigidbody>().AddForce(bowMiddle.transform.forward * 850);
			hasArrow = false;
		}
	}

	private float ConvertToPullBackCoefficient(float rotaryEncoderData)
	{
		//Debug.Log((rotaryEncoderData * 5) / 1600 );
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
				string portChoice = "COM4";
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
	}
}

