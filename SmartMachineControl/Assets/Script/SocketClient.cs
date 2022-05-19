using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using UnityEngine;
using System;

public class SocketClient : MonoBehaviour
{
	TcpClient mTcpClient;
	byte[] mReceiveBuffer;
	byte[] mSendBuffer;
	int mBufferSize = 1024;

	bool m_isConnected = false;
	private bool Connected { get => mTcpClient == null ? false : mTcpClient.Connected && m_isConnected; } //람다 .. 

	int mPort;
	IPAddress mIPAddress;

	System.Diagnostics.Stopwatch mStopwatch = new System.Diagnostics.Stopwatch();

	// Start is called before the first frame update
	void Start()
    {
		TCPConnect();
		TimeCheck();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void TCPConnect()
	{
		if (mTcpClient != null && Connected) return;

		string inputServerIP = "172.30.1.23"; //입력 받는 부분으로 변경하면 바꿔서 사용...
		string inputServerPort = "5556";
		int serverPort;
		IPAddress iPA;
		
		try
		{
			if (string.IsNullOrEmpty(inputServerIP) || string.IsNullOrEmpty(inputServerPort))
				return;
			if (!IPAddress.TryParse(inputServerIP, out iPA))
			{
				Debug.Log("IP 재 확인 필요");
				return;
			}
			if (!int.TryParse(inputServerPort, out serverPort))
			{
				serverPort = 5556;
			}

			mTcpClient = new TcpClient();
			mTcpClient.BeginConnect(iPA, serverPort, OnCompleteConnect, mTcpClient);

			mIPAddress = iPA;
			mPort = serverPort;
			m_isConnected = true;
			mStopwatch.Start();
		}
		catch (Exception e)
		{
			m_isConnected = false;
			Debug.LogException(e);
		}
	}

	void OnCompleteConnect(IAsyncResult iar)
	{
		TcpClient tcpClient;

		try
		{
			tcpClient = (TcpClient)iar.AsyncState;
			tcpClient.EndConnect(iar);
			mReceiveBuffer = new byte[mBufferSize];
			tcpClient.GetStream().BeginRead(mReceiveBuffer, 0, mReceiveBuffer.Length, OnCompleteReadFromServerStream, tcpClient);
			m_isConnected = true;
		}
		catch (Exception e)
		{
			Debug.LogException(e);
			m_isConnected = false;
		}
	}

	void OnCompleteReadFromServerStream(IAsyncResult iar)
	{
		TcpClient tcpClient;
		int CountByReceivedFromServer;
		string stringReceived;

		try
		{
			tcpClient = (TcpClient)iar.AsyncState;
			CountByReceivedFromServer = tcpClient.GetStream().EndRead(iar);

			if(CountByReceivedFromServer == 0)
			{
				Debug.Log("Connect broken");
				return;
			}
			stringReceived = Encoding.UTF8.GetString(mReceiveBuffer, 0, CountByReceivedFromServer);
			string s1 = Encoding.UTF8.GetString(mReceiveBuffer, 0, CountByReceivedFromServer - 1);
			Debug.Log("read Data => " + s1);
			if (mReceiveBuffer[CountByReceivedFromServer] != '\n') return;

			m_isConnected = true;
		}
		catch (Exception e)
		{
			m_isConnected = false;
			Debug.LogException(e);
		}
	}
	void SendData()
	{
		string inputString = "TEST"; 

		if (string.IsNullOrEmpty(inputString)) return;

		try
		{
			if (!Connected) return;

			mSendBuffer = Encoding.UTF8.GetBytes(inputString + "\r\n");
			if (mTcpClient != null)
			{
				if(mTcpClient.Client.Connected)
				{
					mTcpClient.GetStream().BeginWrite(mSendBuffer, 0, mSendBuffer.Length, OnCompleteWriteToServer, mTcpClient);
				}
			}
		}
		catch(Exception e)
		{
			Debug.LogException(e);
			m_isConnected = false;
		}
	}

	void OnCompleteWriteToServer(IAsyncResult iar)
	{
		TcpClient tcpClient;

		try
		{
			tcpClient = (TcpClient)iar.AsyncState;
			tcpClient.GetStream().EndWrite(iar);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	void TimeCheck()
	{
		if (mStopwatch.ElapsedMilliseconds > 1000)
		{
			mStopwatch.Stop();
			if (!mTcpClient.Connected && !m_isConnected)
			{
				m_isConnected = true;
				mTcpClient.Close();
				mTcpClient = new TcpClient();
				mTcpClient.BeginConnect(mIPAddress, mPort, OnCompleteConnect, mTcpClient);
			}
			mStopwatch.Restart();
		}
	}
}
