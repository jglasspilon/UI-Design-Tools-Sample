using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChartDraw : MonoBehaviour
{
    public float xScale = 0.01f;
    public float yScale = 0.005f;

    public float xLabelOffset = 0.2f;
    public float yLabelOffset = 0.2f;

    public GameObject Line_Single;
    public GameObject Line_Perf1;
    public GameObject Line_Perf2;

    public GameObject LineMask;
    public GameObject SideMaskLeft;
    public GameObject SideMaskRight;

    public GameObject PDC_Object;
    public GameObject Background;
    public GameObject Graph_Border;

    public GameObject Graph_Dot;
    public GameObject Graph_DotPerf1;
    public GameObject Graph_DotPerf2;

    public GameObject X_Label;
    public GameObject Y_Label;

    public GameObject CandleStick;
    public GameObject CandleStickLine;

    private GameObject[] Y_Label_Respawns;
    private GameObject[] X_Label_Respawns;
    private GameObject[] Border_Respawns;
    private GameObject[] CandleStick_Respawns;

    private float PaddingTop = 0.0f;
    private float PaddingSides = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void ClearGraph()
    {
        //Single Chart Specifics
        PDC_Object.SetActive(false);
        Graph_Dot.SetActive(false);
        Line_Single.SetActive(false);

        //Performance Chart Specifics
        Line_Perf1.SetActive(false);
        Line_Perf2.SetActive(false);
        Graph_DotPerf1.SetActive(false);
        Graph_DotPerf2.SetActive(false);

        //Global
        Background.SetActive(false);
        LineMask.SetActive(false);
        SideMaskLeft.SetActive(false);
        SideMaskRight.SetActive(false);

        if (Line_Single.activeSelf == true)
        {
            Line_Single.GetComponent<LineRenderer>().positionCount = 0;
        }
        if (Line_Perf1.activeSelf == true)
        {
            Line_Perf1.GetComponent<LineRenderer>().positionCount = 0;
        }
        if (Line_Perf2.activeSelf == true)
        {
            Line_Perf2.GetComponent<LineRenderer>().positionCount = 0;
        }

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if(child.gameObject.tag == "Graph_Border" || child.gameObject.tag == "Graph_LabelY" || child.gameObject.tag == "Graph_LabelX" || child.gameObject.tag == "Graph_CandleStick")
                Destroy(child.gameObject);
        }
    }

    private void SetupGraph(float Padding_Top = 0.0f, float Padding_Sides = 0.0f)
    {
        //////////////////////////////////////////////////////////////////////////////
        //Global Specifics
        //////////////////////////////////////////////////////////////////////////////

        Background.SetActive(true);
        LineMask.SetActive(true);
        SideMaskLeft.SetActive(true);
        SideMaskRight.SetActive(true);

        //////////////////////////////////////////////////////////////////////////////
        //Setting up the borders
        //////////////////////////////////////////////////////////////////////////////

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Graph_Border")
                Destroy(child.gameObject);
        }

        GameObject Border_Left = Instantiate(Graph_Border, this.GetComponent<Transform>());
        Border_Left.transform.localPosition = new Vector3(-0.05f - (Padding_Sides * xScale), 0.0f, -0.04f);
        Border_Left.transform.localScale = new Vector3(0.05f, (1000.0f + Padding_Top) * yScale, 1.0f);

        GameObject Border_Right = Instantiate(Graph_Border, this.GetComponent<Transform>());
        Border_Right.transform.localPosition = new Vector3((1000.0f + Padding_Sides) * xScale, 0.0f, -0.04f);
        Border_Right.transform.localScale = new Vector3(0.05f, (1000.0f + Padding_Top) * yScale, 1.0f);

        GameObject Border_Top = Instantiate(Graph_Border, this.GetComponent<Transform>());
        Border_Top.transform.localPosition = new Vector3(-0.05f - (Padding_Sides * xScale), (1000.0f + Padding_Top) * yScale, -0.04f);
        Border_Top.transform.localScale = new Vector3(((1000.0f + (Padding_Sides * 2.0f)) * xScale) + (0.05f * 2.0f), 0.05f, 1.0f);

        GameObject Border_Bottom = Instantiate(Graph_Border, this.GetComponent<Transform>());
        Border_Bottom.transform.localPosition = new Vector3(-0.05f - (Padding_Sides * xScale), -0.05f, -0.04f);
        Border_Bottom.transform.localScale = new Vector3(((1000.0f + (Padding_Sides * 2.0f)) * xScale) + (0.05f * 2.0f), 0.05f, 1.0f);

        //////////////////////////////////////////////////////////////////////////////
        //Setting up the background
        //////////////////////////////////////////////////////////////////////////////

        Background.transform.localPosition = new Vector3(-(Padding_Sides * xScale), 0.0f, 0.1f);
        Background.transform.localScale = new Vector3((1000.0f + (Padding_Sides * 2.0f)) * xScale, (1000.0f + Padding_Top) * yScale, 1.0f);
        //Background.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f, 0.2f);

        //////////////////////////////////////////////////////////////////////////////
        //Setting up the Side Masks
        //////////////////////////////////////////////////////////////////////////////

        SideMaskLeft.transform.localPosition = new Vector3(-(Padding_Sides * xScale), 0.0f, -0.045f);
        SideMaskLeft.transform.localScale = new Vector3(SideMaskLeft.transform.localScale.x, (1000.0f + Padding_Top) * yScale, 1.0f);

        SideMaskRight.transform.localPosition = new Vector3((1000.0f + Padding_Sides) * xScale, 0.0f, -0.045f);
        SideMaskRight.transform.localScale = new Vector3(SideMaskRight.transform.localScale.x, (1000.0f + Padding_Top) * yScale, 1.0f);
    }

    public void DrawChart_Single(string CLOSE_VALUES_NORMALIZED, string Y_AXIS_LABELS_CLOSE_RAW, string X_AXIS_LABEL, string X_AXIS_LABEL_POS, string PREVIOUSDAY_CLOSE_NORM)
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        if (LineMask.GetComponent<Animation>().isPlaying)
            LineMask.GetComponent<Animation>().Stop();

        //////////////////////////////////////////////////////////////////////////////
        //Single Chart Specific
        //////////////////////////////////////////////////////////////////////////////

        ClearGraph();

        LineRenderer LR;
        float PDC_Value;

        PDC_Object.SetActive(true);
        Graph_Dot.SetActive(true);
        Line_Single.SetActive(true);


        PaddingTop = 0.0f;
        PaddingSides = 0.0f;

        //////////////////////////////////////////////////////////////////////////////
        //Setup the Graph
        //////////////////////////////////////////////////////////////////////////////

        SetupGraph(PaddingTop, PaddingSides);

        //////////////////////////////////////////////////////////////////////////////
        //Drawing the line
        //////////////////////////////////////////////////////////////////////////////

        string[] graphPoints = CLOSE_VALUES_NORMALIZED.Split(';');
        float pxSpacing = 1000.0f / graphPoints.Length;

        LR = Line_Single.GetComponent<LineRenderer>();

        PDC_Value = float.Parse(PREVIOUSDAY_CLOSE_NORM) * yScale;
        PDC_Object.GetComponent<Transform>().localPosition = new Vector3((-PaddingSides * xScale), PDC_Value, -0.03f);
        PDC_Object.GetComponent<Transform>().localScale = new Vector3((1000.0f + (PaddingSides * 2.0f)) * xScale, 1.0f, 0.1f);
        Line_Single.GetComponent<Renderer>().material.SetFloat("_PDC_Line", PDC_Object.transform.position.y);

        Vector3[] positions = new Vector3[graphPoints.Length];

        int posCount = 0;

        for (int i = 0; i < graphPoints.Length; i++)
        {
            if (graphPoints[i] != "")
            {
                posCount++;

                positions[i] = new Vector3((((pxSpacing) + (pxSpacing / (graphPoints.Length - 1))) * i) * xScale, float.Parse(graphPoints[i]) * yScale, 0.0f);
                Graph_Dot.transform.localPosition = new Vector3((((pxSpacing) + (pxSpacing / (graphPoints.Length - 1))) * i) * xScale, float.Parse(graphPoints[i]) * yScale, -0.05f);
            }
        }

        LR.positionCount = posCount;
        LR.SetPositions(positions);
        LR.Simplify(0.15f);

        //////////////////////////////////////////////////////////////////////////////
        //Displaying the Y-Axis
        //////////////////////////////////////////////////////////////////////////////

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Graph_LabelY")
                Destroy(child.gameObject);
        }

        string[] yLabels = Y_AXIS_LABELS_CLOSE_RAW.Split(';');
        float pySpacing = 1000.0f / yLabels.Length;

        for (int i = 0; i < yLabels.Length; i++)
        {
            GameObject instaY = Instantiate(Y_Label, this.GetComponent<Transform>());
            instaY.transform.localPosition = new Vector3((((instaY.GetComponent<RectTransform>().rect.width / 2.0f) * -1.0f) * 0.1f) - yLabelOffset - (PaddingSides * xScale), (((pySpacing) + (pySpacing / (yLabels.Length - 1))) * i) * yScale, -0.01f);
            instaY.GetComponent<TextMeshPro>().SetText("$" + yLabels[i]);

            Transform GridY = instaY.transform.GetChild(0);
            GridY.localPosition = new Vector3((instaY.GetComponent<RectTransform>().rect.width / 2.0f) + (yLabelOffset * 10.0f), 0.0f);
            GridY.localScale = new Vector3((1000.0f + (PaddingSides * 2.0f)) * xScale * 10.0f, 0.2f, 1.0f);
            GridY.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
        }

        //////////////////////////////////////////////////////////////////////////////
        //Displaying the X-Axis
        //////////////////////////////////////////////////////////////////////////////

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Graph_LabelX")
                Destroy(child.gameObject);
        }

        string[] xLabels = X_AXIS_LABEL.Split(';');
        string[] xLabelsPos = X_AXIS_LABEL_POS.Split(';');

        for (int i = 0; i < xLabelsPos.Length; i++)
        {
            if (xLabelsPos[i] == "")
            {
                xLabelsPos[i] = "0";
            }

            GameObject instaX = Instantiate(X_Label, this.GetComponent<Transform>());
            instaX.transform.localPosition = new Vector3(float.Parse(xLabelsPos[i]) * xScale, 0.0f - xLabelOffset, -0.01f);
            instaX.GetComponentInChildren<TextMeshPro>().SetText(xLabels[i]);

            Transform GridX = instaX.transform.GetChild(1);
            GridX.transform.localPosition = new Vector3(0.0f, xLabelOffset * 10.0f);
            GridX.transform.localScale = new Vector3(0.2f, (1000.0f + PaddingTop) * yScale * 10.0f, 1.0f);
            GridX.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);

            //if (float.Parse(xLabelsPos[i]) < 25.0f)
            //{
            //   instaX.GetComponent<TextMeshPro>().enabled = false;
            //}
        }

        //////////////////////////////////////////////////////////////////////////////
        //Setting up the Line Mask
        //////////////////////////////////////////////////////////////////////////////

        LineMask.transform.localPosition = new Vector3(-PaddingSides * xScale, 0.0f, -0.06f);
        LineMask.transform.localScale = new Vector3(((1000.0f + (PaddingSides * 2.0f)) * xScale) + 0.1f, (1000.0f + PaddingTop) * yScale, 1.0f);
        LineMask.GetComponent<Animation>().Play();
    }

    public void DrawChart_Performance(string NET_CHANGE_NORMALIZED1, string NET_CHANGE_NORMALIZED2, string Y_AXIS_LABELS_NET_CHANGE, string X_AXIS_LABEL, string X_AXIS_LABEL_POS)
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        if (LineMask.GetComponent<Animation>().isPlaying)
            LineMask.GetComponent<Animation>().Stop();

        //////////////////////////////////////////////////////////////////////////////
        //Performance Chart Specific
        //////////////////////////////////////////////////////////////////////////////

        ClearGraph();

        LineRenderer LR1;
        LineRenderer LR2;
        
        Line_Perf1.SetActive(true);
        Line_Perf2.SetActive(true);
        Graph_DotPerf1.SetActive(true);
        Graph_DotPerf2.SetActive(true);

        PaddingTop = 0.0f;
        PaddingSides = 0.0f;

        //////////////////////////////////////////////////////////////////////////////
        //Setup the Graph
        //////////////////////////////////////////////////////////////////////////////

        SetupGraph(PaddingTop, PaddingSides);

        //////////////////////////////////////////////////////////////////////////////
        //Drawing the Two lines
        //////////////////////////////////////////////////////////////////////////////

        string[] graphPoints_1 = NET_CHANGE_NORMALIZED1.Split(';');
        string[] graphPoints_2 = NET_CHANGE_NORMALIZED2.Split(';');
        float pxSpacing_1 = 1000.0f / graphPoints_1.Length;
        float pxSpacing_2 = 1000.0f / graphPoints_2.Length;

        LR1 = Line_Perf1.GetComponent<LineRenderer>();
        LR2 = Line_Perf2.GetComponent<LineRenderer>();

        int posCount_1 = 0;
        int posCount_2 = 0;

        var positions_1 = new List<Vector3>();
        var positions_2 = new List<Vector3>();

        if (graphPoints_1.Length > 1 && graphPoints_2.Length > 1)
        {
            //Line 1
            for (int i = 0; i < graphPoints_1.Length; i++)
            {
                if (graphPoints_1[i] != "")
                {
                    posCount_1++;
                    try
                    {
                        positions_1.Add(new Vector3((((pxSpacing_1) + (pxSpacing_1 / (graphPoints_1.Length - 1))) * i) * xScale, float.Parse(graphPoints_1[i]) * yScale, 0.0f));
                        Graph_DotPerf1.transform.localPosition = new Vector3((((pxSpacing_1) + (pxSpacing_1 / (graphPoints_1.Length - 1))) * i) * xScale, float.Parse(graphPoints_1[i]) * yScale, -0.05f);
                    }

                    catch
                    {
                        Debug.LogError("graph 1 point = " + graphPoints_1[i]);
                    }
                }
            }

            //Line 2
            for (int i = 0; i < graphPoints_2.Length; i++)
            {
                if (graphPoints_2[i] != "")
                {
                    posCount_2++;
                    try
                    {
                        positions_2.Add(new Vector3((((pxSpacing_2) + (pxSpacing_2 / (graphPoints_2.Length - 1))) * i) * xScale, float.Parse(graphPoints_2[i]) * yScale, 0.0f));
                        Graph_DotPerf2.transform.localPosition = new Vector3((((pxSpacing_2) + (pxSpacing_2 / (graphPoints_2.Length - 1))) * i) * xScale, float.Parse(graphPoints_2[i]) * yScale, -0.05f);
                    }

                    catch
                    {
                        Debug.LogError("graph 2 point = " + graphPoints_2[i]);
                    }
                }
            }
        }
        else
        {
            ClearGraph();
        }

        if (posCount_1 > 0 || posCount_2 > 0)
        {
            LR1.positionCount = posCount_1;
            LR1.SetPositions(positions_1.ToArray());
            LR1.Simplify(0.15f);

            LR2.positionCount = posCount_2;
            LR2.SetPositions(positions_2.ToArray());
            LR2.Simplify(0.15f);
        }
        else
        {
            ClearGraph();
        }

        //////////////////////////////////////////////////////////////////////////////
        //Displaying the Y-Axis
        //////////////////////////////////////////////////////////////////////////////

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Graph_LabelY")
                Destroy(child.gameObject);
        }

        string[] yLabels = Y_AXIS_LABELS_NET_CHANGE.Split(';');
        float pySpacing = 1000.0f / yLabels.Length;

        for (int i = 0; i < yLabels.Length; i++)
        {
            GameObject instaY = Instantiate(Y_Label, this.GetComponent<Transform>());
            instaY.transform.localPosition = new Vector3((((instaY.GetComponent<RectTransform>().rect.width / 2.0f) * -1.0f) * 0.1f) - yLabelOffset - (PaddingSides * xScale), (((pySpacing) + (pySpacing / (yLabels.Length - 1))) * i) * yScale, -0.01f);
            instaY.GetComponent<TextMeshPro>().SetText(yLabels[i] + "%");

            Transform GridY = instaY.transform.GetChild(0);
            GridY.localPosition = new Vector3((instaY.GetComponent<RectTransform>().rect.width / 2.0f) + (yLabelOffset * 10.0f), 0.0f);
            GridY.localScale = new Vector3((1000.0f + (PaddingSides * 2.0f)) * xScale * 10.0f, 0.2f, 1.0f);
            GridY.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
        }

        //////////////////////////////////////////////////////////////////////////////
        //Displaying the X-Axis
        //////////////////////////////////////////////////////////////////////////////

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Graph_LabelX")
                Destroy(child.gameObject);
        }

        string[] xLabels = X_AXIS_LABEL.Split(';');
        string[] xLabelsPos = X_AXIS_LABEL_POS.Split(';');

        for (int i = 0; i < xLabelsPos.Length; i++)
        {
            if (xLabelsPos[i] == "")
            {
                xLabelsPos[i] = "0";
            }

            GameObject instaX = Instantiate(X_Label, this.GetComponent<Transform>());
            instaX.transform.localPosition = new Vector3(float.Parse(xLabelsPos[i]) * xScale, 0.0f - xLabelOffset, -0.01f);
            instaX.GetComponentInChildren<TextMeshPro>().SetText(xLabels[i]);

            Transform GridX = instaX.transform.GetChild(0);
            GridX.transform.localPosition = new Vector3(0.0f, xLabelOffset * 10.0f);
            GridX.transform.localScale = new Vector3(0.2f, (1000.0f + PaddingTop) * yScale * 10.0f, 1.0f);
            GridX.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);

            //if (float.Parse(xLabelsPos[i]) < 25.0f)
            //{
            //    instaX.GetComponent<TextMeshPro>().enabled = false;
            //}
        }

        //////////////////////////////////////////////////////////////////////////////
        //Setting up the Line Mask
        //////////////////////////////////////////////////////////////////////////////

        LineMask.transform.localPosition = new Vector3(-PaddingSides * xScale, 0.0f, -0.06f);
        LineMask.transform.localScale = new Vector3(((1000.0f + (PaddingSides * 2.0f)) * xScale) + 0.1f, (1000.0f + PaddingTop) * yScale, 1.0f);
        LineMask.GetComponent<Animation>().Play();

    }

    public void DrawChart_CandleStick(string CLOSE_VALUES_NORMALIZED, string Y_AXIS_LABELS_CLOSE_RAW, string X_AXIS_LABEL, string X_AXIS_LABEL_POS, string OPEN_VALUES_NORMALIZED, string LOW_VALUES_NORMALIZED, string HIGH_VALUES_NORMALIZED)
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        if (LineMask.GetComponent<Animation>().isPlaying)
            LineMask.GetComponent<Animation>().Stop();

        //////////////////////////////////////////////////////////////////////////////
        //CandleStick Chart Specific
        //////////////////////////////////////////////////////////////////////////////

        ClearGraph();

        //PaddingTop = 75.0f;
        //PaddingSides = 75.0f;

        //////////////////////////////////////////////////////////////////////////////
        //Setup the Graph
        //////////////////////////////////////////////////////////////////////////////

        SetupGraph(PaddingTop, PaddingSides);

        //////////////////////////////////////////////////////////////////////////////
        //Drawing the CandleSticks
        //////////////////////////////////////////////////////////////////////////////

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Graph_CandleStick")
                Destroy(child.gameObject);
        }

        string[] closeValues = CLOSE_VALUES_NORMALIZED.Split(';');
        string[] openValues = OPEN_VALUES_NORMALIZED.Split(';');

        string[] highValues = HIGH_VALUES_NORMALIZED.Split(';');
        string[] lowValues = LOW_VALUES_NORMALIZED.Split(';');

        float pxSpacing = 1000.0f / closeValues.Length;

        int CandlesCount = 0;

        for (int i = 0; i < closeValues.Length; i++)
        {
            if (closeValues[i] == "")
                break;

            CandlesCount++;

            GameObject instaCandle = Instantiate(CandleStick, this.GetComponent<Transform>());
            GameObject instaCandleLine = Instantiate(CandleStickLine, this.GetComponent<Transform>());

            instaCandleLine.transform.localPosition = new Vector3((((pxSpacing) + (pxSpacing / (closeValues.Length - 1))) * i) * xScale, int.Parse(lowValues[i]) * yScale, -0.03f);
            instaCandleLine.transform.GetChild(0).localScale = new Vector3(0.0015f, (int.Parse(highValues[i]) - int.Parse(lowValues[i])) * (yScale / 10.0f), instaCandleLine.transform.GetChild(0).localScale.z);

            if (int.Parse(closeValues[i]) > int.Parse(openValues[i]))
            {
                //Green
                instaCandle.transform.localPosition = new Vector3((((pxSpacing) + (pxSpacing / (closeValues.Length - 1))) * i) * xScale, int.Parse(openValues[i]) * yScale, -0.04f);
                instaCandle.transform.GetChild(0).localScale = new Vector3((1.0f / closeValues.Length) / 2.0f, (int.Parse(closeValues[i]) - int.Parse(openValues[i])) * (yScale / 10.0f), instaCandle.transform.GetChild(0).localScale.z);
                instaCandle.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else
            {
                //Red
                instaCandle.transform.localPosition = new Vector3((((pxSpacing) + (pxSpacing / (closeValues.Length - 1))) * i) * xScale, int.Parse(closeValues[i]) * yScale, -0.04f);
                instaCandle.transform.GetChild(0).localScale = new Vector3((1.0f / closeValues.Length) / 2.0f, (int.Parse(openValues[i]) - int.Parse(closeValues[i])) * (yScale / 10.0f), instaCandle.transform.GetChild(0).localScale.z);
                instaCandle.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
        }


        //////////////////////////////////////////////////////////////////////////////
        //Displaying the Y-Axis
        //////////////////////////////////////////////////////////////////////////////

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Graph_LabelY")
                Destroy(child.gameObject);
        }

        string[] yLabels = Y_AXIS_LABELS_CLOSE_RAW.Split(';');
        float pySpacing = 1000.0f / yLabels.Length;

        for (int i = 0; i < yLabels.Length; i++)
        {
            if (yLabels[i] != "")
            {
                GameObject instaY = Instantiate(Y_Label, this.GetComponent<Transform>());
                instaY.transform.localPosition = new Vector3((((instaY.GetComponent<RectTransform>().rect.width / 2.0f) * -1.0f) * 0.1f) - yLabelOffset - (PaddingSides * xScale), (((pySpacing) + (pySpacing / (yLabels.Length - 1))) * i) * yScale, -0.01f);
                instaY.GetComponent<TextMeshPro>().SetText(yLabels[i]);

                Transform GridY = instaY.transform.GetChild(0);
                GridY.localPosition = new Vector3((instaY.GetComponent<RectTransform>().rect.width / 2.0f) + (yLabelOffset * 10.0f), 0.0f);
                GridY.localScale = new Vector3((1000.0f + (PaddingSides * 2.0f)) * xScale * 10.0f, 0.2f, 1.0f);
                GridY.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        //Displaying the X-Axis
        //////////////////////////////////////////////////////////////////////////////

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Graph_LabelX")
                Destroy(child.gameObject);
        }

        string[] xLabels = X_AXIS_LABEL.Split(';');
        string[] xLabelsPos = X_AXIS_LABEL_POS.Split(';');

        for (int i = 0; i < xLabelsPos.Length; i++)
        {
            if (xLabelsPos[i] == "")
            {
                xLabelsPos[i] = "0";
            }

            GameObject instaX = Instantiate(X_Label, this.GetComponent<Transform>());
            instaX.transform.localPosition = new Vector3(float.Parse(xLabelsPos[i]) * xScale, 0.0f - xLabelOffset, -0.01f);
            instaX.GetComponentInChildren<TextMeshPro>().SetText(xLabels[i]);

            Transform GridX = instaX.transform.GetChild(0);
            GridX.transform.localPosition = new Vector3(0.0f, xLabelOffset * 10.0f);
            GridX.transform.localScale = new Vector3(0.2f, (1000.0f + PaddingTop) * yScale * 10.0f, 1.0f);
            GridX.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);

            //if (float.Parse(xLabelsPos[i]) < 25.0f)
            //{
            //   instaX.GetComponent<TextMeshPro>().enabled = false;
            //}
        }

        //////////////////////////////////////////////////////////////////////////////
        //Setting up the Line Mask
        //////////////////////////////////////////////////////////////////////////////

        LineMask.transform.localPosition = new Vector3(-PaddingSides * xScale, 0.0f, -0.06f);
        LineMask.transform.localScale = new Vector3(((1000.0f + (PaddingSides * 2.0f)) * xScale) + 0.1f, (1000.0f + PaddingTop) * yScale, 1.0f);
        LineMask.GetComponent<Animation>().Play();
    }
}
