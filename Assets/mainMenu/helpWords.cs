using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class helpWords : MonoBehaviour {
	public Sprite[] thisSprite;
	public GameObject Pre, next;
	int spriteNum;
	// Use this for initialization
	void Start () {
		spriteNum = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void changeSprite(bool PlusorMinus){
		//print (thisSprite.Length);
		Pre.SetActive(true);
		next.SetActive(true);
		if (PlusorMinus)
			spriteNum++;
		else
			spriteNum--;
		if (spriteNum == 0)
			Pre.SetActive(false);
		if (spriteNum >= (thisSprite.Length-1))
			next.SetActive(false);
		this.GetComponent<SpriteRenderer>().sprite = thisSprite[spriteNum];
	}
}
