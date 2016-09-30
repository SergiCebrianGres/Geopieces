using UnityEngine;
using System.Collections;

public class CollisionErrorDetector : MonoBehaviour {
    [SerializeField] private Material errorMaterial;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "UnfoldPiece")
        {
            GetComponentInParent<Renderer>().material = errorMaterial;
            DesdoplegamentSceneController.Instance.errorRotating();
        }
    }
}
