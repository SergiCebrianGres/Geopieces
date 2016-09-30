using System;
using System.Collections.Generic;

[Serializable]
public class Mission2D{
    public int id;
    public Level lev;
    public Size size;
    public float seconds;
    public List<Objectives2D> objectives;
    public bool discover;

    public int experienceReward;
    public int goldReward;
    public float coinFixReward;
    public int numReward;
    public RewardType typeReward;

    public float maxTimeStar;

    public bool completed;

    public Mission2D()
    {
        objectives = new List<Objectives2D>();
    }

    // Use this for initialization
    void Start () {
        
	}
}
