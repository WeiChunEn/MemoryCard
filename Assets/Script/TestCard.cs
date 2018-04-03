using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCard : MonoBehaviour {
	public GameObject[] Card;
	public int currentOpenedCard;
	public bool stopOpenCard = false;
    public GameObject _gChangeCard;
    public GameObject _gCardList;
	string firstHitCardTag;
	float rotationZ = 0.7071068f;

	int currentPlayer; // 1 = p1, 2 = p2
	int player1Score;
	int player2Score;
	int openedCard;
	public Text hud_Player1Score;
	public Text hud_Player2Score;
//	public Text hud_CurrentPlayer;
	public GameObject hud_Player1Win;
	public GameObject hud_Player2Win;
	public Image hud_Player1Face;
	public Image hud_Player2Face;
	public Sprite[] faceList;
//	public GameObject hud_Player1Time;
//	public GameObject hud_Player2Time;
	public GameObject hud_countdown1;
	public GameObject hud_countdown2;
	public GameObject hud_countdown3;

	public AudioSource audioSource;
	public AudioSource BGM;
	public AudioClip[] audioClip;

	// Use this for initialization
	void Start ()
	{
        Card = new GameObject[40];
		openedCard = 0;
		currentPlayer = 1;
		player1Score = 0;
		player2Score = 0;
		hud_Player1Win.SetActive (false);
		hud_Player2Win.SetActive (false);
		hud_Player1Face.color = Color.white;
		hud_Player2Face.color = Color.gray;
//		hud_Player1Time.SetActive (true);
//		hud_Player2Time.SetActive (false);
		Invoke ("Countdown3", 1.0f);
		Invoke ("Countdown2", 2.0f);
		Invoke ("Countdown1", 3.0f);
		Invoke ("FindCard", 4.0f);
       // Invoke ("OpenCard", 4.0f);
    }

	void Countdown3 () {
		hud_countdown1.SetActive (false);
		hud_countdown2.SetActive (false);
		hud_countdown3.SetActive (true);
	}

	void Countdown2 () {
		hud_countdown1.SetActive (false);
		hud_countdown2.SetActive (true);
		hud_countdown3.SetActive (false);
	}

	void Countdown1 () {
		hud_countdown1.SetActive (true);
		hud_countdown2.SetActive (false);
		hud_countdown3.SetActive (false);
	}
	
    public void FindCard ()
	{
		hud_countdown1.SetActive (false);
		hud_countdown2.SetActive (false);
		hud_countdown3.SetActive (false);

		for (int i = 0; i < 40; i++) {
			Card [i] = GameObject.Find ("CardList/" + i + "(Clone)").gameObject;
			//Card [i].gameObject.GetComponent<Card_Anime> ()._bFlip = true; // 把卡翻開動畫
		}
        OpenCard();
		Invoke ("CloseAllCard", 5.0f);
	}

    // Update is called once per frame
    void Update () 
	{
		UpdateScore ();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

       
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {
//			if (hit.collider) {
//				Debug.DrawLine (ray.origin, hit.transform.position, Color.red, 0.1f, true);
//				Debug.Log (hit.transform.tag);
//			}

			if (hit.transform.gameObject.transform.rotation.z == rotationZ && hit.transform.gameObject.GetComponent<Card_Anime>()._bFlip== false) {

				if (!stopOpenCard) {
					FlipBack();

					currentOpenedCard ++;

					hit.transform.gameObject.GetComponent<Card_Anime>()._bFlip = true;

					hit.transform.gameObject.transform.rotation = Quaternion.Euler (90, 0, 360);

					if (currentOpenedCard == 1) {
						firstHitCardTag = hit.transform.tag;

						AudioManager (IsSpecialCard () ? 1 : 4);

					} else if (currentOpenedCard == 2) {
						
						if (firstHitCardTag == hit.transform.tag) {
							StartCoroutine (DeleteCard ());

						} else {
							StartCoroutine (WrongPair ());
						}
					}
				}
			}
		}
	}

	bool IsSpecialCard () {
		
		if (firstHitCardTag == "4" ||
		    firstHitCardTag == "7" ||
		    firstHitCardTag == "10" ||
		    firstHitCardTag == "12" ||
		    firstHitCardTag == "15") {
			return true;

		} else {
			return false;
		}
	}

	public void FlipBack()
	{
		for (int i = 0; i < 40; i++) {
			// Card[i].gameObject.GetComponent<Card_Anime>()._bFlip = false;
			Card [i].gameObject.GetComponent<Card_Anime> ()._bBack_Flip = false;
		}
	}

	void UpdateScore () {

		hud_Player1Score.text = player1Score.ToString ();
		hud_Player2Score.text = player2Score.ToString ();
//		if (currentPlayer == 1) {
//			hud_CurrentPlayer.text = "Turn P1";
//
//		} else if (currentPlayer == 2) {
//			hud_CurrentPlayer.text = "Turn P2";
//		}
	}

	IEnumerator DeleteCard ()
	{
		stopOpenCard = true;
		openedCard++;
		_gChangeCard.GetComponent<Change>()._iSum--;
		yield return new WaitForSeconds(1.5f);
		GameObject.FindGameObjectWithTag(firstHitCardTag).gameObject.SetActive(false);
		GameObject.FindGameObjectWithTag(firstHitCardTag).gameObject.SetActive(false);

		currentOpenedCard = 0;
		stopOpenCard = false;
		CloseAllCard ();

		GetScore ();

		AudioManager (IsSpecialCard () ? 2 : 5);

		if (openedCard == 20||player1Score>15 || player2Score>15)
			Gameover ();
	}
		
	IEnumerator WrongPair ()
	{
		stopOpenCard = true;
		yield return new WaitForSeconds(1.5f);
		currentOpenedCard = 0;
		stopOpenCard = false;

		AudioManager (IsSpecialCard () ? 3 : 6);

		ChangeFace (false);

		CloseAllCard ();

		ChangePlayer ();
	}

	void AudioManager (int audioState) {
		
		
			
			audioSource.volume = 0.5f;
			switch (audioState) {
			case 1: // 光牌翻排音效
				BGM.Pause ();
				audioSource.PlayOneShot (audioClip [0]);
				break;

			case 2: // 光牌翻對音效
				audioSource.Stop ();
				audioSource.volume = 1.0f;
				audioSource.PlayOneShot (audioClip [1]);
				Invoke ("ResumeBGM", 3.0f);
				break;

			case 3: // 光牌翻錯音效
				audioSource.Stop ();
				audioSource.PlayOneShot (audioClip [2]);
				Invoke ("ResumeBGM", 3.0f);
				break;

			case 4: // 普通翻牌無音效
				break;

			case 5: // 普通翻對音效
				audioSource.PlayOneShot (audioClip [3]);
				break;

			case 6: // 普通翻錯音效
				audioSource.volume = 1.0f;
				audioSource.PlayOneShot (audioClip [4]);
				break;
            case 7:
                if(IsInvoking("ResumeBGM"))CancelInvoke("ResumeBGM");
                BGM.Stop();
                audioSource.Stop();
                audioSource.volume = 1.0f;
                audioSource.PlayOneShot(audioClip[5]);
            break;

		}
	}

	void ResumeBGM () {
		audioSource.Stop ();
		BGM.UnPause ();
	}

	public void CloseAllCard ()
	{
		for (int i = 0; i < 40; i++) {
			Card [i].gameObject.transform.rotation = Quaternion.Euler (90, 0, 180);
			Card[i].gameObject.GetComponent<Card_Anime>()._bFlip = false;
			Card[i].gameObject.GetComponent<Card_Anime>()._bBack_Flip = true;
		}
	}

	void GetScore () {

		int getScore = 1;

		if (IsSpecialCard ()) {
			getScore = 3;
		}

		if (currentPlayer == 1) {
			player1Score += getScore;

		} else if (currentPlayer == 2) {
			player2Score += getScore;
		}

		ChangeFace (true);
	}

	void ChangeFace (bool isCorrect) {

		if ((currentPlayer == 1 && isCorrect) || (currentPlayer == 2 && !isCorrect)) {
			hud_Player1Face.sprite = faceList [0];
			hud_Player2Face.sprite = faceList [5];

		} else if ((currentPlayer == 2 && isCorrect) || (currentPlayer == 1 && !isCorrect)) {
			hud_Player1Face.sprite = faceList [2];
			hud_Player2Face.sprite = faceList [3];
		}

		Invoke ("IdleFace", 3.0f);
	}

	void IdleFace () {
		hud_Player1Face.sprite = faceList [1];
		hud_Player2Face.sprite = faceList [4];
	}

	void ChangePlayer () {

		if (currentPlayer == 1) {
			currentPlayer = 2;
			hud_Player1Face.color = Color.gray;
			hud_Player2Face.color = Color.white;
//			hud_Player1Time.SetActive (false);
//			hud_Player2Time.SetActive (true);

		} else if (currentPlayer == 2) {
			currentPlayer = 1;
			hud_Player1Face.color = Color.white;
			hud_Player2Face.color = Color.gray;
//			hud_Player1Time.SetActive (true);
//			hud_Player2Time.SetActive (false);
		}
	}

	void Gameover () {

		if (player1Score > player2Score) {
			hud_Player1Win.SetActive (true);

		}
        else if(currentPlayer==1)
        {
            hud_Player1Win.SetActive(true);
        }
        else {
			hud_Player2Win.SetActive (true);
		}
		AudioManager (7);
		Invoke ("RestartGame", 14.0f);
	}

	void RestartGame () {
		FindObjectOfType<ScenesManager> ().LaunchStart ();
	}
    void OpenCard()
    {
        int Rnd;
        Rnd = Random.Range(0, 2);
        switch (Rnd)
        {
            //case 0:
            //    for (int i = 0; i < 40; i += 2)
            //    {
            //    //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
            //        _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
            //    }
            //break;
            //case 1:
            //    for (int i = 1; i < 41; i += 2)
            //    {
            //        //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
            //        _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
            //    }
            //    break;
            //case 2:
            //    for (int i =0 ; i < 20; i ++)
            //    {
            //        //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
            //        _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
            //    }
            //    break;
            //case 3:
            //    for (int i = 20; i < 40; i++)
            //    {
            //        //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
            //        _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
            //    }
            //    break;
            case 0:
                for (int i = 0; i < 40; i+=8)
                {
                    //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
                    _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
                }
                for (int i = 5; i < 40; i += 8)
                {
                    //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
                    _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
                }
                for (int i = 2; i < 40; i += 8)
                {
                    //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
                    _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
                }
                for (int i = 7; i < 40; i += 8)
                {
                    //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
                    _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
                }
                break;
            case 1:
                for (int i = 4; i < 40; i += 8)
                {
                    //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
                    _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
                }
                for (int i = 1; i < 40; i += 8)
                {
                    //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
                    _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
                }
                for (int i = 6; i < 40; i += 8)
                {
                    //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
                    _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
                }
                for (int i = 3; i < 40; i += 8)
                {
                    //Card[i] = GameObject.Find("CardList/" + i + "(Clone)").gameObject;
                    _gCardList.transform.GetChild(i).GetComponent<Card_Anime>()._bFlip = true; // 把卡翻開動畫
                }
                break;


                //Invoke("CloseAllCard", 5.0f);
        }
    }
}