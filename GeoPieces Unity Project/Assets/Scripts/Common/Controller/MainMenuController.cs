using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public Button login;
    public Button signup;

    public InputField userInput;
    public InputField passInput;

    public Text errorText;

    public User user;

    public noDestroy nD;

    // Use this for initialization
    void Start () {
        login.interactable = false;
        signup.interactable = false;
        user = new User();
    }
	
    public void userEdit ()
    {
        if (userInput.text.Length >= 3 && passInput.text.Length >= 8)
        {
            login.interactable = true;
            signup.interactable = true;
        }
        else
        {
            login.interactable = false;
            signup.interactable = false;
        }
    }

    public void passEdit()
    {
        if (userInput.text.Length >= 3 && passInput.text.Length >= 8)
        {
            login.interactable = true;
            signup.interactable = true;
        }
        else
        {
            login.interactable = false;
            signup.interactable = false;
        }
    }

    public void loginButton()
    {
        try
        {
            StreamReader file = new StreamReader(Application.dataPath + "/" + userInput.text + ".txt");
            string json = file.ReadToEnd();
            user = JsonUtility.FromJson<User>(json);
            user.inventories = new Dictionary<int, Inventory>();
            foreach (var i in user.invs)
            {
                user.inventories[i.campaignID] = i;
            }

            if (!user.user.Equals(""))
            {
                //noDestroy.u = user;
                nD.setUser(user);
                SceneManager.LoadScene(1);
            }
        }
        catch
        {
            errorText.text = "User name incorrect. Try again!";
        }
    }

    public void signupButton()
    {
        User newUser = new User(userInput.text, 1);
        newUser.save();
    }
}
