using UnityEngine;
using System.Collections.Generic;
using System;

public class Figura2Desdoplegament : MonoBehaviour
{
    private Material matOri;
    [SerializeField] private Material errorMat;
    private Vector3 posOri;
    private Quaternion rotOri;
    //public Figura2D figura;
    public int costats;
    public float apotema;
    public List<Aresta> arestesOri;
    public List<Aresta> arestes;
    [SerializeField] private int foldingSpeed;
    private Vector3 rotationAxis;
    private Aresta rotationEdge;
    private float rotatedAngle;
    private float rotationGoalAngle;
    private bool error = false;
    private bool rotating = false;
    private bool toStartRecursive = false;
    public bool toDelete;
    private Objective foldingObjective;

    void Awake()
    {
        arestes = new List<Aresta>();
        arestesOri = new List<Aresta>();
        matOri = GetComponent<Renderer>().material;
    }

    void Start ()
    {
        posOri = transform.position;
        rotOri = transform.rotation;
        apotema = 1 / (2 * Mathf.Tan(Mathf.PI / costats));
    }

    void Update()
    {   
        if (rotating)
        {
            float newAngle = Time.deltaTime * foldingSpeed;
            
            if (rotatedAngle + newAngle > rotationGoalAngle)
            {
                newAngle = rotationGoalAngle - rotatedAngle;
                rotating = false;
            } 

            transform.RotateAround(rotationEdge.de.transform.position + rotationEdge.vec * rotationEdge.de.apotema, rotationAxis, newAngle);
            rotatedAngle += newAngle;

            foreach (var aresta in arestes)
            {
                aresta.vec = Quaternion.AngleAxis(newAngle, rotationAxis) * aresta.vec;
            }
            
            if (!rotating && !error)
            {
                if (toStartRecursive) foldRecursive(foldingObjective);
                DesdoplegamentSceneController.Instance.rotated++;
            }

        }
	}

    public void readyToDelete()
    {
        toDelete = true;
        GetComponentInParent<Renderer>().material = errorMat;
        foreach (var aresta in arestes)
        {
            if (aresta.a != null)
            {
                aresta.a.readyToDelete();
            }
        }
    }

    public void foldRecursive(Objective obj)
    {
        toStartRecursive = false;
        foreach (var aresta in arestes)
        {
            if (aresta.a != null)
            {
                //Exemple directe de prisma triangular. hauria de llegir-se des de les Folding Rules dins de Objective.
                if ((costats == 4 && aresta.a.costats == 5) || (costats == 5 && aresta.a.costats == 4)) aresta.a.foldSubTree(aresta, obj.foldingRules["S-P"]);
                if ((costats == 3 && aresta.a.costats == 5) || (costats == 5 && aresta.a.costats == 3)) aresta.a.foldSubTree(aresta, obj.foldingRules["T-P"]);
                if ((costats == 3 && aresta.a.costats == 4) || (costats == 4 && aresta.a.costats == 3)) aresta.a.foldSubTree(aresta, obj.foldingRules["S-T"]);
                if (costats == 3 && aresta.a.costats == 3) aresta.a.foldSubTree(aresta, obj.foldingRules["T-T"]);
                if (costats == 4 && aresta.a.costats == 4) aresta.a.foldSubTree(aresta, obj.foldingRules["S-S"]);
                if (costats == 5 && aresta.a.costats == 5) aresta.a.foldSubTree(aresta, obj.foldingRules["P-P"]);
                aresta.a.foldingObjective = obj;
                aresta.a.toStartRecursive = true;
            }
        }
    }

    public void foldSubTree(Aresta arestaOri, float angle)
    {
        rotationAxis = Quaternion.AngleAxis(-90, transform.up) * arestaOri.vec;
        rotationEdge = arestaOri;
        rotating = true;
        rotatedAngle = 0f;
        rotationGoalAngle = angle;
        foreach (var aresta in arestes)
        {
            if (aresta.a != null)
            {
                aresta.a.foldSubTree(arestaOri, angle);
            }
        }
    }

    public void stopRotation()
    {
        rotating = false;       
        foreach (var aresta in arestes)
        {
            if (aresta.a != null)
            {
                aresta.a.stopRotation();
            }
        }
    }

    public void resetPosition()
    {
        for (int i=0; i<arestes.Count; i++)
        {
            arestes[i].vec = arestesOri[i].vec;
        }
        transform.position = posOri;
        transform.rotation = rotOri;
        foreach (var aresta in arestes)
        {
            if (aresta.a != null)
            {
                aresta.a.resetPosition();
            }
        }
    }

    public void paintYellow()
    {
        toDelete = false;
        GetComponentInParent<Renderer>().material = matOri;
        foreach (var aresta in arestes)
        {
            if (aresta.a != null)
            {
                aresta.a.paintYellow();
            }
        }
    }
}
