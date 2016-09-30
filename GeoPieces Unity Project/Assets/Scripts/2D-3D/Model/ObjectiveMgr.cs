using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectiveMgr {
    public int len;
    public List<Objective> objs;

	public void saveObjectives()
    {
        foreach (Objective o in objs)
        {
            o.foldingFloat = new List<float>();
            o.foldingString = new List<string>();
            foreach (KeyValuePair<string, float> p in o.foldingRules)
            {
                o.foldingString.Add(p.Key);
                o.foldingFloat.Add(p.Value);
            }
        }
        len = objs.Count;
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Application.dataPath + "/Docs/UnfoldingObjectives/objs.json", json);
        //AssetDatabase.Refresh();
    }
}
