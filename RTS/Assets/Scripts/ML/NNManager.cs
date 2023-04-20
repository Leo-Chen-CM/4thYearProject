using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNManager : MonoBehaviour
{
    NeuralNetwork net;
    int[] layers = new int[3] { 3, 5, 1 };
    string[] activation = new string[2] { "leakyrelu", "leakyrelu" };

    void Start()
    {
        this.net = new NeuralNetwork(layers, activation);
        for (int i = 0; i < 20000; i++)
        {
            net.BackPropagate(new float[] { 0, 0, 0 }, new float[] { 0 });
            net.BackPropagate(new float[] { 1, 0, 0 }, new float[] { 1 });
            net.BackPropagate(new float[] { 0, 1, 0 }, new float[] { 1 });
            net.BackPropagate(new float[] { 0, 0, 1 }, new float[] { 1 });
            net.BackPropagate(new float[] { 1, 1, 0 }, new float[] { 1 });
            net.BackPropagate(new float[] { 0, 1, 1 }, new float[] { 1 });
            net.BackPropagate(new float[] { 1, 0, 1 }, new float[] { 1 });
            net.BackPropagate(new float[] { 1, 1, 1 }, new float[] { 1 });
        }
        print("cost: " + net.cost);

        Debug.Log(net.FeedForward(new float[] { 0, 0, 0 })[0]);
        Debug.Log(net.FeedForward(new float[] { 1, 0, 0 })[0]);
        Debug.Log(net.FeedForward(new float[] { 0, 1, 0 })[0]);
        Debug.Log(net.FeedForward(new float[] { 0, 0, 1 })[0]);
        Debug.Log(net.FeedForward(new float[] { 1, 1, 0 })[0]);
        Debug.Log(net.FeedForward(new float[] { 0, 1, 1 })[0]);
        Debug.Log(net.FeedForward(new float[] { 1, 0, 1 })[0]);
        Debug.Log(net.FeedForward(new float[] { 1, 1, 1 })[0]);
        //We want the gate to simulate 3 input or gate (A or B or C)
        // 0 0 0    => 0
        // 1 0 0    => 1
        // 0 1 0    => 1
        // 0 0 1    => 1
        // 1 1 0    => 1
        // 0 1 1    => 1
        // 1 0 1    => 1
        // 1 1 1    => 1
    }
}
