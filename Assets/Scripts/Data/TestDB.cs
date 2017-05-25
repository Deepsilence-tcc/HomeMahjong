using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDB : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DataBaseService service = new DataBaseService("DB.db");
        PlayerData player = service.GetByID();
        //player.ToString();

        Debug.Log(player.level);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
