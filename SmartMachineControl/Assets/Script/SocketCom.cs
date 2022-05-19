using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using UnityEngine;

public class SocketCom : MonoBehaviour
{
	string serverIp = "172.30.1.23";
	//string serverIp = "localhost";
	int serverPort = 5556;

	private static TcpClient tcpClient = new TcpClient();
	System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();

	// Start is called before the first frame update
	void Start()
    {
		TcpConnect();
	}

    // Update is called once per frame
    void Update()
    {
		ReceiveMessage();
		if (tcpClient.Connected)
		{
			Debug.Log("Connected");
		}
		else
			Debug.Log("not connected");
	}

	void TcpConnect()
	{
		if (tcpClient == null)
		{
			tcpClient = new TcpClient();
		}
		else
		{
			try
			{
				if (!tcpClient.Connected)
				{
					if (!Stopwatch.IsRunning)
					{
						Stopwatch.Reset();
						Stopwatch.Start();
					}
					else
					{
						if (Stopwatch.ElapsedMilliseconds > 1000)
						{
							Debug.Log(" TCP Not Connect ");
							tcpClient.BeginConnect(serverIp, serverPort);
							Stopwatch.Restart();
						}
						else
							Debug.Log("not connect stopwatch time -> " + Stopwatch.ElapsedMilliseconds);
					}
				}
				else
				{
					Debug.Log(" TCP Connected");
					Stopwatch.Stop();
					Stopwatch.Reset();
				}
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}
	}
	public void SendMessage(Byte[] buffer)
	{
		if (tcpClient.Connected)
		{ 
			try
			{
				NetworkStream stream = tcpClient.GetStream();// 통신용 Stram을 가져오는것 .. 
				if (stream.CanWrite)
				{
					stream.Write(buffer, 0, buffer.Length);
					Debug.Log("Send Message -> " + buffer);
				}
			}
			catch (SocketException e)
			{
				Debug.LogException(e);
			}
		}
		else
		{
			TcpConnect();
		}
	}
	
	private void ReceiveMessage()
	{
		if (tcpClient.Connected)
		{
			try
			{
				NetworkStream stream = tcpClient.GetStream(); ;
				if (stream.CanRead)
				{
					Byte[] buffer = new Byte[1024];
					int nBytes;
					MemoryStream mem = new MemoryStream();

					while ((nBytes = stream.Read(buffer, 0, buffer.Length)) > 0)
					{
						String logString = Encoding.UTF8.GetString(buffer, 0, nBytes);
						Debug.Log("while Receive Message -> " + logString);
						mem.Write(buffer, 0, nBytes);
					}
					buffer = mem.ToArray();
					if (buffer.Length > 0)
					{ 
						String readString = Encoding.UTF8.GetString(buffer, 0, nBytes);
						Debug.Log("Receive Message -> " + readString);
					}
					buffer.Initialize();
					
				}
				else
				{
					Debug.Log("Can not Read");
				}
			}
			catch (SocketException e)
			{
				Debug.LogException(e);
			}
		}
		else
		{
			TcpConnect();
		}
	}
	
}
