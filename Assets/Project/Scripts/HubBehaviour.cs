using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class HubBehaviour : MonoBehaviour
{
    private GameObject bandManager;
    private GameObject player;
    private PlayerInfo lastPlayerInfo = new PlayerInfo();
    private string id = Guid.NewGuid().ToString();

    public string Id { get { return id; } }

    void Start()
    {
    }

    void Awake()
    {
        //InvokeRepeating("ReportPosition", 1, 1.0F);
        bandManager = GameObject.Find("BandManager");
        player = GameObject.Find("Player");
    }

    float lastTime;

    void Update()
    {
        if(Time.time - lastTime >= 1)
        {
            lastTime = Time.time;
            ReportPosition();
        }
    }

    void ReportPosition()
    {
            Vector3 position = player.transform.position;
            Vector3 forward = player.transform.forward;

            PlayerInfo playerInfo = new PlayerInfo
            {
                Id = id,
                PositionX = position.x,
                PositionY = position.y,
                PositionZ = position.z,
                ForwardX = forward.x,
                ForwardY = forward.y,
                ForwardZ = forward.z,
            };

       if (!playerInfo.Equals(lastPlayerInfo) && (bandManager != null))
        {
            bandManager.SendMessage("ReportPosition", playerInfo);
            lastPlayerInfo = playerInfo;
        }
    }



}
