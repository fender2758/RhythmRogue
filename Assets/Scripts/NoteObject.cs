using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class NoteObject : MonoBehaviour
{
    public bool life = false;

    public Note note = new Note();
    // [추가]
    public int hitPoint; // 기본은 1회 터치, 변경 시 2 이상 가능
    public TextMeshPro hpText;

    /// <summary>
    /// ��Ʈ �ϰ� �ӵ�
    /// interval�� ���� ���ؾ���. ��Ʈ�� �и������� ������ ����� �ϰ� �ְ� ������ �ð�ȭ�ϱ� ����, �⺻����(defaultInterval)�� 0.005 �� �����ϰ� ���� (���Ϸ� ������ ���� ��Ʈ �׷����� ��ĥ ���ɼ� ����)
    /// �׷��Ƿ� ��Ʈ�� �ϰ��ϴ� �ӵ��� 5�� �Ǿ����. ex) 0.01 = 10speed, 0.001 = 1speed
    /// </summary>
    public float speed = 5f;

    /// <summary>
    /// ��Ʈ �ϰ�
    /// </summary>
    public abstract void Move();
    public abstract IEnumerator IEMove();

    /// <summary>
    /// ��Ʈ ��ġ���� (�������)
    /// </summary>
    public abstract void SetPosition(Vector3[] pos);
    public abstract void UpdateHPText();

    public abstract void Interpolate(float curruntTime, float interval);

    /// <summary>
    /// Editor - Collider optimization
    /// </summary>
    public abstract void SetCollider();
    public abstract IEnumerator IECheckCollier();
}

public class NoteShort : NoteObject
{
    public override void Move()
    {
        StartCoroutine(IEMove());
    }

    public override IEnumerator IEMove()
    {
        while (true)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            if (transform.position.y < -1f)
                life = false;

            yield return null;
        }
    }

    public override void SetPosition(Vector3[] pos)
    {
        transform.position = new Vector3(pos[0].x, pos[0].y, pos[0].z);

        if (hpText == null)
        {
            GameObject textObj = new GameObject("HPText");
            textObj.transform.SetParent(this.transform);
            textObj.transform.localPosition = Vector3.zero;

            hpText = textObj.AddComponent<TextMeshPro>();
            hpText.alignment = TextAlignmentOptions.Center;
            hpText.fontSize = 5f; // 줄여도 큼직하게
            hpText.enableAutoSizing = false;

            hpText.color = Color.red;

            Renderer textRenderer = hpText.GetComponent<Renderer>();
            textRenderer.sortingLayerName = "Note";         // 또는 원하는 Sorting Layer 이름
            textRenderer.sortingOrder = 8;               // 높을수록 앞에 표시됨

            // 항상 카메라를 향하도록
            textObj.transform.rotation = Quaternion.identity;
        }

        hpText.text = hitPoint.ToString();
    }
    public override void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = hitPoint.ToString();
        }
    }

    public override void Interpolate(float curruntTime, float interval)
    {
        transform.position = new Vector3(transform.position.x, (note.time - curruntTime) * interval, transform.position.z);
    }

    public override void SetCollider()
    {
        if (GameManager.Instance.state == GameManager.GameState.Game)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            StartCoroutine(IECheckCollier());
        }
    }

    public override IEnumerator IECheckCollier()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (true)
        {
            int time = (int)transform.localPosition.y;
            int currentBar = Editor.Instance.currentBar;
            //Debug.Log(time + " @  " + (currentBar - 3) * 16 + " / " + (currentBar + 3) * 16);
            if (time >= (currentBar - 3) * 16 && time <= (currentBar + 3) * 16)
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }

            yield return wait;
        }
    }
}

public class NoteLong : NoteObject
{
    LineRenderer lineRenderer;
    public GameObject head;
    public GameObject tail;
    GameObject line;

    void Awake()
    {
        head = transform.GetChild(0).gameObject;
        tail = transform.GetChild(1).gameObject;
        line = transform.GetChild(2).gameObject;
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    public override void Move()
    {
        StartCoroutine(IEMove());
    }

    public override IEnumerator IEMove()
    {
        while (true)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;

            if (tail.transform.position.y < -1f)
                life = false;

            yield return null;
        }
    }

    public override void SetPosition(Vector3[] pos)
    {
        transform.position = new Vector3(pos[0].x, pos[0].y, pos[0].z);
        head.transform.position = new Vector3(pos[0].x, pos[0].y, pos[0].z);
        tail.transform.position = new Vector3(pos[1].x, pos[1].y, pos[1].z);
        line.transform.position = head.transform.position;

        Vector3 linePos = tail.transform.position - head.transform.position;
        linePos.x = 0f;
        linePos.z = 0f;
        lineRenderer.SetPosition(1, linePos);

        if (hpText == null)
        {
            GameObject textObj = new GameObject("HPText");
            textObj.transform.SetParent(this.transform);
            textObj.transform.localPosition = Vector3.zero;

            hpText = textObj.AddComponent<TextMeshPro>();
            hpText.alignment = TextAlignmentOptions.Center;
            hpText.fontSize = 5f; // 줄여도 큼직하게
            hpText.enableAutoSizing = false;
            hpText.color = Color.red;

            Renderer textRenderer = hpText.GetComponent<Renderer>();
            textRenderer.sortingLayerName = "Note";         // 또는 원하는 Sorting Layer 이름
            textRenderer.sortingOrder = 8;               // 높을수록 앞에 표시됨

            // 항상 카메라를 향하도록
            textObj.transform.rotation = Quaternion.identity;
        }

        hpText.text = hitPoint.ToString();
    }

    public override void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = hitPoint.ToString();
        }
    }

    public override void Interpolate(float curruntTime, float interval)
    {
        transform.position = new Vector3(head.transform.position.x, (note.time - curruntTime) * interval, head.transform.position.z);
        head.transform.position = new Vector3(head.transform.position.x, (note.time - curruntTime) * interval, head.transform.position.z);
        tail.transform.position = new Vector3(tail.transform.position.x, (note.tail - curruntTime) * interval, tail.transform.position.z);
        line.transform.position = head.transform.position;

        Vector3 linePos = tail.transform.position - head.transform.position;
        linePos.x = 0f;
        linePos.z = 0f;
        lineRenderer.SetPosition(1, linePos);
    }

    public override void SetCollider()
    {
        if (GameManager.Instance.state == GameManager.GameState.Game)
        {
            head.GetComponent<BoxCollider2D>().enabled = true;
            tail.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            StartCoroutine(IECheckCollier());
        }
    }

    public override IEnumerator IECheckCollier()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (true)
        {
            int time = (int)transform.localPosition.y;
            int currentBar = Editor.Instance.currentBar;
            if (time >= (currentBar - 3) * 16 && time <= (currentBar + 3) * 16)
            {
                head.GetComponent<BoxCollider2D>().enabled = true;
                tail.GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                head.GetComponent<BoxCollider2D>().enabled = false;
                tail.GetComponent<BoxCollider2D>().enabled = false;
            }

            yield return wait;
        }
    }
}