using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class noDestroy : MonoBehaviour {
    [SerializeField]
    public User u;
    //public List<Campaign> campaigns;
    public Campaign c;
    public RewardType reward;
    public bool to3D, to2D;
    public Texture t;

    public Font textFont;
    public List<string> scenesNoAvatar;


    // Use this for initialization
    void Start () {
        to2D = false;
        to3D = false;
        u = new User();
        //campaigns = new List<Campaign>();
        DontDestroyOnLoad(this);

        loadScenesToShowAvatars();
    }

    private void loadScenesToShowAvatars()
    {
        scenesNoAvatar = new List<string>();
        scenesNoAvatar.Add("2D Game");
        scenesNoAvatar.Add("Log in");
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void setUser(User user)
    {
        u = user;
        //t = Resources.Load<Texture>(u.texturePath);
    }

    public void showUserIcon()
    {        
        float sw = Screen.width;
        float sh = Screen.height;

        float h = sh / 10;
        t = Resources.Load<Texture>("avatars/avatar"+ u.currentAvatar);
        Rect icon = new Rect(sw - h,0, h, h);
        GUI.DrawTexture(icon, t);

        Rect goldRect = new Rect(sw - 2.7f * h, h/6.0f, 2*h/3.0f, 2*h/3.0f);
        string texture = "Textures/gold";
        Texture goldTexture = Resources.Load<Texture>(texture);
        GUI.DrawTexture(goldRect, goldTexture);

        Rect money = new Rect(sw - 2*h, 0, h, h);
        GUIStyle style = new GUIStyle();
        style.fontSize = (int) (h * .7f);
        style.alignment = TextAnchor.MiddleLeft;
        style.font = textFont;
        style.normal.textColor = Color.white;

        GUI.Label(money, u.gold.ToString(), style);

    }

    void OnGUI()
    {
        if (u.user.Length > 0)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (!scenesNoAvatar.Contains(scene.name))
            {
                showUserIcon();
            }
        }
    }
}
