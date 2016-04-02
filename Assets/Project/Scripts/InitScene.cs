using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InitScene : MonoBehaviour {
    private GameObject bandManager;
    private XAMLConnection xamlConnnection;
	// Use this for initialization
	void Start () {
        bandManager = GameObject.Find("BandManager");
        xamlConnnection = bandManager.GetComponent<XAMLConnection>();
        xamlConnnection.Player = GameObject.Find("Player");

        GameObject Speed = GameObject.Find("Speed");
        var speedText = Speed.GetComponent<Text>();
        xamlConnnection.Speed = speedText;

        GameObject Distance = GameObject.Find("Distance");
        var distanceText = Distance.GetComponent<Text>();
        xamlConnnection.Distance = distanceText;

        GameObject HeartRate = GameObject.Find("HeartRate");
        var heartRateText = HeartRate.GetComponent<Text>();
        xamlConnnection.HeartRate = heartRateText;

        GameObject TimeElapsed = GameObject.Find("TimeElapsed");
        var timeElapsedText = TimeElapsed.GetComponent<Text>();
        xamlConnnection.TimeElapsed = timeElapsedText;

        GameObject CaloriesBurned = GameObject.Find("CaloriesBurned");
        var caloriesBurnedText = CaloriesBurned.GetComponent<Text>();
        xamlConnnection.CaloriesBurned = caloriesBurnedText;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
