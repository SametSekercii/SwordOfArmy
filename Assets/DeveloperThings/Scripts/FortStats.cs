using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class FortStats 
{
    public int level;
    public int id;

    public FortStats(int id) 
    {
        level = 1;
        this.id = id;
    }
    
}
