using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNCommanderBrain : MonoBehaviour
{
    NeuralNetwork m_neuralNetwork;

    int[] layers = new int[3] { 4, 5, 3 };
    string[] activation = new string[2] { "sigmoid", "sigmoid" };

    NNCommanderBody m_NNCommanderBody;

    public float m_evaluationTime = 3;

    private void Start()
    {
        m_neuralNetwork = new NeuralNetwork(layers, activation);
        m_NNCommanderBody = GetComponent<NNCommanderBody>();
        /*
        4 basic inputs for feed forward:

        Control points owned by NN Commander
        Total friendly units in the field
        Current resource count
        Current score count compared to enemy score
        */
        float[] floats = { 2, 14, 500, 56 };

        Debug.Log(m_neuralNetwork.FeedForward(floats));
        //Debug.Log(m_neuralNetwork.FeedForward(m_NNCommanderBody.GetInputs()));
    }


    IEnumerator EvaluateInputs()
    {
        while (true)
        {


            yield return new WaitForSeconds(m_evaluationTime);
        }
    }
}
