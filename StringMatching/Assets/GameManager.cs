using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private TMP_Text searchWord;
    private TMP_Text searchSubject;
    private string[] stringMatchingMethods;
    private string currentMethod;
    bool found = false;
    // Start is called before the first frame update
    void Start()
    {
        searchWord = GameObject.Find("SearchString").GetComponent<TMP_Text>();
        searchSubject = GameObject.Find("Subject").GetComponent<TMP_Text>();

        stringMatchingMethods = new string[] {"StraightForward","Knuth-Morris-Pratt","Boyer-Moore(New)","Boyer-Moore(Old)","Approximate"};
        currentMethod = stringMatchingMethods[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        found = false;
        switch (currentMethod)
        {
            case "StraightForward":
                Debug.Log("StraightForward");
                if(StraighForward()>0)
                {
                    found = true;
                    Debug.Log("Found");
                }
                break;
        }

    }

    int StraighForward()
    {
        int m = SearchWord.Length;
        int i, j, k;
        int match;

        match = -1;
        i = 1;
        j = 1;
        k = 1;
        while(!endText(SearchSubject,j))
        {
            if(k>m)
            {
                match = i;
                break;
            }
            if (SearchSubject[j] == SearchWord[k])
            {
                j++; k++;
            }
            else
            {
                int backup = k - 1;
                j = j - backup;
                k = k - backup;
                j++;
                i = j;
            }
        }
        return match;
    }

    bool endText(string subject,int index)
    {
        if(index >subject.Length-1)
        {
            return true;
        }
        return false;
    }
    public void UpdateAlgorithim(TMP_Dropdown dropDown)
    {
        currentMethod = stringMatchingMethods[dropDown.value];
    }

    public string SearchWord
    {
        get { return searchWord.text; }
    }
    public string SearchSubject
    {
        get { return searchSubject.text; }
    }

}
