using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class ChordActor : MonoBehaviour
{
    private int[] major3 = { 0, 4, 7, 12, 16};
    private int[] minor3 = { 0, 3, 7, 12, 15};
    private int[] dim3 = { 0, 3, 6, 12, 15};
    private int[] aug3 = { 0, 4, 8, 12, 16 };

    private int[] maj7 = { 0, 4, 7, 11, 12, 16, 19 };
    private int[] min7 = { 0, 3, 7, 10, 12, 15, 19 };
    private int[] dom7 = { 0, 4, 7, 10, 12, 16, 19 };
    private int[] minMaj7 = { 0, 3, 7, 11, 12, 15, 19 };
    private int[] halfDim7 = { 0, 3, 6, 10, 12, 15, 18 };
    private int[] dim7 = { 0, 3, 6, 9, 12, 15, 18 };
    private int[] augMaj7 = { 0, 4, 8, 11, 12, 16, 20 };
    private int[] augDom7 = { 0, 4, 8, 10, 12, 16, 20 };

    public Transform torus;
    public Transform circle;
    public Transform explosion;
    public Transform smoke;
    public Transform rose;
    public GameObject chordHolder;

    readonly int min = 21, max = 108, discretePosition = 5;
    // Update is called once per frame
    private bool[] active;

    private float ScrHeight, ScrWidth;
    
    private void Start()
    {
        ScrHeight = Camera.main.orthographicSize * 2f;
        ScrWidth = ScrHeight * Screen.width / Screen.height;
        active = new bool[130];
        for (int i = 0; i < 130; ++i) active[i] = false;
    }

    void DrawSmoke(int id, Vector3 position, float randmin, float randmax, float scale, Transform parent)
    {
        Transform t = Instantiate(smoke,
                                position + new Vector3(Random.Range(randmin, randmax), Random.Range(randmin, randmax)),
                                smoke.transform.rotation);
        t.position = new Vector3(t.position.x, t.position.y, 4);
        t.GetComponent<ParticleBehavior>().id = id;
        var main = t.GetComponent<ParticleSystem>().main;
        main.simulationSpeed = 0.6f;
        return;
    }

    void DrawExplosion(int id, Vector3 position, float randmin, float randmax, float scale, Transform parent)
    {
        Transform t = Instantiate(explosion,
                                position + new Vector3(Random.Range(randmin, randmax), Random.Range(randmin, randmax)),
                                explosion.transform.rotation, parent);
        t.GetComponent<ParticleBehavior>().id = id;
        return;
    }

    void DrawRose(int id, Vector3 position, float randmin, float randmax, float scale, bool flesh, Transform parent)
    {
        Transform t;
        if(flesh)
        {
            t = Instantiate(rose,
                                position + new Vector3(Random.Range(randmin, randmax), Random.Range(randmin, randmax)),
                                rose.transform.rotation, parent);
        }
        else
        {
            t = Instantiate(rose,
                                position + new Vector3(Random.Range(randmin, randmax), Random.Range(randmin, randmax)),
                                rose.transform.rotation, parent);
        }
        t.GetComponent<ParticleBehavior>().id = id;
        var main = t.GetComponent<ParticleSystem>().main;
        main.simulationSpeed = 0.1f;
        return;
    }

    void DrawTorus(int id, Vector3 position, float randmin, float randmax, float scale, bool grey, Transform parent)
    {
        Transform t = Instantiate(torus, 
                                position + new Vector3(Random.Range(randmin, randmax), Random.Range(randmin, randmax)), 
                                torus.transform.rotation, parent);
        t.GetComponent<TorusBehavior>().id = id;
        Color[] colors = GameObject.Find("NoteActor").GetComponent<SingleNoteBehavior>().noteColors;
        Color minorGrey = GameObject.Find("KeyActor").GetComponent<KeyActor>().minor;
        t.GetComponentInChildren<Renderer>().material.color = grey ? colors[id % 12] - minorGrey : colors[id % 12];
        t.localScale *= scale * 2;
        return;
    }

    void DrawCircle(int id, Vector3 position, float randmin, float randmax, float scale, bool grey, Transform parent)
    {
        Transform t = Instantiate(circle,
                    position + new Vector3(Random.Range(randmin, randmax), Random.Range(randmin, randmax)), 
                    Quaternion.identity, parent);
        t.GetComponent<CircleBehavior>().id = id;
        Color[] colors = GameObject.Find("NoteActor").GetComponent<SingleNoteBehavior>().noteColors;
        Color minorGrey = GameObject.Find("KeyActor").GetComponent<KeyActor>().minor;
        t.GetComponentInChildren<Renderer>().material.color = grey ? colors[id % 12] - minorGrey : colors[id % 12];
        scale = scale - 4;
        if (scale < 0)
            t.localScale *= (-1 / scale);
        else
            t.localScale *= (scale * 2 + 1);
        return;
    }

    void DrawOval(int id, Vector3 position, float randmin, float randmax, float scale, bool grey, Transform parent)
    {
        Transform t = Instantiate(circle,
                    position + new Vector3(Random.Range(randmin, randmax), Random.Range(randmin, randmax)),
                    Quaternion.identity, parent);
        t.GetComponent<CircleBehavior>().id = id;
        Color[] colors = GameObject.Find("NoteActor").GetComponent<SingleNoteBehavior>().noteColors;
        Color minorGrey = GameObject.Find("KeyActor").GetComponent<KeyActor>().minor;
        t.GetComponentInChildren<Renderer>().material.color = grey ? colors[id % 12] - minorGrey : colors[id % 12];
        scale /= 2;
        scale += 1;
        t.localScale = Vector3.Scale(t.localScale, new Vector3(scale, 1 / scale, 1));
        t.localScale *= 2;
        return;
    }

    public bool CheckSame(string tag, int s1, int s2, int s3, out float rd_x, out float rd_y)
    {
        rd_x = 0f;
        rd_y = 0f;
        foreach (var circle in GameObject.FindGameObjectsWithTag(tag))
        {
            if (tag == "Circle")
            {
                if (!circle.GetComponent<CircleBehavior>().keyup &&
                    (circle.GetComponent<CircleBehavior>().id == s1 ||
                     circle.GetComponent<CircleBehavior>().id == s2 ||
                     circle.GetComponent<CircleBehavior>().id == s3))
                {
                    rd_x = circle.transform.position.x;
                    rd_y = circle.transform.position.y;
                    return true;
                }
            }
            else if (tag == "Torus")
            {
                if (!circle.GetComponent<TorusBehavior>().keyup &&
                    (circle.GetComponent<TorusBehavior>().id == s1 ||
                     circle.GetComponent<TorusBehavior>().id == s2 ||
                     circle.GetComponent<TorusBehavior>().id == s3))
                {
                    rd_x = circle.transform.position.x;
                    rd_y = circle.transform.position.y;
                    return true;
                }
            }
            else if (tag == "Particle")
            {
                if (!circle.GetComponent<ParticleBehavior>().keyup &&
                    (circle.GetComponent<ParticleBehavior>().id == s1 ||
                     circle.GetComponent<ParticleBehavior>().id == s2 ||
                     circle.GetComponent<ParticleBehavior>().id == s3))
                {
                    rd_x = circle.transform.position.x;
                    rd_y = circle.transform.position.y;
                    return true;
                }
            }
        }
        return false;
    }

    public void DrawMaj3(int root_note, int i, float random_x, float random_y, bool discrete = false)
    {
        GameObject Maj3 = Instantiate(chordHolder);
        Maj3.transform.position = Vector3.zero;
        Maj3.transform.rotation = Quaternion.identity;
        Maj3.GetComponent<HolderBehavior>().id = root_note;
        Maj3.tag = "M3";
        
        float volume = MidiMaster.GetKey(root_note + major3[i - 2]);
        int priority = (int)(5 * volume);
        if(discrete)
        {
            priority = discretePosition;
        }
        
        DrawTorus(root_note + major3[i - 2], new Vector3(random_x, random_y, 5f * priority), 0f, 0f, volume, false, Maj3.transform);

        volume = MidiMaster.GetKey(root_note + major3[i - 1]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawTorus(root_note + major3[i - 1], new Vector3(random_x, random_y, 5f * priority), 0f, 0f, volume * 1.3f, false, Maj3.transform);

        volume = MidiMaster.GetKey(root_note + major3[i]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawTorus(root_note + major3[i], new Vector3(random_x, random_y, 5f * priority), 0f, 0f, volume * 1.5f, false, Maj3.transform);
        return;
    }
    public void DrawMin3(int root_note, int i, float random_x, float random_y, int shape_decider, bool discrete = false)
    {
        GameObject Min3 = Instantiate(chordHolder);
        Min3.transform.position = Vector3.zero;
        Min3.transform.rotation = Quaternion.identity;
        Min3.GetComponent<HolderBehavior>().id = root_note;
        Min3.tag = "m3";

        float volume = MidiMaster.GetKey(root_note + minor3[i - 2]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }


        if ((shape_decider & 4) == 4)
            DrawCircle(root_note + minor3[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min3.transform);
        else
            DrawOval(root_note + minor3[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min3.transform);

        volume = MidiMaster.GetKey(root_note + minor3[i - 1]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 2) == 2)
            DrawCircle(root_note + minor3[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min3.transform);
        else
            DrawOval(root_note + minor3[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min3.transform);

        volume = MidiMaster.GetKey(root_note + minor3[i]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 1) == 1)
            DrawCircle(root_note + minor3[i], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min3.transform);
        else
            DrawOval(root_note + minor3[i], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min3.transform);
    }
    public void DrawDim3(int root_note, int i, float random_x, float random_y, bool discrete = false)
    {
        GameObject Dim3 = Instantiate(chordHolder);
        Dim3.transform.position = Vector3.zero;
        Dim3.transform.rotation = Quaternion.identity;
        Dim3.GetComponent<HolderBehavior>().id = root_note;
        Dim3.tag = "dim3";

        float volume = MidiMaster.GetKey(root_note + dim3[i - 2]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawCircle(root_note + dim3[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Dim3.transform);
        volume = MidiMaster.GetKey(root_note + dim3[i]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawCircle(root_note + dim3[i], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8) - 2, true, Dim3.transform);
        volume = MidiMaster.GetKey(root_note + dim3[i - 1]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawSmoke(root_note + dim3[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), Dim3.transform);
        return;
    }
    public void DrawAug3(int root_note, int i, float random_x, float random_y, bool discrete = false)
    {
        GameObject Aug3 = Instantiate(chordHolder);
        Aug3.transform.position = Vector3.zero;
        Aug3.transform.rotation = Quaternion.identity;
        Aug3.GetComponent<HolderBehavior>().id = root_note;
        Aug3.tag = "aug3";

        float volume = MidiMaster.GetKey(root_note + aug3[i - 2]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawExplosion(root_note + aug3[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), Aug3.transform);
    }
    public void DrawMaj7(int root_note, int i, float random_x, float random_y, bool discrete = false)
    {
        GameObject Maj7 = Instantiate(chordHolder);
        Maj7.transform.position = Vector3.zero;
        Maj7.transform.rotation = Quaternion.identity;
        Maj7.GetComponent<HolderBehavior>().id = root_note;
        Maj7.tag = "M7";

        float volume = MidiMaster.GetKey(root_note + maj7[i - 3]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawTorus(root_note + maj7[i - 3], new Vector3(random_x, random_y, 5f * priority), 0f, 0f, volume, false, Maj7.transform);

        volume = MidiMaster.GetKey(root_note + maj7[i - 2]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawTorus(root_note + maj7[i - 2], new Vector3(random_x, random_y, 5f * priority), 0f, 0f, volume * 1.3f, false, Maj7.transform);

        volume = MidiMaster.GetKey(root_note + maj7[i - 1]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawTorus(root_note + maj7[i - 1], new Vector3(random_x, random_y, 5f * priority), 0f, 0f, volume * 1.5f, false, Maj7.transform);

        volume = MidiMaster.GetKey(root_note + maj7[i]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawTorus(root_note + maj7[i], new Vector3(random_x, random_y, 5f * priority), 0f, 0f, volume * 1.8f, false, Maj7.transform);
        return;
    }
    public void DrawMin7(int root_note, int i, float random_x, float random_y, int shape_decider, bool discrete = false)
    {
        GameObject Min7 = Instantiate(chordHolder);
        Min7.transform.position = Vector3.zero;
        Min7.transform.rotation = Quaternion.identity;
        Min7.GetComponent<HolderBehavior>().id = root_note;
        Min7.tag = "m7";

        float volume = MidiMaster.GetKey(root_note + min7[i - 3]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 8) == 8)
            DrawCircle(root_note + min7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min7.transform);
        else
            DrawOval(root_note + min7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min7.transform);

        volume = MidiMaster.GetKey(root_note + min7[i - 2]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 4) == 4)
            DrawCircle(root_note + min7[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min7.transform);
        else
            DrawOval(root_note + min7[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min7.transform);

        volume = MidiMaster.GetKey(root_note + min7[i - 1]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 2) == 2)
            DrawCircle(root_note + min7[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min7.transform);
        else
            DrawOval(root_note + min7[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min7.transform);

        volume = MidiMaster.GetKey(root_note + min7[i]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 1) == 1)
            DrawCircle(root_note + min7[i], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min7.transform);
        else
            DrawOval(root_note + min7[i], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Min7.transform);
        return;
    }
    public void DrawDom7(int root_note, int i, float random_x, float random_y, int shape_decider, bool discrete = false)
    {
        GameObject Dom7 = Instantiate(chordHolder);
        Dom7.transform.position = Vector3.zero;
        Dom7.transform.rotation = Quaternion.identity;
        Dom7.GetComponent<HolderBehavior>().id = root_note;
        Dom7.tag = "dom7";

        float volume = MidiMaster.GetKey(root_note + dom7[i - 3]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 8) == 8)
            DrawCircle(root_note + dom7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), false, Dom7.transform);
        else
            DrawOval(root_note + dom7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), false, Dom7.transform);

        volume = MidiMaster.GetKey(root_note + dom7[i - 2]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 4) == 4)
            DrawCircle(root_note + dom7[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), false, Dom7.transform);
        else
            DrawOval(root_note + dom7[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), false, Dom7.transform);

        volume = MidiMaster.GetKey(root_note + dom7[i - 1]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 2) == 2)
            DrawCircle(root_note + dom7[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), false, Dom7.transform);
        else
            DrawOval(root_note + dom7[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), false, Dom7.transform);

        volume = MidiMaster.GetKey(root_note + dom7[i]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 1) == 1)
            DrawCircle(root_note + dom7[i], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), false, Dom7.transform);
        else
            DrawOval(root_note + dom7[i], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), false, Dom7.transform);
    }
    public void DrawMM7(int root_note, int i, float random_x, float random_y, int shape_decider, bool discrete = false)
    {
        GameObject MM7 = Instantiate(chordHolder);
        MM7.transform.position = Vector3.zero;
        MM7.transform.rotation = Quaternion.identity;
        MM7.GetComponent<HolderBehavior>().id = root_note;
        MM7.tag = "mM7";

        float volume = MidiMaster.GetKey(root_note + minMaj7[i - 3]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 4) == 4)
            DrawCircle(root_note + minMaj7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, MM7.transform);
        else
            DrawOval(root_note + minMaj7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, MM7.transform);

        volume = MidiMaster.GetKey(root_note + minMaj7[i - 2]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 2) == 2)
            DrawCircle(root_note + minMaj7[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, MM7.transform);
        else
            DrawOval(root_note + minMaj7[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, MM7.transform);

        volume = MidiMaster.GetKey(root_note + minMaj7[i - 1]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 1) == 1)
            DrawCircle(root_note + minMaj7[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, MM7.transform);
        else
            DrawOval(root_note + minMaj7[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, MM7.transform);

        volume = MidiMaster.GetKey(root_note + minMaj7[i]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawRose(root_note + minMaj7[i], new Vector3(random_x, random_y, 5f * priority), -3, 3, (int)(volume * 8), false, MM7.transform);
        return;
    }
    public void DrawHDim7(int root_note, int i, float random_x, float random_y, bool discrete = false)
    {
        GameObject HDim7 = Instantiate(chordHolder);
        HDim7.transform.position = Vector3.zero;
        HDim7.transform.rotation = Quaternion.identity;
        HDim7.GetComponent<HolderBehavior>().id = root_note;
        HDim7.tag = "h-dim7";

        float volume = MidiMaster.GetKey(root_note + halfDim7[i - 3]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawCircle(root_note + halfDim7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, HDim7.transform);
        volume = MidiMaster.GetKey(root_note + halfDim7[i - 1]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawCircle(root_note + halfDim7[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8) - 2, true, HDim7.transform);
        volume = MidiMaster.GetKey(root_note + halfDim7[i - 2]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawSmoke(root_note + halfDim7[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), HDim7.transform);

        DrawRose(root_note + halfDim7[i], new Vector3(random_x, random_y, 5f * priority), -3, 3, (int)(volume * 8), true, HDim7.transform);
        return;
    }
    public void DrawDim7(int root_note, int i, float random_x, float random_y, int shape_decider, bool discrete = false)
    {
        GameObject Dim7 = Instantiate(chordHolder);
        Dim7.transform.position = Vector3.zero;
        Dim7.transform.rotation = Quaternion.identity;
        Dim7.GetComponent<HolderBehavior>().id = root_note;
        Dim7.tag = "dim7";

        float volume = MidiMaster.GetKey(root_note + dim7[i - 3]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 4) == 4)
            DrawCircle(root_note + dim7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Dim7.transform);
        else
            DrawOval(root_note + dim7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Dim7.transform);

        volume = MidiMaster.GetKey(root_note + dim7[i - 2]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 2) == 2)
            DrawCircle(root_note + dim7[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Dim7.transform);
        else
            DrawOval(root_note + dim7[i - 2], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Dim7.transform);

        volume = MidiMaster.GetKey(root_note + dim7[i - 1]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        if ((shape_decider & 1) == 1)
            DrawCircle(root_note + dim7[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Dim7.transform);
        else
            DrawOval(root_note + dim7[i - 1], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), true, Dim7.transform);

        volume = MidiMaster.GetKey(root_note + dim7[i]);
        priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawRose(root_note + dim7[i], new Vector3(random_x, random_y, 5f * priority), -3, 3, (int)(volume * 8), true, Dim7.transform);
        return;
    }
    public void DrawAMaj7(int root_note, int i, float random_x, float random_y, bool discrete = false)
    {
        GameObject AMaj7 = Instantiate(chordHolder);
        AMaj7.transform.position = Vector3.zero;
        AMaj7.transform.rotation = Quaternion.identity;
        AMaj7.GetComponent<HolderBehavior>().id = root_note;
        AMaj7.tag = "a-M7";

        float volume = MidiMaster.GetKey(root_note + augMaj7[i - 3]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawExplosion(root_note + augMaj7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), AMaj7.transform);

        DrawRose(root_note + augMaj7[i], new Vector3(random_x, random_y, 5f * priority), -3, 3, (int)(volume * 8), false, AMaj7.transform);
        return;
    }
    public void DrawADom7(int root_note, int i, float random_x, float random_y, bool discrete = false)
    {
        GameObject ADom7 = Instantiate(chordHolder);
        ADom7.transform.position = Vector3.zero;
        ADom7.transform.rotation = Quaternion.identity;
        ADom7.GetComponent<HolderBehavior>().id = root_note;
        ADom7.tag = "a-dom7";

        float volume = MidiMaster.GetKey(root_note + augDom7[i - 3]);
        int priority = (int)(5 * volume);
        if (discrete)
        {
            priority = discretePosition;
        }

        DrawExplosion(root_note + augDom7[i - 3], new Vector3(random_x, random_y, 5f * priority), -1, 1, (int)(volume * 8), ADom7.transform);

        DrawRose(root_note + augDom7[i], new Vector3(random_x, random_y, 5f * priority), -3, 3, (int)(volume * 8), true, ADom7.transform);
        return;
    }

    bool TestMajor3(int root_note)
    {
        int count = 0;
        for(int i = 0; i < major3.Length; ++i)
        {
            if(active[root_note + major3[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if(count == 3)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;

                foreach (var donut in GameObject.FindGameObjectsWithTag("Torus"))
                {
                    if (!donut.GetComponent<TorusBehavior>().keyup && 
                        (donut.GetComponent<TorusBehavior>().id == root_note + major3[i] ||
                         donut.GetComponent<TorusBehavior>().id == root_note + major3[i - 1] ||
                         donut.GetComponent<TorusBehavior>().id == root_note + major3[i - 2]))
                    {
                        random_x = donut.transform.position.x;
                        random_y = donut.transform.position.y;
                        break;
                    }
                }

                DrawMaj3(root_note, i, random_x, random_y);
                return true;
            }
        }
        return false;
    }
    bool TestMinor3(int root_note)
    {
        int count = 0;
        for (int i = 0; i < minor3.Length; ++i)
        {
            if (active[root_note + minor3[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 3)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;
                int shape_decider = Random.Range(0, 7);

                if (CheckSame("Circle", root_note + minor3[i], root_note + minor3[i - 1], root_note + minor3[i - 2], 
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }

                DrawMin3(root_note, i, random_x, random_y, shape_decider);
                return true;
            }
        }
        return false;
    }
    bool TestDim3(int root_note)
    {
        int count = 0;
        for (int i = 0; i < dim3.Length; ++i)
        {
            if (active[root_note + dim3[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 3)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;
                
                if (CheckSame("Circle", root_note + dim3[i], root_note + dim3[i - 1], root_note + dim3[i - 2],
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }
                if (CheckSame("Particle", root_note + dim3[i], root_note + dim3[i - 1], root_note + dim3[i - 2],
                                out tmpx, out tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }
                DrawDim3(root_note, i, random_x, random_y);
                return true;
            }
        }
        return false;
    }
    bool TestAug3(int root_note)
    {
        int count = 0;
        for (int i = 0; i < aug3.Length; ++i)
        {
            if (active[root_note + aug3[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 3)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;

                if (CheckSame("Particle", root_note + aug3[i], root_note + aug3[i - 1], root_note + aug3[i - 2],
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }
                DrawAug3(root_note, i, random_x, random_y);
                return true;
            }
        }
        return false;
    }
    bool TestMaj7(int root_note)
    {
        int count = 0;
        for (int i = 0; i < maj7.Length; ++i)
        {
            if (active[root_note + maj7[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 4)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;
                if (CheckSame("Torus", root_note + maj7[i], root_note + maj7[i - 1], root_note + maj7[i - 3],
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }

                DrawMaj7(root_note, i, random_x, random_y);
                return true;
            }
        }
        return false;
    }
    bool TestMin7(int root_note)
    {
        int count = 0;
        for (int i = 0; i < min7.Length; ++i)
        {
            if (active[root_note + min7[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 4)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;
                int shape_decider = Random.Range(0, 15);
                if (CheckSame("Circle", root_note + min7[i], root_note + min7[i - 1], root_note + min7[i - 3],
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }

                DrawMin7(root_note, i, random_x, random_y, shape_decider);
                return true;
            }
        }
        return false;
    }
    bool TestDom7(int root_note)
    {
        int count = 0;
        for (int i = 0; i < dom7.Length; ++i)
        {
            if (active[root_note + dom7[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 4)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;
                int shape_decider = Random.Range(0, 15);
                if (CheckSame("Circle", root_note + dom7[i], root_note + dom7[i - 1], root_note + dom7[i - 3],
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }

                DrawDom7(root_note, i, random_x, random_y, shape_decider);
                return true;
            }
        }
        return false;
    }
    bool TestMinMaj7(int root_note)
    {
        int count = 0;
        for (int i = 0; i < minMaj7.Length; ++i)
        {
            if (active[root_note + minMaj7[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 4)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;
                int shape_decider = Random.Range(0, 7);

                if (CheckSame("Circle", root_note + minMaj7[i - 3], root_note + minMaj7[i - 1], root_note + minMaj7[i],
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }

                DrawMM7(root_note, i, random_x, random_y, shape_decider);
                return true;
            }
        }
        return false;
    }
    bool TestHalfDim7(int root_note)
    {
        int count = 0;
        for (int i = 0; i < halfDim7.Length; ++i)
        {
            if (active[root_note + halfDim7[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 4)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;

                if (CheckSame("Circle", root_note + halfDim7[i], root_note + halfDim7[i - 1], root_note + halfDim7[i - 3],
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }
                if (CheckSame("Particle", root_note + halfDim7[i], root_note + halfDim7[i - 1], root_note + halfDim7[i - 3],
                                out tmpx, out tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }
                DrawHDim7(root_note, i, random_x, random_y);
                return true;
            }
        }
        return false;
    }
    bool TestDim7(int root_note)
    {
        int count = 0;
        for (int i = 0; i < dim7.Length; ++i)
        {
            if (active[root_note + dim7[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 4)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;
                int shape_decider = Random.Range(0, 7);

                if (CheckSame("Circle", root_note + dim7[i - 3], root_note + dim7[i - 1], root_note + dim7[i],
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }

                DrawDim7(root_note, i, random_x, random_y, shape_decider);
                return true;
            }
        }
        return false;
    }
    bool TestAugMaj7(int root_note)
    {
        int count = 0;
        for (int i = 0; i < augMaj7.Length; ++i)
        {
            if (active[root_note + augMaj7[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 4)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;

                if (CheckSame("Particle", root_note + augMaj7[i], root_note + augMaj7[i - 1], root_note + augMaj7[i - 3],
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }
                DrawAMaj7(root_note, i, random_x, random_y);
                return true;
            }
        }
        return false;
    }
    bool TestAugDom7(int root_note)
    {
        int count = 0;
        for (int i = 0; i < augDom7.Length; ++i)
        {
            if (active[root_note + augDom7[i]] == true)
            {
                count += 1;
            }
            else
            {
                count = 0;
            }
            if (count == 4)
            {
                float random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
                float random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;

                if (CheckSame("Particle", root_note + augDom7[i], root_note + augDom7[i - 1], root_note + augDom7[i - 3],
                                out float tmpx, out float tmpy))
                {
                    random_x = tmpx;
                    random_y = tmpy;
                }
                DrawADom7(root_note, i, random_x, random_y);
                return true;
            }
        }
        return false;
    }

    bool[] visited = new bool[130];
    void Update()
    {
        for(int i = min; i <= max; ++i)
        {
            active[i] = false;
            if(MidiMaster.GetKey(i) > 0f)
            {
                active[i] = true;
            }
            visited[i] = false;
        }

        for (int i = min; i <= max; ++i)
        {
            if (visited[i]) continue;
            if (TestMaj7(i))
            {
                for(int j = 0; j < 4; ++j)
                {
                    visited[i + maj7[j]] = true;
                }
                continue;
            }
            if(TestMin7(i))
            {
                for (int j = 0; j < 4; ++j)
                {
                    visited[i + min7[j]] = true;
                }
                continue;
            }
            if (TestDom7(i))
            {
                for (int j = 0; j < 4; ++j)
                {
                    visited[i + dom7[j]] = true;
                }
                continue;
            }
            if (TestMinMaj7(i))
            {
                for (int j = 0; j <4; ++j)
                {
                    visited[i + minMaj7[j]] = true;
                }
                continue;
            }
            if (TestHalfDim7(i))
            {
                for (int j = 0; j < 4; ++j)
                {
                    visited[i + halfDim7[j]] = true;
                }
                continue;
            }
            if (TestDim7(i))
            {
                for (int j = 0; j < 4; ++j)
                {
                    visited[i + dim7[j]] = true;
                }
                continue;
            }
            if (TestAugMaj7(i))
            {
                for (int j = 0; j < 4; ++j)
                {
                    visited[i + augMaj7[j]] = true;
                }
                continue;
            }
            if (TestAugDom7(i))
            {
                for (int j = 0; j < 4; ++j)
                {
                    visited[i + augDom7[j]] = true;
                }
                continue;
            }
          
            if (TestMajor3(i))
            {
                for (int j = 0; j < 3; ++j)
                {
                    visited[i + major3[j]] = true;
                }
                continue;
            }
            if (TestMinor3(i))
            {
                for (int j = 0; j < 3; ++j)
                {
                    visited[i + minor3[j]] = true;
                }
                continue;
            }
            if (TestDim3(i))
            {
                for (int j = 0; j < 3; ++j)
                {
                    visited[i + dim3[j]] = true;
                }
                continue;
            }
            if (TestAug3(i))
            {
                for (int j = 0; j < 3; ++j)
                {
                    visited[i + aug3[j]] = true;
                }
                continue;
            }
        }
    }
}
