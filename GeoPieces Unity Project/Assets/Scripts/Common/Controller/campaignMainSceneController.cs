using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class campaignMainSceneController : MonoBehaviour {
    private Inventory inv;
    private Campaign cam;
    public Texture check;
    public Texture notCheck;
    public Font font;
    private User u;
    private noDestroy script;
    GUIStyle styleBuy;
    public GUISkin buyok_style;
    public GUISkin buynotok_style;
    public GUISkin myStyle2;
    public float yposTitle = -0.01f;
    private bool finishCampaignActivate = false;
    private float countdownTime;
    public Image campaignImage;
    public float alpha = 1;

    // Use this for initialization
    void Start () {
        GameObject go = GameObject.Find("DontDestroyOnLoad");
        script = go.GetComponent<noDestroy>();
        cam = script.c;
        u = script.u;
        inv = script.u.inventories[cam.id];

        showCampaignImage();
        
    }

    private void showCampaignImage()
    {
        
        Sprite temp = Resources.Load<Sprite>("Textures/" + cam.name);

        if (temp != null) campaignImage.GetComponent<Image>().sprite = temp;
        else campaignImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Default");
    }

    void OnGUI()
    {
        showProgress();
        showTitle();
    }

    private void showTitle()
    {
        float sw = Screen.width;
        float sh = Screen.height;
        float width = sw / 3;
        float height = width / 3;

        Rect r = new Rect((sw - width) / 2, sh * yposTitle, width, height);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = (int)(height * 2 / 3);
        style.font = font;

        GUI.Label(r, cam.name, style);
    }

    private void showProgress()
    {

        float sw = Screen.width;
        float sh = Screen.height;
        float xPosition = 0.25f;
        int numCells = inv.items.Count;

        float height = sh / (2.5f * numCells + 1);
        float widthImage, widthQuantity, widthCheckbox, widthButton, widthBuy;

        widthImage = height;
        widthQuantity = height;
        widthCheckbox = height;
        widthButton = 6 * height;
        widthBuy = 2 * height;

        float offset = 0.5f * height;

        float totalWidth = widthImage + widthCheckbox + widthQuantity + widthButton + 2 * height + height * 0.5f + offset * 5;
        xPosition = .5f * (1 - (totalWidth / sw));
        float rectXPosition = xPosition * sw;

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = (int)(height * 2 / 3);
        style.font = font;

        Texture texture;

        float ycoord = 0;

        bool allChecked = true;

        for (int i = 0; i < numCells; i++)
        {
            GUI.skin = myStyle2;
            rectXPosition = xPosition * sw;
            InventoryItem ii = inv.items[i];
            texture = ii.getDisplayTexture();
            int quantity = ii.quantity;
            int goal = ii.campaignGoal;
            float totalCellLength = sh - (numCells * height);
            ycoord = totalCellLength - 0.5f * (totalCellLength) + (height * i);

            Rect rectImage, rectQuantity, rectCheckbox, rectButton, rectBuy, rectCoin, rectPrice;
            rectImage = new Rect(rectXPosition, ycoord, widthImage, height);
            rectXPosition += (offset + widthImage);
            rectQuantity = new Rect(rectXPosition, ycoord, widthQuantity, height);
            rectXPosition += (offset + widthQuantity);
            rectCheckbox = new Rect(rectXPosition, ycoord, widthCheckbox, height);
            rectXPosition += (offset + widthCheckbox);
            rectButton = new Rect(rectXPosition, ycoord, widthButton, height);
            rectXPosition += (offset + widthButton);
            rectBuy = new Rect(rectXPosition, ycoord, height, height);
            rectXPosition += (offset + height);
            rectCoin = new Rect(rectXPosition, ycoord + 0.5f * offset, height * 0.5f, height * 0.5f);
            rectXPosition += (height * 0.5f);
            rectPrice = new Rect(rectXPosition, ycoord, height, height);
            //rectBuy = new Rect(rectXPosition, ycoord, widthBuy, height);

            GUI.DrawTexture(rectImage, texture);

            string quantityStr = quantity.ToString();
            string goalStr = goal.ToString();
            GUI.Label(rectQuantity, quantityStr + "/" + goalStr, style);

            if (ii.quantity == ii.campaignGoal) GUI.DrawTexture(rectCheckbox, check);
            else
            {
                GUI.DrawTexture(rectCheckbox, notCheck);
                allChecked = false;
            }

            string buttonString = "Get " + ii.name.ToString() + "!";
            if (inv.hasParts(ii) && ii.quantity < ii.campaignGoal)
            {
                if (GUI.Button(rectButton, buttonString))
                {
                    if (ii.type == InventoryItemType.type2D)
                    {
                        script.to2D = true;
                        script.reward = ii.name;
                        SceneManager.LoadScene(4);
                    }
                    if (ii.type == InventoryItemType.type3D)
                    {
                        script.to3D = true;
                        script.reward = ii.name;
                        SceneManager.LoadScene(5);
                    }
                }
            }

            int price = 1000 / ii.initialGoal;// + ii.quantity * 10;
            string textBuy = price.ToString();

            if (canBuy(ii, price)) GUI.skin = buyok_style;
            else GUI.skin = buynotok_style;


            if (inv.hasParts(ii) && ii.quantity < ii.campaignGoal)
            {
                if (GUI.Button(rectBuy, ""))
                {
                    if (canBuy(ii, price))
                    {
                        inv.Add(ii.name, 1);
                        if (ii.type == InventoryItemType.type3D)
                        {
                            inv.Spend(RewardType.Triangle, ii.cost[0]);
                            inv.Spend(RewardType.Square, ii.cost[1]);
                            inv.Spend(RewardType.Pentagon, ii.cost[2]);
                        }
                        u.gold -= price;
                        u.save();
                    }
                    else
                    {
                        //toast
                    }
                }

                string goldText = "Textures/gold";
                Texture2D goldTexture = Resources.Load<Texture2D>(goldText);
                GUI.DrawTexture(rectCoin, goldTexture);

                style.alignment = TextAnchor.MiddleLeft;
                GUI.Label(rectPrice, textBuy, style);
                style.alignment = TextAnchor.MiddleCenter;
            }
        }

        GUI.skin = myStyle2;
        myStyle2.button.fontSize = (int)(Screen.width * alpha);
        float finalButtonWidth = (8.0f * height);
        xPosition = .5f * (1 - (finalButtonWidth / sw));
        ycoord = .5f * (1 - (2.0f * height / sh));
        Rect finishedButton = new Rect(xPosition * sw, ycoord * sh, finalButtonWidth, 2 * height);

        if (allChecked && !inv.completed)
        {
            if (GUI.Button(finishedButton, "Campaign Completed!"))
            {
                inv.completed = true;
                countdownTime = 3.0f;
                finishCampaignActivate = true;
            }
        }
    }

    public void backToSelection()
    {
        SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    void Update () {

        if (finishCampaignActivate)
        {

            countdownTime -= Time.deltaTime;

            if (countdownTime <= 0)
            {
                finishCampaignActivate = false;
                backToSelection();
            }
        }

    }

    private bool canBuy(InventoryItem ii, int price)
    {
        return (inv.hasParts(ii) && u.gold >= price);
    }

    public void infoCampaign()
    {

        SceneManager.LoadScene(7);
    }
}
