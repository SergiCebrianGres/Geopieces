using UnityEngine;
using System;

[Serializable]
public class InventoryItem {
    public RewardType name;
    public int quantity;
    public int campaignGoal;
    public int initialGoal;
    private string texturePath;
    private Texture displayTexture;
    public InventoryItemType type;
    public int[] cost = new int[3];


    public void loadTexture()
    {
        texturePath = "Textures/" + name.ToString();
        displayTexture = getTextureFromPath();
    }

    private Texture getTextureFromPath()
    {
        return Resources.Load<Texture>(texturePath);
    }

    internal Texture getDisplayTexture()
    {
        return displayTexture;
    }
}
