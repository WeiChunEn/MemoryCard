using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change : MonoBehaviour
{
    public GameObject Card;
    //[SerializeField]
   // private float _fChange_Start = 0.0f;
    //[SerializeField]
   // private float _fChange_Time = 3.0f;
    private int[] _iLeftCard = new int[40];
    [SerializeField]
    private int _iRnd_Num1;
    [SerializeField]
    private int _iRnd_Num2;
    private int _iNUM = 0;
    [SerializeField]
    private Vector2 _Card1Posi;
    [SerializeField]
    private Vector2 _Card2Posi;
    public int _iSum;
    public int _itmp = 0;

    void Start()
    {
        _iSum = 20;
    }


    void Update()
	{
       
		if (_iSum <= 10 && _itmp == 0) {
			
			for (int i = 0; i < 5; i++) {
				//Change_Card();
				Invoke ("Change_Card", 2.0f);
			}
		}
	}

    private void Change_Card ()
	{
		_itmp++;
		_iNUM = 0;

		for (int i = 0; i < 40; i++) {
			if (Card.GetComponent<Random_Card> ()._itmp [i] == 0) {

				_iLeftCard [_iNUM] = i;
				_iNUM++;

			}
		}
       
		_iRnd_Num1 = Random.Range (0, 20);
		_Card1Posi = Card.transform.GetChild (_iLeftCard [_iRnd_Num1]).transform.position;
		_iRnd_Num2 = Random.Range (20, 40);
		_Card2Posi = Card.transform.GetChild (_iLeftCard [_iRnd_Num2]).transform.position;
		Card.transform.GetChild (_iLeftCard [_iRnd_Num1]).transform.position = _Card2Posi;
		Card.transform.GetChild (_iLeftCard [_iRnd_Num2]).transform.position = _Card1Posi;   
	}
}
