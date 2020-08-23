using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script básico de encerrar o jogo
public class Quit : MonoBehaviour
{

    public void AppQuit()
    {
        Debug.Log("quit");
        Application.Quit();
    }
  
}
