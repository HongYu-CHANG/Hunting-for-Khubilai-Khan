using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System.Threading;

public class horseControl : MonoBehaviour {

	public GameObject playerPosition;
	public GameObject playerController;
	public FixedEnvironmentController speedController;
	private float XdisOfPlayerAndHorse;
	private float YdisOfPlayerAndHorse;
	private float ZdisOfPlayerAndHorse;
	private Animator _animator;
	private int temp = 0;
	public int pressure;

	// Arduino connection
	private CommunicateWithArduino Uno = new CommunicateWithArduino();
	
	void Start ()
	{
		//new Thread(Uno.connectToArdunio).Start
		_animator = this.GetComponent<Animator>();
		XdisOfPlayerAndHorse = -1.5f;
		YdisOfPlayerAndHorse = 4f;
		ZdisOfPlayerAndHorse = -1.2f;


		transform.position = playerPosition.transform.position;
		transform.forward = playerPosition.transform.forward;
		transform.position -= transform.up.normalized * 2f;
		transform.position -= transform.forward.normalized * 1.3f;

		if ((playerController.transform.position.x - transform.position.x) > XdisOfPlayerAndHorse)
			XdisOfPlayerAndHorse = playerController.transform.position.x - transform.position.x;
		if ((playerController.transform.position.y - transform.position.y) > YdisOfPlayerAndHorse)
			YdisOfPlayerAndHorse = playerController.transform.position.y - transform.position.y;
		if ((playerController.transform.position.z - transform.position.z) > ZdisOfPlayerAndHorse)
			ZdisOfPlayerAndHorse = playerController.transform.position.z - transform.position.z;
	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.H))
		{
			Debug.Log("Press H");

			transform.position = playerPosition.transform.position;
			transform.forward = playerPosition.transform.forward;
			transform.position -= transform.up.normalized * 2f;
			transform.position -= transform.forward.normalized * 1.3f;
			
			if((playerController.transform.position.x - transform.position.x) > XdisOfPlayerAndHorse)
				XdisOfPlayerAndHorse = playerController.transform.position.x - transform.position.x;
			if ((playerController.transform.position.y - transform.position.y) > YdisOfPlayerAndHorse)
				YdisOfPlayerAndHorse = playerController.transform.position.y - transform.position.y;
			if ((playerController.transform.position.z - transform.position.z) > ZdisOfPlayerAndHorse)
				ZdisOfPlayerAndHorse = playerController.transform.position.z - transform.position.z;
		}
		transform.eulerAngles= new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);//保持馬的水平
		playerController.transform.position = new Vector3(transform.position.x + XdisOfPlayerAndHorse, transform.position.y + YdisOfPlayerAndHorse, transform.position.z + ZdisOfPlayerAndHorse);
		//馬的XZ位置需再FOR騎馬機調整
		try {
			pressure = Uno.ReceiveData();
		}
		catch (NullReferenceException ex) {
            pressure = pressure;
        }

        if(pressure != 4)
        {
	        _animator.SetInteger("horseSpeed", pressure);
			speedController.setSpeed(5*pressure);
			temp = 0;
		}
		else
		{	
			temp = temp % 3;
			temp ++;
			//Debug.Log(temp);
			_animator.SetInteger("horseSpeed", temp);
			speedController.setSpeed( 5*temp );
		}
		
		/*if (pressure < 100) // 走路
		{
			_animator.SetInteger("horseSpeed", pressure);
			speedController.setSpeed(5);
		}
		else if (pressure < 200) //小跑步
		{
			_animator.SetInteger("horseSpeed", pressure);
			speedController.setSpeed(10);
		}
		else if (pressure < 300)//跑
		{ 
			_animator.SetInteger("horseSpeed", pressure);
			speedController.setSpeed(15);
		}
		else//瘋狂跑
		{
			_animator.SetInteger("horseSpeed", pressure);
			speedController.setSpeed(20);
		}
		*/
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
				string portChoice = "COM6";
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
				Debug.LogWarning(arduinoController);
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

		public int ReceiveData()
		{
			return int.Parse(arduinoController.ReadLine());
		}
	}

}
