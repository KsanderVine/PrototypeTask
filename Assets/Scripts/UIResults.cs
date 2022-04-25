using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResults : MonoBehaviour
{
    public GameObject MainPanel;
    public Image Background;
    public Text Title;

    public float AlphaSpeed = 5f;
    
    public void Show ()
    {
        Color color = Background.color;
        color.a = 0;
        Background.color = color;
        Title.gameObject.SetActive(false);
        MainPanel.SetActive(true);
    }

    public void Update()
    {
        if (MainPanel.activeSelf == false)
            return;

        Color color = Background.color;
        color.a += Time.deltaTime * AlphaSpeed;
        color.a = Mathf.Clamp(color.a, 0, 0.5f);
        Background.color = color;

        if(color.a == 0.5f)
        {
            if (Title.gameObject.activeSelf == false)
            {
                Title.gameObject.SetActive(true);
            }
        }
    }
}
