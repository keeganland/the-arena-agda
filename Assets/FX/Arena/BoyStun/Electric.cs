using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : MonoBehaviour {

    private LineRenderer lineRenderer;
    private Vector3[] points = new Vector3[5];

    private readonly int point_Begin = 0;
    private readonly int point_Middle_Left = 1;
    private readonly int point_Center = 2;
    private readonly int point_Middle_Right = 3;
    private readonly int point_End = 4;

    public Transform lineEnd;
    public Transform lineBeginning;

    public Texture ElectricShape2;
    public Texture ElectricShape;

    private readonly float randomPosOffset = .3f;
    private float randomWithOffsetMax = 5f;
    private float randomWithOffsetMin = 3f;

    public float Speed;
    public float fadeOutSpeed = 1f;

    private readonly WaitForSeconds customFrame = new WaitForSeconds(0.05f);

    private int FrameNumber;
    private float alpha = 0.5f;
    private bool fadeOut = false;

    private Material newMat;
    private Material newMat2;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(Beam());
    }

    private IEnumerator Beam()
    {
        yield return customFrame;
        CalculateMiddle();
        lineRenderer.SetWidth(RandomWidthOffset(), RandomWidthOffset());
        FrameNumber += 1;
        if (FrameNumber < 5)
            StartCoroutine(Beam());
        else
        {
            newMat = new Material(Shader.Find("Particles/Additive"));
            newMat.mainTexture = ElectricShape2;
            lineRenderer.materials[0] = newMat;
            lineRenderer.materials[0].SetColor("_TintColor" ,Color.white);
            newMat2 = new Material(Shader.Find("Particles/Additive"));
            newMat2.mainTexture = ElectricShape;
            lineRenderer.materials[1] = newMat2;
            lineRenderer.materials[1].SetColor("_TintColor",Color.white);

            fadeOut = true;
        }
    }

    private float RandomWidthOffset()
    {
        return Random.Range(randomWithOffsetMin, randomWithOffsetMax);
    }

    private void CalculateMiddle()
    {
        Vector3 center = GetMiddleWithRandomness(lineBeginning.position, lineEnd.position);

        points[point_Center] = center;
        points[point_Middle_Left] = GetMiddleWithRandomness(lineBeginning.position, center);
        points[point_Middle_Right] = GetMiddleWithRandomness(center, lineEnd.position);
    }

    private Vector3 GetMiddleWithRandomness(Vector3 point1, Vector3 point2)
    {
        float x = (point1.x + point2.x) / point_Center;
        float finalX = Random.Range(x - randomPosOffset, x + randomPosOffset);
        float z = (point1.z + point2.z) / point_Center;
        float finalZ = Random.Range(z - randomPosOffset, z + randomPosOffset);

        return new Vector3(finalX, 0, finalZ);
    }
    private void Update()
    {
        points[point_Begin] = new Vector3(lineBeginning.position.x, 0, lineBeginning.position.z);
        points[point_End] = new Vector3(lineEnd.position.x, 0, lineEnd.position.z);
        lineRenderer.SetPositions(points);

        if(fadeOut)
        {
            fadeOutSpeed += Time.deltaTime;
            Color m_color = Color.Lerp(new Color(0.5f, 0.5f, 0.5f , 0.5f), new Color(0f, 0f, 0f, 0f), fadeOutSpeed);
            Debug.Log(m_color);
            lineRenderer.materials[0].SetColor("_TintColor", m_color);
            lineRenderer.materials[1].SetColor("_TintColor", m_color);
        }
    }
}
