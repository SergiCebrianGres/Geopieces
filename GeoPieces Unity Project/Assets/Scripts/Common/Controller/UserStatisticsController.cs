using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class UserStatisticsController : MonoBehaviour {

    private User user;
    private noDestroy script;
    public Font font;

    // Use this for initialization
    void Start () {
        GameObject go = GameObject.Find("DontDestroyOnLoad");
        script = go.GetComponent<noDestroy>();

        user = script.u;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void returnToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    void OnGUI()
    {
        showProgress();
    }

    private void showProgress()
    {

        float sw = Screen.width;
        float sh = Screen.height;
        float xPosition = 0.1f * sw;


        float xPositionTemp = xPosition;
        float yPosition = 0.1f * sh;

        int numCells = user.competencies.Length + 4;

        float height = sh / (numCells + 2);

        float widthCompetencies, widthUserName, widthLev, widthExp;

        widthCompetencies = height * 4;
        widthUserName = height * 3;
        widthLev = height;
        widthExp = height * 2;

        float totalWidth = widthCompetencies + widthLev + 4 * widthExp;
        xPosition = (.5f * (1 - (totalWidth / sw)))*sw;


        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleLeft;
        style.fontSize = (int)(height *2/ 3);
        style.font = font;
        style.normal.textColor = Color.white;

        xPositionTemp = xPosition;
        Rect userTag, nameTag;
        userTag = new Rect(xPositionTemp, yPosition, widthUserName/2, height);
        xPositionTemp += widthUserName/2;
        style.alignment = TextAnchor.MiddleRight;
        GUI.Label(userTag, "Name: ", style);
        nameTag = new Rect(xPositionTemp, yPosition, widthUserName, height);
        style.alignment = TextAnchor.MiddleLeft;
        GUI.Label(nameTag, user.user, style);

        xPositionTemp = xPosition;
        yPosition += height;
        Rect goldTag, goldIm, quantityTag;
        goldTag = new Rect(xPosition, yPosition, widthUserName/2, height);
        xPositionTemp += widthUserName/2;
        style.alignment = TextAnchor.MiddleRight;
        GUI.Label(goldTag, "Gold: ", style);
        goldIm = new Rect(xPositionTemp, yPosition + 0.25f * height, height * 0.5f, height * 0.5f);
        string goldText = "Textures/gold";
        Texture2D goldTexture = Resources.Load<Texture2D>(goldText);
        GUI.DrawTexture(goldIm, goldTexture);
        xPositionTemp += height * 0.5f;
        quantityTag = new Rect(xPositionTemp, yPosition, widthUserName, height);
        style.alignment = TextAnchor.MiddleLeft;
        GUI.Label(quantityTag, "" + user.gold, style);

        style.alignment = TextAnchor.MiddleCenter;
        xPositionTemp = xPosition;
        yPosition += height;
        Rect competenciesTag, levTag, expTag, numMissionsTag, numSuccesTag, numCheatTag;
        competenciesTag = new Rect(xPosition, yPosition, widthCompetencies, height);
        xPositionTemp += widthCompetencies;
        levTag = new Rect(xPositionTemp, yPosition, widthLev, height);
        xPositionTemp += widthLev;
        expTag = new Rect(xPositionTemp, yPosition, widthExp, height);
        xPositionTemp += widthExp;
        numMissionsTag = new Rect(xPositionTemp, yPosition, widthExp, height);
        xPositionTemp += widthExp;
        numSuccesTag = new Rect(xPositionTemp, yPosition, widthExp, height);
        xPositionTemp += widthExp;
        numCheatTag = new Rect(xPositionTemp, yPosition, widthExp, height);
        GUI.Label(competenciesTag, "", style);
        GUI.Label(levTag, "Level", style);
        GUI.Label(expTag, "Exp", style);
        GUI.Label(numMissionsTag, "Missions", style);
        GUI.Label(numSuccesTag, "Success", style);
        GUI.Label(numCheatTag, "Cheat", style);



        List<Competencies> compList = new List<Competencies>();
        compList.Add(Competencies.Identification);
        compList.Add(Competencies.Symmetry);
        compList.Add(Competencies.Angles);
        compList.Add(Competencies.AreaPerimeter);
        foreach (Competencies c in compList)
        {
            xPositionTemp = xPosition;
            yPosition += height;
            competenciesTag = new Rect(xPosition, yPosition, widthCompetencies, height);
            xPositionTemp += widthCompetencies;
            levTag = new Rect(xPositionTemp, yPosition, widthLev, height);
            xPositionTemp += widthLev;
            expTag = new Rect(xPositionTemp, yPosition, widthExp, height);
            xPositionTemp += widthExp;
            numMissionsTag = new Rect(xPositionTemp, yPosition, widthExp, height);
            xPositionTemp += widthExp;
            numSuccesTag = new Rect(xPositionTemp, yPosition, widthExp, height);
            xPositionTemp += widthExp;
            numCheatTag = new Rect(xPositionTemp, yPosition, widthExp, height);


            if (c == Competencies.AreaPerimeter) GUI.Label(competenciesTag, "Area & Perimeter", style);
            else GUI.Label(competenciesTag, c.ToString(), style);
            GUI.Label(levTag, "" + user.getLevel(user.competencies[(int)c]), style);
            GUI.Label(expTag, "" + user.competencies[(int)c], style);
            GUI.Label(numMissionsTag, "" + user.numMissionsForCompetency[(int)c], style);
            GUI.Label(numSuccesTag, "" + user.numSuccessMissionForCompetency[(int)c], style);
            GUI.Label(numCheatTag, "" + user.numCheatsForCompetency[(int)c], style);
        }

        xPositionTemp = xPosition;
        yPosition += height;
        Rect finishedCampaigns = new Rect(xPosition, yPosition, widthCompetencies * 2, height);
        style.alignment = TextAnchor.MiddleCenter;

        int finished = 0;
        for (int i=0; i< user.invs.Count; i++)
        {
            if (user.invs[i].completed) finished += 1;
        }
        GUI.Label(finishedCampaigns, "Finished Campaigns: " + finished + "/" + user.inventories.Count, style);



    }
}
