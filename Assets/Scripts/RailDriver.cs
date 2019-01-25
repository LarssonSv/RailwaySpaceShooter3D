using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailDriver : MonoBehaviour
{

    public Rail rail;
    public float speed = 2.5f;
    public PlayMode mode;
    public bool isReversed;
    public bool isLooping;
    public bool pingPong;

    private bool isCompleted = false;
    private float dot = 0.0f;
    private int currentIndex = 0;



    private void Update()
    {
        if (rail || !isCompleted)
        {
            Play(!isReversed);
        }
        else
        {
            Debug.LogWarning("No Rail Found!");
        }
    }

    private void Play(bool forward = true)
    {
        float m = (rail.nodes[currentIndex + 1].position - rail.nodes[currentIndex].position).magnitude;
        float s = (Time.deltaTime * 1 / m) * speed;

        dot += (forward) ? s : -s; 
        if (dot > 1)
        {
            dot = 0.0f;
            currentIndex++;
            if(currentIndex == rail.nodes.Length - 1)
            {
                if (isLooping)
                {
                    if (pingPong)
                    {
                        dot = 1;
                        currentIndex = rail.nodes.Length - 2;
                        isReversed = !isReversed;
                    }
                    else
                    {
                        currentIndex = 0;
                    }
                }
                else
                {
                    isCompleted = true;
                    return;
                }
            }
        }
        else if (dot < 0)
        {
            dot = 1;
            currentIndex--;
            if (currentIndex ==  -1)
            {
                if (isLooping)
                {
                    if (pingPong)
                    {
                        dot = 0;
                        currentIndex = 0;
                        isReversed = !isReversed;
                    }
                    else
                    {
                        currentIndex = rail.nodes.Length - 2;
                    }
                }
                else
                {
                    isCompleted = true;
                    return;
                }
            }
        }

        transform.position = rail.PositionOnRail(currentIndex, dot, mode);
        transform.rotation = rail.Orientation(currentIndex, dot);
    }



}
