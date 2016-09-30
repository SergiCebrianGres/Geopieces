using UnityEngine;
using System.Collections;

public class rotation : MonoBehaviour {
    public float speed = 10;
    int rotationType;

	// Use this for initialization
	void Start () {
        rotationType = Random.Range(0, 4);
	}
	
	// Update is called once per frame
	void Update () {
        switch (rotationType)
        {
            case 0:
                transform.Rotate(0.0f, 0.0f, 2 * speed * Time.deltaTime);
                break;

            case 1:
                transform.Rotate(0.0f, 0.0f, 4 * speed * Time.deltaTime);
                break;

            case 2:
                transform.Rotate(0.0f, 0.0f, - 2 * speed * Time.deltaTime);
                break;

            case 3:
                transform.Rotate(0.0f, 0.0f, -speed * Time.deltaTime);
                break;
        }
	}
}
