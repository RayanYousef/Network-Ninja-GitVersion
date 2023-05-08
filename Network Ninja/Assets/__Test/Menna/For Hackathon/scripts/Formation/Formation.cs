using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FormationShape
{
    StraightLine,
    Circle,
    Square,
    Cone
}

public class Formation : MonoBehaviour
{
    #region Inspector Exposed Variables
    //Required Properties!!
    public FormationShape formationShape;
    public GameObject indicator;
    public FormationAgent formationAgent;
    public GameObject spawnEffect;
  //  public AudioSource onAddSound;
    //Circle Properties
    public float radius;
    //Horizontal Line Properties
    public float lenght;
    //Cone Properties
    public float angle;
    public float slantHeight;
    //Generic Properties
    public int numberToSpawn;
    public int amountToSpawn;
    public int amountToRemove;
    #endregion

    #region Private Variables
    //Private Variables Section
    private List<GameObject> indicatorsList = new List<GameObject>();
    [SerializeField] private List<GameObject> agentsList = new List<GameObject>();
    private IShape currentFormationShape;

    private CircleFormation circleFormation;
    private HorizontalLineFormation horizontalLineFormation;
    private SquareFormation squareFormation;
    private ConeFormation coneFormation;

    private bool formShape;

    public List<GameObject> AgentsList { get => agentsList; set => agentsList = value; }
    public List<GameObject> IndicatorsList { get => indicatorsList; set => indicatorsList = value; }
    public List<GameObject> AgentsList1 { get => agentsList; set => agentsList = value; }
    #endregion

    #region Unity Defined Functions
    void Start()
    {
        ////Circle
        //circleFormation = new CircleFormation();
        //circleFormation.radius = radius;

        ////Horizontal Line formation
        //horizontalLineFormation = new HorizontalLineFormation();
        //horizontalLineFormation.length = lenght;

        //Square
        squareFormation = new SquareFormation();
        squareFormation.sideLength = lenght;

        ////Cone
        //coneFormation = new ConeFormation();
        //coneFormation.angle = angle;

        //Current Shape
        switch (formationShape)
        {
            case FormationShape.StraightLine:
                currentFormationShape = horizontalLineFormation;
                break;
            case FormationShape.Circle:
                currentFormationShape = circleFormation;
                break;
            case FormationShape.Square:
                currentFormationShape = squareFormation;
                break;
            case FormationShape.Cone:
                currentFormationShape = coneFormation;
                break;
            default:
                break;
        }
        
        //SpawnFormationPointsAndAgents(numberToSpawn);
    }

    void Update()
    {
        if (formShape)
        {
            currentFormationShape.Form(this.transform, indicatorsList);
            switch (formationShape)
            {
                case FormationShape.StraightLine:
                    currentFormationShape = horizontalLineFormation;
                    horizontalLineFormation.length = lenght;
                    break;
                case FormationShape.Circle:
                    currentFormationShape = circleFormation;
                    circleFormation.radius = radius;
                    break;
                case FormationShape.Square:
                    currentFormationShape = squareFormation;
                    squareFormation.sideLength = lenght;
                    break;
                case FormationShape.Cone:
                    currentFormationShape = coneFormation;
                    coneFormation.angle = angle;
                    coneFormation.slantHeight = slantHeight;
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region Custom Functions
    //Spawning formation agents and formation transforms - where agents are assigned each transform!!
    public void SpawnFormationPointsAndAgents(int number)
    {
        numberToSpawn = number;
        for (int i = 0; i < number; i++)
        {
            GameObject go = GameObject.Instantiate(indicator, Vector3.zero, Quaternion.identity);
            go.transform.parent = this.transform;
            FormationAgent agent = Instantiate(formationAgent, transform.position, Quaternion.identity);
            agent.transform.parent = this.transform;
            agent.toFollow = go.transform;
            indicatorsList.Add(go);
            agentsList.Add(agent.gameObject);
        }

        formShape = true;
    }

    //Call this function if you want to add a formation agent into your existing formation!!
    public void AddFormationAgent(int count)
    {
        if (spawnEffect != null)
            Instantiate(spawnEffect, transform.position, Quaternion.identity);

        //if (onAddSound != null)
        //    onAddSound.Play();

        count = currentFormationShape.AmountToExpand(count);

        numberToSpawn += count;
        formShape = false;
        SpawnFormationPointsAndAgents(count);
    }

    //Call this function if you want to remove an agent in your existing formation
    //public void RemoveFormationAgent(int count)
    //{
    //    formShape = false;

    //    //if (onAddSound != null)
    //    //    onAddSound.Play();

    //    count = currentFormationShape.AmountToExpand(count);

    //    for (int i = 0; i < count; i++)
    //    {
    //        //onAddSound.Play();
    //        GameObject toRemove = indicatorsList[i];
    //        GameObject toRemoveAgent = agentsList[i];
    //        toRemove.SetActive(false);
    //        toRemoveAgent.SetActive(false);

    //        indicatorsList.RemoveAt(i);
    //        agentsList.RemoveAt(i);

    //        if (spawnEffect != null)
    //            Instantiate(spawnEffect, toRemoveAgent.transform.position, Quaternion.identity);        
    //    }

    //    formShape = true;
    //}

    public void RemoveFormationAgent()
    {
        if (indicatorsList.Count > 0 && agentsList.Count > 0)
        {
            //indicatorsList[0].SetActive(false);
            //agentsList[0].SetActive(false);

            Destroy(indicatorsList[0]);
            Destroy(agentsList[0]);

            if (spawnEffect != null)
               Instantiate(spawnEffect, agentsList[0].transform.position, Quaternion.identity);  
            
            indicatorsList.RemoveAt(0);
            agentsList.RemoveAt(0);
        }

    }

    public void clearList()
    {
        if(indicatorsList.Count != 0)
        {
            foreach (GameObject go in indicatorsList)
            {
                Destroy(go);
            }
        }
        if(agentsList.Count != 0)
        {
            foreach (GameObject go in agentsList)
            {
                if (spawnEffect != null)
                    Instantiate(spawnEffect, go.transform.position, Quaternion.identity);
                Destroy(go);
            }
        }
        indicatorsList.Clear();
        agentsList.Clear();
    }
    #endregion
}
