using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class campaignMenuScene : MonoBehaviour {
    public CampaingManager cm;
    public Transform posIni;
    [SerializeField] private GameObject town;
    [SerializeField] private GameObject tutorialPanel;
    public float cameraSpeed = 1;
    public Material completedMat;

    // Use this for initialization
    void Start () {
        cm.loadCampaings();
        GameObject noDestroy = GameObject.Find("DontDestroyOnLoad");
        noDestroy script = noDestroy.GetComponent<noDestroy>();

        if (!script.u.tutorialSeen)
        {
            script.u.tutorialSeen = true;
            script.u.save();
            tutorialPanel.SetActive(true);
        }
        else tutorialPanel.SetActive(false);

        int i = 0;
        foreach (var c in cm.camps)
        {
            foreach (InventoryItem ii in c.inventory.items) ii.loadTexture();
            c.setPath("Textures/" + c.name);
            c.setGoPreview();
            GameObject go = (GameObject)Instantiate(c.getGoPreview(), posIni.position + 5 * i * Vector3.right + Random.Range(-1f,1f) * Vector3.forward, posIni.rotation);
            PreviewCampaignModel sc = go.AddComponent<PreviewCampaignModel>();
            sc.cId = c.id;
            if (script.u.inventories.ContainsKey(c.id))
            {
                if (script.u.inventories[c.id].completed)
                {
                    sc.completed = true;
                    sc.paint(completedMat);
                    town.GetComponent<TownMgr>().activateBuilding(c.id);
                }
            }
            else
            {
                sc.completed = false;
            }
            i++;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!tutorialPanel.activeSelf)
        {
            if (Input.GetMouseButton(0))
            {
                float dir = Input.GetAxis("Mouse X");
                if (Mathf.Abs(Camera.main.transform.position.x - dir * cameraSpeed) <= 20)
                    Camera.main.transform.Translate(-dir * cameraSpeed * Vector3.right);
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    GameObject selectedGO = hitInfo.transform.gameObject;
                    PreviewCampaignModel sc = selectedGO.GetComponent<PreviewCampaignModel>();
                    if (sc != null && !sc.completed) callCampaign(sc.cId);
                }
            }
        }
    }

    public void callCampaign(int i)
    {
        GameObject noDestroy = GameObject.Find("DontDestroyOnLoad");
        noDestroy script = noDestroy.GetComponent<noDestroy>();
        Campaign c = cm.camps[i];

        if (!script.u.inventories.ContainsKey(c.id)) script.u.inventories[c.id] = c.inventory;

        script.c = c;
        SceneManager.LoadScene(3);
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void endTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}
