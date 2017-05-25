using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SQLite4Unity3d;

public class PlayerData
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public int gold { get; set; }
    public int life { get; set; }
    public int level { get; set; }

    public PlayerData()
    {}
}
