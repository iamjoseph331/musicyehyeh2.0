using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class DiscreteChord : MonoBehaviour
{
    private int[] major3 = { 0, 4, 7, 12, 16 };
    private int[] minor3 = { 0, 3, 7, 12, 15 };
    private int[] dim3 = { 0, 3, 6, 12, 15 };
    private int[] aug3 = { 0, 4, 8, 12, 16 };

    private int[] maj7 = { 0, 4, 7, 11, 12, 16, 19 };
    private int[] min7 = { 0, 3, 7, 10, 12, 15, 19 };
    private int[] dom7 = { 0, 4, 7, 10, 12, 16, 19 };
    private int[] minMaj7 = { 0, 3, 7, 11, 12, 15, 19 };
    private int[] halfDim7 = { 0, 3, 6, 10, 12, 15, 18 };
    private int[] dim7 = { 0, 3, 6, 9, 12, 15, 18 };
    private int[] augMaj7 = { 0, 4, 8, 11, 12, 16, 20 };
    private int[] augDom7 = { 0, 4, 8, 10, 12, 16, 20 };

    private bool[] checklist = new bool[130];
    private float ScrHeight, ScrWidth, random_x, random_y;
    ChordActor ca;
    void Start()
    {
        ca = transform.GetComponent<ChordActor>();
        for(int i = 0; i < checklist.Length; ++i)
        {
            checklist[i] = false;
        }
        ScrHeight = Camera.main.orthographicSize * 2f;
        ScrWidth = ScrHeight * Screen.width / Screen.height;
    }

    bool TestMajor3(int root)
    {
        int cnt = 0, len = 3;
        for(int i = 0; i < major3.Length; ++i)
        {
            if(checklist[root + major3[i]])
            {
                cnt += 1;
                print(cnt);
            }
            else
            {
                cnt = 0;
            }
            if(cnt == len)
            {
                ca.DrawMaj3(root, i, random_x, random_y, false);
                return true;
            }
        }
        return false;
    }
    bool TestMinor3(int root)
    {
        int cnt = 0, len = 3;
        for (int i = 0; i < minor3.Length; ++i)
        {
            if (checklist[root + minor3[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawMin3(root, i, random_x, random_y, Random.Range(0,7), true);
                return true;
            }
        }
        return false;
    }
    bool TestDim3(int root)
    {
        int cnt = 0, len = 3;
        for (int i = 0; i < dim3.Length; ++i)
        {
            if (checklist[root + dim3[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawDim3(root, i, random_x, random_y, true);
                return true;
            }
        }
        return false;
    }
    bool TestAug3(int root)
    {
        int cnt = 0, len = 3;
        for (int i = 0; i < aug3.Length; ++i)
        {
            if (checklist[root + aug3[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawAug3(root, i, random_x, random_y, true);
                return true;
            }
        }
        return false;
    }
    bool TestMaj7(int root)
    {
        int cnt = 0, len = 4;
        for (int i = 0; i < maj7.Length; ++i)
        {
            if (checklist[root + maj7[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawMaj7(root, i, random_x, random_y, true);
                return true;
            }
        }
        return false;
    }
    bool TestMin7(int root)
    {
        int cnt = 0, len = 4;
        for (int i = 0; i < min7.Length; ++i)
        {
            if (checklist[root + min7[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawMin7(root, i, random_x, random_y, Random.Range(0, 7), true);
                return true;
            }
        }
        return false;
    }
    bool TestDom7(int root)
    {
        int cnt = 0, len = 4;
        for (int i = 0; i < dom7.Length; ++i)
        {
            if (checklist[root + dom7[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawDom7(root, i, random_x, random_y, Random.Range(0,7), true);
                return true;
            }
        }
        return false;
    }
    bool TestMinMaj7(int root)
    {
        int cnt = 0, len = 4;
        for (int i = 0; i < minMaj7.Length; ++i)
        {
            if (checklist[root + minMaj7[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawMM7(root, i, random_x, random_y, Random.Range(0, 7), true);
                return true;
            }
        }
        return false;
    }
    bool TestHalfDim7(int root)
    {
        int cnt = 0, len = 4;
        for (int i = 0; i < halfDim7.Length; ++i)
        {
            if (checklist[root + halfDim7[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawHDim7(root, i, random_x, random_y, true);
                return true;
            }
        }
        return false;
    }
    bool TestDim7(int root)
    {
        int cnt = 0, len = 4;
        for (int i = 0; i < dim7.Length; ++i)
        {
            if (checklist[root + dim7[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawDim7(root, i, random_x, random_y, Random.Range(0, 7), true);
                return true;
            }
        }
        return false;
    }
    bool TestAugMaj7(int root)
    {
        int cnt = 0, len = 4;
        for (int i = 0; i < augMaj7.Length; ++i)
        {
            if (checklist[root + augMaj7[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawAMaj7(root, i, random_x, random_y, true);
                return true;
            }
        }
        return false;
    }
    bool TestAugDom7(int root)
    {
        int cnt = 0, len = 4;
        for (int i = 0; i < augDom7.Length; ++i)
        {
            if (checklist[root + augDom7[i]])
            {
                cnt += 1;
            }
            else
            {
                cnt = 0;
            }
            if (cnt == len)
            {
                ca.DrawADom7(root, i, random_x, random_y, true);
                return true;
            }
        }
        return false;
    }


    bool[] visited = new bool[130];
    void Update()
    {
        for(int i = 0; i < 130; ++i)
        {
            visited[i] = false;
            checklist[i] = false;
        }
        foreach (GameObject note in GameObject.FindGameObjectsWithTag("Note"))
        {
            checklist[note.GetComponent<DeleteNote>().id] = true;
        }
        for (int i = 0; i < checklist.Length; ++i)
        {
            random_y = Random.Range(0, ScrHeight) - ScrHeight / 2;
            random_x = Random.Range(0, ScrWidth) - ScrWidth / 2;
            if (checklist[i])
            {
                if (visited[i]) continue;
                if (TestMaj7(i))
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        visited[i + maj7[j]] = true;
                    }
                    continue;
                }
                if (TestMin7(i))
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
                    for (int j = 0; j < 4; ++j)
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
}
