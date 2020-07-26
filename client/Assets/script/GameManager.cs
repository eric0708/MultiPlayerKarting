using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;


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

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == client.instance.id)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
            GameObject textfield = _player.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject;
            textfield.GetComponent<Text>().text = _username;
        }
        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        _player.GetComponent<PlayerManager>().moving = 0;
        /* 
        for(int i = 0; i < 10; i++)
        {
            _player.GetComponent<PlayerManager>().times.Enqueue(0.1f);
        }
        _player.GetComponent<PlayerManager>().lastpacketarrivaltime = Time.time;
        */
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }
}