using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NNCommanderBrain : MonoBehaviour
{
    NeuralNetwork m_neuralNetwork;
    int[] layers = new int[3] { 4, 5, 3 };
    string[] activation = new string[2] { "relu", "sigmoid" };

    NNCommanderBody m_NNCommanderBody;


    public static NNCommanderBrain instance;

    public float m_evaluationTime = 3;
    public float[] m_outputs;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        m_neuralNetwork = new NeuralNetwork(layers, activation);
        m_NNCommanderBody = GetComponent<NNCommanderBody>();
        /*
        4 basic inputs for feed forward:

        Total friendly units in the field
        Control points owned by NN Commander
        Current score count compared to enemy score
        Current resource count
        */

        /*
         
        3 basic outputs once all inputs are accounted for
        Make more units and then group them up 
        Send units to control point
        Send units to resource point
         
        */


        StartCoroutine(EvaluateInputs());
    }


    IEnumerator EvaluateInputs()
    {
        while (true)
        {
            m_outputs = m_neuralNetwork.FeedForward(m_NNCommanderBody.GetInputs());

            //for (int i = 0; i < m_outputs.Length; i++)
            //{
            //    Debug.Log(m_outputs[i]);
            //}

            //Debug.Log("Spawn a unit: " + m_outputs[0]);
            //Debug.Log("Send units to CP: " + m_outputs[1]);
            //Debug.Log("Send units to RP: " + m_outputs[2]);

            yield return new WaitForSeconds(m_evaluationTime);

            m_NNCommanderBody.DecideAction();
        }
    }
}
