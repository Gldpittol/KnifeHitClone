using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script básico de mudañça de cena para ser usado em botões
public class LevelChange : MonoBehaviour
{
     public void ChangeScene(string sceneName)
     {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

}
