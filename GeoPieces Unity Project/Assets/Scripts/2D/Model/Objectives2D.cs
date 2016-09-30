using System;

[Serializable]
public class Objectives2D {
    public ObjectiveID id;
    public string description;

    public Objectives2D(ObjectiveID i, string desc)
    {
        id = i;
        description = desc;
    }
}
