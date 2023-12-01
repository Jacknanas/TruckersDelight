using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunCardUI : MonoBehaviour
{	
	[Header("UI")]
	public Text title;
	public Text time;
	public Text distance;
	public Text mass;
	public Text type;
	public Text difficulty;
	public Text pay;

	public Image typeImage;
	public List<Sprite> typeIcons;

	[Header("Button stuff")]
	public Animator runSelectButton;
	public TruckStopMenu menu;
	public List<RunCardUI> otherCards;

	

	public bool isGameTime = false;
	public bool isSummaryTime = false;

	Run storedRun;


	void Start()
	{
		if (StaticStats.run != null && isGameTime)
            ExtractRunData();
		else if (StaticStats.run != null && isSummaryTime)
			ExtractRunDataForSummary();
	}

	void ExtractRunData()
	{
		SetCardInformation(StaticStats.run);
	}

	void ExtractRunDataForSummary()
	{
		Run run = StaticStats.run;

		time.text = $"{StaticStats.timeElapsed} / {run.expectedTime}";
		distance.text = run.length.ToString();
		mass.text = $"{StaticStats.remainingMass} / {run.mass}";
		difficulty.text = run.difficulty.ToString();
		//type.text = run.type;
		pay.text = run.pay.ToString();
		
		typeImage.sprite = typeIcons[(int)run.type];
		storedRun = run;
	}

	public void SetCardInformation(Run run)
	{
		title.text = run.name;
		time.text = run.expectedTime.ToString();
		distance.text = run.length.ToString();
		mass.text = run.mass.ToString();
		difficulty.text = run.difficulty.ToString();
		//type.text = run.type;
		pay.text = run.pay.ToString();
		
		typeImage.sprite = typeIcons[(int)run.type];
		storedRun = run;
	}

	public void OnSelectClick()
	{
		runSelectButton.SetTrigger("OnClick");
		runSelectButton.gameObject.SetActive(false);
		gameObject.GetComponent<Animator>().SetTrigger("Picked");
		StartCoroutine(GoAway());

		menu.SelectedRun(storedRun);

		foreach (RunCardUI card in otherCards)
		{
			card.OnNotSelected();
		}

	}

	public void OnNotSelected()
	{
		runSelectButton.gameObject.SetActive(false);
		gameObject.GetComponent<Animator>().SetTrigger("NotPicked");
		StartCoroutine(GoAway());
	}


    IEnumerator GoAway()
	{
		yield return new WaitForSeconds(0.5f);
		gameObject.SetActive(false);
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