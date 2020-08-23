using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script com o objetivo de aplicar uma rotação randômica no círculo, tanto em velocidade quanto em sentido (horário/anti-horário)
public class CircleRotation : MonoBehaviour
{
    public GameControllerScript gc;
    public Rigidbody2D rb;
    public float speed;
    public float maxSpeed;
    public bool accelerate;

    private void Start()
    {
        speed = 0;
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {   
        //se velocidade é zero ou negativa, calcula a próxima velocidade a ser atingida, assim como o lado para o qual o circulo irá rotacionar.
        if (speed <= 0)
        {
            accelerate = true;
            maxSpeed = Random.Range(gc.circleSpeedMin, gc.circleSpeedMax);
            int side = Random.Range(0, 2);
            if (side == 1)
            {
                gc.circleSide = true;
            }   
            else
            {
                gc.circleSide = false;
            }
        }
        //Círculo acelera até atingir velocidade máxima, e então começa a desacelerar. Aceleração decidida pelo booleano accelerate,
        //sentido decidido pelo booleano circleSide.
        if (speed >= maxSpeed)
        {
            accelerate = false;
        }
        if (accelerate)
        {
            speed += 0.5f * Time.deltaTime;
        }
        else
        {
            speed -= 0.5f * Time.deltaTime;
        }
        if (gc.circleSide)
        {
            transform.Rotate(0, 0, speed);
        }
        else
        {
            transform.Rotate(0, 0, -speed);
        }
    }
}
