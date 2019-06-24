using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessBlock : MonoBehaviour
{
    public enum Spot { SAFE, QUEEN, UNDERATTACK, NOTTHISSPOT }

    [SerializeField]
    private Text mText;

   
    public GameObject mQueen;
    public GameObject mMarked;
    public GameObject mNotThisSpot;

    private Spot mSpot;

    private int _mRow;
    private int _mCol;

   // public void SetSpot(Spot spot)
   // { mSpot = spot; }
   // public Spot GetSpot()
  //  { return mSpot; }

    public Spot MSpot
    {
        get{ return mSpot;  }


        set{ mSpot = value; }
    }



    public void SetBlock(int r, int c) 
    { _mRow = r; _mCol = c; }

    public  int GetR()
    { return _mRow; }

    public  int GetC()
    { return _mCol; }

    

    void Start()
    { mText.text = _mRow + "," + _mCol; }

    public void TouchTheSpot()
    {
        if (mSpot == Spot.QUEEN)
            mSpot = Spot.SAFE;
        else if (mSpot == Spot.SAFE)
            mSpot = Spot.QUEEN;
    }
}

