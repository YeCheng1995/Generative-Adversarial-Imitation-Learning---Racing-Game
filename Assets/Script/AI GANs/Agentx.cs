using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class Agentx : Agent
{
    internal NeuralNetwork nn;
    internal Genome ge;
    public bool isDraw;

    internal NeuralNetwork Dnn;
    internal Genome Dge;

    public virtual double[] GetInputs()
    {
        throw new System.Exception();
    }

    public virtual double[] DGetInputs(double[] outputs)                     //Discriminative 
    {
        throw new System.Exception();
    }

    public virtual void ResetAgent()
    {
        throw new System.Exception();
    }

    public virtual void ResetD()
    {
        throw new System.Exception();
    }

    public virtual void UseOutputs(double[] outputs)
    {
        throw new System.Exception();
    }

    public virtual void DUseOutputs(double[] outputs)
    {
        throw new System.Exception();
    }

    public virtual void SetFitness()
    {
        throw new System.Exception();
    }

    public virtual void DSetFitness() //D fit
    {
        throw new System.Exception();
    }

    public virtual void Draw()
    {
        throw new System.Exception();
    }

    public void SetInfo(NeuralNetwork nn, Genome ge)
    {
        this.nn = nn;
        this.ge = ge;
    }

    public void DSetInfo(NeuralNetwork Dnn, Genome Dge)//D的set
    {
        this.Dnn = Dnn;
        this.Dge = Dge;
    }


    public void FixedUpdate()
    {
        double[] inputs = GetInputs();
        double[] outputs = nn.Run(inputs);
        UseOutputs(outputs);
        SetFitness();


        double[] Dinputs = DGetInputs(outputs);
        double[] Doutputs = Dnn.Run(Dinputs);
        DUseOutputs(Doutputs);

    }
}
