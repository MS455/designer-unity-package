using System;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityAtoms;

public class ARMeasure : MonoBehaviour
{
    #region variables
    [SerializeField]
    private GameObject m_pointPrefab;
    public GameObject pointPrefab
    {
        get { return m_pointPrefab; }
        private set { m_pointPrefab = value; }
    }

    [SerializeField]
    private GameObject m_linePrefab;
    public GameObject linePrefab
    {
        get { return m_linePrefab; }
        private set { m_linePrefab = value; }
    }

    [SerializeField]
    ARSessionHandler aRSessionHandler;

    [SerializeField]
    GameObject arSessionOrigin;

    [SerializeField]
    StringEvent E_ShowAreaResult;

    [SerializeField]
    BoolEvent E_Activate_SideButton;

    [Space]
    public int maxSectionCount;
    [SerializeField]
    int currentSection;
    [Space]
    [SerializeField]
    float movementFactor = 0.5f;
    [Space]
    [SerializeField]
    double combinedVolume;
    [Space]
    public VolumeData volumeData;
    [Space]
    [SerializeField]
    List<GameObject> currentSpawnedPoints = new List<GameObject>();
    [SerializeField]
    List<GameObject> currentSpawnedLines = new List<GameObject>();
    [SerializeField]
    List<GameObject> allSpawnedPoints = new List<GameObject>();
    [SerializeField]
    List<GameObject> allSpawnedLines = new List<GameObject>();

    public Action onPlacedObject;
    [SerializeField]
    private bool canStartMeasure;
    private ARRaycastManager m_RaycastManager;
    private readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();
    #endregion

    void Awake()
    {
        m_RaycastManager = arSessionOrigin.GetComponent<ARRaycastManager>();
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");

        if (arSessionOrigin != null)
            arSessionOrigin.GetComponent<PlaceARFocus>().enabled = true;
    }

    void OnDisable()
    {
        DestroyAllPointsAndLines();

        if (arSessionOrigin != null)
            arSessionOrigin.GetComponent<PlaceARFocus>().enabled = false;
    }

    #region Gizmo for editor

    //[SerializeField]
    //bool showProjection = true;
    //[SerializeField]
    //bool showDirectionOfMovement = true;
    //[SerializeField]
    //bool showMovedPosition = true;
    //[SerializeField]
    //bool showDirectionOfMovement2D = true;
    //[SerializeField]
    //bool showProjectionDirection = true;    

    //void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying)
    //        return;

    //    if (showMovedPosition)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawLine(movedPosition, movedPosition * 100);
    //    }

    //    if (showDirectionOfMovement)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawLine(directionOfMovement, directionOfMovement * 100);
    //    }

    //    if (showDirectionOfMovement2D)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawLine(directionOfMovement2D, directionOfMovement2D * 30);
    //    }

    //    if (showProjection)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(projection, projection * 70);
    //    }

    //    if (showProjectionDirection)
    //    {
    //        Gizmos.color = Color.magenta;
    //        Gizmos.DrawLine(projectionDirection, projectionDirection * 50);
    //    }
    //}

    #endregion

    void Update()
    {
        if (!canStartMeasure || currentSection > maxSectionCount)
            return;

#if UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            #region For generating points and lines on click...
            if (Input.GetMouseButtonDown(0))
            {
                if (aRSessionHandler != null)
                    aRSessionHandler.StopPlaneTrackingOnObjectPlaced(false);

                GenerateARRuler(hit.point);
            }
            #endregion

            CalculateMovementOfPointsAndLines(hit.point);
        }
#else
        #region For generating points and lines on click...
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    if (m_RaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose hitPose = PlaceARFocus.s_Hits[0].pose;
                        GenerateARRuler(hitPose.position);
                    }                        

                    if (onPlacedObject != null)
                        onPlacedObject();
                }
            }
        }
        #endregion

        if(PlaceARFocus.s_Hits.Count > 0)
        {
            Pose hitPose = PlaceARFocus.s_Hits[0].pose;
            CalculateMovementOfPointsAndLines(hitPose.position);
        }
#endif
    }

    private void CalculateMovementOfPointsAndLines(Vector3 hitPosition)
    {
        if (currentSpawnedPoints.Count > 0 && currentSpawnedPoints.Count <= 2)
        {
            #region calculate points to draw line...

            Vector3 movedPosition = currentSpawnedPoints[1].transform.position - hitPosition;

#if UNITY_EDITOR
            float movedDistance = Vector3.Magnitude(movedPosition);
            if (movedDistance <= 0.05f)
                return;
#endif

            // updating the 2nd dot position..
            currentSpawnedPoints[1].transform.position = hitPosition;

            // updating line between 1st and 2nd dots...
            DrawLine(currentSpawnedLines[0], currentSpawnedPoints[0].transform.position, currentSpawnedPoints[1].transform.position);

            #endregion
        }
        else if (currentSpawnedPoints.Count > 2 && currentSpawnedPoints.Count <= 4)
        {
            #region calculte points to draw rectangle...

            Vector3 movedPosition = currentSpawnedPoints[1].transform.position - hitPosition;
            float movedDistance = Vector3.Magnitude(movedPosition);

            Vector3 directionOfMovement = Vector3.Normalize(currentSpawnedPoints[1].transform.position - currentSpawnedPoints[0].transform.position);
            Vector2 directionOfMovement2D = new Vector2(directionOfMovement.x, directionOfMovement.z);

            Vector2 perpendicularDirection = Vector2.Perpendicular(directionOfMovement2D);

            float movedAngle = Vector3.SignedAngle(directionOfMovement, movedPosition, Vector3.up);

            if (movedAngle < 0)
            {
                perpendicularDirection = Vector2.Perpendicular(perpendicularDirection);
                perpendicularDirection = Vector2.Perpendicular(perpendicularDirection);
            }

            directionOfMovement = new Vector3(perpendicularDirection.x, 0, perpendicularDirection.y);

            Vector3 projectionDirection = directionOfMovement * 100;
            Vector3 projection = Vector3.Project(movedPosition.normalized, projectionDirection);

            movedDistance *= projection.magnitude;

            // updating the 3rd dot position..
            currentSpawnedPoints[2].transform.position = currentSpawnedPoints[1].transform.position + directionOfMovement * movedDistance;
            // updating the 4th dot position..
            currentSpawnedPoints[3].transform.position = currentSpawnedPoints[0].transform.position + directionOfMovement * movedDistance;

            // updating line between 2nd and 3rd dots...
            DrawLine(currentSpawnedLines[1], currentSpawnedPoints[1].transform.position, currentSpawnedPoints[2].transform.position);
            // updating line between 3rd and 4th dots...
            DrawLine(currentSpawnedLines[2], currentSpawnedPoints[2].transform.position, currentSpawnedPoints[3].transform.position);
            // updating line between 4th and 1st dots...
            DrawLine(currentSpawnedLines[3], currentSpawnedPoints[3].transform.position, currentSpawnedPoints[0].transform.position);

            #endregion
        }
        else if (currentSpawnedPoints.Count > 4)
        {
            #region calculate points to draw cube...

            Vector3 movedPosition = currentSpawnedPoints[2].transform.position - hitPosition;
            float movedDistance = Vector3.Magnitude(movedPosition);

            Vector3 directionOfMovement = Vector3.up * movementFactor;

            // updating the 5th dot position..
            currentSpawnedPoints[4].transform.position = currentSpawnedPoints[0].transform.position + directionOfMovement * movedDistance;
            // updating the 6th dot position..
            currentSpawnedPoints[5].transform.position = currentSpawnedPoints[1].transform.position + directionOfMovement * movedDistance;
            // updating the 7th dot position..
            currentSpawnedPoints[6].transform.position = currentSpawnedPoints[2].transform.position + directionOfMovement * movedDistance;
            // updating the 8th dot position..
            currentSpawnedPoints[7].transform.position = currentSpawnedPoints[3].transform.position + directionOfMovement * movedDistance;

            DrawLine(currentSpawnedLines[4], currentSpawnedPoints[0].transform.position, currentSpawnedPoints[4].transform.position);
            DrawLine(currentSpawnedLines[5], currentSpawnedPoints[1].transform.position, currentSpawnedPoints[5].transform.position);
            DrawLine(currentSpawnedLines[6], currentSpawnedPoints[2].transform.position, currentSpawnedPoints[6].transform.position);
            DrawLine(currentSpawnedLines[7], currentSpawnedPoints[3].transform.position, currentSpawnedPoints[7].transform.position);

            DrawLine(currentSpawnedLines[8], currentSpawnedPoints[4].transform.position, currentSpawnedPoints[5].transform.position);
            DrawLine(currentSpawnedLines[9], currentSpawnedPoints[5].transform.position, currentSpawnedPoints[6].transform.position);
            DrawLine(currentSpawnedLines[10], currentSpawnedPoints[6].transform.position, currentSpawnedPoints[7].transform.position);
            DrawLine(currentSpawnedLines[11], currentSpawnedPoints[7].transform.position, currentSpawnedPoints[4].transform.position);

            #endregion
        }
    }

    private void GenerateARRuler(Vector3 hitPoint)
    {
        //E_Activate_ResetButton.Raise(true);

        switch (currentSpawnedPoints.Count)
        {
            // on 1st click...
            case 0:
                GeneratePoints(2, hitPoint);
                GenerateLines(1);
                PrintPointAndLineCounts();
                break;
            // on 2nd click...
            case 2:
                GeneratePoints(2, hitPoint);
                GenerateLines(3);
                PrintPointAndLineCounts();
                break;
            // on 3rd click...
            case 4:
                MeasureVolume();
                //GeneratePoints(4, hitPoint);
                //GenerateLines(8);
                //PrintPointAndLineCounts();
                break;
            //case 8:
            //    MeasureVolume();
            //    break;
        }
    }

    private void PrintPointAndLineCounts()
    {
        string pointString = (currentSpawnedPoints.Count < 0) ? "point" : "points";
        string lineString = (currentSpawnedLines.Count < 0) ? "line" : "lines";
        //Debug.Log("Generated " + currentSpawnedPoints.Count + " " + pointString + " and " + currentSpawnedLines.Count + " " + lineString);
    }

    private void GeneratePoints(int pointCount, Vector3 hitPose)
    {
        for (int i = 0; i < pointCount; i++)
        {
            GameObject spawnedPoint = Instantiate(pointPrefab, hitPose, Quaternion.identity);
            currentSpawnedPoints.Add(spawnedPoint);
            allSpawnedPoints.Add(spawnedPoint);
        }
    }

    private void GenerateLines(int lineCount)
    {
        for (int i = 0; i < lineCount; i++)
        {
            GameObject spawnedLine = Instantiate(linePrefab);
            currentSpawnedLines.Add(spawnedLine);
            allSpawnedLines.Add(spawnedLine);
        }
    }

    private void DrawLine(GameObject line, Vector3 start, Vector3 end)
    {
        line.GetComponent<LineRenderer>().SetPosition(0, start);
        line.GetComponent<LineRenderer>().SetPosition(1, end);

        Vector3 centerPoint_1 = (end + start) / 2f;
        line.transform.GetChild(0).localPosition = centerPoint_1;

        Vector3 direction_1 = end - start;
        float distance_1 = Vector3.Magnitude(direction_1);
        line.GetComponentInChildren<Text>().text = Mathf.Round(distance_1 * 100f).ToString() + " cm";
    }

    private float SuperficieIrregularPolygon()
    {
        float temp = 0;
        int i = 0;

        Debug.Log("spawnedPoints count" + currentSpawnedPoints.Count);

        for (; i < currentSpawnedPoints.Count; i++)
        {
            if (i != currentSpawnedPoints.Count - 1)
            {
                float mulA = currentSpawnedPoints[i].transform.position.x * currentSpawnedPoints[i + 1].transform.position.z;
                float mulB = currentSpawnedPoints[i + 1].transform.position.x * currentSpawnedPoints[i].transform.position.z;
                temp = temp + (mulA - mulB);
            }
            else
            {
                float mulA = currentSpawnedPoints[i].transform.position.x * currentSpawnedPoints[0].transform.position.z;
                float mulB = currentSpawnedPoints[0].transform.position.x * currentSpawnedPoints[i].transform.position.z;
                temp = temp + (mulA - mulB);
            }
        }
        return Mathf.Abs(temp) * 1550;
    }

    private void ClearCurrentPointsAndLines()
    {
        currentSpawnedPoints.Clear();
        currentSpawnedLines.Clear();
    }

    public void MeasureVolume()
    {
        if (currentSpawnedLines.Count <= 0)
        {
            Debug.Log("Measure First, then Calculate Volume");
            return;
        }
        ;
        canStartMeasure = false;
        volumeData.volumeDataList = new List<VolumeData.Data>();
        
        if (volumeData.volumeDataList.Count != maxSectionCount)
        {
            
            int length = int.Parse(currentSpawnedLines[1].GetComponentInChildren<Text>().text.Replace(" cm", string.Empty));
            int breadth = int.Parse(currentSpawnedLines[0].GetComponentInChildren<Text>().text.Replace(" cm", string.Empty));
            Debug.Log("breadth " + breadth );
            //int height = int.Parse(currentSpawnedLines[4].GetComponentInChildren<Text>().text.Replace(" cm", string.Empty));
            int height = 1;
            Debug.Log("height " + height);
            VolumeData.Data volumeData = new VolumeData.Data(currentSection, length, breadth, height);
            this.volumeData.volumeDataList.Add(volumeData);

            double volume = double.Parse(Mathf.Round((length * breadth * height)).ToString());
            Debug.Log("Volume : " + volume);
            Debug.Log("CalculateVolume() --> " + "VOLUME : " + volume + " sq cm");
        }

        E_Activate_SideButton.Raise(currentSection == maxSectionCount);
    }

    public void StartMeasure(int totalSection)
    {
        Debug.Log("Start measure for " + totalSection + " section");
        maxSectionCount = totalSection;
        currentSection = 0;
        combinedVolume = 0;
        volumeData = new VolumeData();
        MeasureNextVolume();
    }

    public void MeasureNextVolume()
    {
        E_Activate_SideButton.Raise(false);

        canStartMeasure = true;
        currentSection++;
        ClearCurrentPointsAndLines();
    }

    public void UndoLastSection()
    {
        DestroyCustomPointsAndLines(currentSpawnedPoints.Count, currentSpawnedLines.Count);
    }

    public void Remeasure()
    {
        int sectionCountForRemeasure = maxSectionCount;
        DestroyAllPointsAndLines();
        StartMeasure(sectionCountForRemeasure);
    }

    public void CalculateVolume()
    {
        Debug.Log("Calculate Volume called");
        foreach (var item in volumeData.volumeDataList)
            combinedVolume += item.volume;

        E_ShowAreaResult.Raise(volumeData.SaveToString());
    }

    public void DestroyCustomPointsAndLines(int removePointCounts, int removeLineCounts)
    {
        Debug.Log("Destroy Custom Points("+ removePointCounts + ") And Lines(" + removeLineCounts + ")");

        for (int i = allSpawnedPoints.Count - removePointCounts; i < allSpawnedPoints.Count; i++)
            Destroy(allSpawnedPoints[i]);

        for (int i = allSpawnedLines.Count - removeLineCounts; i < allSpawnedLines.Count; i++)
            Destroy(allSpawnedLines[i]);

        ClearCurrentPointsAndLines();

        allSpawnedPoints.RemoveRange((allSpawnedPoints.Count - removePointCounts), removePointCounts);
        allSpawnedLines.RemoveRange((allSpawnedLines.Count - removeLineCounts), removeLineCounts);

        volumeData.volumeDataList.RemoveAt(volumeData.volumeDataList.Count - 1);

        //currentSection--;
        canStartMeasure = true;

        if (currentSection == 0)
            StartMeasure(maxSectionCount);
        else
        {
            E_Activate_SideButton.Raise(false);
        }
            
    }

    public void DestroyAllPointsAndLines()
    {
        //Debug.Log("Destroy All Points And Lines");

        foreach (var point in allSpawnedPoints)
            Destroy(point);

        foreach (var line in allSpawnedLines)
            Destroy(line);

        ClearCurrentPointsAndLines();

        allSpawnedPoints.Clear();
        allSpawnedLines.Clear();

        volumeData = new VolumeData();

        canStartMeasure = false;

        currentSection = 0;
        maxSectionCount = 0;
        combinedVolume = 0;

        E_Activate_SideButton.Raise(false);
    }
}

[Serializable]
public class VolumeData
{
    [Serializable]
    public class Data
    {
        public int length;
        public int width;
        public int height;

        public double volume;

        public int sectionIndex;
        public Data(int sectionIndex, int l, int b, int h)
        {
            length = (int)(l / 30.8);
            width = (int)(b / 30.8);
            height = h;

            volume = double.Parse(Mathf.Round((length * width * height)).ToString());

            this.sectionIndex = sectionIndex;
        }
    }

    public List<Data> volumeDataList;

    public static VolumeData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<VolumeData>(jsonString);
    }
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}