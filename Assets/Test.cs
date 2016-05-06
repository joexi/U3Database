﻿using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        U3Database db = U3Database.DatabaseWithPath(Application.dataPath + "/test.db");
        db.Open();
        db.Update("UPDATE Main SET Value=3 WHERE ID=1");
        db.Update("Main", "Value", 5);
        db.Update("Main", "Value", 6, "ID=1");
        db.Update("Main", "Value", 9, "ID", 2);
        U3DBResultSet result = db.Select("SELECT * FROM MAIN");
        result.Show();
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }
}