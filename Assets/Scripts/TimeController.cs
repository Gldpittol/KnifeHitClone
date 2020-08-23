using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Script para controle de tempo. Responsável por checar se ainda há tempo disponível para continuar a fase, 
//e caso a fase seja vencida, da ao jogador um rank dependendo da sua performance (tempo restante)

public class TimeController : MonoBehaviour
{
    public float timeRemaining;
    public GameControllerScript gc;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
        timeRemaining = gc.timeAvailable;
    }

    void Update()
    {
        //Se fase estiver em andamento, decrementa o tempo
        if (!gc.gameOver && !gc.gameWon)
        {
            timeRemaining -= Time.deltaTime;
            gc.timeCount.text = "Time Left: " + timeRemaining.ToString("F0");
        }
        //Se fase é vencida, atribui um rank
        if (gc.gameWon)
        {
            StartCoroutine(WaitSecs());
        }
        //Se tempo acaba, é game over
        if (timeRemaining < 0)
        {
            gc.gameOver = true;
            Destroy(gameObject);
        }

    }

    //Método para decidir rank do jogador baseado no tempo restante
    public string RankDecision()
    {
        if (timeRemaining > 22) return "SSS";
        else if (timeRemaining > 19) return "SS";
        else if (timeRemaining > 16) return "S";
        else if (timeRemaining > 13) return "A";
        else if (timeRemaining > 10) return "B";
        else if (timeRemaining > 7) return "C";
        else return "D";
    }
    //Co-rotina para esperar um tempo antes de mostrar o rank do jogadorna tela
    public IEnumerator WaitSecs()
    {
        yield return new WaitForSeconds(1.0f);
        gc.rank.text = "Rank: " + RankDecision();
    }
}
