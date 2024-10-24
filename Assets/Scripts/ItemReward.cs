using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;


namespace MyGame.GuestSystem
{
    public class ItemReward : IQuestReward
    {
        private string itemid;
        private int amount;

        public ItemReward(string itemid, int amount)
        {
            this.itemid = itemid;
            this.amount = amount;
        }

        public void Grand(GameObject player)
        {
            Debug.Log($"Geanted {amount} {itemid}");
        }

        public string GetDescription() => $"{amount} {itemid}";

        public void Grant(GameObject player)
        {
            throw new System.NotImplementedException();
        }
    }
}

