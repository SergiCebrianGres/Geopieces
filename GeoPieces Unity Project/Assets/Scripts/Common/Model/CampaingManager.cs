using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class CampaingManager {
    public int numCampaigns;
    public List<Campaign> camps;

    public void saveCampaings()
    {
        numCampaigns = camps.Count;
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Application.dataPath + "/Docs/campaigns.json", json);
        //AssetDatabase.Refresh();
    }

    public void loadCampaings()
    {
        TextAsset text = Resources.Load<TextAsset>("Campaigns/campaigns");
        //string json = File.ReadAllText(Application.dataPath + "/Docs/campaigns.json");
        string json = text.text;
        CampaingManager aux = JsonUtility.FromJson<CampaingManager>(json);
        //Debug.Log(json);
        this.numCampaigns = aux.numCampaigns;
        this.camps = aux.camps;
        numCampaigns = camps.Count;
    }
}
