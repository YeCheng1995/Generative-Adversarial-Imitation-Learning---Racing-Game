using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;

public class Evolution1 : Academy
{
    public static System.DateTime startTime;
    public GameObject agentPrefab;
    public Transform agentsParent;
    public int populationSize;

    public int[] layerShape;
    public int[] DlayerShape;

    public string populationName;
    public string DpopulationName;

    private GA ga;
    private List<Genome> genomeList = new List<Genome>();
    private List<Genome> DgenomeList = new List<Genome>();
    private List<NeuralNetwork> neuralNetworkList = new List<NeuralNetwork>();
    private List<NeuralNetwork> DneuralNetworkList = new List<NeuralNetwork>();
    protected List<Agentx> agentList = new List<Agentx>();

    public bool loadWeights;
    public bool DloadWeights;
    public bool justRunTest;
    public TextAsset bestTextAsset;

    public GameObject resultpanel;
    public Text resulttext;
    public double TotalTime;
    public double PassTime;

    public override void AcademyReset()
    {
        
    }

    public override void AcademyStep()
    {
       
    }
    void Start()
    {
        CheckName();
        ResetEnvironmental();
        Init();
    }

    protected void Update()
    {
        Run();
    }

    void CheckName()
    {
        //Determine whether the string is empty
        if (string.IsNullOrEmpty(DpopulationName))
        {
            throw new System.Exception("Name cannot be null！");
        }
        if (FindObjectsOfType<Evolution1>().ToList().Find(a => a.DpopulationName == DpopulationName) != this)
        {
            throw new System.Exception("Name has been used try another:" + DpopulationName);
        }

        if (string.IsNullOrEmpty(populationName))
        {
            throw new System.Exception("Name cannot be null！");
        }
        if (FindObjectsOfType<Evolution1>().ToList().Find(a => a.populationName == populationName) != this)
        {
            throw new System.Exception("Name has been used try another:" + populationName);
        }

    }

    void SaveD(double[] best, double score)
    {
        string str = "";
        for (int i = 0; i < best.Length; i++)
        {
            str += best[i] + (i == best.Length - 1 ? "" : ",");
        }
        StreamWriter sw = new StreamWriter(Path.Combine(Application.streamingAssetsPath, DpopulationName + ".txt"));
        sw.WriteLine(str + ":" + score);
        sw.Close();
        PlayerPrefs.SetString(DpopulationName, str);
        PlayerPrefs.Save();
    }
    void SaveBest(double[] best, double score)
    {
        string str = "";
        for (int i = 0; i < best.Length; i++)
        {
            str += best[i] + (i == best.Length - 1 ? "" : ",");
        }
        StreamWriter sw = new StreamWriter(Path.Combine(Application.streamingAssetsPath, populationName + ".txt"));
        sw.WriteLine(str + ":" + score);
        sw.Close();
        PlayerPrefs.SetString(populationName, str);
        PlayerPrefs.Save();
    }


    void Init() 
    {
       
        ga = new GA(populationSize);
        for (int i = 0; i < populationSize; i++)
        {
           
            NeuralNetwork nn = new NeuralNetwork(layerShape);
            NeuralNetwork Dnn = new NeuralNetwork(DlayerShape);
            //if load
            if (loadWeights)
            {
                if (File.Exists(Path.Combine(Application.streamingAssetsPath, populationName + ".txt"))) //open file
                {
                    StreamReader sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, populationName + ".txt"));
                    string temp = sr.ReadLine().Split(':')[0];
                    sr.Close();
                    string[] strArr = temp.Split(',');
                    double[] dw = new double[nn.weightNum];
                    for (int z = 0; z < nn.weightNum; z++)
                    {
                        dw[z] = double.Parse(strArr[z]);
                    }
                    if (i == 0)
                    {
                        nn.SetWeights(dw);
                    }
                    else
                    {
                        //every car is different, change one weight
                        int index = UnityEngine.Random.Range(0, dw.Length);
                        dw[index] = UnityEngine.Random.Range(-1f, 1f);
                        nn.SetWeights(dw);
                    }
                }
                else
                {
                    Debug.LogError("Did not save best weight. Cannot upload");
                }
            }
            else
            {
                nn.RandomWeights();
            }

            if (DloadWeights)
            {
                if (File.Exists(Path.Combine(Application.streamingAssetsPath, DpopulationName + ".txt")))
                {
                    StreamReader sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, DpopulationName + ".txt"));
                    string temp = sr.ReadLine().Split(':')[0];
                    sr.Close();
                    string[] strArr = temp.Split(',');
                    double[] dw = new double[Dnn.weightNum];
                    for (int z = 0; z < Dnn.weightNum; z++)
                    {
                        dw[z] = double.Parse(strArr[z]);
                    }
                    if (i == 0)
                    {
                        Dnn.SetWeights(dw);
                    }
                    else
                    {
                        int index = UnityEngine.Random.Range(0, dw.Length);
                        dw[index] = UnityEngine.Random.Range(-1f, 1f);
                        Dnn.SetWeights(dw);
                    }
                }
                else
                {
                    Debug.LogError("Did not save best weight. Cannot upload");
                }
            }
            else
            {
                Dnn.RandomWeights();
            }

            Genome ge = new Genome(nn.GetWeights(), 0, nn.splitPoints);
            Genome Dge = new Genome(Dnn.GetWeights(), 0, Dnn.splitPoints);

            Agentx ac = (Instantiate(agentPrefab, GetStartPos(), Quaternion.identity, agentsParent) as GameObject).GetComponent<Agentx>();
            ac.name = agentPrefab.name + i;
            ac.SetInfo(nn, ge);
            ac.DSetInfo(Dnn, Dge);

            neuralNetworkList.Add(nn);
            DneuralNetworkList.Add(Dnn);

            genomeList.Add(ge);
            DgenomeList.Add(Dge);

            agentList.Add(ac);
        }
    }

   

    void Run()
    {
        if (!justRunTest)
        {
            if (CheckGenerationEnd())
            {
                FinishDiscriminative();
                FinishGeneration();
                ResetEnvironmental();
            }

            PassTime += Time.deltaTime;

            if (PassTime >= TotalTime)
            {
                PassTime = 0;
                StopAll();
            }
        }
        else
        {
            if (CheckGenerationEnd())
            {
                resultpanel.SetActive(true);
                resulttext.text = "You Win\n All AI Was Damaged";
                Time.timeScale = 0;
                AudioListener.pause = true;
            }
        }   
    }

    public void StopAll()
    {
        foreach (var item in agentList)
        {
            item.gameObject.SetActive(false);
        }
    }

    void FinishGeneration()                                                   
    {
        double[] best;
        double score;
        List<double[]> weightsList = ga.Run(genomeList, out best, out score);
        SaveBest(best, score);
         //Debug.Log("This is debug"+best.Length);
       // Debug.Log("weightsList" + weightsList.Count);

        for (int i = 0; i < weightsList.Count; i++)
        {
            neuralNetworkList[i].SetWeights(weightsList[i]);
            agentList[i].nn = neuralNetworkList[i];
            genomeList[i] = new Genome(neuralNetworkList[i].GetWeights(), 0, neuralNetworkList[i].splitPoints);
            agentList[i].ge = genomeList[i];
            agentList[i].ResetAgent();
            agentList[i].gameObject.SetActive(true);
        }
    }

    void FinishDiscriminative()
    {
        double[] best;
        double score;

        List<double[]> DweightsList = ga.Run(DgenomeList, out best, out score);
       // Debug.Log("DweightsList" + DweightsList.Count);
        SaveD(best, score);

        for (int i = 0; i < DweightsList.Count; i++)
        {
            DneuralNetworkList[i].SetWeights(DweightsList[i]);
            agentList[i].Dnn = DneuralNetworkList[i];
            DgenomeList[i] = new Genome(DneuralNetworkList[i].GetWeights(), 0, DneuralNetworkList[i].splitPoints);
            agentList[i].Dge = DgenomeList[i];
            agentList[i].ResetD();

        }
    }


    public virtual Vector3 GetStartPos()
    {
        return Vector3.zero;
    }

    public virtual bool CheckGenerationEnd()
    {
        bool isEnd = true;
        foreach (var t in agentList)
        {
            if (t.gameObject.activeSelf)
            {
                isEnd = false;
                break;
            }
        }
        return isEnd;
    }
    /// <summary>
    /// Tire scratch
    /// </summary>
    public virtual void ResetEnvironmental()
    {
        startTime = System.DateTime.Now;
        foreach (var item in GameObject.FindGameObjectsWithTag("Trail"))
        {
            Destroy(item);
        }
      
    }
}
