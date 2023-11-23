using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run
{
	public string name;
	public int expectedTime;
	public int length;
	public int mass;
	public int difficulty;

	public JobType type;

	public int pay;

	public Run(string name, int expectedTime, int length, int mass, int difficulty, JobType type, int pay)
	{
		this.name = name;
		this.expectedTime = expectedTime;
		this.length = length;
		this.mass = mass;
		this.difficulty = difficulty;
		this.type = type;
		this.pay = pay;
	}

}
