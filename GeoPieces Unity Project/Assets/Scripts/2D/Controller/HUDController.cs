using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour{

    //buttons
    public Button finishButton;
    public Button getMoreButton;

    //feedback elements
    public Text timerText;
    public Text resultText;
    public Text resultText2;
    public Text reward2Text;
    public Text tryagainText;

    public Text countdownText;
    public Text star1Text;
    public Text star2Text;
    public Text star3Text;
    public Text preStar1Text;
    public Text preStar2Text;
    public Text preStar3Text;

    public Text textInfoPanel;

    //Canvas
    public GameObject playingScene;

    //Panels
    public GameObject tryAgainPanel;
    public GameObject startMissionPanel;
    public GameObject rewardPanel;
    public GameObject infoPanel;

    //figures materials
    public Material highlightMaterial;
    public Material defaultMaterial;

    //Textures
    public Sprite star;
    public Sprite noStar;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public GameObject result_star1;
    public GameObject result_star2;
    public GameObject result_star3;

    //Semaphore gameObjects
    public GameObject greenSemaphore, yellowSemaphore, redSemaphore;
    public List<GameObject> semaphore;

    //Semaphore variables
    public bool showSemaphoreGUI = false;
    public int numCorrectFigures;
    public int numNoSelectedCorrectFigures;
    public int numErrors;
    public int numCells;

    public float start;

    internal int nextToShow;
    internal float deltaSemaphore;

    public Transform traficLightsContainer;

    private bool[] stars = new bool[3];


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    internal void start2D()
    {

        timerText.text = "";

        semaphore = new List<GameObject>();
        for (int i=0; i<3; i++)
        {
            stars[i] = false;
        }
    }



    internal void showMenu()
    {
        startMissionPanel.SetActive(false);
        rewardPanel.SetActive(false);
        tryAgainPanel.SetActive(false);
        playingScene.SetActive(false);

        //startGameScreen.SetActive(true);
    }


    internal void loadMission()
    {
        //inventory.SetActive(false);
        
        playingScene.SetActive(true);
        initStars();
        //startGameScreen.SetActive(false);
        startMissionPanel.SetActive(false);
        rewardPanel.SetActive(false);
        tryAgainPanel.SetActive(false);
        infoPanel.SetActive(false);


    }

    internal void controlFinishBomb()
    {
        destroySemaphores();
        activateTryAgainPanel();

        tryagainText.text = "A bomb just exploted! Be careful... and try again!";
    }

    internal void controlFinishTimeOver()
    {
        destroySemaphores();
        activateTryAgainPanel();

        tryagainText.text = "Time is over. You need to improve. C'mon! I know you can do a lot better!";
    }

    public void initStars()
    {
        activeStar(1,true);
        activeStar(2, true);
        activeStar(3, true);
    }

    public void activeStar(int i, bool active)
    {
        GameObject temp;
        if (i == 1) temp = star1;
        else if (i == 2) temp = star2;
        else temp = star3;

        if (active) temp.GetComponent<Image>().sprite = star;
        else temp.GetComponent<Image>().sprite = noStar; 
    }

    internal void manageMissionFinished()
    {
        playingScene.SetActive(false);
        //startGameScreen.SetActive(true);
    }


    internal void showUpdatedNumSelectedObjects(int numSelectedObjects)
    {
        
    }

    internal void showUpdatedTime(float time)
    {
        if (time > 5) timerText.color = Color.white;
        else timerText.color = Color.red;

        if (time < 0) time = 0;

        string fmt = "00.00";
        timerText.text = (Math.Round(time, 2)).ToString(fmt);

        if (time < GameController.Instance.missionController.currentMission.maxTimeStar) star2.GetComponent<Image>().sprite = noStar;
    }

    internal void showMissionObjective(string obj, bool playing, float timeObj)
    {
        Text temp;
        if (playing)
        {
            temp = star1Text;
            star2Text.text = "Under " + Math.Round(timeObj, 1)+"s";
            star3Text.text = "No errors";
        }
        else
        {
            temp = preStar1Text;
            preStar2Text.text = "Under " + Math.Round(timeObj, 1) + "s";
            preStar3Text.text = "No errors";
        }

        temp.text = "" + obj;

    }



    

    internal void showObjectives(Mission2D m, bool playing=true)
    {
        Text temp;
        if (playing) temp = resultText;
        else temp = preStar1Text;

        temp.text = "";
        foreach (Objectives2D obj in m.objectives)
        {
            temp.text = temp.text + obj.id;
        }
    }

    internal void showInfo(string infoObj)
    {
        textInfoPanel.text = "Description: \n" + infoObj;
    }

    internal void activateInfoPanel()
    {
        rewardPanel.SetActive(false);
        tryAgainPanel.SetActive(false);
        playingScene.SetActive(false);
        startMissionPanel.SetActive(false);

        infoPanel.SetActive(true);
    }

    internal void activatePanelStartMission()
    {
        rewardPanel.SetActive(false);
        tryAgainPanel.SetActive(false);
        playingScene.SetActive(false);
        infoPanel.SetActive(false);

        startMissionPanel.SetActive(true);

    }

    internal void activateTryAgainPanel()
    {
        startMissionPanel.SetActive(false);
        rewardPanel.SetActive(false);
        playingScene.SetActive(false);
        infoPanel.SetActive(false);


        tryAgainPanel.SetActive(true);
    }

    internal void activateRewardPanel()
    {
        startMissionPanel.SetActive(false);
        tryAgainPanel.SetActive(false);
        playingScene.SetActive(false);
        infoPanel.SetActive(false);


        rewardPanel.SetActive(true);
    }

    internal void showSemaphore()
    {
        numCells = numCorrectFigures + numNoSelectedCorrectFigures + numErrors;

        //greenSemaphore.GetComponent<BoxCollider>().size.x
        float cellDim = greenSemaphore.GetComponent<BoxCollider>().size.x;
        //float offset = cellDim / 5.0f;
        float xPosition = ((30 - (numCells * cellDim)) / 2.0f) - 15 + (0.5f * cellDim);
        float yPosition = 2.0f;
        float zPosition = 0.0f;

        for (int i = 0; i < numCorrectFigures; i++)
        {

            printUniqueSemaphore(xPosition, yPosition, zPosition, greenSemaphore);
            xPosition += cellDim;
        }
        for (int i = 0; i < numNoSelectedCorrectFigures; i++)
        {

            printUniqueSemaphore(xPosition, yPosition, zPosition, yellowSemaphore);
            xPosition += cellDim;

        }
        for (int i = 0; i < numErrors; i++)
        {
            printUniqueSemaphore(xPosition, yPosition, zPosition, redSemaphore);
            xPosition += cellDim;
        }
        if (numCells <=4) traficLightsContainer.transform.localScale *= 1;
        else traficLightsContainer.transform.localScale *= 4.0f / numCells;
        traficLightsContainer.transform.position = new Vector3(5.5f, 0, -3f);
        deltaSemaphore = 1.0f / numCells;
        nextToShow = 0;
    }

    private void printUniqueSemaphore(float xPosition, float yPosition, float zPosition, GameObject texture)
    {
        GameObject s = (GameObject)Instantiate(texture, new Vector3(xPosition, yPosition, zPosition), Quaternion.Euler(90, 0, 0));
        s.transform.parent = traficLightsContainer;
        s.GetComponent<Renderer>().enabled = false;
        semaphore.Add(s);
    }

    internal void printNextSemaphore()
    {
        if (nextToShow < numCells)
        {
            semaphore[nextToShow].GetComponent<Renderer>().enabled = true;

            nextToShow += 1;
        }
    }

    internal void destroySemaphores()
    {
        foreach (GameObject go in semaphore)
        {
            Destroy(go);
        }

        traficLightsContainer.transform.position = new Vector3(0, 0, 0);
        traficLightsContainer.transform.localScale = new Vector3(1f, 1f, 1f);

        semaphore = new List<GameObject>();
    }


    internal void showCountdown(float countdown, bool finished)
    {
        if (!finished)
        {
            countdownText.text = "" + (int)countdown;
        }
        else
        {
            countdownText.text = "GO!";
        }
    }

    internal void showResult(float time, float seconds, Competencies comp, RewardType typeReward, int numReward, float objII, float currentII, int reward)
    {
        if (time < 0) time = 0;
        resultText.text = "";
        resultText2.text = "";

        resultText.text = resultText.text + "Time used: \n";
        resultText2.text = resultText2.text + Math.Round(time, 2) + "/" + seconds + "\n";

        if (comp != Competencies.AreaPerimeter)
        {
            resultText.text = resultText.text + "Competency: \n";
            resultText2.text = resultText2.text + comp.ToString() + "\n";
        }
        else
        {
            resultText.text = resultText.text + "Competency: \n";
            resultText2.text = resultText2.text + "Area & Perimeter\n";
        }
        resultText.text = resultText.text + typeReward.ToString() + "s won: \n" ;
        resultText2.text = resultText2.text + numReward + "\n";
        resultText.text = resultText.text + "You have: \n";
        resultText2.text = resultText2.text + currentII + "/"+ objII + "\n";
        reward2Text.text = "" + reward;
    }

    internal void showResult(float time, float seconds, int numErrors, int reward)
    {

        if (time < 0) time = 0;
        resultText.text = "";
        resultText.text = resultText.text + "Time used: " + Math.Round(time, 2) + "/" + seconds + "\n";
        resultText.text = resultText.text + "Errors commited: " + numErrors + "\n";
        resultText.text = resultText.text + "Reward: " + reward;

    }

    internal void setGetMoreButtonInteractable(bool notFinished)
    {
        getMoreButton.interactable = notFinished;
    }

    internal void showResultStars()
    {
        if (stars[0]) result_star1.GetComponent<Image>().sprite = star;
        else result_star1.GetComponent<Image>().sprite = noStar;

        if (stars[1]) result_star2.GetComponent<Image>().sprite = star;
        else result_star2.GetComponent<Image>().sprite = noStar;

        if (stars[2]) result_star3.GetComponent<Image>().sprite = star;
        else result_star3.GetComponent<Image>().sprite = noStar;
    }

    internal void setStarsBooleans()
    {
        stars[0] = (star1.GetComponent<Image>().sprite == star);
        stars[1] = (star2.GetComponent<Image>().sprite == star);
        stars[2] = (star3.GetComponent<Image>().sprite == star);
    }

    internal int goldReward(Level lev)
    {
        int multiplier = 0;
        for (int i = 0; i<3; i++)
        {
            if (stars[i]) multiplier += 1;
        }

        switch (lev)
        {
            case Level.VeryEasy:
                return (15 * multiplier);
            case Level.Easy:
                return (20 * multiplier);
            case Level.Medium:
                return (30 * multiplier);
            case Level.Difficult:
                return (40 * multiplier);
            case Level.VeryDifficult:
                return (50 * multiplier);
        }
        return (20 * multiplier);
    }

    internal int starsWon()
    {
        int multiplier = 0;
        for (int i = 0; i < 3; i++)
        {
            if (stars[i]) multiplier += 1;
        }

        return multiplier;
    }
}
