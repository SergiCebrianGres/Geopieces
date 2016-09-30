using UnityEngine;
using System.Collections;

public class TownPart : MonoBehaviour {
    public GameObject ps;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void paint(Material mat)
    {
        //gameObject
        Renderer r = transform.GetComponent<Renderer>();
        if (r != null) r.material = mat;
        for (int i = 0; i < transform.childCount; i++)
        {
            r = transform.GetChild(i).GetComponent<Renderer>();
            if (r != null && transform.GetChild(i).gameObject != ps) r.material = mat;
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                r = transform.GetChild(i).GetChild(j).GetComponent<Renderer>();
                if (r != null && transform.GetChild(i).gameObject != ps) r.material = mat;
            }
        }
    }
}
