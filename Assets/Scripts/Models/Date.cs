using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Date{
	String[] monthNames = { "Iwris", "Quizic", "Dicalion", "Nugrah", "Magthys", "Baldion", "Guroth", "Otus", "Orbis", "Pathedos", "Zitharous", "Shah" };

	//incase i want to have different sized months
	int[] monthDays = { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, };

	public int hour { get; private set;}
	public int day{ get; private set;}
	public int month{ get; private set;}
	public int year{ get; private set;}

	public Date(){
		hour = 22; 
		day = 30;
		month = 12;
		year = 1;
	}

	public void advanceHour(){
		hour++;
		if(hour > 23){
			hour = 0;
			day++;
			if (day > monthDays[month-1]) {
				day = 1;
				month++;
				if (month > 12) {
					month = 1;
					year++;
				}
			}
		}
	}

	public String GetCurrentDateString(){
		//return "Date: " + year + "/" + month + "/" + day + "/" + hour + ":00";
		return "Date: " + year + "/" + monthNames[month-1] + "/" + day + "/" + hour + ":00";
	}	
}

