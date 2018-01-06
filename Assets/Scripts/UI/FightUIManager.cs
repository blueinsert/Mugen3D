using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mugen3D;
public class FightUIManager : MonoBehaviour {
    public Image p1LifeBar;
    public Image p2LifeBar;
    private Player mP1;
    private Player mP2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (mP1 != null && mP2 != null)
        {
            p1LifeBar.fillAmount = mP1.HP / mP1.MaxHP;
            p2LifeBar.fillAmount = mP2.HP / mP2.MaxHP;
        }
	}

    public void SetPlayer(Player p1, Player p2)
    {
        mP1 = p1;
        mP2 = p2;
    }
}
