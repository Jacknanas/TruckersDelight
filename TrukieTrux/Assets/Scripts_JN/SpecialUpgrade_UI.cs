using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialUpgrade_UI : MonoBehaviour
{

	public Image icon;
	public GameObject infoButton;

	public bool isFull = false;

	public Sprite specialIcon;

	string description;

	SpecialUpgrade sp;

	Text descriptionText;

	public void AddUpgrade(SpecialUpgrade sp_up, Text desc)
	{	
		isFull = true;

		sp = sp_up;

		description = sp.Description;
		
		descriptionText = desc;

		infoButton.SetActive(true);
		
		icon.gameObject.SetActive(true);
		icon.sprite  = specialIcon;
	}

	public void OnInfoButton()
	{
		descriptionText.text = description;
	}

}
