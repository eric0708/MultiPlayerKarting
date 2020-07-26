using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(client.instance.id);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(Vector3 _input)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {

            _packet.Write(_input);
            if (client.instance.isConnected)
            {
                Debug.Log($"Send local player position ({_input.x}, {_input.y}, {_input.z})");
                _packet.Write(GameManager.players[client.instance.id].transform.rotation);

                SendUDPData(_packet);
            }
        }
    }
    #endregion
}
