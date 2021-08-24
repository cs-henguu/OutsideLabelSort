using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AotoLayout : MonoBehaviour
{
    List<Vector3> rootPos;
    List<GameObject> Labels;
    List<Vector3> targetPos;
    List<int> inViewLabels;
    List<int> outViewLabels;
    public GameObject m_LinePrefab;
    List<GameObject> Lines;
    int State = 0; //0表示不处于排序状态，1表示处于排序状态
    // Start is called before the first frame update
    void Start()
    {
        initList();  //初始化List
        updateLabels(); //载入标签和对应root数据
    }
    void initList()
    {
        rootPos = new List<Vector3>();
        Labels = new List<GameObject>();
        targetPos = new List<Vector3>();
        inViewLabels = new List<int>();
        outViewLabels = new List<int>();
        Lines = new List<GameObject>();
    }
    void updateLabels()
    {
        rootPos.Clear();
        Labels.Clear();
        GameObject obj = GameObject.FindGameObjectWithTag("Lables");
        foreach (Transform child in obj.transform)
        {
            rootPos.Add(child.position);
            Labels.Add(child.gameObject);
            targetPos.Add(child.position);
            //Lines.Add(Instantiate(m_LinePrefab, new Vector3(0, 0, 0), Quaternion.identity));     //注释掉本行和63行不画线  取消注释画线
        }
    }
    // Update is called once per frame
    void Update()
    {
        checkLabelsIsInView();
        int flag = checkState();
        if (flag == 1)  //进入有序排列
        {
            Debug.Log("Enter");
            OrderlyLayout();
        }
        if (flag == 2)  //退出有序排列
        {
            Debug.Log("Exit");
            RestoreOriginalLayout();
        }
        if (State == 0)
        {
            OutViewLabelsOrderlyLayout();
        }
        updateLabelsTransform();
        //updateLines();
    }
    void checkLabelsIsInView()  //检查label的原始位置是否在视野内
    {
        inViewLabels.Clear();
        outViewLabels.Clear();
        for (int i = 0; i < Labels.Count; i++)
        {
            if (isInView(rootPos[i]))
            {
                inViewLabels.Add(i);
            }
            else
            {
                outViewLabels.Add(i);
            }
        }
    }
    void updateLabelsTransform()  //设置Label的位置和朝向
    {
        for (int i = 0; i < Labels.Count; i++)
        {
            float speed = 7.0f;
            Labels[i].transform.position = Vector3.MoveTowards(Labels[i].transform.position, targetPos[i], Time.deltaTime * speed);
            Labels[i].transform.forward = Labels[i].transform.position - Camera.main.transform.position;
        }
    }

    void RestoreOriginalLayout()
    {
        for(int i = 0; i < Labels.Count; i++)
        {
            Vector3 p = new Vector3();
            p.x = rootPos[i].x;
            p.y = rootPos[i].y;
            p.z = rootPos[i].z;
            targetPos[i] = p;
        }
    }
    void OrderlyLayout()
    {
        int sumCnt = inViewLabels.Count;
        for (int i = 0; i < sumCnt; i++)
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3((i+1)*(Screen.width/(sumCnt+1)), Screen.height*0.5f, 8));
            targetPos[inViewLabels[i]] = p;
        }
    }

    void OutViewLabelsOrderlyLayout()
    {
        int inLabelsCnt = inViewLabels.Count;
        for (int i = 0; i < inLabelsCnt; i++)
        {
            Vector3 p = new Vector3();
            p.x = rootPos[inViewLabels[i]].x;
            p.y = rootPos[inViewLabels[i]].y;
            p.z = rootPos[inViewLabels[i]].z;
            targetPos[inViewLabels[i]] = p;
        }
        int outLabelsCnt = outViewLabels.Count;
        int LCnt = (int)Mathf.Ceil(outLabelsCnt * 0.5f);
        for (int i = 0; i < outLabelsCnt; i++)
        {
            Vector3 p;
            if (i < LCnt)
            {
                p = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.05f, (LCnt - i) * Screen.height / (LCnt + 1), 8));
            }
            else
            {
                p = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.95f, (outLabelsCnt - LCnt - (i-LCnt)) * Screen.height / (outLabelsCnt - LCnt + 1), 8));
            }
            targetPos[outViewLabels[i]] = p;
            Labels[outViewLabels[i]].transform.position = p;
        }
    }
    void updateLines()
    {
        for(int i = 0; i < Lines.Count; i++)
        {
            LineRenderer lineRender = Lines[i].GetComponent<LineRenderer>();
            lineRender.SetPosition(0, rootPos[i]);
            lineRender.SetPosition(1, Labels[i].transform.position);
        }
    }

    int checkState()  //return 0表示没有状态变化，1表示进入排序，2表示退出排序
    {
        if (State==0 && Vector3.Angle(Camera.main.transform.forward, new Vector3(0, 1, 0))<65.0f)
        {
            State = 1;
            return 1;
        }
        if (State == 1 && Vector3.Angle(Camera.main.transform.forward, new Vector3(0, 1, 0)) > 70.0f)
        {
            State = 0;
            return 2;
        }
        return 0;
    }
    bool isInView(Vector3 pos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        if (screenPos.x > Screen.width || screenPos.x < 0 ||  screenPos.z<0)
        {
            return false;
        }
        else
            return true;
    }
}
