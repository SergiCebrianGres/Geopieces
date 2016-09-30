using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Campaign {

    public int id;
    public string name;
    public Inventory inventory;
    public float[] weights;
    public bool weightsMatter;
    public List<Competencies> compToLearn;
    private string imgPath;
    private GameObject goPreview;

    public Campaign (int i, string n, Inventory inv, string path)
    {
        id = i;
        name = n;
        inventory = inv;
        imgPath = path;
        weights = new float[4];
        weightsMatter = false;
        compToLearn = new List<Competencies>();
        compToLearn.Add(Competencies.Identification);
        compToLearn.Add(Competencies.Symmetry);
        compToLearn.Add(Competencies.Angles);
        compToLearn.Add(Competencies.AreaPerimeter);
    }

    internal void setPath(string v)
    {
        imgPath = v;
    }

    internal GameObject getGoPreview()
    {
        return goPreview;
    }

    internal void setGoPreview()
    {
        goPreview = Resources.Load<GameObject>("Figures3D/" + name);
        if (goPreview == null) goPreview = Resources.Load<GameObject>("Figures3D/Default");
        
    }
}
