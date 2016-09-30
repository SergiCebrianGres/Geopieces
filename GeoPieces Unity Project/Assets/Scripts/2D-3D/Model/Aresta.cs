using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Aresta {
    public Figura2Desdoplegament de;
    public Figura2Desdoplegament a;
    public Vector3 vec;
    public bool blocked;

    public Aresta(Figura2Desdoplegament from, Figura2Desdoplegament to, Vector3 vector)
    {
        de = from;
        a = to;
        vec = vector.normalized;
        blocked = false;
    }

    public Aresta (Figura2Desdoplegament from, Figura2Desdoplegament to, Vector3 vector, bool b)
    {
        de = from;
        a = to;
        vec = vector.normalized;
        blocked = b;
    }
}
