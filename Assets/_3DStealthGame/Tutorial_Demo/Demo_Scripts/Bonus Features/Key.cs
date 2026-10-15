using System;
using UnityEngine;

namespace StealthGame
{
    public class Key : MonoBehaviour
    {
        public string KeyName = "key1";

        private void OnTriggerEnter(Collider other)
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();

            //this wasn't a player
            if (player == null)
                return;
        
            player.AddKey(KeyName);
            Destroy(gameObject);
        }
    }
}