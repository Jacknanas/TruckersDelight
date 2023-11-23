using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunCardUI : MonoBehaviour
{
	public Text title;
	public Text time;
	public Text distance;
	public Text mass;
	public Text type;
	public Text difficulty;
	public Text pay;

	public Image typeImage;
	public List<Sprite> typeIcons;


	public void SetCardInformation(Run run)
	{
		title.text = run.name;
		time.text = run.expectedTime.ToString();
		distance.text = run.length.ToString();
		mass.text = run.mass.ToString();
		difficulty.text = run.difficulty.ToString();
		//type.text = run.type;
		pay.text = run.pay.ToString();
		
		
	}
}

public enum JobType{
	Food,
	Parcels,
	Garbage,
	Furniture,
	Water,
	Milk,
	IceCream,
	Tech,
	Gas,
	NuclearWaste
}