using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void selectCampaign()
    {
        SceneManager.LoadScene(2);
    }

    public void seeStatistics()
    {
        SceneManager.LoadScene(6);
    }

    public void seeAvatarStore()
    {
        SceneManager.LoadScene(8);
    }

    public void exitApp()
    {
        Application.Quit();
    }
}
