using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteType
{
    Short = 0,
    Long = 1,
}

public class Note
{
    public int id;
    public int time;
    public int type;
    public int line;
    public int tail;
    public int hitPoint;

    public Note() { } // 기본 생성자 (필수는 아님, 명시하면 좋음)
    public Note(int id, int time, int type, int line, int tail, int hitPoint)
    {
        this.id = id;
        this.time = time;
        this.type = type;
        this.line = line;
        this.tail = tail;
        this.hitPoint = hitPoint;
    }
    public Note Clone()
    {
        return new Note
        {
            id = this.id,
            type = this.type,
            line = this.line,
            time = this.time,
            tail = this.tail,
            hitPoint = this.hitPoint
        };
    }
}

public class Sheet
{
    // [Description]
    public string title;
    public string artist;

    // [Audio]
    public int bpm;
    public int offset;
    public int[] signature;

    // [Note]
    public List<Note> notes = new List<Note>();


    public AudioClip clip;
    public Sprite img;
    
    public float BarPerSec { get; private set; }
    public float BeatPerSec { get; private set; }

    public int BarPerMilliSec { get; private set; }
    public int BeatPerMilliSec { get; private set; }

    public void Init()
    {
        BarPerMilliSec = (int)(signature[0] / (bpm / 60f) * 1000);
        BeatPerMilliSec = BarPerMilliSec / 64;

        BarPerSec = BarPerMilliSec * 0.001f;
        BeatPerSec = BarPerMilliSec / 64f;
    }
}