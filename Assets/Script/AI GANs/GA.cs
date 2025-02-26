using System;
using System.Collections.Generic;
using System.Linq;
public class GA
{
    private int populationSize;
    private double cross_ratio = 0.8;
    private double muta_ratio = 0.4;
    private double dertFit;

    public GA(int populationSize)
    {
        this.populationSize = populationSize;
    }
    public List<double[]> Run(List<Genome> parents, out double[] best,out double score)
    {
        List<double[]> children = new List<double[]>();
        
        //
        #region Sort groups by fitness
        parents.Sort((x, y) =>
        {
            if (x.fitness > y.fitness)
            {
                return 1;
            }
            else if (x.fitness < y.fitness)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });
        #endregion

        //
        #region assignment best
        best = parents[parents.Count - 1].weights;
        score = parents[parents.Count - 1].fitness;
        #endregion

        //
        #region output
        double totalFit = 0;
        foreach (var t in parents)
        {
            totalFit += t.fitness;
        }
        UnityEngine.Debug.Log("Best Fitness:" + parents[parents.Count - 1].fitness + "   Average Fitness:" + totalFit / parents.Count);
        #endregion

        //
        #region Modification of negative fitness
        dertFit = parents.Min(a => a.fitness);
        if (dertFit < 0) dertFit = -dertFit;
        parents.ForEach(a => a.fitness += dertFit);
        #endregion

        //
        #region Elite car account for 1 / 4 of the total
        int elite_num = populationSize / 4;
        List<double[]> elite = new List<double[]>();
        //List<Genome> eliteGenomeList = new List<Genome>();
        for (int i = 0; i < elite_num; i++)
        {
            //eliteGenomeList.Add(parents[populationSize - i - 1]);
            elite.Add(parents[populationSize - i - 1].weights.CloneArr());
        }
        for (int i = 0; i < elite.Count; i++)
        {
            children.Add(elite[i].CloneArr());
        }
        #endregion

        //
        #region Cross
        while (true)
        {
            Genome dad = GetParent(parents);
            Genome mum = GetParent(parents);
            double[] baby1 = null;
            double[] baby2 = null;
            CrossoverAtSplitPoint(dad.splitPoints, dad.weights, mum.weights, out baby1, out baby2);
            children.Add(baby1);
            children.Add(baby2);
            int n = populationSize;
            if (children.Count >= n)
            {
                while (true)
                {
                    if (children.Count > n)
                    {
                        children.RemoveAt(children.Count - 1);
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            }
        }
        #endregion

        //
        #region variation weight
        for (int i = elite.Count; i < children.Count; i++)
        {
            if (UnityEngine.Random.Range(0f, 1f) < muta_ratio)
            {
                int index = UnityEngine.Random.Range(0, children[0].Length);
                children[i][index] = UnityEngine.Random.Range(-1f, 1f);
            }
        }
        #endregion


        return children;
    }

    private Genome GetParent(List<Genome> parents)
    {
        double totalFit = 0;
        double min = 9999;

        for(int i= 0; i < parents.Count; i++)
        {
            if (parents[i].fitness < min)
            {
                min = parents[i].fitness;//find the worst
            }
        }

        for (int i = 0; i < parents.Count; i++)
        {
            if (min < 0)
            {
                parents[i].fitness += Math.Abs(min); //no negative fitne
            }
            totalFit += parents[i].fitness;
        }


        float rand = UnityEngine.Random.Range(0f, (float)totalFit);
        double tempFit = 0;
        int index = parents.Count - 1;
        for (int i = 0; i < parents.Count; i++)
        {
            tempFit += parents[i].fitness;
            if (tempFit >= rand)
            {
                index = i;
                break;
            }
        }

        return new Genome(parents[index].weights.CloneArr(), parents[index].fitness, parents[index].splitPoints);
    }

    private void CrossoverAtSplitPoint(int[] splitPoints, double[] dad, double[] mum, out double[] baby1, out double[] baby2)
    {
        baby1 = new double[dad.Length];
        baby2 = new double[dad.Length];
        if ((UnityEngine.Random.Range(0f, 1f) > cross_ratio) || (mum == dad))
        {
            baby1 = mum.CloneArr();
            baby2 = dad.CloneArr();
            return;
        }
        int index1 = UnityEngine.Random.Range(0, splitPoints.Length - 2);
        int index2 = UnityEngine.Random.Range(index1, splitPoints.Length - 1);

        //find cross section
        int cp1 = splitPoints[index1];
        int cp2 = splitPoints[index2];

        for (int i = 0; i < mum.Length; ++i)
        {
            if ((i < cp1) || (i >= cp2))
            {
                // If it's outside the crossing point, keep the original gene
                baby1[i] = mum[i];
                baby2[i] = dad[i];
            }
            else
            {
                // Cross the middle section
       
                baby1[i] = (dad[i] + mum[i]) / 2;
                baby2[i] = (dad[i] + mum[i]) / 2;
            }
        }
    }
}

public static class AAA
{
    public static double[] CloneArr(this double[] a)
    {
        double[] dl = new double[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            dl[i] = a[i];
        }
        return dl;
    }
}

