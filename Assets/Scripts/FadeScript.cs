using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Script com o objetivo de aplicar fade out em um texto ao longo de um segundo. 
public class FadeScript : MonoBehaviour
{
    public Text toFade;
    private float i;

    void Start()
    {
        i = 1;
        toFade = GetComponent<Text>();
    }


    private void Update()
    {
        if (i >= 0)
        {
            toFade.color = new Color(255, 255, 255, i);
            i -= Time.deltaTime;
        }
        else Destroy(this.gameObject);
    }

   
}
