using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Experimental.AI;

public class GameManager : MonoBehaviour
{
    private string word = "Rowan";
    private string subject;
    private string[] stringMatchingMethods;

    //"DNA","2CharSequence","Lorem ipsum","SimilarWordings"
    private string[] dataSets;
    private string currentMethod;
    private string dataSetName;
    private float time;
    private float preprocessingTime;
    bool found = false;

    int datalen = 100000;

    DateTime before;
    DateTime after;
    TimeSpan duration;

    int NO_OF_CHARS = 256;
    int max(int a, int b) { return (a > b) ? a : b; }




    // Start is called before the first frame update
    void Start()
    {
        Sheets.ConnectToGoogle();
        stringMatchingMethods = new string[] { "StraightForward", "Knuth-Morris-Pratt", "Boyer-Moore"};
        dataSets = new string[4];
        DNAInit();
        TwoCharInit();
        LoremIpsumInit();
        SimilarWordings();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        Sheets.ClearRequest();
        for (int i = 0;i<3;i++)
        {
            for (int j = 0;j<4;j++)
            {
                Expirement(i,j);
            }
        }

    }
    

    void Expirement(int algorithim,int dataSet)
    {
        if(dataSet == 1)
        {
            word = "aabb";
        }
        else
        {
            word = "Rowan";
        }
        switch (dataSet)
        {
            case 0:
                dataSetName = "DNA";
                break;
            case 1:
                dataSetName = "2Char";
                break;
            case 2:
                dataSetName = "LoremIpsum";
                break;
            case 3:
                dataSetName = "SimilarWords";
                break;
        }

        int len;
        found = false;
        subject = dataSets[dataSet];
        currentMethod = stringMatchingMethods[algorithim];
        switch (currentMethod)
        {
            case "StraightForward":
                //Debug.Log("StraightForward");

                before = DateTime.Now;

                len = StraighForward(subject, word);

                after = DateTime.Now;
                duration = after.Subtract(before);
                time = duration.Milliseconds;
                //Debug.Log("Duration in milliseconds: " + duration.Milliseconds);
        
                if (len > 0)
                {
                    found = true;
                    Debug.Log("Found");
                }
                Sheets.AddScoreEntry(currentMethod, word, dataSetName, time, preprocessingTime,found);
                break;
            case "Knuth-Morris-Pratt":
                //Debug.Log("Knuth-Morris-Pratt");

                before = DateTime.Now;

                len = KnuthMorrisPratt(subject, word);

                after = DateTime.Now;
                duration = after.Subtract(before);
                time = duration.Milliseconds;
                //Debug.Log("Total Duration in milliseconds: " + duration.Milliseconds);

                if (len > 0)
                {
                    found = true;
                    Debug.Log("Found");
                }
                Sheets.AddScoreEntry(currentMethod, word, dataSetName, time, preprocessingTime, found);
                break;
            case "Boyer-Moore":
               // Debug.Log("BoyerMoore");

                before = DateTime.Now;

                len = BoyerMoore(subject, word);

                after = DateTime.Now;
                duration = after.Subtract(before);
                time = duration.Milliseconds;
                //Debug.Log("Total Duration in milliseconds: " + duration.Milliseconds);
  
                if (len > 0)
                {
                    found = true;
                    Debug.Log("Found");
                }
                Sheets.AddScoreEntry(currentMethod, word, dataSetName, time, preprocessingTime, found);
                break;
        }
    }

    int StraighForward(string txt, string pat)
    {
        int M = pat.Length;
        int N = txt.Length;

        /* A loop to slide pat one by one */
        for (int i = 0; i <= N - M; i++)
        {
            int j;

            /* For current index i, check for pattern  
            match */
            for (j = 0; j < M; j++)
            {
                if (txt[i + j] != pat[j])
                {
                    break;
                }
            }

            // if pat[0...M-1] = txt[i, i+1, ...i+M-1] 
            if (j == M)
            {
                Debug.Log("Pattern found at index " + i);
                return i;
            }
        }
        return -1;
    }
    int KnuthMorrisPratt(String txt, String pat)
    {
        int M = pat.Length;
        int N = txt.Length;

        // create lps[] that will hold the longest 
        // prefix suffix values for pattern 
        int[] lps = new int[M];
        int j = 0; // index for pat[] 

        // Preprocess the pattern (calculate lps[] 
        // array) 

        before = DateTime.Now;

        computeLPSArray(pat, M, lps);

        after = DateTime.Now;
        duration = after.Subtract(before);

        preprocessingTime = duration.Milliseconds;

        int i = 0; // index for txt[] 
        while (i < N)
        {
            if (pat[j] == txt[i])
            {
                j++;
                i++;
            }
            if (j == M)
            {
                Console.Write("Found pattern "
                              + "at index " + (i - j));
                j = lps[j - 1];
                return (i-j);
            }

            // mismatch after j matches 
            else if (i < N && pat[j] != txt[i])
            {
                // Do not match lps[0..lps[j-1]] characters, 
                // they will match anyway 
                if (j != 0)
                    j = lps[j - 1];
                else
                    i = i + 1;
            }
        }
        return -1;
    }
    void computeLPSArray(string pat, int M, int[] lps)
    {
        // length of the previous longest prefix suffix 
        int len = 0;
        int i = 1;
        lps[0] = 0; // lps[0] is always 0 

        // the loop calculates lps[i] for i = 1 to M-1 
        while (i < M)
        {
            if (pat[i] == pat[len])
            {
                len++;
                lps[i] = len;
                i++;
            }
            else // (pat[i] != pat[len]) 
            {
                // This is tricky. Consider the example. 
                // AAACAAAA and i = 7. The idea is similar 
                // to search step. 
                if (len != 0)
                {
                    len = lps[len - 1];

                    // Also, note that we do not increment 
                    // i here 
                }
                else // if (len == 0) 
                {
                    lps[i] = len;
                    i++;
                }
            }
        }
    }
    int BoyerMoore(String txt,String pat)
    {
        int m = pat.Length;
        int n = txt.Length;

        int[] badchar = new int[NO_OF_CHARS];

        /* Fill the bad character array by calling  
            the preprocessing function badCharHeuristic()  
            for given pattern */

        before = DateTime.Now;

        badCharHeuristic(pat, m, badchar);
        after = DateTime.Now;
        duration = after.Subtract(before);

        preprocessingTime = duration.Milliseconds;

        int s = 0; // s is shift of the pattern with  
                   // respect to text  
        while (s <= (n - m))
        {
            int j = m - 1;

            /* Keep reducing index j of pattern while  
                characters of pattern and text are  
                matching at this shift s */
            while (j >= 0 && pat[j] == txt[s + j])
                j--;

            /* If the pattern is present at current  
                shift, then index j will become -1 after  
                the above loop */
            if (j < 0)
            {
                Console.WriteLine("Patterns occur at shift = " + s);
                
                /* Shift the pattern so that the next  
                    character in text aligns with the last  
                    occurrence of it in pattern.  
                    The condition s+m < n is necessary for  
                    the case when pattern occurs at the end  
                    of text */
                s += (s + m < n) ? m - badchar[txt[s + m]] : 1;
                return s;
            }
            else
            {
                /* Shift the pattern so that the bad character  
                    in text aligns with the last occurrence of  
                    it in pattern. The max function is used to  
                    make sure that we get a positive shift.  
                    We may get a negative shift if the last  
                    occurrence of bad character in pattern  
                    is on the right side of the current  
                    character. */
                s += max(1, j - badchar[txt[s + j]]);
            }
        }
        return -1;
    }
    void badCharHeuristic(String str, int size, int[] badchar)
    {
        int i;
        // Initialize all occurrences as -1  
        for (i = 0; i < NO_OF_CHARS; i++)
            badchar[i] = -1;

        // Fill the actual value of last occurrence  
        // of a character  
        for (i = 0; i < size; i++)
            badchar[(int)str[i]] = i;
    }
    public void UpdateAlgorithim(TMP_Dropdown dropDown)
    {
        currentMethod = stringMatchingMethods[dropDown.value];
    }

    void DNAInit()
    {
        int num;
        string output = "";
        int pos = UnityEngine.Random.Range(0, datalen - word.Length);
        for (int i = 0; i < datalen; i++)
        {
            num = UnityEngine.Random.Range(0, 4);
            if (i == pos)
            {
                output += word;
            }
            if (num == 0)
            {
                output += "A";
            }
            if (num == 1)
            {
                output += "T";
            }
            if (num == 2)
            {
                output += "C";
            }
            if (num == 3)
            {
                output += "G";
            }
        }
        dataSets[0] = output;

    }
    void TwoCharInit()
    {
        int len1 = UnityEngine.Random.Range(0,datalen-100);
        int len2 =datalen-len1;
        string output = "";
        for(int i =0;i<len1;i++)
        {
            output += "a";
        }
        output += "aabb";
        for (int j = 0; j < len2; j++)
        {
            output += "b";
        }
        dataSets[1] = output;
    }
    void LoremIpsumInit()
    {
        string loremIpsum = "";
        for (int i = 0;i<datalen/1000;i++)
        {
            loremIpsum += Resources.Load<TextAsset>("LoremIpsum").text.Trim('\n');
        }
        int num = UnityEngine.Random.Range(0,loremIpsum.Length-word.Length);
        string first = loremIpsum.Substring(0,num);
        string second = loremIpsum.Substring(num, loremIpsum.Length-num);
        dataSets[2] = first + word + second;
    }
    void SimilarWordings()
    {
        string output = "";
        for(int i = 0;i<datalen;i++)
        {
            int len = UnityEngine.Random.Range(0,word.Length-1);
            output += word.Substring(len);
        }
        dataSets[3] = output;
    }



}
