using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour {
	
	public delegate void HourAction();
	public static event HourAction onHour;

	public delegate void DayAction();
	public static event HourAction onDay;

	public delegate void MonthAction();
	public static event HourAction onMonth;

	public delegate void YearAction();
	public static event HourAction onYear;

	//real time seconds per in game hour
	public float hourTime = 2;
	private Date currentDate;
	float currentTime;

	// Use this for initialization
	void Start () {
		currentTime = Time.realtimeSinceStartup;
		currentDate = new Date ();
	}
	
	// Update is called once per frame
	void Update () {
		if((Time.realtimeSinceStartup - currentTime) >= hourTime){
			currentTime = Time.realtimeSinceStartup;
			currentDate.advanceHour ();
			nextHour ();
			//printDateTime ();
		}
	}

	void printDateTime(){
		Debug.Log (currentDate.GetCurrentDateString());
	}

	void nextHour(){
		if (onHour != null) {
			onHour ();
		}
		if(currentDate.hour == 0){
			nextDay ();
		}
	}

	void nextDay(){
		if (onDay != null) {
			onDay ();
		}
		if(currentDate.day == 1){
			nextMonth ();
		}

	}
	void nextMonth(){
		if (onMonth != null) {
			onMonth ();
		}
		if(currentDate.month == 1){
			nextYear ();
		}
	}

	void nextYear(){
		if (onYear != null) {
			onYear ();
		}
	}
}
