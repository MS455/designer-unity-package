using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class Measurement : MonoBehaviour
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

    public Action onPlacedObject;

    [SerializeField]
    private Button buttonPlacePoint;
    [SerializeField]
    private Button buttonClear;
    [SerializeField]
    private Button buttonUndo;
    [SerializeField]
    private Button buttonCalculate;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip clip;

    [SerializeField]
    private GameObject arSessionOrigin;
    [SerializeField]
    private GameObject areaResultPanel;

    [SerializeField]
    private TextMeshProUGUI areaResultText;
    [SerializeField]
    private float planeMeasurementResult;
    [Space]
    [SerializeField]
    List<GameObject> spawnedPoints = new List<GameObject>();
    [SerializeField]
    List<GameObject> spawnedLines = new List<GameObject>();
    [SerializeField]
    List<Vector3> FloorCordinates = new List<Vector3>();

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    bool canCalculateVolume;
    Vector3 projection;
    Vector3 directionOfMovement;
    [SerializeField]
    float movementFactor = 0.5f;

    [SerializeField]
    float Length;
    [SerializeField]
    float Breadth;
    [SerializeField]
    float Height;
    #endregion

    void Awake()
    {
        m_RaycastManager = arSessionOrigin.GetComponent<ARRaycastManager>();

        buttonUndo.onClick.AddListener(ClickUndo);
        buttonCalculate.onClick.AddListener(CalculateVolume);
        buttonClear.onClick.AddListener(ClearPointsAndLines);
        buttonPlacePoint.onClick.AddListener(ClickPlacePoint);
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
        areaResultPanel.SetActive(false);
        buttonCalculate.gameObject.SetActive(true);

        if (arSessionOrigin != null)
            arSessionOrigin.GetComponent<PlaceARFocus>().enabled = true;
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        ClearPointsAndLines();
        planeMeasurementResult = 0;

        if (arSessionOrigin != null)
            arSessionOrigin.GetComponent<PlaceARFocus>().enabled = false;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(directionOfMovement, directionOfMovement * 100);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(projection, projection * 10);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            planeMeasurementResult = 120;
            ShowVolumeResult(planeMeasurementResult);
            GetComponent<MeasurementUIManager>().moveDeviceCanvasGroup.alpha = 0;
            areaResultPanel.SetActive(true);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            #region For generating points and lines on click...
            if (Input.GetMouseButtonDown(0))
            {
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
        if (canCalculateVolume)
            return;

        if (spawnedPoints.Count > 0 && spawnedPoints.Count <= 2)
        {
            #region calculate points to draw line...

            Vector3 movedPosition = spawnedPoints[1].transform.position - hitPosition;

#if UNITY_EDITOR
            float movedDistance = Vector3.Magnitude(movedPosition);
            if (movedDistance <= 0.05f)
                return;
#endif

            // updating the 2nd dot position..
            spawnedPoints[1].transform.position = hitPosition;

            // updating line between 1st and 2nd dots...
            DrawLine(spawnedLines[0], spawnedPoints[0].transform.position, spawnedPoints[1].transform.position);

            #endregion
        }
        else if (spawnedPoints.Count > 2 && spawnedPoints.Count <= 4)
        {
            #region calculte points to draw rectangle...

            Vector3 movedPosition = spawnedPoints[1].transform.position - hitPosition;
            float movedDistance = Vector3.Magnitude(movedPosition);

#if UNITY_EDITOR
            if (movedDistance <= 0.05f)
                return;
#endif

            directionOfMovement = Vector3.Normalize(spawnedPoints[1].transform.position - spawnedPoints[0].transform.position);
            Vector2 directionOfMovement2D = new Vector2(directionOfMovement.x, directionOfMovement.z);

            Vector2 perpendicularDirection = Vector2.Perpendicular(directionOfMovement2D);

            float movedAngle = Vector3.SignedAngle(directionOfMovement, movedPosition.normalized, Vector3.forward);

            if (movedAngle < 0)
            {
                perpendicularDirection = Vector2.Perpendicular(perpendicularDirection);
                perpendicularDirection = Vector2.Perpendicular(perpendicularDirection);
            }

            directionOfMovement = new Vector3(perpendicularDirection.x, directionOfMovement.y, perpendicularDirection.y);

            Vector3 projectionDirection = directionOfMovement * 100;
            projection = Vector3.Project(movedPosition.normalized, projectionDirection);

            movedDistance *= projection.magnitude;

            // updating the 3rd dot position..
            spawnedPoints[2].transform.position = spawnedPoints[1].transform.position + directionOfMovement * movedDistance;
            // updating the 4th dot position..
            spawnedPoints[3].transform.position = spawnedPoints[0].transform.position + directionOfMovement * movedDistance;

            // updating line between 2nd and 3rd dots...
            DrawLine(spawnedLines[1], spawnedPoints[1].transform.position, spawnedPoints[2].transform.position);
            // updating line between 3rd and 4th dots...
            DrawLine(spawnedLines[2], spawnedPoints[2].transform.position, spawnedPoints[3].transform.position);
            // updating line between 4th and 1st dots...
            DrawLine(spawnedLines[3], spawnedPoints[3].transform.position, spawnedPoints[0].transform.position);

            #endregion
        }
        else if (spawnedPoints.Count > 4)
        {
            #region calculate points to draw cube...

            Vector3 movedPosition = spawnedPoints[2].transform.position - hitPosition;
            float movedDistance = Vector3.Magnitude(movedPosition);

#if UNITY_EDITOR
            if (movedDistance <= 0.05f)
                return;
#endif

            directionOfMovement = Vector3.up * movementFactor;

            // updating the 5th dot position..
            spawnedPoints[4].transform.position = spawnedPoints[0].transform.position + directionOfMovement * movedDistance;
            // updating the 6th dot position..
            spawnedPoints[5].transform.position = spawnedPoints[1].transform.position + directionOfMovement * movedDistance;
            // updating the 7th dot position..
            spawnedPoints[6].transform.position = spawnedPoints[2].transform.position + directionOfMovement * movedDistance;
            // updating the 8th dot position..
            spawnedPoints[7].transform.position = spawnedPoints[3].transform.position + directionOfMovement * movedDistance;

            DrawLine(spawnedLines[4], spawnedPoints[0].transform.position, spawnedPoints[4].transform.position);
            DrawLine(spawnedLines[5], spawnedPoints[1].transform.position, spawnedPoints[5].transform.position);
            DrawLine(spawnedLines[6], spawnedPoints[2].transform.position, spawnedPoints[6].transform.position);
            DrawLine(spawnedLines[7], spawnedPoints[3].transform.position, spawnedPoints[7].transform.position);

            DrawLine(spawnedLines[8], spawnedPoints[4].transform.position, spawnedPoints[5].transform.position);
            DrawLine(spawnedLines[9], spawnedPoints[5].transform.position, spawnedPoints[6].transform.position);
            DrawLine(spawnedLines[10], spawnedPoints[6].transform.position, spawnedPoints[7].transform.position);
            DrawLine(spawnedLines[11], spawnedPoints[7].transform.position, spawnedPoints[4].transform.position);

            #endregion
        }
    }

    private void GenerateARRuler(Vector3 hitPoint)
    {
        switch (spawnedPoints.Count)
        {
            // on 1st click...
            case 0:
                GeneratePoints(2, hitPoint);
                GenerateLines(1);
                break;
            // on 2nd click...
            case 2:

                GeneratePoints(2, hitPoint);
                GenerateLines(3);
                break;
            // on 3rd click...
            case 4:

                GeneratePoints(4, hitPoint);
                GenerateLines(8);
                break;
            case 8:
                CalculateVolume();
                break;
        }
        PrintPointAndLineCounts();
    }

    private void PrintPointAndLineCounts()
    {
        string pointString = (spawnedPoints.Count < 0) ? "point" : "points";
        string lineString = (spawnedLines.Count < 0) ? "line" : "lines";
        Debug.Log("Generated " + spawnedPoints.Count + " " + pointString + " and " + spawnedLines.Count + " " + lineString);
    }

    private void GeneratePoints(int pointCount, Vector3 hitPose)
    {
        for (int i = 0; i < pointCount; i++)
        {
            GameObject spawnedPoint = Instantiate(pointPrefab, hitPose, Quaternion.identity);
            spawnedPoints.Add(spawnedPoint);

            FloorCordinates.Add(new Vector2(hitPose.x, hitPose.z));
        }
    }

    private void GenerateLines(int lineCount)
    {
        for (int i = 0; i < lineCount; i++)
        {
            GameObject line = Instantiate(linePrefab);
            spawnedLines.Add(line);
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

    private double polygonArea(int n)
    {
        // Initialze area 
        double area = 0.0;

        // Calculate value of shoelace formula 
        int j = n - 1;

        for (int i = 0; i < n; i++)
        {
            area += (FloorCordinates[j].x + FloorCordinates[i].x) * (FloorCordinates[j].y - FloorCordinates[i].y);

            // j is previous vertex to i 
            j = i;
        }

        // Return absolute value 
        //Area in CM * CM 
        return Math.Abs(area / 2.0f) * 1550;
    }

    private float SuperficieIrregularPolygon()
    {
        float temp = 0;
        int i = 0;

        Debug.Log("spawnedPoints count" + spawnedPoints.Count);

        for (; i < spawnedPoints.Count; i++)
        {
            if (i != spawnedPoints.Count - 1)
            {
                float mulA = spawnedPoints[i].transform.position.x * spawnedPoints[i + 1].transform.position.z;
                float mulB = spawnedPoints[i + 1].transform.position.x * spawnedPoints[i].transform.position.z;
                temp = temp + (mulA - mulB);
            }
            else
            {
                float mulA = spawnedPoints[i].transform.position.x * spawnedPoints[0].transform.position.z;
                float mulB = spawnedPoints[0].transform.position.x * spawnedPoints[i].transform.position.z;
                temp = temp + (mulA - mulB);
            }
        }
        return Mathf.Abs(temp) * 1550;
    }

    public void JointAllPoints()
    {
        Debug.Log("ClearPointsAndLines");

        if (spawnedPoints.Count > 2)
        {
            Vector3 firstHit = spawnedPoints[0].transform.position;
            Vector3 lastHit = spawnedPoints[spawnedPoints.Count - 1].transform.position;
            GameObject line = Instantiate(linePrefab);
            line.GetComponent<LineRenderer>().SetPosition(0, firstHit);
            line.GetComponent<LineRenderer>().SetPosition(1, lastHit);
            spawnedLines.Add(line);
            Vector3 centerPoint = (firstHit + lastHit) / 2f;
            line.transform.GetChild(0).localPosition = centerPoint;

            Vector3 direction = firstHit - lastHit;
            float distance = Vector3.Magnitude(direction);
            line.GetComponentInChildren<Text>().text = Mathf.Round(distance * 100f).ToString() + " cm";

            //float area = SuperficieIrregularPolygon();
            //double area = (int)polygonArea(FloorCordinates.Count);

            double area = SuperficieIrregularPolygon();

            Debug.Log(
                "From A: " + firstHit + " to B: " + lastHit + "\n" +
                "Center of Line: " + centerPoint + "\n" +
                "Direction: " + direction + "\n" +
                "Area" + Mathf.Round((float)area)
               );

            ShowVolumeResult(Mathf.Round((float)area));
        }
    }

    public void ClickPlacePoint()
    {
        if (areaResultPanel.activeSelf)
            return;

        Debug.Log("ClickPlacePoint");

        audioSource.PlayOneShot(clip);

        Pose hitPose = PlaceARFocus.s_Hits[0].pose;
        FloorCordinates.Add(new Vector2(hitPose.position.x, hitPose.position.z));
        GameObject spawnedPoint = Instantiate(pointPrefab, hitPose.position, hitPose.rotation);
        spawnedPoints.Add(spawnedPoint);

        if (spawnedPoints.Count > 1)
        {
            Vector3 from = spawnedPoints[spawnedPoints.Count - 2].transform.position;
            Vector3 to = spawnedPoints[spawnedPoints.Count - 1].transform.position;

            GameObject line = Instantiate(linePrefab);
            line.GetComponent<LineRenderer>().SetPosition(0, from);
            line.GetComponent<LineRenderer>().SetPosition(1, to);
            spawnedLines.Add(line);

            Vector3 centerPoint = (to + from) / 2f;
            line.transform.GetChild(0).localPosition = centerPoint;

            Vector3 direction = to - from;
            float distance = Vector3.Magnitude(direction);
            line.GetComponentInChildren<Text>().text = Mathf.Round(distance * 100f).ToString() + " cm";

            Debug.Log(
                "From A: " + from + " to B: " + to + "\n" +
                "Center of Line: " + centerPoint + "\n" +
                "Direction: " + direction + "\n");
        }

        // this event raised in 'MeasurementUIManager' to deactivate UI message "Tap button to place points"...
        if (onPlacedObject != null)
            onPlacedObject();
    }

    public void ClickUndo()
    {
        Debug.Log("ClickUndo");

        if (spawnedPoints.Count > 0)
        {
            Destroy(spawnedPoints[spawnedPoints.Count - 1]);
            spawnedPoints.RemoveAt(spawnedPoints.Count - 1);
        }

        if (spawnedLines.Count > 0)
        {
            Destroy(spawnedLines[spawnedLines.Count - 1]);
            spawnedLines.RemoveAt(spawnedLines.Count - 1);
        }
    }

    public void ShowVolumeResult(double volume)
    {
        Debug.Log("Show result --> " + "VOLUME : " + volume + " sq cm");
        buttonCalculate.gameObject.SetActive(false);

        areaResultPanel.SetActive(true);
        areaResultText.text = "";
        areaResultText.text = "VOLUME : " + volume + " sq cm";

        planeMeasurementResult = (float)volume;
    }

    public void CalculateVolume()
    {
        canCalculateVolume = true;

        Breadth = float.Parse(spawnedLines[0].GetComponentInChildren<Text>().text.Replace(" cm", string.Empty));
        Length = float.Parse(spawnedLines[1].GetComponentInChildren<Text>().text.Replace(" cm", string.Empty));
        Height = float.Parse(spawnedLines[4].GetComponentInChildren<Text>().text.Replace(" cm", string.Empty));

        double volume = double.Parse(Mathf.Round((Length * Breadth * Height)).ToString());

        Debug.Log("Volume : " + volume);
        ShowVolumeResult(volume);
    }

    public void MeasureAgain()
    {
        Debug.Log("MeasureAgain");
        ClearPointsAndLines();
        areaResultPanel.SetActive(false);
        buttonCalculate.gameObject.SetActive(true);
        planeMeasurementResult = 0;
        canCalculateVolume = false;
    }

    public void ClearPointsAndLines()
    {
        Debug.Log("ClearPointsAndLines");

        foreach (var point in spawnedPoints)
            Destroy(point);

        foreach (var line in spawnedLines)
            Destroy(line);

        spawnedPoints.Clear();
        spawnedLines.Clear();
        FloorCordinates.Clear();
        canCalculateVolume = false;
    }
}