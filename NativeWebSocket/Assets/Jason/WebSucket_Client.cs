using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using NativeWebSocket;
using System;
public class WebSucket_Client : Singleton<WebSucket_Client>
{
  WebSocket websocket;
  public InputField ipt;
  public Button sendData;
  public Action<byte[]> WebSocketApi;//�����K�u�n�ק�өe���Y�i ���Υh��  ws.OnMessage
  async void Start()
  {
    websocket = new WebSocket("ws://localhost:8080");
    websocket.OnOpen += () =>
    {
      Debug.Log("Connection open!");
    };

    websocket.OnError += (e) =>
    {
      Debug.Log("Error! " + e);
    };

    websocket.OnClose += (e) =>
    {
      Debug.Log("Connection closed!");
    };

    //websocket.OnMessage += (bytes) =>
    //{
    //  // Reading a plain text message
    //  var message = System.Text.Encoding.UTF8.GetString(bytes);
    //  Debug.Log(message);
    //};
    Action<string> GetData = (data) => { Debug.Log(data); };
    websocket.OnMessage += (bytes) => WebSocketApi(bytes);//��Lamda�૬�� eventhandler
    sendData.onClick.AddListener(() => {
      
      // SendMessageToServer(ipt.text);
      CallApi("ws://localhost:8080", ipt.text, GetData);
    });
    WebSocketApi = GetMessage;
    await websocket.Connect();
    //�g�b�o���U
  }

  // Update is called once per frame
  void Update()
  {
   websocket.DispatchMessageQueue();//�ƶ�������²��ի׵{�ǡC �@�w�n�[�W
 
  }
  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }

  
  public void GetMessage(byte[] data) {
    var message = System.Text.Encoding.UTF8.GetString(data);
    Debug.Log("GetMessage ����T�� �A���׬�: (" + data.Length + " bytes)  ,���e :" + message);
    WebSocketApi = GetMessage2;
  }
  public  void GetMessage2(byte[] data)
  {
    var message = System.Text.Encoding.UTF8.GetString(data);
    Debug.Log("GetMessage2 ����T�� �A���׬�: (" + data.Length + " bytes)  ,���e :" + message);
    WebSocketApi = GetMessage;
  }
  async void SendMessageToServer(string message) {
     
    await websocket.SendText(message);
  }


  public async void CallApi(string url, string message, Action<string> CallBack)
  {
    WebSocket ws_ = new WebSocket(url);//�s���� websocket
    ws_.OnMessage += (bytes) => {
      var data = System.Text.Encoding.UTF8.GetString(bytes);
      CallBack(data);
    };
    Debug.Log("Connect");
    await ws_.Connect(); 
    Debug.Log("SendText");
    await ws_.SendText(message); 
  }
}

