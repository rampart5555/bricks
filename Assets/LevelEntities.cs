using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntities : MonoBehaviour 
{    
    Paddle m_paddle;
    Ball m_ball;

    void Start()
    {
        m_paddle = transform.Find("paddle").gameObject.GetComponent<Paddle>();
        m_ball = transform.Find("ball").gameObject.GetComponent<Ball>();
        m_paddle.gameObject.SetActive(false);
        m_ball.gameObject.SetActive(false);
    }

    public void MouseRelease()
    {
        if (m_paddle.m_ballStatus == Paddle.BallStatus.BallReleased)
        {
            m_paddle.m_ballStatus = Paddle.BallStatus.BallRunning;
            m_ball.SetSpeed(0.0f, 1.0f);
            Debug.Log("Ball is running");
        }

        if (m_paddle.m_ballStatus == Paddle.BallStatus.BallAttached)
        {
            m_paddle.BallRelease();
        }
    }

    public void MouseDrag(Vector2 pos)
    {        
        m_paddle.PaddleMove(pos);
    }
           
    public void EnablePaddleBall()
    {
        Debug.Log("LevelEntities.EnablePaddleBall");
        m_paddle.gameObject.SetActive(true);
        m_ball.gameObject.SetActive(true);
        FixedJoint2D pfj = m_paddle.gameObject.GetComponent<FixedJoint2D>();
        pfj.connectedBody= m_ball.gameObject.GetComponent<Rigidbody2D>();
    }
}
