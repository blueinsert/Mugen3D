using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Color enteredColor;
    private Button m_button;
    private Color m_normalColor;

    public void OnPointerEnter(PointerEventData eventData) {
        m_button.targetGraphic.color = enteredColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
         m_button.targetGraphic.color = m_normalColor;
    }

    // Use this for initialization
    void Start () {
		m_button = this.GetComponent<Button>();
        m_normalColor = m_button.targetGraphic.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
