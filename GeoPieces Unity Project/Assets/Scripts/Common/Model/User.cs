using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class User {

    public string user;
    public int id;
    public List<int> avatars;
    //public enum level { Level1, Level2, Level3, Level4, Level5};
    public int[] competencies;
    public Dictionary<int, Inventory> inventories;
    public List<Inventory> invs;
    public List<Figures2D> trianglesFiguresSeen;
    public List<Figures2D> quadrilateralFiguresSeen;
    public List<ObjectiveID> symmetricCompetenciesSeen;
    public List<ObjectiveID> anglesCompetenciesSeen;

    public int[] numMissionsForCompetency;
    public int[] numCheatsForCompetency;
    public int[] numSuccessMissionForCompetency;

    public int gold = 200;
    public int currentAvatar;
    public string texturePath;
    public bool tutorialSeen = false;
    public bool tutorialUnfoldingSeen = false;



    public User(string user, int id)
    {
        this.user = user;
        this.id = id;
        competencies = new int[4];
        //competencies = new Dictionary<Competencies, int>();
        competencies[(int)Competencies.Identification] = 0;
        competencies[(int)Competencies.Symmetry] = 0;
        competencies[(int)Competencies.Angles] = 0;
        competencies[(int)Competencies.AreaPerimeter] = 0;
        inventories = new Dictionary<int, Inventory>();
        trianglesFiguresSeen = new List<Figures2D>();
        quadrilateralFiguresSeen = new List<Figures2D>();
        symmetricCompetenciesSeen = new List<ObjectiveID>();
        anglesCompetenciesSeen = new List<ObjectiveID>();

        numMissionsForCompetency = new int[4];
        numCheatsForCompetency = new int[4];
        numSuccessMissionForCompetency = new int[4];
        for (int i = 0; i < 4; i++)
        {
            numMissionsForCompetency[i] = 0;
            numSuccessMissionForCompetency[i] = 0;
            numCheatsForCompetency[i] = 0;
        }

        avatars = new List<int>();
        avatars.Add(0);
        currentAvatar = 0;
    }

    public User()
    {
        this.user = "";
        //this.competencies = new Dictionary<Competencies, int>();
        competencies = new int[4];
        competencies[(int)Competencies.Identification] = 0;
        competencies[(int)Competencies.Symmetry] = 0;
        competencies[(int)Competencies.Angles] = 0;
        competencies[(int)Competencies.AreaPerimeter] = 0;
        inventories = new Dictionary<int, Inventory>();
        trianglesFiguresSeen = new List<Figures2D>();
        quadrilateralFiguresSeen = new List<Figures2D>();
        symmetricCompetenciesSeen = new List<ObjectiveID>();
        anglesCompetenciesSeen = new List<ObjectiveID>();
        texturePath = "userIcon.png";
    }

    public int getLevel(int experience)
    {
        if (experience < 30) return 0;
        else if (experience >= 30 && experience < 100) return 1;
        else if (experience >= 100 && experience < 400) return 2;
        else if (experience >= 400 && experience < 1200) return 3;
        else return 4;
    }

    internal int selectObjective()
    {
        int len = competencies.Length;
        int[] list = new int [len];

        List<int> nextMission = new List<int>();

        list[0] = getLevel(competencies[(int)Competencies.Identification]);
        list[1] = getLevel(competencies[(int)Competencies.Symmetry]);
        list[2] = getLevel(competencies[(int)Competencies.Angles]);
        list[3] = getLevel(competencies[(int)Competencies.AreaPerimeter]);

        for (int i = 0; i< len; i++)
        {
            if (list[i] <= list[0] - i)
            {
                nextMission.Add(i);
            }
        }

        if (nextMission.Count == 0)
        {
            return 0;
        }

        return nextMission[UnityEngine.Random.Range(0, nextMission.Count)];
    }

    internal void giveExperience(Competencies comp, int reward)
    {
        competencies[(int)comp] += reward;
    }

    internal void giveGold(int reward)
    {
        gold += reward;
    }

    public void save()
    {
        invs = new List<Inventory>();
        foreach (KeyValuePair<int, Inventory> pair in inventories)
        {
            invs.Add(pair.Value);
        }
        string json = JsonUtility.ToJson(this);
        StreamWriter file = new StreamWriter(Application.dataPath + "/" + user + ".txt");
        file.WriteLine(json);
        file.Close();
    }

    internal Competencies selectCompetency(List<Competencies> compForCampaign)
    {
        int[] levelsForChosenComp = new int[compForCampaign.Count];
        int max = 0;
        for (int i=0; i<compForCampaign.Count; i++)
        {
            levelsForChosenComp[i] = getLevel((int)compForCampaign[i]);
            if (levelsForChosenComp[i] > max) max = levelsForChosenComp[i]; 
        }

        int sum = 0;
        for (int i = 0; i < compForCampaign.Count; i++)
        {
            levelsForChosenComp[i] = (int)Math.Pow(2, max - levelsForChosenComp[i]);
            sum += levelsForChosenComp[i];
        }


        System.Random ran = new System.Random();
        double randomNum = ran.NextDouble() * sum;
        for (int i = 0; i < compForCampaign.Count; i++)
        {
            if (randomNum > levelsForChosenComp[i]) randomNum -= levelsForChosenComp[i];
            else
            {
                return compForCampaign[i];
            }
        }
        Debug.Log("Error. Not Competency assigned. Default: Identification.");
        return Competencies.Identification;
    }
}
