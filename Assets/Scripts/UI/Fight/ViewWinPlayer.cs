using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewWinPlayer : UIView {

    public Button btnContinue;
    public Button btnReturn;

    public void Awake()
    {
        btnContinue.onClick.AddListener(OnClickContinue);
        btnReturn.onClick.AddListener(OnClickReturn);
    }

    public void Show()
    {
        base.Show();
    }

    private void OnClickContinue()
    {
        Debug.Log("click continue");
    }

    private void OnClickReturn()
    {
        Debug.Log("ClickReturn");
    }
}
