using UnityEngine;

public class PreviewDesdoblegament : MonoBehaviour {
    public Aresta aresta;
    public GameObject finalObject;
    public int costats;
    public float apotema;
    public bool blocked;

    void Start()
    {
        apotema = 1 / (2 * Mathf.Tan(Mathf.PI / costats));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "UnfoldPiece")
        {
            //Figura2Desdoplegament script = other.GetComponentInParent<Figura2Desdoplegament>();
            //if ((transform.position - other.transform.position).magnitude < 0.95 * (script.apotema + apotema)) Destroy(gameObject);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "PreviewPiece")
        {
            if ((transform.position - other.transform.position).magnitude < 0.01) Destroy(gameObject);
        }
    }
}
