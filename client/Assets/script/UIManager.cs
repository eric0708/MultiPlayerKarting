using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    public InputField IPaddress;
    public InputField Port;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        client.instance.ConnectToServer(IPaddress.text.ToString(), int.Parse(Port.text));
    }

    public void ConnectToServerUDP(int _local)
    {
        client.instance.udp.Connect(_local, IPaddress.text.ToString(), int.Parse(Port.text));
        Debug.Log("test for connect to server with UDP");
    }
}
