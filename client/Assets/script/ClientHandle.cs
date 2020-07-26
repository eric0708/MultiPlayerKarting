using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Net;

public class ClientHandle : MonoBehaviour
{
    protected static float XRotateVelocity;
    protected static float YRotateVelocity;
    protected static float ZRotateVelocity;
    protected static float WRotateVelocity;
    protected static float XPositionVelocity;
    protected static float YPositionVelocity;
    protected static float ZPositionVelocity;
    protected static float con = 2f;
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();
        Debug.Log($"Message from server: {_msg}");
        client.instance.id = _myId;
        ClientSend.WelcomeReceived();
        Debug.Log("Get the welcome message");
        UIManager.instance.ConnectToServerUDP(((IPEndPoint)client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }
    
    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        if ((_id != client.instance.id))
        {
            Debug.Log($"UDP Message from server: {_id} and new position x:{ _position.x}");
            try
            {
                while(GameManager.players[_id].transform.position != _position)
                {
                    float newPositionX = Mathf.SmoothDamp(GameManager.players[_id].transform.position.x, _position.x, ref XPositionVelocity, Time.deltaTime);
                    float newPositionY = Mathf.SmoothDamp(GameManager.players[_id].transform.position.y, _position.y, ref YPositionVelocity, Time.deltaTime);
                    float newPositionZ = Mathf.SmoothDamp(GameManager.players[_id].transform.position.z, _position.z, ref ZPositionVelocity, Time.deltaTime);
                    GameManager.players[_id].transform.position = new Vector3(newPositionX, newPositionY, newPositionZ);
                }
                /* 
                var LagDistance = GameManager.players[_id].transform.position - _position;

                if(LagDistance.magnitude < 0.1f)
                {
                    GameManager.players[_id].transform.position = _position;
                    LagDistance = Vector3.zero;
                }
                else
                {
                    while(LagDistance.magnitude >= 0.1f)
                    {
                        GameManager.players[_id].transform.position = _position + 0.1f*LagDistance.normalized;
                    }
                }
                */

            }
            catch (KeyNotFoundException e)
            {
                // do nothing, since we have not spawn the client
                Debug.Log($"{e}");
            }
        }
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        try
        {
            while(GameManager.players[_id].transform.rotation != _rotation)
            {
                float newRotationX = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.x, _rotation.x, ref XRotateVelocity, Time.deltaTime);
                float newRotationY = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.y, _rotation.y, ref YRotateVelocity, Time.deltaTime);
                float newRotationZ = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.z, _rotation.z, ref ZRotateVelocity, Time.deltaTime);
                float newRotationW = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.w, _rotation.w, ref WRotateVelocity, Time.deltaTime);
                GameManager.players[_id].transform.rotation = new Quaternion(newRotationX, newRotationY, newRotationZ, newRotationW);
            }
            
        }
        catch (KeyNotFoundException e)
        {
            Debug.Log($"{e}");
            // do nothing, since we have not spawn the client
        }
    }
    
     public static void PlayerPosition_Rotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        if ((_id != client.instance.id))
        {
            Debug.Log($"UDP Message from server: {_id} and new position x:{ _position.x}");
            try
            {
                /* 
                float packetarrivaltime = Time.time;
                float timeinterval = packetarrivaltime - GameManager.players[_id].lastpacketarrivaltime;
                GameManager.players[_id].times.Dequeue();
                GameManager.players[_id].times.Enqueue(timeinterval);
                GameManager.players[_id].lastpacketarrivaltime = packetarrivaltime;
                float averagetimeinterval = GameManager.players[_id].times.Average();
                */
                GameManager.players[_id].moving += 1;
                /* 
                if(GameManager.players[_id].moving == 1)
                {
                    while(((GameManager.players[_id].transform.rotation != _rotation) || (GameManager.players[_id].transform.position != _position)) && (GameManager.players[_id].moving == 1))
                    {
                        float newPositionX = Mathf.SmoothDamp(GameManager.players[_id].transform.position.x, _position.x, ref XPositionVelocity, 2f*Time.deltaTime);
                        float newPositionY = Mathf.SmoothDamp(GameManager.players[_id].transform.position.y, _position.y, ref YPositionVelocity, 2f*Time.deltaTime);
                        float newPositionZ = Mathf.SmoothDamp(GameManager.players[_id].transform.position.z, _position.z, ref ZPositionVelocity, 2f*Time.deltaTime);
                        float newRotationX = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.x, _rotation.x, ref XRotateVelocity, 2f*Time.deltaTime);
                        float newRotationY = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.y, _rotation.y, ref YRotateVelocity, 2f*Time.deltaTime);
                        float newRotationZ = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.z, _rotation.z, ref ZRotateVelocity, 2f*Time.deltaTime);
                        float newRotationW = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.w, _rotation.w, ref WRotateVelocity, 2f*Time.deltaTime);
                        GameManager.players[_id].transform.SetPositionAndRotation(new Vector3(newPositionX, newPositionY, newPositionZ), new Quaternion(newRotationX, newRotationY, newRotationZ, newRotationW));
                    }

                    GameManager.players[_id].moving -= 1;
                }
                else
                {
                    while(((GameManager.players[_id].transform.rotation != _rotation) || (GameManager.players[_id].transform.position != _position)) && (GameManager.players[_id].moving != 1))
                    {
                        float newPositionX = Mathf.SmoothDamp(GameManager.players[_id].transform.position.x, _position.x, ref XPositionVelocity, 2f*Time.deltaTime);
                        float newPositionY = Mathf.SmoothDamp(GameManager.players[_id].transform.position.y, _position.y, ref YPositionVelocity, 2f*Time.deltaTime);
                        float newPositionZ = Mathf.SmoothDamp(GameManager.players[_id].transform.position.z, _position.z, ref ZPositionVelocity, 2f*Time.deltaTime);
                        float newRotationX = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.x, _rotation.x, ref XRotateVelocity, 2f*Time.deltaTime);
                        float newRotationY = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.y, _rotation.y, ref YRotateVelocity, 2f*Time.deltaTime);
                        float newRotationZ = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.z, _rotation.z, ref ZRotateVelocity, 2f*Time.deltaTime);
                        float newRotationW = Mathf.SmoothDamp(GameManager.players[_id].transform.rotation.w, _rotation.w, ref WRotateVelocity, 2f*Time.deltaTime);
                        GameManager.players[_id].transform.SetPositionAndRotation(new Vector3(newPositionX, newPositionY, newPositionZ), new Quaternion(newRotationX, newRotationY, newRotationZ, newRotationW));
                    }
                }
                */

                if(GameManager.players[_id].moving == 1)
                {
                    while((GameManager.players[_id].transform.position != _position) && (GameManager.players[_id].moving == 1))
                    {
                        float newPositionX = Mathf.SmoothDamp(GameManager.players[_id].transform.position.x, _position.x, ref XPositionVelocity, con*Time.deltaTime);
                        float newPositionY = Mathf.SmoothDamp(GameManager.players[_id].transform.position.y, _position.y, ref YPositionVelocity, con*Time.deltaTime);
                        float newPositionZ = Mathf.SmoothDamp(GameManager.players[_id].transform.position.z, _position.z, ref ZPositionVelocity, con*Time.deltaTime);
                        GameManager.players[_id].transform.SetPositionAndRotation(new Vector3(newPositionX, newPositionY, newPositionZ), _rotation);
                    }

                    GameManager.players[_id].moving -= 1;
                }
                else
                {
                    while((GameManager.players[_id].transform.position != _position) && (GameManager.players[_id].moving != 1))
                    {
                        float newPositionX = Mathf.SmoothDamp(GameManager.players[_id].transform.position.x, _position.x, ref XPositionVelocity, con*Time.deltaTime);
                        float newPositionY = Mathf.SmoothDamp(GameManager.players[_id].transform.position.y, _position.y, ref YPositionVelocity, con*Time.deltaTime);
                        float newPositionZ = Mathf.SmoothDamp(GameManager.players[_id].transform.position.z, _position.z, ref ZPositionVelocity, con*Time.deltaTime);
                        GameManager.players[_id].transform.SetPositionAndRotation(new Vector3(newPositionX, newPositionY, newPositionZ), _rotation);
                    }
                }
                
            }
            catch (KeyNotFoundException e)
            {
                // do nothing, since we have not spawn the client
                Debug.Log($"{e}");
            }
        }
    }

}
