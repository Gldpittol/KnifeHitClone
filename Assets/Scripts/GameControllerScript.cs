using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//Script geral para controlar o jogo. Possui variáveis separadas em diferentes headers
//e métodos que são acessados por diversos scripts com o intuito de facilitar
//o balanceamento do jogo, assim como possíveis futuras alterações.
public class GameControllerScript : MonoBehaviour
{   
    //Controla o estado do jogo, assim como quantos spawns randômicos de facas e maçãs já começam no círculo.
    [Header("Controle")]
    private int howManyApples;
    private int howManyKnives;
    public bool gameWon;
    public string gameWonScene;
    public string gameOverScene;
    public bool gameOver;
    public bool gameOverHappened;

    //Controla os clipes de audio que serão tocados
    [Header("Audio")]
    public AudioSource aud;
    public AudioClip throwClip;
    public AudioClip onHitClip;
    public AudioClip onKnifeHitClip;
    public AudioClip onAppleHitClip;
    public AudioClip onCircleBreakClip;

    //Controla elementos de interface
    [Header("UI")]
    public Text healthCount;
    public Text timeCount;
    public Text rank;
    public Text addSecsText;
    public Canvas canv;

    //Controla prefabs que são utilizadas para spawn de objetos
    [Header("Prefabs")]
    public GameObject knifePrefab;
    public GameObject knifeBrokenPrefab;
    public GameObject applePrefab;
    public GameObject appleBrokenPrefab;
    public GameObject greenApplePrefab;
    public GameObject greenAppleBrokenPrefab;
    public GameObject circleBrokenPrefab;
    private GameObject rotatingCircle;

    //Controla a faca a ser arremessada e quantas facas já poderão vir no círculo (mín-max)
    [Header("Knife Settings")]
    public float knifeSpeed;
    public float knifeScale;
    public float knifePosY;
    public int minKnife;
    public int maxKnife;

    //Controla o círculo (velocidade de rotação/sentido de rotação/vida)
    [Header("Circle Settings")]
    public float circleSpeedMin;
    public float circleSpeedMax;
    public bool circleSide;
    public int circleHP;

    //Controla maçãs verdes e vermelhas (onde eles surgirão e quantas já poderão vir no círculo (mín-max))
    [Header("Apple Settings")]
    public float appleHeight;
    public int maxApple;
    public int minApple;
    public int maxGreenApple;
    public int minGreenApple;

    //Controla o tempo limite para concluir a fase e quantos segundos uma maçã adiciona ao ser destruída;
    [Header("Time Settings")]
    public float timeAvailable;
    public float countdown;
    public float appleTime;

    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        gameWon = false;
        gameOver = false;
        gameOverHappened = false;

        rotatingCircle = GameObject.FindGameObjectWithTag("Circle");

        /*Inicializa maçãs vermelhas
        Para gerar as maçãs, é randomizado um número de maçãs a serem criadas entre um mínimo e o máximo (definidos neste script),
        a maçã é criada num ponto abaixo do círculo e fixada ao mesmo como filho, o círculo então rotaciona uma quantia aleatória
        entre 0 e 360 graus, e repete o ciclo N vezes, sendo N o número de maçãs à serem geradas*/
        howManyApples = Random.Range(minApple, maxApple);
        for (int i = 0; i < howManyApples; i++)
        {
            GameObject temp = Instantiate(applePrefab, new Vector3(0, appleHeight, 0), Quaternion.identity);
            temp.transform.SetParent(rotatingCircle.transform);
            rotatingCircle.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        //Inicializa maçãs verdes usando o mesmo método das maçãs vermelhas
        howManyApples = Random.Range(minGreenApple, maxGreenApple);
        for (int i = 0; i < howManyApples; i++)
        {
            GameObject temp = Instantiate(greenApplePrefab, new Vector3(0, appleHeight, 0), Quaternion.identity);
            temp.transform.SetParent(rotatingCircle.transform);
            rotatingCircle.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        //Inicializa facas usando o mesmo método das maçãs vermelhas
        howManyKnives = Random.Range(minKnife, maxKnife);
        for (int i = 0; i < howManyKnives; i++)
        {
            GameObject temp = Instantiate(knifePrefab, new Vector3(0, appleHeight, 0), Quaternion.identity);
            temp.transform.SetParent(rotatingCircle.transform);
            temp.tag = "KnifeHit";
            rotatingCircle.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }

    void Update()
    {   //Destroi círculo quando vida chegar em zero, e atualiza status do jogo para vencido e inicializa co-rotina de transição de cena
        if (circleHP <= 0 && !gameWon)
        {
            PlayAudio(4);

            StartCoroutine(TransitionCountdown(countdown, true));
            Instantiate(circleBrokenPrefab, rotatingCircle.transform.position, Quaternion.identity);
            Destroy(rotatingCircle);
            Destroy(healthCount);
            print("You win!");
            gameWon = true;
            

        }

        //Atualiza status do jogo para derrota e inicializa co-rotina de transição de cena
        if (gameOver && !gameOverHappened)
        {
            gameOverHappened = true;
           // Debug.LogError("Game Over");
            StartCoroutine(TransitionCountdown(countdown, false));
        }
    }

    //Após X segundos, vai para tela de vitória (próximo nível/tela de fim) ou derrota.
    public IEnumerator TransitionCountdown(float seconds, bool win)
    {     
        yield return new WaitForSeconds(seconds);

        if (!win)
        {
            SceneManager.LoadScene(gameOverScene, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(gameWonScene, LoadSceneMode.Single);
        }
    }

    //Atualiza vida do círculo
    public void UpdateCircleHP()
    {
        circleHP -= 1;
        if (circleHP > 0)
        {
            healthCount.text = circleHP.ToString();
        }
    }

    //Toca audio
    public void PlayAudio(int i)
    {
        switch(i)
        {
            case 1:
                aud.PlayOneShot(throwClip, 0.3f);
                break;
            case 2:
                aud.PlayOneShot(onHitClip, 0.3f);
                break;
            case 3:
                aud.PlayOneShot(onKnifeHitClip, 0.2f);
                break;
            case 4:
                aud.PlayOneShot(onCircleBreakClip, 0.5f);
                break;
            case 5:
                aud.PlayOneShot(onAppleHitClip, 0.8f);
                break;
        }
    }

}
