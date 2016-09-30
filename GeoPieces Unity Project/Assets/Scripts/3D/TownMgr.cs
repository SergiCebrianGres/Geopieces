using UnityEngine;
using System.Collections;

public class TownMgr : MonoBehaviour {
    public Material yellow;
    public Material gray;
    [SerializeField]
    private GameObject arc;
    [SerializeField]
    private GameObject houses;
    [SerializeField]
    private GameObject fountain;

    // Use this for initialization
    void Start () {
        arc.GetComponent<TownPart>().paint(gray);
        houses.GetComponent<TownPart>().paint(gray);
        fountain.GetComponent<TownPart>().paint(gray);
        fountain.GetComponent<TownPart>().ps.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void activateBuilding(int i)
    {
        switch(i)
        {
            case (0):
                arc.GetComponent<TownPart>().paint(yellow);
                break;
            case (1):
                houses.GetComponent<TownPart>().paint(yellow);
                break;
            case (2):
                fountain.GetComponent<TownPart>().paint(yellow);
                fountain.GetComponent<TownPart>().ps.SetActive(true);
                break;
        }
    }
}
