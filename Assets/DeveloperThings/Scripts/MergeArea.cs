using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CreateAssetMenu(fileName = "New MergeArea", menuName = "MergeArea/Create New MergeArea")]
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
