using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Car : Agentx
{
    public float engine;
    public float angle;
    public WheelCollider[] wcArr;
    public Transform[] cArr;
    public Material deng;
    public Rigidbody rb;
    public Vector3 center;
    public float curSpeed;
    public GameObject trail;
    private GameObject t1;
    private GameObject t2;

    public List<LineRenderer> lineList = new List<LineRenderer>();
    public int num;
    public float perAngle;
    public float seedis;
    public Material mat;

    public float moveTime;
    public float moveDis;
    public float fit;
    public int nextIndex;
    public int jianceNum;
    public List<Transform> jianceList = new List<Transform>();
    public List<float> jianceDis = new List<float>();

    public float lowSpeedTime;
    private float t;
    public double[] ops;
    public AudioSource[] asrArr;
    public AudioClip[] acArr;
    public int quanshu;

    public double Discriminative;
    public double Dfit;
    public int fact;
    private double total;
    public double Loss;
    public int n;
    public double wrongD;
    public float dt;

    public double waits;
    public bool waitok;

    public double[] zxc;
    public double[] zxctemp;

    public double[] zzz;
    public double MLtestDiscriminative;
    private Rigidbody ml;



    // Use this for initialization
    void Start()
    {
        waitok = true;
        AudioListener.pause = false;
        asrArr = GetComponents<AudioSource>();
        mat = new Material(Shader.Find("Legacy Shaders/Self-Illumin/Diffuse"));

        while (true)
        {
            Vector3 vc = new Vector3(Random.value, Random.value, Random.value);
            float a = Vector3.Angle(vc, Vector3.one);
            float d = Mathf.Sin(a / 180 * Mathf.PI) * vc.magnitude;
            if (d > 0.8f)
            {
                mat.color = new Color(vc.x, vc.y, vc.z);
                break;
            }
        }
        //mat.color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        deng.shader = Shader.Find("Legacy Shaders/Self-Illumin/Diffuse");
        rb.centerOfMass = center;
        for (int i = 0; i < num; i++)
        {
            LineRenderer lr = new GameObject("line" + i).AddComponent<LineRenderer>();
            lr.transform.SetParent(transform);
            lr.transform.localPosition = Vector3.zero;
            lr.transform.localRotation = Quaternion.identity;
            lr.SetWidth(0.06f, 0.06f);
            lr.material = mat;
            lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lr.receiveShadows = false;
            lineList.Add(lr);
        }


        //Monitoring point

        Transform[] trArr = FindObjectsOfType<Transform>();
        for (int i = 0; i < jianceNum; i++)
        {
            for (int j = 0; j < trArr.Length; j++)
            {
                if (trArr[j].name == i.ToString() && trArr[j].gameObject.layer == 10)
                {
                    
                    jianceList.Add(trArr[j]);
                    if (i == 0)
                    {
                        jianceDis.Add(0);
                    }
                    else
                    {
                        jianceDis.Add(Vector3.Distance(jianceList[i - 1].position, jianceList[i].position));
                    }
                    break;
                }
            }
        }
        jianceDis[0] = Vector3.Distance(jianceList[0].position, jianceList[jianceNum - 1].position);

        InvokeRepeating("DSetFitness", 1, 1);//1s dis..1time
        

    }

    // Update is called once per frame
    void Update()
    {
        if (waitok == false)
        {
            waits += Time.deltaTime;
        }
        if (waits >= 1.0f)
        {
            gameObject.SetActive(false);
        }

        moveTime += Time.deltaTime;
        CheckHuaXing();
        curSpeed = rb.velocity.magnitude;
        SetWc();

        foreach (var item in lineList)
        {
            item.gameObject.SetActive(isDraw);
        }

        //base.Update();

    }


    // WheelCollider wcArr
    // Transform cArr
    // AudioSource asrArr
    // AudioClip acArr
    public void Move(float f)
    {
        wcArr[2].motorTorque = f * engine;
        wcArr[3].motorTorque = f * engine;
        //if (Mathf.Abs(f) > 0.5f)
        //{
        //    if (f > 0)
        //    {
        //        wcArr[2].motorTorque = 1 * engine;
        //        wcArr[3].motorTorque = 1 * engine;
        //    }
        //    else
        //    {
        //        wcArr[2].motorTorque = -1 * engine;
        //        wcArr[3].motorTorque = -1 * engine;
        //    }
        //}
        if (curSpeed / 30f < 0.3f)
        {
            asrArr[1].clip = acArr[0];
        }
        else if (curSpeed / 30f < 0.6f)
        {
            asrArr[1].clip = acArr[1];
        }
        else if (curSpeed / 30f < 2f)
        {
            asrArr[1].clip = acArr[2];
        }
        if (!asrArr[1].isPlaying)
        {
            asrArr[1].Play();
        }
    }

    public void Turn(float f)
    {
        //if (Mathf.Abs(f) > 0.3f)
        {
            wcArr[0].steerAngle = f * angle;
            wcArr[1].steerAngle = f * angle;
        }
    }

    private void SetWc()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos;
            Quaternion qua;
            wcArr[i].GetWorldPose(out pos, out qua);
            cArr[i].position = pos;
            cArr[i].rotation = qua;
        }
    }

    public void Brake(float f)
    {
        if (f > 0)
        {
            wcArr[2].brakeTorque = engine * 2 * 1;
            wcArr[3].brakeTorque = engine * 2 * 1;
            deng.color = Color.red;
        }
        else
        {
            wcArr[2].brakeTorque = 0;
            wcArr[3].brakeTorque = 0;
            deng.color = new Color(0.1f, 0, 0);
        }
    }

    public void HuaXing()
    {
        if (t1 == null)
        {
            t1 = Instantiate(trail, wcArr[2].transform.position, trail.transform.rotation);
            t2 = Instantiate(trail, wcArr[3].transform.position, trail.transform.rotation);
            t1.transform.SetParent(wcArr[2].transform);
            t2.transform.SetParent(wcArr[3].transform);
            t1.transform.localPosition = new Vector3(0, -0.40f, 0);
            t2.transform.localPosition = new Vector3(0, -0.40f, 0);
            t1.GetComponent<TrailRenderer>().material.color = mat.color;
            t2.GetComponent<TrailRenderer>().material.color = mat.color;
        }
    }

    public void TingZhiHuaXing()
    {
        if (t1 != null)
        {
            t1.transform.SetParent(null);
            t2.transform.SetParent(null);
            t1 = null;
            t2 = null;
        }
    }

    public void CheckHuaXing()
    {
        float an = Vector3.Angle(rb.velocity, transform.forward);
        if (an > 10)// || wcArr[2].brakeTorque > 0)// Mathf.Abs(wcArr[2].rpm / 30 * wcArr[2].radius * Mathf.PI * transform.localScale.x - curSpeed * Mathf.Cos(an / 180 * Mathf.PI)) > 7f)
        {
            HuaXing();
        }
        else
        {
            TingZhiHuaXing();
        }
    }

    public override double[] GetInputs()
    {
        List<double> list = new List<double>();
        Vector3 vf = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        for (int i = 1; i <= (num - 1) / 2; i++)
        {
            lineList[i].SetPosition(0, transform.position);
            lineList[num - i].SetPosition(0, transform.position);
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Quaternion.AngleAxis(-i * perAngle, Vector3.up) * vf);
            if (Physics.Raycast(ray, out hit, seedis, 1 << 8))
            {
                list.Add(Mathf.Pow(1 - hit.distance / seedis, 1));
                lineList[i].SetPosition(1, hit.point);
            }
            else
            {
                list.Add(0);
                lineList[i].SetPosition(1, transform.position + ray.direction * seedis);
            }

            RaycastHit hit2;
            Ray ray2 = new Ray(transform.position, Quaternion.AngleAxis(i * perAngle, Vector3.up) * vf);
            if (Physics.Raycast(ray2, out hit2, seedis, 1 << 8))
            {
                list.Add(Mathf.Pow(1 - hit2.distance / seedis, 1));
                lineList[num - i].SetPosition(1, hit2.point);
            }
            else
            {
                list.Add(0);
                lineList[num - i].SetPosition(1, transform.position + ray2.direction * seedis);
            }
        }
        lineList[0].SetPosition(0, transform.position);
        RaycastHit hit3;
        Ray ray3 = new Ray(transform.position, vf);
        if (Physics.Raycast(ray3, out hit3, seedis, 1 << 8))
        {
            list.Add(Mathf.Pow(1 - hit3.distance / seedis, 1));
            lineList[0].SetPosition(1, hit3.point);
        }
        else
        {
            list.Add(0);
            lineList[0].SetPosition(1, transform.position + ray3.direction * seedis);
        }

        list.Add(Mathf.Pow(curSpeed / 40f, 1));
        //  Debug.Log("list" + list.Count);

        zxc = list.ToArray();

        return list.ToArray();
    }

    public override double[] DGetInputs(double[] outputs)
    {
        List<double> list = new List<double>();

        double[] inputs = GetInputs();

        if (inputs != null)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                list.Add(inputs[i]);
            }
        }

        if (outputs != null)
        {
            for (int i = 0; i < outputs.Length; i++)
            {
                list.Add(outputs[i]);
            }
        }

        return list.ToArray();
    }

    public override void UseOutputs(double[] outputs)
    {
        ops = outputs;
        Move((float)outputs[0]);
        Turn((float)outputs[1]);
        Brake((float)outputs[2]);
    }

    public override void DUseOutputs(double[] outputs)
    {
        Discriminative = outputs[0];
        // Debug.Log("This is D" + Discriminative);
    }
    /// <summary>
    /// test code
    /// </summary>
    public override void SetFitness()
    {

        float f = moveDis + jianceDis[nextIndex] - Vector3.Distance(transform.position, jianceList[nextIndex].position);
        //f *= 1000;
        if (f > fit)
        {
            t = 0;
            fit = f;
        }
        else
        {
            t += Time.deltaTime;
            if (t > lowSpeedTime)
            {
                gameObject.SetActive(false);
            }
        }
        ge.fitness = fit;

    }

    public override void DSetFitness()
    {
        bool lol;
        lol = gameObject.activeSelf;
        double tempDiscriminative;

        if (lol == true)
        {
            fact = 1;
        }
        else
        {
            fact = 0;
        }
        if (lol == true)
        {
            n++;
            tempDiscriminative = Math.Round(Discriminative, 5);
            tempDiscriminative = Sigmoid(tempDiscriminative);
            total += fact * Math.Log(tempDiscriminative) + (1 - fact) * Math.Log(1 - tempDiscriminative);

            if (tempDiscriminative <= 0.5)
            {
                int i = 0;

                foreach (var item in zxctemp)
                {
                    if (item > 0.8)
                    {
                        i++;
                    }
                   // if (i >= 2)
                   // {
                         Debug.Log("maybe hit:  "+ gameObject.name);
                    //}
                }

                dt++;
                /*
                if (dt > wrongD)
                {
                    Dfit -= 5;
                    Dge.fitness = Dfit;
                    gameObject.SetActive(false);
                    
                }
                */
            }
        }
        if (n != 0 && lol == true)
        {
            Loss = -total / n;
            Loss = Math.Round(Loss, 5);
            Dfit = 10 - Loss;
            Dge.fitness = Dfit;
        }
        zxctemp = zxc;
    }

    private double Sigmoid(double s)
    {
        return (1 / (1 + Math.Exp(-s)));
    }

    public override void ResetAgent()
    {
        quanshu = 0;
        moveTime = 0;
        t = 0;
        nextIndex = 0;
        moveDis = 0;
        fit = 0;
        ge.fitness = 0;
        transform.position = Vector3.up * 0.9f;
        transform.rotation = Quaternion.identity;
        waits = 0;
        waitok = true;
    }

    public override void ResetD()
    {
        Discriminative = 0;
        Dfit = 0;
        Loss = 0;
        n = 0;
        total = 0;
        Dge.fitness = 0;
    }


    public override void Draw()
    {
        //base.Draw();
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == 8 || c.gameObject.layer == 11)
        {

            double tempD;
            tempD = Math.Round(Discriminative, 5);
            tempD = Sigmoid(tempD);

            if (tempD >= 0.5)
            {
                Dfit -= 5;
                //Debug.Log("Bad &&&");
            }
            else
            {
                n--;
                foreach (var item in zxctemp)
                {
                    if (item > 0.8)
                    {
                        Dfit += 10;
                        //Debug.Log("Great ***");
                    }
                    else
                    {
                        Dfit += 5;
                        //Debug.Log("Good ");
                    }
                }
            }
            waitok = false;
            Dge.fitness = Dfit;
        }
    }


    private void OnDisable()
    {
        if (quanshu >= 2)
        {
            fit += (800000 - (System.DateTime.Now - Evolution1.startTime).Ticks / 10000) / 100;
            ge.fitness = fit;
        }
    }

    public override void InitializeAgent()
    {
        ml = gameObject.GetComponent<Rigidbody>();
    }

    public override void AgentReset()
    {

        MLtestDiscriminative = 0;
       
    }

    public override void CollectObservations()
    {
       
        AddVectorObs((float)zxc[0]);
        AddVectorObs((float)zxc[1]);
        AddVectorObs((float)zxc[2]);
        AddVectorObs((float)zxc[3]);
        AddVectorObs((float)zxc[4]);

        AddVectorObs(curSpeed);

        AddVectorObs((float)ops[0]);
        AddVectorObs((float)ops[1]);
        AddVectorObs((float)ops[2]);

    }
 
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        bool lol;
        lol = gameObject.activeSelf;

        MLtestDiscriminative = Mathf.Clamp(vectorAction[0], 0f, 1f);
       
        if (MLtestDiscriminative < 0.5 && MLtestDiscriminative >= 0 && lol)
        {
            Debug.Log("Attention"+ gameObject.name +": " + Math.Round(MLtestDiscriminative, 3));
        }
       // else if(MLtestDiscriminative != -1)
       // {
          //  Debug.Log("Good");
       // }
        
        if (!lol)
        {
            MLtestDiscriminative = -1;
        }
       
        if (MLtestDiscriminative <= 0.5 && MLtestDiscriminative>=0 && fact == 1)
        {
            SetReward(-1.0f);
            //Done();
        }

        else if (MLtestDiscriminative > 0.5 && fact == 1)
        {
            SetReward(0.1f);
                    
        }

        else if (MLtestDiscriminative <= 0.5 && MLtestDiscriminative >= 0 && fact == 0)
        {
            SetReward(0.1f);
                   
        }

        else if (MLtestDiscriminative > 0.5 && fact == 0)
        {
            SetReward(-1.0f);
           // Done();
        } 
    }
}
