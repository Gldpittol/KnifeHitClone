using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script que trata do movimento e colisões das facas.
public class KnifeScript : MonoBehaviour
{
    public bool isKnifeReady;
    public float speed;

    private Rigidbody2D rb;

    public GameControllerScript gc;

    public TimeController tc;

    private GameObject rotatingCircle;
    private GameObject temp;

    private Text tempText;

    void Start()
    {
        //Facas não grudadas no círculo recebem tag "Knife" e podem ser atiradas.
        speed = 20;
        if (CompareTag("Knife"))
        {
            isKnifeReady = true;
        }
        rb = GetComponent<Rigidbody2D>();            
        rotatingCircle = GameObject.FindGameObjectWithTag("Circle");
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
        tc = GameObject.FindGameObjectWithTag("Time").GetComponent<TimeController>();
    }

    void Update()
    {   //Ao clicar no botão esquerdo do mouse para atirar, e existir uma faca pronta para arremesso e o jogo estiver em andamento, atira a faca e toca o clipe de audio
        if(Input.GetButtonDown("Fire1") && isKnifeReady && !gc.gameOver && !gc.gameWon)
        {
            if (isKnifeReady)
            {
                gc.PlayAudio(1);
            }

            isKnifeReady = false;
            rb.velocity = new Vector3(0, gc.knifeSpeed, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        /*Em colisão entra uma faca arremessada e o círculo, causa dano ao círculo, toca o audio equivalente, 
        transforma a faca em objeto filho do círculo para os dois rodarem juntos, para a faca para ela nao continuar se movendo, e muda a tag da faca para "KnifeHit", 
        com o intuito de diferenciar a faca que está em andamento ou pronta para arremessar das facas grudadas no círculo. Também cria uma nova faca
        pronta para poder ser arremessada numa posição pré-definida*/
        if (other.gameObject.CompareTag("Circle") && this.gameObject.CompareTag("Knife"))
        {
            gc.PlayAudio(2);

            gc.UpdateCircleHP();
            speed = 0;
            transform.SetParent(rotatingCircle.transform);
            temp = Instantiate(gc.knifePrefab, new Vector3(0, gc.knifePosY, 0), Quaternion.identity);
            temp.transform.localScale = new Vector3(gc.knifeScale, gc.knifeScale, gc.knifeScale);
            gameObject.tag = "KnifeHit";
        }
        /*Em colisão entre uma faca grudada no círculo (tag "KnifeHit") e uma faca arremessada (Tag "Knife"), e apenas nesta colisão, 
        quebra a faca (destroi faca antiga e instancia um prefab da faca quebrada) que foi arremessada 
        e muda o status do jogo para game over, o que impede novas facas de serem criadas e atiradas, assim como move o jogador para a tela de game over após um tempo.*/
        if (other.gameObject.CompareTag("KnifeHit") && this.gameObject.CompareTag("Knife"))
        {
            gc.PlayAudio(3);

            temp = Instantiate(gc.knifeBrokenPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            gc.gameOver = true;
        }

       

    }

    void OnTriggerEnter2D(Collider2D other)
    {   //Em colisão entre faca arremessada e uma maçã (vermelha), toca o audio equivalente, destroi a maçã, adiciona dois segundos ao tempo restante,
        //e mostra na tela um texto de +2.0s para feedback visual do que ocorreu.
        if (other.gameObject.CompareTag("Apple") && this.gameObject.CompareTag("Knife"))
        {
            gc.PlayAudio(5);

            tempText = Instantiate(gc.addSecsText, other.transform.position, Quaternion.identity);
            tempText.transform.SetParent(gc.canv.transform);
            tempText.transform.localScale = new Vector3(1, 1, 1);
            tempText.text = "+" + gc.appleTime.ToString("F1").Replace(",", ".") + "s";
            tc.timeRemaining += gc.appleTime;

            temp = Instantiate(gc.appleBrokenPrefab, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            //Debug.Log("Apple Destroyed!");
        }

        //Em colusão com maçã verde, o mesmo ocorre quanto à colisão com maçã vermelha, porém perde dois segundos ao invés de ganhar, e o texto muda.
        if (other.gameObject.CompareTag("GreenApple") && this.gameObject.CompareTag("Knife"))
        {
            gc.PlayAudio(5);

            tempText = Instantiate(gc.addSecsText, other.transform.position, Quaternion.identity);
            tempText.transform.SetParent(gc.canv.transform);
            tempText.transform.localScale = new Vector3(1, 1, 1);
            tempText.text = "+" + gc.appleTime.ToString("F1").Replace(",", ".") + "s";
            tc.timeRemaining -= gc.appleTime;

            temp = Instantiate(gc.greenAppleBrokenPrefab, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            //Debug.Log("Apple Destroyed!");
        }
    }

 
}

