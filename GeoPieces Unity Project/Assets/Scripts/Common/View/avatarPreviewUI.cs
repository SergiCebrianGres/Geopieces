using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class avatarPreviewUI : MonoBehaviour {

    public Image preview;
    public Image cover;
    public int id;
    public Button button;
    public Text textButton;
    public Image coin;
    public Image tic;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setTexture(string path)
    {
        preview.sprite = Resources.Load<Sprite>("avatars/" + path);
    }

    public void setAsBought()
    {
        cover.gameObject.SetActive(false);
        coin.gameObject.SetActive(false);
        tic.gameObject.SetActive(false);
        textButton.text = "EQUIP";
    }

    public void setAsNotBought(int gold, bool hasMoney)
    {
        if (hasMoney) button.interactable = true;
        else button.interactable = false;
        tic.gameObject.SetActive(false);
        cover.gameObject.SetActive(true);
        coin.gameObject.SetActive(true);
        textButton.text = gold.ToString();
    }

    public void setAsCurrent()
    {
        tic.gameObject.SetActive(true);
        cover.gameObject.SetActive(false);
        coin.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
    }

    public void click()
    {
        transform.parent.GetComponent<AvatarStoreController>().clicked(id);
    }
}
