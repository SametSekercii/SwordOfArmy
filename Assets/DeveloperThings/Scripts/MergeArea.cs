using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MergeArea
{
    public int areaLevel;
    public bool isSolded;
    public int id;

    public MergeArea(int id)
    {
        areaLevel=1;
        isSolded = false;
        this.id=id;
    }

}
