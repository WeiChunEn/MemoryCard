using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Card : MonoBehaviour
{
    public GameObject[] _gCard;
    [SerializeField]
    private int _iRnd;
    [SerializeField]
    private int _iPosiY = 0;
    [SerializeField]
    private float _fPosiX = 0;
	public int[] _itmp = new int[40];
    //public GameObject[] _gInit_Card = new GameObject[40];

	void Start ()
    {
		// _itmp[0~39] = 1, is able to set gCard
		for (int i = 0; i < 40; i++) {
			_itmp [i] = 1;
		}
        appear();
    }

    void appear()
	{
		for (int i = 0; i < 40; i++) {
			
			_iRnd = Random.Range (0, 40); // _iRnd = 0~39

			for (int j = 0; j < 40; j++) {       
				
				if (j == _iRnd && _itmp [_iRnd] == 1) {
					
					_itmp [_iRnd]--;
					//print(1);
					GameObject _card;
                    //_gInit_Card[j] = _gCard[_iRnd];
                    _card = Instantiate (_gCard [_iRnd], new Vector2 (_fPosiX, _iPosiY), _gCard [_iRnd].transform.rotation);
					_card.transform.parent = gameObject.transform;
					//_itmp[_iRnd]++;
					_iPosiY += 2;

					if (_iPosiY == 8) {
						_fPosiX+=0.9f;
						_iPosiY = 0;
					}

				} else if (j == _iRnd && _itmp [_iRnd] == 0) {
					i--;
				}
			}
		}
	}
}
