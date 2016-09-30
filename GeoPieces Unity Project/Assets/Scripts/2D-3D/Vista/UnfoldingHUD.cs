using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UnfoldingHUD : MonoBehaviour {
    [SerializeField] private Sprite Star;
    [SerializeField] private Sprite NoStar;
    
    [SerializeField] private Image imgTime;
    [SerializeField] private Image imgErrors;
    [SerializeField] private Text labelTime;
    [SerializeField] private Text textTimer;

    [SerializeField] private GameObject panelTutorial;

    [SerializeField] private Image imgPanelComplete;
    [SerializeField] private Image imgPanelTime;
    [SerializeField] private Image imgPanelTries;

    [SerializeField] private GameObject ResultPanel;
    [SerializeField] private Text figureName;
    [SerializeField] private Text resultTitle;

    [SerializeField] private Button resultButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button getMoreButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button sButton;
    [SerializeField] private Button tButton;
    [SerializeField] private Button pButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button resetButton;

    private bool tryStar;
    private bool timeStar;

    // Use this for initialization
    void Start () {
        
        int h = Screen.height;
        int w = Screen.width;
        RectTransform r = ResultPanel.GetComponent<RectTransform>();
        r.rect.Set(0, 0, w, h);
        ResultPanel.SetActive(false);
        sButton.transform.position = new Vector3(sButton.transform.position.x, h / 10,sButton.transform.position.z);
        tButton.transform.position = new Vector3(tButton.transform.position.x, h / 10, tButton.transform.position.z);
        pButton.transform.position = new Vector3(pButton.transform.position.x, h / 10, pButton.transform.position.z);
        readyButton.transform.position = new Vector3(w * 9 / 10, h / 10, readyButton.transform.position.z);
        resetButton.transform.position = new Vector3(w / 10, h / 10, resetButton.transform.position.z);
        backButton.transform.position = new Vector3(w / 10, h * 90 / 100, backButton.transform.position.z);
        figureName.transform.position = new Vector3(figureName.transform.position.x, h *  90 / 100, figureName.transform.position.z);
        tryStar = true;
        timeStar = true;
        showTimer(true);
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void showTutorial(bool v)
    {
        panelTutorial.SetActive(v);
    }

    public void updateTimer(float t, float maxTime)
    {
        if (t + 3 < maxTime) textTimer.color = Color.white;
        else textTimer.color = Color.red;

        if (t > maxTime)
        {
            imgTime.GetComponent<Image>().sprite = NoStar;
            timeStar = false;
            showTimer(false);
        }

        string fmt = "00.00";
        textTimer.text = (Math.Round(t, 2)).ToString(fmt);
    }

    public void showTimer(bool b)
    {
        textTimer.gameObject.SetActive(b);
    }

    public void ini(float t)
    {
        int h = Screen.height;
        int w = Screen.width;
        RectTransform r = ResultPanel.GetComponent<RectTransform>();
        r.rect.Set(0, 0, w, h);
        ResultPanel.SetActive(false);
        sButton.transform.position = new Vector3(sButton.transform.position.x, h / 10, sButton.transform.position.z);
        tButton.transform.position = new Vector3(tButton.transform.position.x, h / 10, tButton.transform.position.z);
        pButton.transform.position = new Vector3(pButton.transform.position.x, h / 10, pButton.transform.position.z);
        readyButton.transform.position = new Vector3(w * 9 / 10, h / 10, readyButton.transform.position.z);
        resetButton.transform.position = new Vector3(w / 10, h / 10, resetButton.transform.position.z);
        backButton.transform.position = new Vector3(w / 10, h * 90 / 100, backButton.transform.position.z);
        figureName.transform.position = new Vector3(figureName.transform.position.x, h * 90 / 100, figureName.transform.position.z);
        imgTime.GetComponent<Image>().sprite = Star;
        imgErrors.GetComponent<Image>().sprite = Star;
        labelTime.text = "Under " + t.ToString() + "s";
        tryStar = true;
        timeStar = true;
    }

    public void loadObjective(Objective obj)
    {
        //timer
        ResultPanel.SetActive(false);
        figureName.text = obj.figureName;
        UpdateButtons(obj);
        resetButton.interactable = true;
    }

    public void UpdateButtons(Objective obj)
    {
        sButton.GetComponentsInChildren<Text>()[0].text = "Square: " + obj.squares.ToString();
        tButton.GetComponentsInChildren<Text>()[0].text = "Triangle: " + obj.triangles.ToString();
        pButton.GetComponentsInChildren<Text>()[0].text = "Pentagon: " + obj.pentagons.ToString();

        if (obj.squares < 1) sButton.interactable = false;
        else sButton.interactable = true;
        if (obj.triangles < 1) tButton.interactable = false;
        else tButton.interactable = true;
        if (obj.pentagons < 1) pButton.interactable = false;
        else pButton.interactable = true;

        if (obj.squares + obj.triangles + obj.pentagons < 1) readyButton.interactable = true;
        else readyButton.interactable = false;
    }

    public void setResult(int v, bool hasMore)
    {
        Camera.main.transform.position += Camera.main.transform.right * 2.5f;
        if (v == 1)
        {
            imgPanelComplete.GetComponent<Image>().sprite = Star;
            if (tryStar) imgPanelTries.GetComponent<Image>().sprite = Star;
            else imgPanelTries.GetComponent<Image>().sprite = NoStar;
            if (timeStar) imgPanelTime.GetComponent<Image>().sprite = Star;
            else imgPanelTime.GetComponent<Image>().sprite = NoStar;
            imgPanelComplete.gameObject.SetActive(true);
            imgPanelTime.gameObject.SetActive(true);
            imgPanelTries.gameObject.SetActive(true);
            retryButton.gameObject.SetActive(false);
            resultButton.gameObject.SetActive(true);
            resultTitle.text = "SUCCESS!";
            resultTitle.color = Color.green;
            resetButton.interactable = false;
            getMoreButton.gameObject.SetActive(true);
            if (hasMore) getMoreButton.interactable = true;  
            else getMoreButton.interactable = false;
        }
        else
        {
            imgPanelComplete.gameObject.SetActive(false);
            imgPanelTime.gameObject.SetActive(false);
            imgPanelTries.gameObject.SetActive(false);
            resultTitle.text = "ERROR!";
            resultTitle.color = Color.red;
            retryButton.gameObject.SetActive(true);
            retryButton.interactable = true;
            getMoreButton.gameObject.SetActive(true);
        }
        ResultPanel.SetActive(true);
    }

    public void deactivatePanel()
    {
        ResultPanel.SetActive(false);
    }

    public void disableReadyButton()
    {
        readyButton.interactable = false;
    }

    public void disableResetButton()
    {
        resetButton.interactable = false;
    }

    internal void loseTriesStar()
    {
        imgErrors.GetComponent<Image>().sprite = NoStar;
        tryStar = false;
    }
}
