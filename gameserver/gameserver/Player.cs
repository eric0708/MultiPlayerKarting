using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace gameserver
{
    class Player
    {
        public int id;
        public string username;

        public Vector3 position;
        public Quaternion rotation;

        private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
        private Vector3 inputs;
        public Vector3 old_pos = new Vector3(0,0,0);
        public Quaternion old_rot = new Quaternion(0,0,0,0);
        public int _send = 1;
        public Player(int _id, string _username, Vector3 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = Quaternion.Identity;

            inputs = new Vector3();
        }

        public void Update()
        {
            Vector2 _inputDirection = Vector2.Zero;
            Move(_inputDirection);
        }

        private void Move(Vector2 _inputDirection)
        {
            int cnt = Server.clients.Count;
            if (cnt<=6){
                _send = 1;
            }
            this.position = inputs;
            Vector3 new_pos = this.position;
            Quaternion new_rot = this.rotation;
            var diff = new_pos-old_pos;
            var diff_2 = new_rot-old_rot;
            var ang = Math.Pow(diff_2.X,2)+Math.Pow(diff_2.Y,2)+Math.Pow(diff_2.Z,2)+Math.Pow(diff_2.W,2);
            var mag = Math.Pow(diff.X,2)+Math.Pow(diff.Y,2)+Math.Pow(diff.Z,2);
            if (Math.Pow(mag,0.5) >= 0.01f || Math.Pow(ang,0.5)>=0.01f){
                if (_send == 1){
                    ServerSend.PlayerPosition_Rotation(this);
                }
            }
            //ServerSend.PlayerRotation(this);
            if (cnt>6){
                _send*=-1;
            }
            old_pos = new_pos;
            old_rot = new_rot;
        }

        public void SetInput(Vector3 _inputs, Quaternion _rotation)
        {
            inputs = _inputs;
            rotation = _rotation;
        }
    }
}