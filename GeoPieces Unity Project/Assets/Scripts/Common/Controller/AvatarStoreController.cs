using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AvatarStoreController : MonoBehaviour {

    private int[] prices;
    public GameObject avatarPanel;
    
     
	// Use this for initialization
	void Start () {
        GameObject aux = GameObject.Find("DontDestroyOnLoad");
        noDestroy script = aux.GetComponent<noDestroy>();
        prices = new int[8];
        Vector3 pos = new Vector3(Screen.width/6f,Screen.height *.7f,0);
        float xoffset = Screen.width / 5.0f, yoffset = Screen.height / 3.0f;
	    for (int i=0; i<8; i++)
        {
            prices[i] = 60 + 20 * i;
            GameObject go = (GameObject) Instantiate(avatarPanel, pos, transform.rotation);
            go.transform.SetParent(gameObject.transform);
            go.transform.Translate(new Vector3((i % 4) * xoffset, -(i / 4) * yoffset, 0));
            avatarPreviewUI sc = go.GetComponent<avatarPreviewUI>();
            sc.setTexture("avatar" + i.ToString());
            sc.id = i;
            if (script.u.avatars.Contains(i))
            {
                if (script.u.currentAvatar == i) sc.setAsCurrent();
                else
                {
                    sc.setAsBought();
                }
            }
            else
            {
                sc.setAsNotBought(prices[i], script.u.gold >= prices[i]);

            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void buyAvatar(int i)
    {
        GameObject aux = GameObject.Find("DontDestroyOnLoad");
        noDestroy script = aux.GetComponent<noDestroy>();
        Debug.Log(i);
        script.u.gold -= prices[i];
        script.u.avatars.Add(i);

        SceneManager.LoadScene(8);
    }

    public void equipAvatar(int i)
    {
        GameObject aux = GameObject.Find("DontDestroyOnLoad");
        noDestroy script = aux.GetComponent<noDestroy>();

        script.u.currentAvatar = i;

        SceneManager.LoadScene(8);
    }

    public void clicked(int i)
    {
        GameObject aux = GameObject.Find("DontDestroyOnLoad");
        noDestroy script = aux.GetComponent<noDestroy>();

        if (script.u.avatars.Contains(i))
        {
            script.u.currentAvatar = i;
        }
        else
        {
            script.u.gold -= prices[i];
            script.u.avatars.Add(i);
        }

        SceneManager.LoadScene(8);
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
