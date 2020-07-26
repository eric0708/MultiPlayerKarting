using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 old_pos = new Vector3(0,0,0);
    public Quaternion old_rot = new Quaternion(0,0,0,0);
    public int _send = 1;
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer()
    {
        int cnt = GameManager.players.Count;
        if (cnt<=6){
            _send = 1;
        }
        Vector3 new_pos = new Vector3(GameManager.players[client.instance.id].transform.position.x, GameManager.players[client.instance.id].transform.position.y, GameManager.players[client.instance.id].transform.position.z);
        Quaternion new_rot = new Quaternion(GameManager.players[client.instance.id].transform.rotation.x,GameManager.players[client.instance.id].transform.rotation.y,GameManager.players[client.instance.id].transform.rotation.z,GameManager.players[client.instance.id].transform.rotation.w);
        var diff = new_pos-old_pos;
        var ang = Quaternion.Angle(old_rot,new_rot);
        Debug.Log($"mag{diff.magnitude} ang {ang}");
        if (diff.magnitude >= 0.01f || ang !=0f){
            if (_send == 1){
                ClientSend.PlayerMovement(new_pos);
            }
        }
        if (cnt>6){
            _send*=-1;
        }
        old_pos = new_pos;
        old_rot = new_rot;
    }
}