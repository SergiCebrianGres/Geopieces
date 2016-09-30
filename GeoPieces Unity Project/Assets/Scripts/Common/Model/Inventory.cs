using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Inventory {
    public int campaignID;
    public bool completed;
    public List<InventoryItem> items = new List<InventoryItem>();

    public Inventory (int id)
    {
        campaignID = id;
        items = new List<InventoryItem>();
    }

    public void Add(RewardType iname, int quantity) { 
        foreach (var item in items)
        {
            if (item.name == iname)
            {
                item.quantity = Mathf.Max(item.quantity + quantity, 0);
            }
        }
    }

    public bool hasParts(InventoryItem ii)
    {
        int s = 0, t = 0, p = 0;
        foreach (var item in items)
        {
            if (item.name == RewardType.Square) s = item.quantity;
            if (item.name == RewardType.Triangle) t = item.quantity;
            if (item.name == RewardType.Pentagon) p = item.quantity;
        }
        return (t >= ii.cost[0] && s >= ii.cost[1] && p >= ii.cost[2]);
    }

    public int getQuantity(RewardType type)
    {
        foreach (var item in items)
        {
            if (item.name == type) return item.quantity;
        }
        return 0;
    }

    public InventoryItem getII(RewardType type)
    {
        foreach (var item in items)
        {
            if (item.name == type) return item;
        }
        return null;
    }

    public void Spend(RewardType name, int i)
    {
        foreach (var item in items)
        {
            if (item.name == name)
            {
                item.quantity -= i;
                item.campaignGoal -= i;
            }
        }
    }

    public void init()
    {
        foreach (var item in items)
        {
            item.initialGoal = item.campaignGoal;
        }
    }

    internal bool checkIfFinished(RewardType typeReward)
    {
        foreach (var item in items)
        {
            if (item.name == typeReward)
            {
                return (item.campaignGoal - item.quantity <= 0);
            }
        }
        return false;
    }
}
