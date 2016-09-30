using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class DesdoplegamentSceneController : MonoBehaviour {
    #region variables
    //Llistes i objectes
    private ObjectiveMgr objMgr;
    private List<Objective> objectives;
    private List<GameObject> pieces;
    private List<GameObject> previews;
    private Objective objective;
    private Figura2Desdoplegament rootPiece;
    public UnfoldingHUD hud;
    private noDestroy sc;
    private User u;
    private int cId;
    private Inventory inv;

    //Control
    private bool timerEnded = false;
    private bool errorHad = false;
    private bool rotating;
    private bool ended = false;
    private bool cameraMove = false;
    private bool timeRunning = true;
    private bool tutorialShowing = false;
    private int tries = 0;
    private float timer = 0f;
    private int blockedEdges = 0;
    public float cameraSpeed;
    public int rotated;
    private float maxTime;

    //GameObjects, Materials and Vectors
    [SerializeField] private GameObject square2D;
    [SerializeField] private GameObject squareTransparent;
    [SerializeField] private GameObject triangle2D;
    [SerializeField] private GameObject triangleTransparent;
    [SerializeField] private GameObject pentagon2D;
    [SerializeField] private GameObject pentagonTransparent;
    [SerializeField] private Material transparentRed;
    [SerializeField] private Vector3 CameraPositionOrtho;
    [SerializeField] private Vector3 CameraPositionPerspective;
    private GameObject toPreview;

    //Singleton Instance
    public static DesdoplegamentSceneController Instance { get; private set; }
    #endregion

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
        Instance = this;
    }
    
    void Start ()
    {
        GameObject noDestroyGO = GameObject.Find("DontDestroyOnLoad");
        sc = noDestroyGO.GetComponent<noDestroy>();
        u = sc.u;
        cId = sc.c.id;
        inv = u.inventories[cId];

        

        objMgr = new ObjectiveMgr();
        objMgr.objs = new List<Objective>();

        rootPiece = null;
        pieces = new List<GameObject>();
        previews = new List<GameObject>();

        getObjectives();
        if (sc.to3D) {
            switch (sc.reward) {
                case (RewardType.Cube):
                    objective = objectives[1];
                    break;
                case (RewardType.TriangularPrism):
                    objective = objectives[0];
                    break;
                case (RewardType.Tetrahedron):
                    objective = objectives[2];
                    break;
                case (RewardType.Pyramide):
                    objective = objectives[4];
                    break;
                case (RewardType.PentagonalPrism):
                    objective = objectives[3];
                    break;
                case (RewardType.Octahedron):
                    objective = objectives[5];
                    break;
                    //TODO
            }
            hud.loadObjective(objective);
            reloadLevel();
        }
        else {
            objective = objectives[0];
            hud.loadObjective(objectives[0]);
            reloadLevel();
        }

        if (!u.tutorialUnfoldingSeen)
        {
            tutorialShowing = true;
            u.tutorialUnfoldingSeen = true;
            u.save();
            hud.showTutorial(true);
        }
        else
        {
            tutorialShowing = false;
            hud.showTutorial(false);
        }
    }

    void Update()
    {
        if (!tutorialShowing)
        {
            if (Input.GetMouseButton(0) && cameraMove)
            {
                Camera.main.transform.position -= Camera.main.transform.right * 2.5f;
                Camera.main.transform.RotateAround(rootPiece.transform.position, Vector3.up, Input.GetAxis("Mouse X") * cameraSpeed);
                Camera.main.transform.position += Camera.main.transform.right * 2.5f;
            }

            else if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    GameObject selectedGO = hitInfo.transform.gameObject;
                    if (previews.Contains(selectedGO))
                    {
                        PreviewDesdoblegament previewScript = selectedGO.GetComponent<PreviewDesdoblegament>();
                        if (!previewScript.blocked)
                        {
                            GameObject go = Instantiate(previewScript.finalObject, selectedGO.gameObject.transform.position, selectedGO.gameObject.transform.rotation) as GameObject;

                            Figura2Desdoplegament script = go.GetComponent<Figura2Desdoplegament>();
                            previewScript.aresta.a = script;
                            if (pieces.Count == 0)
                            {
                                blockedEdges = Mathf.Min(inv.getQuantity(sc.reward), script.costats - 1);
                                rootPiece = script;
                                script.arestes.Add(new Aresta(script, null, go.transform.forward, blockedEdges > 0));
                                script.arestesOri.Add(new Aresta(script, null, go.transform.forward, blockedEdges > 0));
                                if (blockedEdges > 0) blockedEdges--;
                            }
                            for (int i = 1; i < script.costats; i++)
                            {
                                script.arestes.Add(new Aresta(script, null,
                                    Quaternion.AngleAxis(i * 360f / script.costats, Vector3.up) * go.transform.forward, blockedEdges > 0));
                                script.arestesOri.Add(new Aresta(script, null,
                                    Quaternion.AngleAxis(i * 360f / script.costats, Vector3.up) * go.transform.forward, blockedEdges > 0));
                                if (blockedEdges > 0) blockedEdges--;
                            }
                            if (script.costats == 3) objective.triangles--;
                            if (script.costats == 4) objective.squares--;
                            if (script.costats == 5) objective.pentagons--;
                            hud.UpdateButtons(objective);
                            pieces.Add(go);
                            clearPreviews();
                            if (toPreview == triangleTransparent && objective.triangles > 0) showPreviews(toPreview);
                            if (toPreview == squareTransparent && objective.squares > 0) showPreviews(toPreview);
                            if (toPreview == pentagonTransparent && objective.pentagons > 0) showPreviews(toPreview);
                        }
                    }
                    else if (selectedGO.tag == "UnfoldPiece" && !cameraMove)
                    {
                        clearPreviews();
                        Figura2Desdoplegament script = selectedGO.GetComponentInParent<Figura2Desdoplegament>();
                        if (script.toDelete)
                        {
                            deleteRecursive(script);
                        }
                        else
                        {
                            script.readyToDelete();
                        }
                    }
                    else if (pieces.Count > 0) rootPiece.paintYellow();
                }
                else if (pieces.Count > 0) rootPiece.paintYellow();
            }

            if (rotating)
            {
                if (rotated == pieces.Count - 1)
                {
                    hud.showTimer(false);
                    timeRunning = false;
                    ended = true;
                    rotating = false;
                    rotated = 0;
                    InventoryItem ii = inv.getII(sc.reward);
                    inv.Spend(RewardType.Triangle, ii.cost[0]);
                    inv.Spend(RewardType.Square, ii.cost[1]);
                    inv.Spend(RewardType.Pentagon, ii.cost[2]);
                    inv.Add(sc.reward, 1);
                    u.save();
                    if (!timerEnded) u.gold += 50;
                    if (!errorHad) u.gold += 50;
                    u.gold += 50;
                    hud.setResult(1, inv.hasParts(ii) && ii.quantity < ii.campaignGoal);
                }
            }

            if (timeRunning)
            {
                timer += Time.deltaTime;
                hud.updateTimer(timer, maxTime);
                if (timer >= maxTime)
                {
                    timerEnded = true;
                }
            }
        }
    }

    public void resultButton()
    {
        sc.to2D = false;
        sc.to3D = false;
        SceneManager.LoadScene(3);
    }

    public void getMore()
    {
        sc.to3D = true;
        reloadLevel();
    }

    public void endTutorial()
    {
        hud.showTutorial(false);
        tutorialShowing = false;
    }

    public void errorRotating()
    {
        if (rotating)
        {
            timeRunning = false;
            errorHad = true;
            tries++;
            hud.loseTriesStar();
            rootPiece.stopRotation();
            hud.setResult(0, false);
            rotated = 0;
            rotating = false;
        }
    }

    public void UndoFolding()
    {
        rootPiece.stopRotation();
        rootPiece.resetPosition();
        cameraMove = false;
        Camera.main.orthographic = true;
        Camera.main.transform.position = CameraPositionOrtho;
        Camera.main.transform.LookAt(Vector3.zero);
    }

    private void clearPreviews()
    {
        foreach (GameObject preview in previews)
        {
            Destroy(preview);
        }
        previews.Clear();    
    }

    private void showPreviews(GameObject figuraPreview)
    {
        if (pieces.Count == 0)
        {
            GameObject go = Instantiate(figuraPreview, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<PreviewDesdoblegament>().aresta = new Aresta(null, null, new Vector3(0, 0, 0));
            previews.Add(go);
        }
        else
        {
            RecursiveTreePreview(rootPiece, figuraPreview);
        }
    }

    private void RecursiveTreePreview(Figura2Desdoplegament piece, GameObject figuraPreview)
    {
        PreviewDesdoblegament script = figuraPreview.GetComponent<PreviewDesdoblegament>();
        foreach (Aresta aresta in piece.arestes)
        {
            if (aresta.a == null)
            {
                GameObject go = Instantiate(figuraPreview, aresta.de.transform.position + aresta.vec * (aresta.de.apotema + script.apotema), Quaternion.identity) as GameObject;
                go.transform.forward = -aresta.vec;
                go.GetComponent<PreviewDesdoblegament>().aresta = aresta;
                if (aresta.blocked)
                {
                    go.GetComponent<PreviewDesdoblegament>().blocked = true;
                    go.GetComponent<Renderer>().material = transparentRed;
                }
                previews.Add(go);
            }
            else RecursiveTreePreview(aresta.a, figuraPreview);
        }
    }

    private void getObjectives()
    {
        /*Objective obj1 = new Objective("Triangular Prism", 3, 2, 0);
        obj1.foldingRules.Add("S-S", 120);
        obj1.foldingRules.Add("S-T", 90);
        obj1.foldingRules.Add("T-T", 180);
        Objective obj2 = new Objective("Cube / Hexahedron", 6, 0, 0);
        obj2.foldingRules.Add("S-S", 90);
        Objective obj3 = new Objective("TetraHedron", 0, 4, 0);
        obj3.foldingRules.Add("T-T", 180 - Mathf.Acos(1f/3f) * 360 / (2*Mathf.PI));
        Objective obj4 = new Objective("Pentagonal Prism", 5, 0, 2);
        obj4.foldingRules.Add("S-S", 72);
        obj4.foldingRules.Add("S-P", 90);
        obj4.foldingRules.Add("P-P", 180);
        Objective obj5 = new Objective("Piramide", 1, 4, 0);
        obj5.foldingRules.Add("S-T", 180 - 54.9f);
        obj5.foldingRules.Add("T-T", 180 - 2 * 54.88f);
        Objective obj6 = new Objective("Octahedron", 0, 8, 0);
        obj6.foldingRules.Add("T-T", 2 * (180 - 2 * 54.88f));

        objectives = new List<Objective>();
        objectives.Add(obj1);
        objectives.Add(obj2);
        objectives.Add(obj3);
        objectives.Add(obj4);
        objectives.Add(obj5);
        objectives.Add(obj6);
        objMgr.objs.Add(obj1);
        objMgr.objs.Add(obj2);
        objMgr.objs.Add(obj3);
        objMgr.objs.Add(obj4);
        objMgr.objs.Add(obj5);
        objMgr.objs.Add(obj6);
        saveJsonObjectives();*/
        loadJsonObjectives();
        objectives = new List<Objective>();
        objectives = objMgr.objs;
    }

    private void saveJsonObjectives()
    {
        objMgr.saveObjectives();
    }

    private void loadJsonObjectives()
    {
        TextAsset text = Resources.Load<TextAsset>("UnfoldingObjectives/objs");
        //string json = File.ReadAllText(Application.dataPath + "/Docs/UnfoldingObjectives/objs.json");
        string json = text.text;
        objMgr = JsonUtility.FromJson<ObjectiveMgr>(json);
        foreach (Objective o in objMgr.objs)
        {
            o.foldingRules = new Dictionary<string, float>();
            for (int i = 0; i < o.foldingFloat.Count; i++)
            {
                o.foldingRules[o.foldingString[i]] = o.foldingFloat[i];
            }
        }
    }

    public void previewTriangle()
    {
        toPreview = triangleTransparent;
        clearPreviews();
        showPreviews(toPreview);
    }

    public void previewSquare()
    {
        toPreview = squareTransparent;
        clearPreviews();
        showPreviews(toPreview);
    }

    public void previewPentagon()
    {
        toPreview = pentagonTransparent;
        clearPreviews();
        showPreviews(toPreview);
    }

    public void resetLevel()
    {
        timeRunning = true;
        hud.deactivatePanel();
        clearPreviews();
        if (rotating) rootPiece.stopRotation();
        if (pieces.Count > 0) rootPiece.resetPosition();
        rotating = false;
        rotated = 0;
        cameraMove = false;
        Camera.main.orthographic = true;
        Camera.main.transform.position = CameraPositionOrtho;
        Camera.main.transform.LookAt(Vector3.zero);
        hud.loadObjective(objective);
    }

    public void reloadLevel()
    {
        timeRunning = true;
        clearPreviews();
        deleteRecursive(rootPiece);
        errorHad = false;
        timerEnded = false;
        cameraMove = false;
        Camera.main.orthographic = true;
        Camera.main.transform.position = CameraPositionOrtho;
        Camera.main.transform.LookAt(Vector3.zero);
        tries = 0;
        timer = 0f;
        maxTime = (objective.squares + objective.pentagons + objective.triangles) * 2.5f;
        hud.ini(maxTime);
        hud.loadObjective(objective);
    }

    private void deleteRecursive(Figura2Desdoplegament piece)
    {
        if (pieces.Count > 0)
        {
            if (piece.costats == 3) objective.triangles++;
            if (piece.costats == 4) objective.squares++;
            if (piece.costats == 5) objective.pentagons++;
            foreach (var aresta in piece.arestes)
            {
                if (aresta.a != null) deleteRecursive(aresta.a);
            }
            pieces.Remove(piece.gameObject);
            Destroy(piece.gameObject);
        }
        hud.UpdateButtons(objective);
    }

    public void foldingCheck()
    {
        timeRunning = false;
        rotated = 0;
        rotating = true;
        rootPiece.paintYellow();
        //hud.disableResetButton();
        hud.disableReadyButton();
        Camera.main.orthographic = false;
        Camera.main.transform.position = CameraPositionPerspective;
        Camera.main.transform.LookAt(rootPiece.transform.position);
        //Camera.main.transform.position += Camera.main.transform.right * 2.5f;
        cameraMove = true;
        rootPiece.foldRecursive(objective);
    }
}
