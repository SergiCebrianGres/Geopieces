using System;
using System.Collections.Generic;

[Serializable]
public class Objective {
    public string figureName;
    public int squares;
    public int triangles;
    public int pentagons;
    public List<String> foldingString;
    public List<float> foldingFloat;
    public Dictionary<string, float> foldingRules;

    public Objective(string fn, int s, int t, int p) {
        figureName = fn;
        squares = s;
        triangles = t;
        pentagons = p;
        foldingRules = new Dictionary<string, float>();
        foldingString = new List<string>();
        foldingFloat = new List<float>();
    }
}
