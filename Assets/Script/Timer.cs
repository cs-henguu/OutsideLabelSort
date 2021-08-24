using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int m_RangeplusOne = 13;
    private float time = 0;
    private bool timer = false;
    private int currentNum;
    private int count = 0;
    List<string> wordList;
    // Start is called before the first frame update
    void Start()
    {
        wordList = new List<string>();
        initWordList();
        currentNum = Random.Range(1, m_RangeplusOne);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer)
        {
            time += Time.deltaTime;
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // 主相机屏幕点转换为射线
                //射线碰到了物体
                if (Physics.Raycast(ray, out hit))
                {
                    count++;
                    while (true)
                    {
                        int n = Random.Range(1, m_RangeplusOne);
                        if (n != currentNum)
                        {
                            currentNum = n;
                            break;
                        }
                    }
                    if (count == 8)
                    {
                        timer = false;
                    }
                }
            }
               
        }

        if (timer)
        {
            time += Time.deltaTime;

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    Ray ray;
                    RaycastHit hit;
                    // 主相机屏幕点转换为射线
                    ray = Camera.main.ScreenPointToRay(touch.position);
                    //射线碰到了物体
                    if (Physics.Raycast(ray, out hit))
                    {
                        count++;
                        while (true)
                        {
                            int n = Random.Range(1, m_RangeplusOne);
                            if (n != currentNum)
                            {
                                currentNum = n;
                                break;
                            }
                        }
                        if (count == 8)
                        {
                            timer = false;
                        }
                    }
                }
            }
        }
    }

    public void startTimer()
    {
        if (timer)
        {
            return;
        }
        count = 0;
        time = 0;
        currentNum = Random.Range(1, m_RangeplusOne);
        timer = true;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = null; //这是设置背景填充的
        style.normal.textColor = new Color(0, 1, 1);   //设置字体颜色的
        style.fontSize = 55;
        GUI.Label(new Rect(50, 100, 120, 80), "Find " + wordList[currentNum - 1], style);
        GUI.Label(new Rect(50, 250, 120, 80), "count:" + count.ToString(), style);
        GUI.Label(new Rect(50, 400, 120, 80), "T:" + time.ToString(), style);
    }

    void initWordList()
    {
        wordList.Clear();
        wordList.Add("A");
        wordList.Add("B");
        wordList.Add("C");
        wordList.Add("D");
        wordList.Add("E");
        wordList.Add("F");
        wordList.Add("G");
        wordList.Add("H");
        wordList.Add("I");
        wordList.Add("J");
        wordList.Add("K");
        wordList.Add("L");
    }
}
