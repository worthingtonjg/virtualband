using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class XAMLConnection : MonoBehaviour {

    public delegate void OnEvent(object arg);

    public OnEvent onEvent = null;
    public Text Distance;
    public Text Speed;
    public Text HeartRate;
    public Text TimeElapsed;
    public Text CaloriesBurned;

    public GameObject Player;
    public List<GameObject> OtherPlayers;
    public GameObject OtherPlayerPrefab;

    private long? startDistance;
    private long? startCalories;
    private double speed;
    private double distance;
    private int heartRate;
    private bool isMoving;
    private float startTime;
    private float lastReported;

    void Start()
    {
        startTime = Time.time;
        lastReported = Time.time;
    }

    void Update()
    {
        if(Time.time - lastReported >= 1)
        {
            lastReported = Time.time;
            if (TimeElapsed != null)
            {
                var elapsedTime = (Time.time - startTime);
                

                TimeElapsed.text = String.Format("{0:0}:{1:00}", Mathf.Floor(elapsedTime / 60), elapsedTime % 60);
            }
        }
    }

    public void StartBand()
    {
        if(onEvent != null)
        {
            onEvent.Invoke("Start");
        }
    }

    public void StopBand()
    {
        if(onEvent != null)
        {
            onEvent.Invoke("Stop");
        }
    }

    public void ReportPosition(PlayerInfo info)
    {
        if(onEvent != null)
        {
            onEvent.Invoke(info);
        }
    }

    public void ProcessMessage(string message, object obj)
    {
        //if (message == "FINDBANDS_SUCCESS")
        //{

        //}
        //else if(message == "FINDBANDS_ERROR1")
        //{

        //}
        //else if (message == "FINDBANDS_ERROR2")
        //{

        //}

        if (message == "DISTANCE_CHANGED")
            OnDistanceChanged((SensorData)obj);

        if (message == "MOVING")
            OnMovingChanged((bool)obj);

        if (message == "PLAYER_UPDATED")
            OnPlayerUpdated((PlayerInfo)obj);

        if (message == "HEARTRATE_CHANGED")
            OnHeartRateChanged((int)obj);

        if (message == "CALORIES_CHANGED")
            OnCaloriesBurned((long)obj);
    }

    public void OnCaloriesBurned(long calories)
    {
        if (startCalories == null)
        {
            startCalories = calories;
        }

        long burned  = calories - startCalories.Value;
        
        if (CaloriesBurned != null)
            CaloriesBurned.text = burned.ToString();
    }

    public void OnDistanceChanged(SensorData sensorData)
    {
        distance = CalculateDistance(sensorData);
        speed = CalculateSpeed(sensorData);

        if (Player != null)
        {
            //double fakeSpeed;
            //if (speed < 1 && isMoving)
            //{
            //    fakeSpeed = 1;
            //    Player.SendMessage("SpeedChanged", fakeSpeed);
            //}
            //else if (speed > 1 && !isMoving)
            //{
            //    fakeSpeed = 0;
            //    Player.SendMessage("SpeedChanged", fakeSpeed);
            //}
            //else
            //{
                Player.SendMessage("SpeedChanged", speed);
            //}
        }
    }

    public void OnMovingChanged(bool moving)
    {
        isMoving = moving;
    }

    public void OnPlayerUpdated(PlayerInfo info)
    {
        if (Player == null) return;

        if (Player.GetComponent<HubBehaviour>().Id == info.Id)
        {
            return;
        }

        var otherPlayer = OtherPlayers.FirstOrDefault(o => o.GetComponent<OtherPlayer>().Id == info.Id);
        if (otherPlayer == null)
        {
            Vector3 position = new Vector3(info.PositionX, info.PositionY, info.PositionZ);
            Quaternion rotation = Quaternion.Euler(info.ForwardX, info.ForwardY, info.ForwardZ);
            GameObject prefab = Instantiate(OtherPlayerPrefab, position, rotation) as GameObject;
            prefab.GetComponent<OtherPlayer>().Id = info.Id;
            OtherPlayers.Add(prefab);
        }
        else
        {
            Vector3 position = new Vector3(info.PositionX, info.PositionY, info.PositionZ);
            Vector3 rotation = new Vector3(info.ForwardX, info.ForwardY, info.ForwardZ);

            otherPlayer.GetComponent<OtherPlayer>().SlerpTo(position, rotation);
        }
                
        
    }

    public void OnHeartRateChanged(int heartRate)
    {
        this.heartRate = heartRate;

        if(HeartRate != null)
            HeartRate.text = this.heartRate.ToString();
    }

    private double CalculateDistance(SensorData sensorData)
    {
        if (startDistance == null)
        {
            startDistance = sensorData.TotalDistance;
        }

        double distanceInCm = sensorData.TotalDistance - startDistance.Value;

        double distanceInMiles = distanceInCm / 160934;

        double rounded = Math.Round(distanceInMiles, 2);

        if (Distance != null)
            Distance.text = rounded.ToString();

        return rounded;
    }

    private double CalculateSpeed(SensorData sensorData)
    {
        double cmPerSecond = sensorData.Speed;

        double cmPerHour = cmPerSecond * 60 * 60;

        double milesPerHour = cmPerHour / 160934;

        double rounded = Math.Round(milesPerHour, 2);

        if(Speed != null)
            Speed.text = rounded.ToString();

        return rounded;
    }
}
