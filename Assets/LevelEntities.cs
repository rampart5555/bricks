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

    void FixedUpdate ()
    {   
        if (m_paddle.m_ballStatus==Paddle.BallStatus.BallReleased)
        {
            m_paddle.m_ballStatus = Paddle.BallStatus.BallRunning;
            m_ball.SetSpeed(0.0f, 1.0f);
            Debug.Log("Ball is running");
        }

        if (Input.GetMouseButton(0))
        {                                    
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {  
                m_paddle.PaddleMove(hit.point);
            } 
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_paddle.BallRelease();
        }
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
