using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityAtoms.BaseAtoms;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AreaMeasurementManager : MonoBehaviour
{
    [SerializeField]
    GameObject linePrefab;
    [SerializeField]
    GameObject pointPrefab;
    [SerializeField]
    GameObject arSessionOrigin;

    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    FloatEvent E_ShowAreaResult;

   /* [SerializeField]
    VoidEvent E_OnObjectPlaced;
    [SerializeField]
    FloatEvent E_ShowAreaResult;*/

    List<ARRaycastHit> hits;
    List<Vector3> FloorCordinates = new List<Vector3>();
    List<GameObject> spawnedLines = new List<GameObject>();
    List<GameObject> spawnedPoints = new List<GameObject>();

    void OnDisable()
    {
        ResetArea();
    }

    void Update()
    {
       /* if(Input.GetKeyDown(KeyCode.M))
        {
            E_ShowAreaResult.Raise(10000.0f);
        }*/
    }
   

    public void CalculateArea()
    {
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
            //   float area = SuperficieIrregularPolygon();
            //  double area = (int)polygonArea(FloorCordinates.Count);
            double area = SuperficieIrregularPolygon();

            Debug.Log(
                "From A: " + firstHit + " to B: " + lastHit + "\n" +
                "Center of Line: " + centerPoint + "\n" +
                "Direction: " + direction + "\n" +
                "Area" + Mathf.Round((float)area)
               );

            //ShowAreaResult(Mathf.Round((float)area));
            E_ShowAreaResult.Raise(Mathf.Round((float)area));
        }
    }
    
    double polygonArea(int n)
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

    float SuperficieIrregularPolygon()
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

    public void ClickPlacePoint()
    {
        //if (areaResultPanel.activeSelf)
        //    return;
        if (PlaceARFocus.s_Hits == null || PlaceARFocus.s_Hits.Count <= 0)
            return;
        audioSource.PlayOneShot(clip);
        Debug.Log("ClickPlacePoint");
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

     /*   if (E_OnObjectPlaced != null)
            E_OnObjectPlaced.Raise();*/
    }

    public void ClickUndo()
    {
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

    public void ResetArea()
    {
        foreach (var point in spawnedPoints)
        {
            Destroy(point);
        }
        spawnedPoints.Clear();

        foreach (var line in spawnedLines)
        {
            Destroy(line);
        }
        spawnedLines.Clear();
    }

    public void SpawnPointsOnClick()
    {
        this.ClickPlacePoint();
    }
}