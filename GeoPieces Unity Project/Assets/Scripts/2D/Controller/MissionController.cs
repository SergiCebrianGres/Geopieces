using UnityEngine;
using System.Collections.Generic;
using System;

public class MissionController : MonoBehaviour{


    private List<Mission2D> missions;
    private List<GameObject> highlightedObjects;
    public Mission2D currentMission;
    public Competencies currentComp;

    //flags
    private bool playing;

    //others
    private float time;
    private float semaphoreTime;
    private float showNewSemaphore;
    private int numSelectedObjects;
    
    private int numCorrectFigures;
    private int numNoSelectedCorrectFigures;
    private int numErrors;

    private bool discover;

    private int numberOfCompetencies = 4;

    private int attempts = 0;
    public Competencies comp;

    public GameObject cube;
    public GameObject bomb;
    public GameObject redbomb;
    public Transform container;
    private List<GameObject> possibleFigures;
    private List<GameObject> strangeFigures;
    private List<GameObject> figuresOnScene;
    private List<GameObject> areaAndPerimeterFigures;
    private List<GameObject> possibleCorrectFiguresCurrentMission;
    private List<GameObject> possibleIncorrectFiguresCurrentMission;

    private List<Figures2D> quadrilateral;
    private List<Figures2D> triangle;

    private List<ObjectiveID> symmetryObjectives;
    private List<ObjectiveID> anglesObjectives;

    public Dictionary<ObjectiveID, string> objectivesDescriptions;
    public Dictionary<ObjectiveID, string> objDescriptions;

    //Material
    public Material highlightMaterial;
    public Material defaultMaterial;
    public Material errorMaterial;

    private Color defaultColor, highlightedColor, errorColor, correctColor;

    public int idMission = 0;

    private int dimx, dimz;

    public System.Random ran;



    #region Unity Start / Update
    // Use this for initialization
    void Awake () {
        ran = new System.Random();
        missions = new List<Mission2D>();
        highlightedObjects = new List<GameObject>();
        figuresOnScene = new List<GameObject>();

        initializeAllFigures();

        initializeColors();
        //initializePossibleFigures();
        initializeObjectives2D();
        initializeFiguresOrder();
        objDescription();
    }

    private void initializeAllFigures()
    {
        possibleFigures = new List<GameObject>();
        strangeFigures = new List<GameObject>();
        areaAndPerimeterFigures = new List<GameObject>();

        loadFigures(possibleFigures, "quadrilateral");
        loadFigures(possibleFigures, "triangle");
        loadFigures(possibleFigures, "pentagon");
        loadFigures(strangeFigures, "otherfigures");
        loadFigures(areaAndPerimeterFigures, "areaAndPerimeter");
    }

    private void loadFigures(List<GameObject> temporal, string str)
    {
        GameObject[] aux;
        aux = Resources.LoadAll<GameObject>("Figures2D/" + str);

        foreach (GameObject q in aux)
        {
            GameObject lo = q;
            temporal.Add(lo);
        }
    }

    void Start()
    {
        
    }

    private void initializeFiguresOrder()
    {
        quadrilateral = new List<Figures2D>();
        triangle = new List<Figures2D>();

        quadrilateral.Add(Figures2D.Square);
        quadrilateral.Add(Figures2D.Rectangle);
        quadrilateral.Add(Figures2D.Rhombus);
        quadrilateral.Add(Figures2D.Trapezium);

        triangle.Add(Figures2D.EquilateralTriangle);
        triangle.Add(Figures2D.IsoscelesTriangle);
        triangle.Add(Figures2D.RectangleTriangle);
        triangle.Add(Figures2D.ScaleneTriangle);

        symmetryObjectives = new List<ObjectiveID>();
        symmetryObjectives.Add(ObjectiveID.findReflectionalSymmetry);
        symmetryObjectives.Add(ObjectiveID.findRotationalSymmetry);
        symmetryObjectives.Add(ObjectiveID.findSymmetry);
        symmetryObjectives.Add(ObjectiveID.findNoSymmetry);

        anglesObjectives = new List<ObjectiveID>();
        anglesObjectives.Add(ObjectiveID.findRegularPolygon);
        anglesObjectives.Add(ObjectiveID.findFiguresWithRightAngle);
        anglesObjectives.Add(ObjectiveID.findFiguresWithAcuteAngle);
        anglesObjectives.Add(ObjectiveID.findFiguresWithObtuseAngle);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Unity Initialize
    private void initializeObjectives2D()
    {
        objectivesDescriptions = new Dictionary<ObjectiveID, string>();

        objectivesDescriptions.Add(ObjectiveID.findTriangles, "Find all the triangles");
        objectivesDescriptions.Add(ObjectiveID.findEquilateralTriangle, "Find all equilateral triangles");
        objectivesDescriptions.Add(ObjectiveID.findIsoscelesTriangle, "Find all isosceles triangles");
        objectivesDescriptions.Add(ObjectiveID.findRectangleTriangle, "Find rectangles triangles");
        objectivesDescriptions.Add(ObjectiveID.findScaleneTriangle, "Find all scalene triangles");
        objectivesDescriptions.Add(ObjectiveID.findSquares, "Find all squares");
        objectivesDescriptions.Add(ObjectiveID.findRectangles, "Find all rectangles");
        objectivesDescriptions.Add(ObjectiveID.findRhombus, "Find all rhombus");
        objectivesDescriptions.Add(ObjectiveID.findTrapeziums, "Find all Trapeziums");
        objectivesDescriptions.Add(ObjectiveID.findParallelogrms, "Find all parallelograms");
        objectivesDescriptions.Add(ObjectiveID.findQuadrilaters, "Find all quadrilaters");
        objectivesDescriptions.Add(ObjectiveID.findPentagon, "Find all pentagons");
        objectivesDescriptions.Add(ObjectiveID.findRegularPentagon, "Find all regular pentagons");
        objectivesDescriptions.Add(ObjectiveID.findIrregularPentagon, "Find all irregular pentagons");
        objectivesDescriptions.Add(ObjectiveID.findRegularPolygon, "Find all regular polygons");
        objectivesDescriptions.Add(ObjectiveID.findNoIntEdges, "Find all figures that have no precise number of edges");
        objectivesDescriptions.Add(ObjectiveID.findSymmetry, "Find all figures that have reflectional or rotational symmetry");
        objectivesDescriptions.Add(ObjectiveID.findReflectionalSymmetry, "Find all figures that have reflection symmetry");
        objectivesDescriptions.Add(ObjectiveID.findRotationalSymmetry, "Find all figures that have rotational symmetry");
        objectivesDescriptions.Add(ObjectiveID.findNoSymmetry, "Find all non-symmetric figures");
        objectivesDescriptions.Add(ObjectiveID.findFiguresWithRightAngle , "Find all figures that have a right angle");
        objectivesDescriptions.Add(ObjectiveID.findFiguresWithAcuteAngle, "Find all figures that have an acute angle");
        objectivesDescriptions.Add(ObjectiveID.findFiguresWithObtuseAngle, "Find all figures that have an obtuse angle");
        objectivesDescriptions.Add(ObjectiveID.findParallelograms, "Find all parallelograms");
        objectivesDescriptions.Add(ObjectiveID.findFiguresWithTwoParalelEdges, "Find all figures with two parallel edges");
        objectivesDescriptions.Add(ObjectiveID.findFiguresWithNoParalelEdges, "Find all figures with no parallel edges");
        objectivesDescriptions.Add(ObjectiveID.findFigureWithMaxArea, "Find figures with maximum area");
        objectivesDescriptions.Add(ObjectiveID.findFigureWithMinArea, "Find figures with minimum area");
        objectivesDescriptions.Add(ObjectiveID.findFigureWithMaxPerimeter, "Find figures with maximum perimeter");
        objectivesDescriptions.Add(ObjectiveID.findFigureWithMinPerimeter, "Find figures with minimum perimeter");
    }

    private void objDescription()
    {
        objDescriptions = new Dictionary<ObjectiveID, string>();

        objDescriptions.Add(ObjectiveID.findTriangles, "A triangle is a polygon with three edges.");
        objDescriptions.Add(ObjectiveID.findEquilateralTriangle, "An equilateral triangle is a triangle in which all three sides are equal.");
        objDescriptions.Add(ObjectiveID.findIsoscelesTriangle, "An isosceles triangle is a triangle that has, at least, two sides of equal length.");
        objDescriptions.Add(ObjectiveID.findRectangleTriangle, "A rectangle triangle is a triangle that has a right angle.");
        objDescriptions.Add(ObjectiveID.findScaleneTriangle, "A scalene triangle is a triangle that has no equal sides.");
        objDescriptions.Add(ObjectiveID.findSquares, "A square is a quadrilateral with four equal sides and four right angles.");
        objDescriptions.Add(ObjectiveID.findRectangles, "A rectangle is a quadrilateral with four right angles.");
        objDescriptions.Add(ObjectiveID.findRhombus, "A rhombus is a quadrilateral where all sides have equal lenght. Also opposite sides are parallel and opposite angles are equal.");
        objDescriptions.Add(ObjectiveID.findTrapeziums, "A trapezium is a quadrilateral that has, at least, a pair of parallel opposite sides.");
        objDescriptions.Add(ObjectiveID.findParallelogrms, "A parallelogram is a quadrillateral with opposite sides parallel.");
        objDescriptions.Add(ObjectiveID.findQuadrilaters, "A triangle is a polygon with four edges.");
        objDescriptions.Add(ObjectiveID.findPentagon, "A pentagon is a polygon with five edges.");
        objDescriptions.Add(ObjectiveID.findRegularPentagon, "A regular pentagon is a pentagon in with all five sides and angles are equal.");
        objDescriptions.Add(ObjectiveID.findIrregularPentagon, "A regular pentagon is a pentagon in with all five sides and angles are equal. If a pentagon is not regular, then it is irregular.");
        objDescriptions.Add(ObjectiveID.findRegularPolygon, "A polygon is regular if all sides and engles are equals.");
        objDescriptions.Add(ObjectiveID.findNoIntEdges, "No description here.");
        objDescriptions.Add(ObjectiveID.findSymmetry, "A figure has reflection symmetry if there is a line going through it which divides it into two pieces which are mirror images of each other. \n A figure has rotational symmetry if it still looks the same after a rotation of less than 360 degrees.");
        objDescriptions.Add(ObjectiveID.findReflectionalSymmetry, "A figure has reflection symmetry if there is a line going through it which divides it into two pieces which are mirror images of each other");
        objDescriptions.Add(ObjectiveID.findRotationalSymmetry, "A figure has rotational symmetry if it still looks the same after a rotation of less than 360 degrees");
        objDescriptions.Add(ObjectiveID.findNoSymmetry, "A figure has reflection symmetry if there is a line going through it which divides it into two pieces which are mirror images of each other. \n A figure has rotational symmetry if it still looks the same after a rotation of less than 360 degrees.");
        objDescriptions.Add(ObjectiveID.findFiguresWithRightAngle, "A right angle is a 90 degrees angle.");
        objDescriptions.Add(ObjectiveID.findFiguresWithAcuteAngle, "A acute angle is a an angle that is less than 90 degrees.");
        objDescriptions.Add(ObjectiveID.findFiguresWithObtuseAngle, "A acute angle is a an angle that is grater than 90 degrees but less than 180.");
        objDescriptions.Add(ObjectiveID.findFiguresWithTwoParalelEdges, "No description");
        objDescriptions.Add(ObjectiveID.findFiguresWithNoParalelEdges, "No description");
        objDescriptions.Add(ObjectiveID.findFigureWithMaxArea, "Area is the quantity that expresses the extent of a two-dimensional figure");
        objDescriptions.Add(ObjectiveID.findFigureWithMinArea, "Area is the quantity that expresses the extent of a two-dimensional figure");
        objDescriptions.Add(ObjectiveID.findFigureWithMaxPerimeter, "Perimeter is the sum of all sides of a polygon");
        objDescriptions.Add(ObjectiveID.findFigureWithMinPerimeter, "Perimeter is the sum of all sides of a polygon");
    }
    
    private void initializeColors()
    {
        defaultColor = new Color(0, 0, 176);
        highlightedColor = new Color(176,176,0);
        errorColor = new Color(176, 0, 0);
        correctColor = new Color(0, 176, 0);
    }
    internal void start2D()
    {
        playing = false;

        time = 0.0f;
        numSelectedObjects = 0;
        numErrors = 0;
    }
    #endregion

    #region Info Panel
    internal string getInfoPanel()
    {
        return objDescriptions[currentMission.objectives[0].id];
    }
    #endregion

    #region Getters and setters
    internal bool isPlaying()
    {
        return playing;
    }
    #endregion

    #region Send New Mission
    //This methods are called when button "SEND MISSION" is pressed
    public void sendNewMission(RewardType reward, User user)
    {
        destroyOldObjects();
        container.transform.position = new Vector3(0, 0, 0);

        Mission2D m = new Mission2D();
        m.id = idMission;
        m.completed = false;
        m.discover = false;

        if (GameController.Instance.currentCampaign.weightsMatter)
        {
            float[] weights = GameController.Instance.currentCampaign.weights;
            float total = 0;
            for (int i = 0; i < numberOfCompetencies; i++)
            {
                total += weights[i];
            }
            double randomNum = ran.NextDouble()*total;

            for (int i = 0; i < numberOfCompetencies; i++)
            {
                if (randomNum > weights[i]) randomNum -= weights[i];
                else
                {
                    comp = (Competencies)i;
                    break;
                }
            }
        }
        else
        {
            comp = user.selectCompetency(GameController.Instance.compForCampaign);
        }
        //comp = (Competencies)user.selectObjective();
        //comp = GetRandomEnum<Competencies>();
        //comp = Competencies.Angles;
        List<ObjectiveID> objectivesByID = getRandomObjectiveId(reward, comp);
        m.discover = discover;
        if (objectivesByID.Count == 0)
        {
            Debug.Log("No objectives assigned");
        }

        foreach (ObjectiveID objID in objectivesByID)
        {
            Objectives2D obj = new Objectives2D(objID, objectivesDescriptions[objID]);
            m.objectives.Add(obj);
        }
        
        m.lev = (Level)GameController.Instance.user.getLevel(user.competencies[(int)comp]);
        m.size = chooseSize(m.lev);
        m.seconds = 6 + ((3+(int)m.size) * ((int)m.size));

        m.maxTimeStar = m.seconds / 2;
        //Reward
        float objII = 10;
        foreach (InventoryItem ii in GameController.Instance.inv.items)
        {
            if (ii.name == reward)
            {
                objII = ii.initialGoal;
                //objII = ii.campaignGoal;
                break;
            }
        }

        m.numReward = (int)((int) (.12f * objII) + (0.5f * (int)(m.lev)));
        if (m.numReward <= 0) m.numReward = 1;
        //if (quantityII + m.numReward > objII) m.numReward = objII - quantityII;

        m.typeReward = reward;
        m.coinFixReward = (((int)m.lev)/2.0f) + 1;

        idMission++;

        missions.Add(m);

        currentComp = comp;
        currentMission = missions[0];
        GameController.Instance.user.numMissionsForCompetency[(int)currentComp] += 1;
        showMissionObjectives(false);

    }

    private List<ObjectiveID> getRandomObjectiveId(RewardType reward, Competencies comp)
    {
        List<Figures2D> temp;
        switch (comp)
        {
           
            case Competencies.Identification:
                if (((ran.Next(0, 10) < 7) || 
                    ((reward == RewardType.Triangle) && (GameController.Instance.user.trianglesFiguresSeen.Count == 0)) ||
                    ((reward == RewardType.Square) && (GameController.Instance.user.quadrilateralFiguresSeen.Count == 0))) 
                    &&
                    (((reward == RewardType.Triangle) && (GameController.Instance.user.trianglesFiguresSeen.Count < triangle.Count)) ||
                    ((reward == RewardType.Square) && (GameController.Instance.user.quadrilateralFiguresSeen.Count < quadrilateral.Count))))
                {
                    discover = true;
                    return discoverNewItem(reward);
                }
                
                else
                {
                    discover = false;
                    if (reward == RewardType.Pentagon)
                    {
                        //List<Figures2D> objectives = new List<Figures2D>();
                        return choosePentagonObjective();
                        //objectives.Add(Figures2D.RegularPentagon);
                        //return transformToListObjectiveID(objectives);
                    }
                    else
                    {
                        temp = getListFiguresSeen(reward);
                        return subsetObjectives(temp);
                    }
                    
                }


            case Competencies.Symmetry:
                if (((ran.Next(0, 10) < 7) || (GameController.Instance.user.symmetricCompetenciesSeen.Count == 0)) 
                    &&
                    (GameController.Instance.user.symmetricCompetenciesSeen.Count < symmetryObjectives.Count))
                {
                    discover = true;
                    return discoverNewSymmetryObjective();
                }
                else
                {
                    discover = false;
                    return subsetObjectivesSymmetry();
                }

            case Competencies.Angles:
                if (((ran.Next(0, 10) < 7) || (GameController.Instance.user.anglesCompetenciesSeen.Count == 0))
                    &&
                    (GameController.Instance.user.anglesCompetenciesSeen.Count < anglesObjectives.Count))
                {
                    discover = true;
                    return discoverNewAnglesObjective();
                }
                else
                {
                    discover = false;
                    return subsetObjectivesAreas();
                }

            case Competencies.AreaPerimeter:
                return getAreaPerimeterObjective();
                

            default:
                List<ObjectiveID> obj = new List<ObjectiveID>();
                obj.Add(ObjectiveID.findNoIntEdges);
                return obj;
        }
    }

    private List<ObjectiveID> choosePentagonObjective()
    {
        int random = ran.Next(0, 3);

        List<ObjectiveID> obj = new List<ObjectiveID>();

        switch (random)
        {
            case 0:
                obj.Add(ObjectiveID.findPentagon);
                return obj;
            case 1:
                obj.Add(ObjectiveID.findRegularPentagon);
                return obj;
            case 2:
                obj.Add(ObjectiveID.findIrregularPentagon);
                return obj;
            default:
                obj.Add(ObjectiveID.findPentagon);
                return obj;
        }

    }

    private List<ObjectiveID> getAreaPerimeterObjective()
    {
        int random = ran.Next(0, 4);

        List<ObjectiveID> obj = new List<ObjectiveID>();

        switch (random)
        {
            case 0:
                obj.Add(ObjectiveID.findFigureWithMaxArea);
                return obj;
            case 1:
                obj.Add(ObjectiveID.findFigureWithMinArea);
                return obj;
            case 2:
                obj.Add(ObjectiveID.findFigureWithMaxPerimeter);
                return obj;
            case 3:
                obj.Add(ObjectiveID.findFigureWithMinPerimeter);
                return obj;
            default:
                obj.Add(ObjectiveID.findFigureWithMaxArea);
                return obj;
        }
    }

    private List<ObjectiveID> discoverNewAnglesObjective()
    {
        List<ObjectiveID> objectives = new List<ObjectiveID>();
        int numberFiguresSeen;
        numberFiguresSeen = GameController.Instance.user.anglesCompetenciesSeen.Count;

        //GameController.Instance.user.anglesCompetenciesSeen.Add(anglesObjectives[numberFiguresSeen]);
        objectives.Add(anglesObjectives[numberFiguresSeen]);

        return objectives;
    }

    private List<ObjectiveID> subsetObjectivesAreas()
    {
        List<ObjectiveID> temp = new List<ObjectiveID>();
        if (GameController.Instance.user.anglesCompetenciesSeen.Count == 1)
        {
            temp.Add(GameController.Instance.user.anglesCompetenciesSeen[0]);
        }
        else
        {
            int rand = ran.Next(0, GameController.Instance.user.anglesCompetenciesSeen.Count);

            temp.Add(GameController.Instance.user.anglesCompetenciesSeen[rand]);

            foreach (ObjectiveID obj in temp)
            {
                if (!temp.Contains(obj))
                {
                    if (ran.Next(0, 2) == 0) temp.Add(obj);
                }
            }
        }
        return temp;

    }

    private List<ObjectiveID> subsetObjectivesSymmetry()
    {
        List<ObjectiveID> temp = new List<ObjectiveID>();
        if (GameController.Instance.user.symmetricCompetenciesSeen.Count == 1)
        {
            temp.Add(GameController.Instance.user.symmetricCompetenciesSeen[0]);
        }
        else
        {
            int rand = ran.Next(0, GameController.Instance.user.symmetricCompetenciesSeen.Count);

            temp.Add(GameController.Instance.user.symmetricCompetenciesSeen[rand]);

            foreach (ObjectiveID obj in temp)
            {
                if (!temp.Contains(obj))
                {
                    if (ran.Next(0, 2) == 0) temp.Add(obj);
                }
            }
        }

        return temp;
    }

    private List<ObjectiveID> discoverNewSymmetryObjective()
    {
        List<ObjectiveID> objectives = new List<ObjectiveID>();
        int numberFiguresSeen;
        numberFiguresSeen = GameController.Instance.user.symmetricCompetenciesSeen.Count;

        //GameController.Instance.user.symmetricCompetenciesSeen.Add(symmetryObjectives[numberFiguresSeen]);
        objectives.Add(symmetryObjectives[numberFiguresSeen]);

        return objectives;
    }

    private List<Figures2D> getListFiguresSeen(RewardType reward)
    {
        if (reward == RewardType.Triangle)
        {
            return GameController.Instance.user.trianglesFiguresSeen;
        }
        else if (reward == RewardType.Square)
        {
            return GameController.Instance.user.quadrilateralFiguresSeen;
        }
        else
        {
            return GameController.Instance.user.quadrilateralFiguresSeen;
        }
    }

    private List<ObjectiveID> subsetObjectives(List<Figures2D> figuresSeen)
    {
        List<Figures2D> temp = new List<Figures2D>();

        if (figuresSeen.Count == 1)
        {
            temp.Add(figuresSeen[0]);
        }
        else
        {
            
            int rand = ran.Next(0, figuresSeen.Count);

            temp.Add(figuresSeen[rand]);

            foreach (Figures2D fig in temp)
            {
                if (!temp.Contains(fig))
                {
                    if (ran.Next(0, 2) == 0) temp.Add(fig);
                }
            }
        }

        return transformToListObjectiveID(temp);
        
    }

    private List<ObjectiveID> transformToListObjectiveID(List<Figures2D> temp)
    {

        List<ObjectiveID> objectives = new List<ObjectiveID>();
        
        foreach (Figures2D str in temp)
        {
            switch (str)
            {
                case Figures2D.EquilateralTriangle:
                    objectives.Add(ObjectiveID.findEquilateralTriangle);
                    break;
                case Figures2D.IsoscelesTriangle:
                    objectives.Add(ObjectiveID.findIsoscelesTriangle);
                    break;
                case Figures2D.RectangleTriangle:
                    objectives.Add(ObjectiveID.findRectangleTriangle);
                    break;
                case Figures2D.ScaleneTriangle:
                    objectives.Add(ObjectiveID.findScaleneTriangle);
                    break;
                case Figures2D.Square:
                    objectives.Add(ObjectiveID.findSquares);
                    break;
                case Figures2D.Rectangle:
                    objectives.Add(ObjectiveID.findRectangles);
                    break;
                case Figures2D.Rhombus:
                    objectives.Add(ObjectiveID.findRhombus);
                    break;
                case Figures2D.Trapezium:
                    objectives.Add(ObjectiveID.findTrapeziums);
                    break;
                case Figures2D.RegularPentagon:
                    objectives.Add(ObjectiveID.findPentagon);
                    break;
                default:
                    break;
            }
        }

        return objectives;
    }

    private List<ObjectiveID> discoverNewItem(RewardType type)
    {
        List<Figures2D> objectives = new List<Figures2D>();
        int numberFiguresSeen;
        if (type == RewardType.Triangle)
        {
            numberFiguresSeen = GameController.Instance.user.trianglesFiguresSeen.Count;

            //GameController.Instance.user.trianglesFiguresSeen.Add(triangle[numberFiguresSeen]);
            objectives.Add(triangle[numberFiguresSeen]);    
        }
        else if (type == RewardType.Square)
        {
            numberFiguresSeen = GameController.Instance.user.quadrilateralFiguresSeen.Count;

            //GameController.Instance.user.quadrilateralFiguresSeen.Add(quadrilateral[numberFiguresSeen]);
            objectives.Add(quadrilateral[numberFiguresSeen]);
        }
        else if (type == RewardType.Pentagon)
        {
            //objectives.Add(Figures2D.RegularPentagon);
            return choosePentagonObjective();

        }

        return transformToListObjectiveID(objectives);
    }

    private ObjectiveID getRandomObjectiveId(InventoryItem reward)
    {
        RewardType n = reward.name;
        switch (n)
        {
            case RewardType.Triangle:
                return ObjectiveID.findTriangles;

            case RewardType.Square:
                return ObjectiveID.findSquares;

            case RewardType.Coin:
                return ObjectiveID.findPentagon;

            default:
                return ObjectiveID.findNoIntEdges;
        }
    }

    private Size chooseSize(Level lev)
    {
        int j;

        if ((lev) == Level.VeryEasy) j = ((int)lev) + 1;
        else j = ((int)lev );

        return (Size)j;
    }

    private void setDimensions()
    {
        if (currentComp != Competencies.AreaPerimeter)
        {
            dimx = ((int)currentMission.size + 1) + UnityEngine.Random.Range(0, 2);
            dimz = ((int)currentMission.size + 1) + UnityEngine.Random.Range(0, 2);
        }
        else
        {
            dimx = areaPerimeterDimension();
            dimz = 1;
        }
            
    }

    private int areaPerimeterDimension()
    {
        switch (currentMission.lev)
        {
            case Level.VeryEasy:
                return 2;
            case Level.Easy:
                return 2;
            case Level.Medium:
                return 3;
            case Level.Difficult:
                return 4;
            case Level.VeryDifficult:
                return 5;
            default:
                return 2;
        }
    }


    #endregion

    #region Start New Mission
    internal void loadMission()
    {
        destroyOldObjects();
        container.transform.position = new Vector3(0, 0, 0);

        attempts = 0;
        container.transform.localScale = new Vector3(1f, 1f, 1f);
        figuresOnScene = new List<GameObject>();

        currentMission = missions[0];

        showMissionObjectives(true);

        time = currentMission.seconds;

        //Create grid of dimensions dimx x dimz
        setDimensions();

        prepareMission();

        //Destroy(sendMissionButton);
        int correctx, correctz;
        correctx = ran.Next(0, dimx);
        correctz = ran.Next(0, dimz); 

        for (int i = 0; i < dimx; i++)
        {
            for (int j = 0; j < dimz; j++)
            {
                if ((i == correctx) && (j == correctz)) figuresOnScene.Add(InstantiateNewFigure(i, j, true));
                else figuresOnScene.Add(InstantiateNewFigure(i, j));
            }
        }

        scaleGrid();
        //GameController.Instance.hudController.showObjectives(currentMission, false);
        playing = true;
    }

    internal void setPlaying(bool v)
    {
        playing = v;
    }

    internal void prepareMission()
    {
        possibleCorrectFiguresCurrentMission = new List<GameObject>();
        figureProperties script;

        foreach (var objective in currentMission.objectives)
        {
            if (objective.id == ObjectiveID.findTriangles)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.numOfEdges == NumOfEdges.Three)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findEquilateralTriangle)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if ((script.numOfEdges == NumOfEdges.Three) && script.regular)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findIsoscelesTriangle)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if ((script.numOfEdges == NumOfEdges.Three) && script.isosceles)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findRectangleTriangle)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if ((script.numOfEdges == NumOfEdges.Three) && (script.rightAngle))
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findScaleneTriangle)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if ((script.numOfEdges == NumOfEdges.Three) && (!script.isosceles))
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findSquares)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if ((script.numOfEdges == NumOfEdges.Four) && script.regular)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findRectangles)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if ((script.numOfEdges == NumOfEdges.Four) && script.rectangle)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findRhombus)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if ((script.numOfEdges == NumOfEdges.Four) && script.rhombus)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findTrapeziums)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if ((script.numOfEdges == NumOfEdges.Four) && script.trapezium)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findPentagon)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.numOfEdges == NumOfEdges.Five)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findRegularPentagon)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.numOfEdges == NumOfEdges.Five && script.regular)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }

            else if (objective.id == ObjectiveID.findIrregularPentagon)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.numOfEdges == NumOfEdges.Five && !script.regular)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findSymmetry)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.rotationalSymmetry || script.reflectionSymmetry)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findNoSymmetry)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (!script.rotationalSymmetry && !script.reflectionSymmetry)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findReflectionalSymmetry)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.reflectionSymmetry)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findRotationalSymmetry)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.rotationalSymmetry)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else if (objective.id == ObjectiveID.findRegularPolygon)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.regular)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }

            else if (objective.id == ObjectiveID.findFiguresWithRightAngle)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.rightAngle)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }

            else if (objective.id == ObjectiveID.findFiguresWithAcuteAngle)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.acuteAngle)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }

            else if (objective.id == ObjectiveID.findFiguresWithObtuseAngle)
            {
                foreach (GameObject go in possibleFigures)
                {
                    script = go.GetComponent<figureProperties>();
                    if (script.obtuseAngle)
                    {
                        possibleCorrectFiguresCurrentMission.Add(go);
                    }
                }
            }
            else foreach (GameObject go in strangeFigures) possibleCorrectFiguresCurrentMission.Add(go);
        }
        
        possibleIncorrectFiguresCurrentMission = new List<GameObject>();
        foreach (GameObject go in possibleFigures)
        {
            if (!possibleCorrectFiguresCurrentMission.Contains(go))
            {
                possibleIncorrectFiguresCurrentMission.Add(go);
            }
        }

    }

    private void scaleGrid()
    {
        float max = Math.Max(dimx, dimz);

        container.transform.localScale = new Vector3(8f/max, 1f, 8f/max);
        container.transform.position = new Vector3(-2f, 0, -0.8f);

    }

    private void destroyOldObjects()
    {
        var children = new List<GameObject>();
        foreach (Transform child in container) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        GameObject[] bombs = GameObject.FindGameObjectsWithTag("bomb");

        for (var i = 0; i < bombs.Length; i++)
            Destroy(bombs[i]);

    }
    private void showMissionObjectives(bool playing)
    {
        string obj = "";
        foreach (var objective in currentMission.objectives)
        {
            obj += objective.description + "\n";
        }

        GameController.Instance.hudController.showMissionObjective(obj,playing, currentMission.maxTimeStar);
    }

    private GameObject InstantiateNewFigure(int i, int j, bool needToBeCorrect = false)
    {
        Vector3 position;
        position = new Vector3(-(((float)dimx - 1) / 2) + i, 1f, -(((float)dimz - 1) / 2) + j);

        GameObject instance;
        int rotation = ran.Next(0, ((int)currentMission.lev) + 1);
        Quaternion quat;



        if (currentComp != Competencies.AreaPerimeter)
        {
            if (rotation > 1) quat = Quaternion.Euler(new Vector3(90, ran.Next(0, 12)*30, 0));
            else quat = Quaternion.Euler(new Vector3(90, 0, 0));
            if (needToBeCorrect)
            {
                List<GameObject> gos = getCorrectFigure();
                if (gos.Count == 0) instance = (GameObject)Instantiate(possibleCorrectFiguresCurrentMission[UnityEngine.Random.Range(0, possibleCorrectFiguresCurrentMission.Count)], position, quat);
                else instance = (GameObject)Instantiate(gos[UnityEngine.Random.Range(0, gos.Count)], position, quat);
                numCorrectFigures += 1;
            }
            else
            {
                if (ran.Next(0, 10) < 3)
                {
                    instance = (GameObject)Instantiate(possibleCorrectFiguresCurrentMission[UnityEngine.Random.Range(0, possibleCorrectFiguresCurrentMission.Count)], position, quat);
                    numCorrectFigures += 1;
                }
                else
                {
                    instance = (GameObject)Instantiate(possibleIncorrectFiguresCurrentMission[UnityEngine.Random.Range(0, possibleIncorrectFiguresCurrentMission.Count)], position, quat);
                    if (ran.Next(0, 10) < (((int)currentMission.lev)*2)) instance.GetComponent<figureProperties>().setBomb(true);
                }
            }
        }
        else
        {
            if (rotation > 1) quat = Quaternion.Euler(new Vector3(90, ran.Next(0, 4)*90, 0));
            else quat = Quaternion.Euler(new Vector3(90, 0, 0));
            instance = (GameObject)Instantiate(areaAndPerimeterFigures[UnityEngine.Random.Range(0, areaAndPerimeterFigures.Count)], position, quat);

        }

        instance.transform.parent = container;

        position = new Vector3(-(((float)dimx - 1) / 2) + i, 0f, -(((float)dimz - 1) / 2) + j);
        
        GameObject grid = (GameObject)Instantiate(cube, position, Quaternion.Euler(new Vector3(90, 0, 0)));
        grid.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        grid.transform.parent = container;

        instance.GetComponent<Renderer>().material.color = defaultColor;
        
        if (currentComp != Competencies.AreaPerimeter) return addComponentsIfNecessary(instance);
        else return instance;
    }

    private List<GameObject> getCorrectFigure()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject go in possibleCorrectFiguresCurrentMission)
        {
            figureProperties script = go.GetComponent<figureProperties>();
            if (currentMission.typeReward == script.type)
            {
                temp.Add(go);
            }
        }
        return temp;
    }

    private GameObject addComponentsIfNecessary(GameObject instance)
    {
        switch (currentMission.lev)
        {
            case Level.VeryEasy:
                break;
            case Level.Easy:
                //nothing to do
                break;
            case Level.Medium:
                addRotationScript(25, instance);
                break;
            case Level.Difficult:
                addRotationScript(5, instance);
                break;
            case Level.VeryDifficult:
                addRotationScript(3, instance);
                break;
            default:
                Debug.LogError("No difficulty set in new mission");
                break;
        }
        return instance;
    }
    private void addRotationScript(int probability, GameObject instance)
    {
        //System.Random rand = new System.Random();
        int random = UnityEngine.Random.Range(0, 100);

        if (random < probability)
        {
            instance.AddComponent<rotation>();
        }
    }
    #endregion

    #region PlayingUpdate

    internal void playingUpdate()
    {
        updateTime();

        if (time <= 0)
        {
            GameController.Instance.finishMission();
            return;
        }


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                GameObject selectedGO = hitInfo.transform.gameObject;
                if (selectedGO.tag == "figura")
                {
                    highlight(selectedGO);

                }
            }
        }
    }
    public void highlight(GameObject go)
    {
        bool isHighlighted = false;

        
        foreach (GameObject ob in highlightedObjects)
        {
            if (ob.GetInstanceID() == go.GetInstanceID())
            {
                isHighlighted = true;
                break;
            }
        }

        if (!isHighlighted)
        {
            go.GetComponent<Renderer>().material = defaultMaterial;
            go.GetComponent<Renderer>().material.color = highlightedColor;
            highlightedObjects.Add(go);
            numSelectedObjects++;

            if (go.GetComponent<figureProperties>().hasBomb())
            {
                Vector3 bombPos = go.transform.position;
                GameObject b = (GameObject)Instantiate(bomb, bombPos, Quaternion.Euler(new Vector3(270,0,0)));
                b.transform.parent = container;
                checkResult();
                GameController.Instance.countdownBomb = 2;
                playing = false;
                GameController.Instance.bombactive = true;
            }
        }
        else
        {
            go.GetComponent<Renderer>().material = defaultMaterial;
            go.GetComponent<Renderer>().material.color = defaultColor;
            highlightedObjects.Remove(go);
            numSelectedObjects--;
        }
        
        
    }

    private void checkResult()
    {
        if (currentComp != Competencies.AreaPerimeter)
        {
            foreach (var figure in figuresOnScene)
            {
                checkFigure(figure);
            }
        }
        else
        {
            checkAreaPerimeterMission();
        }
    }

    private void updateTime()
    {
        time = time - Time.deltaTime;
        GameController.Instance.hudController.showUpdatedTime(time);
    }

    #endregion

    #region Finish Mission
    //This methods are called when button "I FINISHED" is pressed
    internal void manageMissionFinished()
    {
        numCorrectFigures = 0;
        numNoSelectedCorrectFigures = 0;
        numErrors = 0;
        playing = false;
        //check mission
        if (currentComp != Competencies.AreaPerimeter)
        {
            foreach (var figure in figuresOnScene)
            {
                checkFigure(figure);
            }
        }
        else
        {
            checkAreaPerimeterMission();
        }
        

        //ShowSemaphore
        GameController.Instance.hudController.numCorrectFigures = numCorrectFigures;
        GameController.Instance.hudController.numNoSelectedCorrectFigures = numNoSelectedCorrectFigures;
        GameController.Instance.hudController.numErrors = numErrors;
        GameController.Instance.hudController.destroySemaphores();
        GameController.Instance.hudController.showSemaphore();

        
        GameController.Instance.semaphore = true;
        playing = false;
        semaphoreTime = -0.1f;
        showNewSemaphore = 0;

    }

    private void checkAreaPerimeterMission()
    {
        List<GameObject> solution = new List<GameObject>();

        Objectives2D obj = currentMission.objectives[0];

        if (obj.id == ObjectiveID.findFigureWithMaxArea)
        {
            float max = 0;
            foreach (var figure in figuresOnScene)
            {
                if ((figure.GetComponent<AreaFiguresProperties>().area) == max)
                {
                    solution.Add(figure);
                }
                else if ((figure.GetComponent<AreaFiguresProperties>().area) > max)
                {
                    solution = new List<GameObject>();
                    solution.Add(figure);
                    max = figure.GetComponent<AreaFiguresProperties>().area;
                }
            }
        }
        else if(obj.id == ObjectiveID.findFigureWithMinArea)
        {
            float min = 30;
            foreach (var figure in figuresOnScene)
            {
                if ((figure.GetComponent<AreaFiguresProperties>().area) == min)
                {
                    solution.Add(figure);
                }
                else if ((figure.GetComponent<AreaFiguresProperties>().area) < min)
                {
                    solution = new List<GameObject>();
                    solution.Add(figure);
                    min = figure.GetComponent<AreaFiguresProperties>().area;
                }
            }
        }
        else if (obj.id == ObjectiveID.findFigureWithMaxPerimeter)
        {
            float max = 0;
            foreach (var figure in figuresOnScene)
            {
                if ((figure.GetComponent<AreaFiguresProperties>().perimeter) == max)
                {
                    solution.Add(figure);
                }
                else if ((figure.GetComponent<AreaFiguresProperties>().perimeter) > max)
                {
                    solution = new List<GameObject>();
                    solution.Add(figure);
                    max = figure.GetComponent<AreaFiguresProperties>().perimeter;
                }
            }
        }
        else if (obj.id == ObjectiveID.findFigureWithMinPerimeter)
        {
            float min = 30;
            foreach (var figure in figuresOnScene)
            {
                if ((figure.GetComponent<AreaFiguresProperties>().perimeter) == min)
                {
                    solution.Add(figure);
                }
                else if ((figure.GetComponent<AreaFiguresProperties>().perimeter) < min)
                {
                    solution = new List<GameObject>();
                    solution.Add(figure);
                    min = figure.GetComponent<AreaFiguresProperties>().perimeter;
                }
            }
        }
        foreach (var figure in figuresOnScene)
        {
            if (solution.Contains(figure) && highlightedObjects.Contains(figure))
            {
                figure.GetComponent<figureProperties>().solutionColor = correctColor;
                numCorrectFigures += 1;
            }
            else if (!solution.Contains(figure) && highlightedObjects.Contains(figure))
            {
                numErrors += 1;
                figure.GetComponent<figureProperties>().solutionColor = errorColor;
            }
            else if (solution.Contains(figure) && !highlightedObjects.Contains(figure))
            {
                figure.GetComponent<figureProperties>().solutionColor = highlightedColor;
                numNoSelectedCorrectFigures += 1;
            }
            else
            {
                figure.GetComponent<figureProperties>().solutionColor = defaultColor;
            }
        }
            

    }

    internal void semaphoreUpdate()
    {
        semaphoreTime = semaphoreTime + Time.deltaTime;

        if ((semaphoreTime <= showNewSemaphore + Time.deltaTime/2) && (semaphoreTime >= showNewSemaphore - Time.deltaTime/2))
        {
            
            showNewSemaphore += GameController.Instance.hudController.deltaSemaphore;

            GameController.Instance.hudController.printNextSemaphore();
        }

        if (semaphoreTime > 1.5)
        {
            GameController.Instance.semaphore = false;
            //GameController.Instance.hudController.destroySemaphores();
            semaphoreFinished();

        }
    }

    private void semaphoreFinished()
    {
        if ((numNoSelectedCorrectFigures == 0) && (numErrors == 0))
        {
            showSolutionColors();
            GameController.Instance.hudController.setStarsBooleans();
            GameController.Instance.hudController.destroySemaphores();
            GameController.Instance.hudController.activateRewardPanel();
            giveReward();
            showResult();
            if (currentMission.discover) addDiscoverMission();

            GameController.Instance.user.numSuccessMissionForCompetency[(int)currentComp] += 1;

            showSolutionColors();
            missions.Remove(currentMission);
            container.transform.position = new Vector3(-3.25f, 0, 0);
        }
        else
        {
            if (time > 0)
            {
                attempts += 1;
                GameController.Instance.hudController.activeStar(3, false);
                playing = true;
            }
            else
            {
                container.transform.position = new Vector3(-3.25f, 0, 0);
                showSolutionColors();
                GameController.Instance.hudController.controlFinishTimeOver();
                /*GameController.Instance.hudController.destroySemaphores();
                GameController.Instance.hudController.activateTryAgainPanel();*/

            }
        }
    }

    internal void controlFinishBomb()
    {
        container.transform.position = new Vector3(-3.25f, 0, 0);
        showSolutionColors();
    }

    private void addDiscoverMission()
    {
        switch (currentComp)
        {
            case Competencies.Identification:
                switch (currentMission.typeReward)
                {
                    case RewardType.Square:
                        GameController.Instance.user.quadrilateralFiguresSeen.Add(getFigure2DFromObjective(currentMission.objectives[0]));
                        return;
                    case RewardType.Triangle:
                        GameController.Instance.user.trianglesFiguresSeen.Add(getFigure2DFromObjective(currentMission.objectives[0]));
                        return;
                }
                break;
            case Competencies.Symmetry:
                GameController.Instance.user.symmetricCompetenciesSeen.Add(currentMission.objectives[0].id);
                break;
            case Competencies.Angles:
                GameController.Instance.user.anglesCompetenciesSeen.Add(currentMission.objectives[0].id);
                return;
        }
    }
    
    private Figures2D getFigure2DFromObjective(Objectives2D objectives2D)
    {
        
        switch (objectives2D.id)
        {
            case ObjectiveID.findEquilateralTriangle:
                return Figures2D.EquilateralTriangle;
            case ObjectiveID.findIsoscelesTriangle:
                return Figures2D.IsoscelesTriangle;
            case ObjectiveID.findRectangleTriangle:
                return Figures2D.RectangleTriangle;
            case ObjectiveID.findScaleneTriangle:
                return Figures2D.ScaleneTriangle;
            case ObjectiveID.findSquares:
                return Figures2D.Square;
            case ObjectiveID.findRectangles:
                return Figures2D.Rectangle;
            case ObjectiveID.findRhombus:
                return Figures2D.Rhombus;
            case ObjectiveID.findTrapeziums:
                return Figures2D.Trapezium;
            default:
                Debug.Log("Error. Se añade figura 2d a vistas de forma erronea.");
                return Figures2D.Square;
        }
    }

    private void showSolutionColors()
    {
        foreach (var figure in figuresOnScene)
        {
            figure.GetComponent<Renderer>().material.color = figure.GetComponent<figureProperties>().solutionColor;
            if (figure.GetComponent<figureProperties>().hasBomb())
            {
                //edit
                //Vector3 position = figure.transform.position + new Vector3(0.3f, 0, 0.3f);
                //quat = Quaternion.Euler(new Vector3(90, 0, 0)
                GameObject instance = (GameObject)Instantiate(redbomb, figure.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
                instance.transform.parent = container;
            }
        }
    }

    private void giveReward()
    {
        recalculateReward();
        int starsWon = GameController.Instance.hudController.starsWon() ;
        currentMission.numReward = (int)((0.33f * starsWon) * currentMission.numReward);
        if (currentMission.numReward == 0) currentMission.numReward = 1;
        GameController.Instance.inv.Add(currentMission.typeReward, currentMission.numReward);
        int experience;

        experience = (int)(currentMission.coinFixReward * ((int)(currentMission.lev)+1)*4);
        experience += (int)(currentMission.coinFixReward * time);
        experience -= (int)((experience * 0.1) * attempts * (4-(int)currentMission.lev));
        experience = (int)(experience * 1.5f) ;
        if (experience <= 0) experience = (int)currentMission.coinFixReward;

        currentMission.experienceReward = experience;

        currentMission.goldReward = GameController.Instance.hudController.goldReward(currentMission.lev);

        GameController.Instance.user.giveExperience(comp, currentMission.experienceReward);
        GameController.Instance.user.giveGold(currentMission.goldReward);
        GameController.Instance.user.save();

        bool finished = GameController.Instance.inv.checkIfFinished(currentMission.typeReward);
        GameController.Instance.hudController.setGetMoreButtonInteractable(!finished);
    }

    private void recalculateReward()
    {
        currentMission.numReward -= attempts;

        if (currentMission.numReward <= 0) currentMission.numReward = 1;

        int objII = 10;
        int quantityII = 0;
        foreach (InventoryItem ii in GameController.Instance.inv.items)
        {
            if (ii.name == currentMission.typeReward)
            {
                quantityII = ii.quantity;
                objII = ii.campaignGoal;
                break;
            }
        }

        if (currentMission.numReward + quantityII > objII) currentMission.numReward = objII - quantityII;

    }

    private void showResult()
    {
        float objII = 0, currentII = 0;
        foreach (InventoryItem ii in GameController.Instance.inv.items)
        {
            if (ii.name == currentMission.typeReward)
            {
                objII = ii.campaignGoal;
                currentII = ii.quantity;
                break;
            }
        }
        GameController.Instance.hudController.showResult(
            time,
            currentMission.seconds,
            currentComp,
            currentMission.typeReward,
            currentMission.numReward,
            objII,
            currentII,
            currentMission.goldReward
            );

        GameController.Instance.hudController.showResultStars();
    }

    private void checkFigure(GameObject figure)
    {
        figureProperties script = figure.GetComponent<figureProperties>();
        bool isObjective = true;

        foreach (var obj in currentMission.objectives)
        {
            if (obj.id == ObjectiveID.findTriangles) isObjective &= (script.numOfEdges == NumOfEdges.Three);
            if (obj.id == ObjectiveID.findSquares) isObjective &= (script.numOfEdges == NumOfEdges.Four);
            if (obj.id == ObjectiveID.findPentagon) isObjective &= (script.numOfEdges == NumOfEdges.Five);
            if (obj.id == ObjectiveID.findRegularPentagon) isObjective &= (script.numOfEdges == NumOfEdges.Five && script.regular);
            if (obj.id == ObjectiveID.findIrregularPentagon) isObjective &= (script.numOfEdges == NumOfEdges.Five && !script.regular);
            if (obj.id == ObjectiveID.findNoIntEdges) isObjective &= (script.numOfEdges == NumOfEdges.NoInt);
            if (obj.id == ObjectiveID.findEquilateralTriangle) isObjective &= (script.regular && script.numOfEdges == NumOfEdges.Three);
            if (obj.id == ObjectiveID.findIsoscelesTriangle) isObjective &= (script.isosceles);
            if (obj.id == ObjectiveID.findRectangleTriangle) isObjective &= (script.rightAngle & script.numOfEdges == NumOfEdges.Three);
            if (obj.id == ObjectiveID.findScaleneTriangle) isObjective &= (script.numOfEdges == NumOfEdges.Three && !script.isosceles);
            if (obj.id == ObjectiveID.findSquares) isObjective &= (script.regular && script.numOfEdges == NumOfEdges.Four);
            if (obj.id == ObjectiveID.findRectangles) isObjective &= (script.rectangle);
            if (obj.id == ObjectiveID.findRhombus) isObjective &= (script.rhombus);
            if (obj.id == ObjectiveID.findTrapeziums) isObjective &= (script.trapezium);
            if (obj.id == ObjectiveID.findReflectionalSymmetry) isObjective &= (script.reflectionSymmetry);
            if (obj.id == ObjectiveID.findRotationalSymmetry) isObjective &= (script.rotationalSymmetry);
            if (obj.id == ObjectiveID.findSymmetry) isObjective &= (script.rotationalSymmetry || script.reflectionSymmetry);
            if (obj.id == ObjectiveID.findNoSymmetry) isObjective &= (!script.rotationalSymmetry && !script.reflectionSymmetry);
            if (obj.id == ObjectiveID.findRegularPolygon) isObjective &= (script.regular);
            if (obj.id == ObjectiveID.findFiguresWithRightAngle) isObjective &= (script.rightAngle);
            if (obj.id == ObjectiveID.findFiguresWithAcuteAngle) isObjective &= (script.acuteAngle);
            if (obj.id == ObjectiveID.findFiguresWithObtuseAngle) isObjective &= (script.obtuseAngle);

        }


        if (isObjective && highlightedObjects.Contains(figure)) 
        {
            figure.GetComponent<figureProperties>().solutionColor = correctColor;
            numCorrectFigures += 1;
        }
        else if (!isObjective && highlightedObjects.Contains(figure))
        {
            numErrors += 1;
            figure.GetComponent<figureProperties>().solutionColor = errorColor;
        }
        else if(isObjective && !highlightedObjects.Contains(figure))
        {
            figure.GetComponent<figureProperties>().solutionColor = highlightedColor;
            numNoSelectedCorrectFigures += 1;
        }
        else
        {
            figure.GetComponent<figureProperties>().solutionColor = defaultColor;
        }



    }
    #endregion

    #region Others
    static T GetRandomEnum<T>()
    {
        Array A = Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }
    #endregion

}
