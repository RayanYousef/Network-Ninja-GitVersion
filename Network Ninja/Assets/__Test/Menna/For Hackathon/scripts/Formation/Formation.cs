using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Formation : MonoBehaviour
{
    #region Inspector Exposed Variables
    //private GameObject spawnEffect;

    public float sideLength;
    #endregion

    #region Private Variables
    [SerializeField] private List<FormationAgent> agentsList = new List<FormationAgent>();
    [SerializeField] private List<Vector3> indicatorsList = new List<Vector3>();
    private int indicatorListCount = 0;

    public List<FormationAgent> AgentsList { get => agentsList; set => agentsList = value; }
    public List<Vector3> IndicatorsList { get => indicatorsList; set => indicatorsList = value; }
    #endregion

    #region Unity Defined Functions
    void Start()
    {
        GetFormationPointsAndAgents(agentsList.Count, this.transform);
        Form(this.transform, indicatorsList);
    }
    #endregion

    #region Custom Functions


    //public void SpawnFormationPointsAndAgents(int number, Transform spawnPos)
    //{
    //    numberToSpawn = number;
    //    for (int i = 0; i < number; i++)
    //    {
    //        Vector3 go = spawnPos.position;
    //        FormationAgent agent = Instantiate(formationAgent, spawnPos.position, Quaternion.identity);
    //        agent.transform.parent = spawnPos.transform;
    //        NavMeshHit hit;
    //        if (NavMesh.SamplePosition(go, out hit, 1.0f, NavMesh.AllAreas))
    //        {
    //            indicatorsList.Add(hit.position);
    //            agent.toFollow = indicatorsList[i];
    //            agent.toFollowIndex = i;

    //            agentsList.Add(agent);
    //        }
    //    }
    //}

    public void GetFormationPointsAndAgents(int number, Transform centerPos)
    {
        for (int i = 0; i < number; i++)
        {
            FormationAgent agent = AgentsList[i];
            NavMeshHit hit;
            if (NavMesh.SamplePosition(centerPos.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                indicatorsList.Add(hit.position);
                agent.toFollow = indicatorsList[i];
                agent.toFollowIndex = i;
            }
        }
    }

    public void Form(Transform objTransform, List<Vector3> indicatorsList)
    {
        indicatorListCount = indicatorsList.Count;
        int numberOnEachLine = (int)Mathf.Sqrt(indicatorsList.Count);
        int indicatorListIndex = 0;

        for (int j = 0; j < numberOnEachLine; j++)
        {
            Vector3 spawnPosition = objTransform.position + (objTransform.right * j * sideLength);
            int lengthMultiplePositive = 1;
            int lengthMultipleNegative = 1;

            indicatorsList[indicatorListIndex] = spawnPosition;
            indicatorListIndex += 1;

            for (int i = 1; i < numberOnEachLine; i++)
            {
                int sign = 1;
                if (numberOnEachLine % i == 0)
                {
                    sign = -1;

                    spawnPosition = (sign * objTransform.forward * sideLength * lengthMultiplePositive) + ((objTransform.right * j * sideLength));
                    indicatorsList[indicatorListIndex] = spawnPosition + objTransform.position;
                    lengthMultiplePositive += 1;
                }
                else
                {
                    spawnPosition = (sign * objTransform.forward * sideLength * lengthMultipleNegative) + ((objTransform.right * j * sideLength));
                    indicatorsList[indicatorListIndex] = spawnPosition + objTransform.position;
                    lengthMultipleNegative += 1;
                }

                indicatorListIndex += 1;
            }
        }
    }


    //public void RemoveFormationAgent()
    //{ 
    //    Destroy(agentsList[0]);

    //    if (spawnEffect != null)
    //        Instantiate(spawnEffect, agentsList[0].transform.position, Quaternion.identity);  
            
    //    indicatorsList.RemoveAt(0);
    //    agentsList.RemoveAt(0);
    //}

    //public void clearList()
    //{
    //    if(agentsList.Count != 0)
    //    {
    //        foreach (FormationAgent go in agentsList)
    //        {
    //            if (spawnEffect != null)
    //                Instantiate(spawnEffect, go.transform.position, Quaternion.identity);
    //            Destroy(go);
    //        }
    //    }
    //    indicatorsList.Clear();
    //    agentsList.Clear();
    //}
    #endregion
}
