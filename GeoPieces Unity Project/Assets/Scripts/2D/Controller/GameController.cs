using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public Inventory inv;
    public GameObject noDestroyGO;
    public User user;
    //public int cId;
    public Campaign currentCampaign;

    public bool countdown;
    public float countdownTime;
    public bool semaphore = false;
    public RewardType rewardType;

    public float countdownBomb = 0;
    public bool bombactive = false;

    public List<Competencies> compForCampaign;

    public HUDController hudController;
    //private InventoryController inventoryController;
    public MissionController missionController;
    
    public static GameController Instance { get; private set; }
    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        noDestroyGO = GameObject.Find("DontDestroyOnLoad");
        noDestroy noDestroyScript = noDestroyGO.GetComponent<noDestroy>();
        user = noDestroyScript.u;
        currentCampaign = noDestroyScript.c;
        inv = noDestroyScript.u.inventories[currentCampaign.id];

        compForCampaign = currentCampaign.compToLearn;

        missionController.start2D();
        hudController.start2D();
      

        if (noDestroyScript.to2D)
        {
            //hudController.setGetMoreButtonInteractable(true);
            hudController.activatePanelStartMission();
            sendNewMission(noDestroyScript.reward);
        }

        //TODO: rest values no Destroy
    }

    private void activateCountdown()
    {
        countdownTime = 6.0f;
        missionController.setPlaying(false);
        countdown = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (missionController.isPlaying())
        {
            missionController.playingUpdate();
        }

        if (semaphore)
        {
            missionController.semaphoreUpdate();
        }

        if (countdown)
        {
            controllCountDown();
        }

        if (bombactive)
        {
            controllBomb();
        }
    }

    private void controllBomb()
    {
        countdownBomb -= Time.deltaTime;

        if (countdownBomb <= 0)
        {
            bombactive = false;
            missionController.controlFinishBomb();
            hudController.controlFinishBomb();
        }
    }

    private void controllCountDown()
    {
        countdownTime -= Time.deltaTime;

        hudController.showCountdown(countdownTime, false);

        if (countdownTime <= 1)
        {
            hudController.showCountdown(countdownTime, true);
        }
        if (countdownTime <= 0)
        {
            countdown = false;
            startMission();
        }
    }

    public void showMenu()
    {
        hudController.showMenu();
    }

    
    public void finishMission()
    {
        //hudController.manageMissionFinished();
        missionController.manageMissionFinished();
    }

    public void startMission()
    {
        hudController.loadMission();
        missionController.loadMission();
    }

    public void continueWithMission()
    {
        hudController.loadMission();
        missionController.setPlaying(true);
    }

    public void sendNewMission(RewardType reward)
    {
        rewardType = reward;
        missionController.sendNewMission(reward, user);
        activateCountdown();
    }

    public void getMore()
    {
        
        hudController.activatePanelStartMission();

        missionController.sendNewMission(rewardType, user);
        activateCountdown();

    }

    public void sendNewMission()
    {
        List<InventoryItem> reward = new List<InventoryItem>();

        Inventory inv = user.inventories[0];

        foreach (InventoryItem ii in inv.items)
        {
            if (ii.type == InventoryItemType.type2D)
            {
                reward.Add(ii);
            }
        }
        

        missionController.sendNewMission(reward[UnityEngine.Random.Range(0, reward.Count)].name, user);
    }

    public void returnToCampaign()
    {
        if (countdown) user.numCheatsForCompetency[(int)missionController.currentComp] += 1;
        noDestroyGO = GameObject.Find("DontDestroyOnLoad");
        noDestroy noDestroyScript = noDestroyGO.GetComponent<noDestroy>();
        noDestroyScript.to2D = false;

        SceneManager.LoadScene(3);
    }
    
    public void onInfoPress()
    {
        countdown = false;

        string infoObj = missionController.getInfoPanel();
        hudController.activateInfoPanel();
        hudController.showInfo(infoObj);
    }

    public void backToCountdown()
    {
        hudController.activatePanelStartMission();
        countdown = true;
    }

    
}
