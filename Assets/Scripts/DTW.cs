using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DTW : MonoBehaviour
{
    public List<List<Vector2>> BasePoints = new();
    [SerializeField] TMP_Text txt;
    [SerializeField] TMP_Text rest;
    [SerializeField] List<LineRenderer> Lines;
    public bool OnRecord = false;

    public static DTW inst;

    private void Awake()
    {
        if (inst == null) { inst = this; DontDestroyOnLoad(gameObject);}
        else Destroy(this);

        rect = GetComponent<RectTransform>();
    }

    // 30Frame -> 1/0.33이라서 ㅇㅇ;
    WaitForSeconds wfs = new WaitForSeconds(0.033f);
    
    IEnumerator Recording()
    {
        if (BasePoints.Count < 2) BasePoints.Add(new List<Vector2>());
        else BasePoints[1].Clear();
        int i = BasePoints.Count - 1;
        Vector2 StartGap = Vector2.zero;
        if (i == 1)
        {
            Lines[1].positionCount = 0;
            StartGap = -MouseToWorld() + BasePoints[0][0];
        }
        int l = 0;
        Vector2 Last = Vector2.zero;
        while (OnRecord && l < 100)
        {
            var cur = MouseToWorld();
            if (i == 0) BasePoints[i].Add(cur);
            else BasePoints[i].Add(cur + StartGap);
            Lines[i].positionCount++;
            Lines[i].SetPosition(Lines[i].positionCount - 1, cur);
            l++;
            yield return wfs;
        }
        if (i == 1) CalcDTW();
        if (OnRecord) OnStart(null);
    }

    void CalcDTW()
    {
        float res = 0;
        int n = BasePoints[0].Count, m = BasePoints[1].Count;
        float[,] dp = new float[n + 1, m + 1];
        for (int i = 0; i <= n; i++) for (int x = 0; x <= m; x++)
            dp[i, x] = float.PositiveInfinity;
        dp[0, 0] = 0;

        for(int i = 1; i <=n; i++) for(int x = 1; x <= m; x++)
        {
            float cost = Vector2.Distance(BasePoints[0][i - 1], BasePoints[1][x - 1]);
            dp[i, x] = cost + Mathf.Min(dp[i - 1, x], dp[i, x - 1], dp[i - 1, x - 1]);
        }
        res = Mathf.Max(0, (1 - (dp[n, m] / (n + m)) * 0.01f)) * 100f;

        rest.text = $"Accuracy : {res}%";
    }
    Coroutine _record = null;

    RectTransform rect;
    Vector2 MouseToWorld()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Mouse.current.position.ReadValue(), Camera.main, out Vector2 res);
        return res;
    }
    void OnStart(InputValue var)
    {
        txt.text = $"Press \"E\" To {(OnRecord ? "Start" : "Stop")} Recording";
        OnRecord = OnRecord == false;
        if (OnRecord)
        {
            if (_record != null) { StopCoroutine(_record); _record = null; }
            _record = StartCoroutine(Recording());
        }
    }
}
