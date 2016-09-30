using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InfoCampaignController : MonoBehaviour {
    private Campaign cam;
    private User user;
    private Inventory inv;
    public Text details, tags;
	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("DontDestroyOnLoad");
        noDestroy script = go.GetComponent<noDestroy>();
        cam = script.c;

        user = script.u;
        inv = script.u.inventories[cam.id];

        showDetails();
    }

    private void showDetails()
    {
        tags.text = "Campaign name: \n\n";
        details.text = cam.name + "\n\n";

        if (cam.weightsMatter)
        {
            tags.text += "Weights\n";
            details.text += "\n";

            float sum = 0;
            for (int i = 0; i < cam.weights.Length; i++)
            {
                sum += cam.weights[i];
            }

            for (int i = 0; i < cam.weights.Length; i++)
            {
                if (cam.weights[i] != 0)
                {
                    tags.text += (Competencies)i + ": \n";
                    details.text += Math.Round((cam.weights[i] / sum),2) * 100 + "% \n";
                } 
            }
        }
        else
        {
            tags.text += "Competencies to learn: \n";
            details.text += "\n";

            for (int i = 0; i < 4; i++)
            {
                tags.text += (Competencies)i + ":\n";

                if (cam.compToLearn.Contains((Competencies)i)) details.text += "Yes\n";
                else details.text += "No\n";
            }
        }

        float completed = 0, total = 0;

        foreach (InventoryItem ii in inv.items)
        {
            total += ii.initialGoal;
            completed += ii.quantity + (ii.initialGoal - ii.campaignGoal);
        }

        tags.text += "\n Completed: ";
        details.text += "\n" + Math.Round((completed / total)*100,2) + "%";   

    }

    public void returnToCampaignMenu()
    {
        SceneManager.LoadScene(3);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
