using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    private TMP_Text searchWord;
    private TMP_Text searchSubject;
    private string[] stringMatchingMethods;
    private string currentMethod;
    bool found = false;

    DateTime before;
    DateTime after;
    TimeSpan duration;

    int NO_OF_CHARS = 256;
    int max(int a, int b) { return (a > b) ? a : b; }




    // Start is called before the first frame update
    void Start()
    {
        searchWord = GameObject.Find("SearchString").GetComponent<TMP_Text>();
        searchSubject = GameObject.Find("Subject").GetComponent<TMP_Text>();

        stringMatchingMethods = new string[] { "StraightForward", "Knuth-Morris-Pratt", "Boyer-Moore(New)"};
        currentMethod = stringMatchingMethods[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
      
        int len;
        found = false;
        switch (currentMethod)
        {
            case "StraightForward":
                Debug.Log("StraightForward");

                before = DateTime.Now;

                len = StraighForward(SearchSubject.Substring(0, SearchSubject.Length - 1), SearchWord.Substring(0, SearchWord.Length - 1));

                 after = DateTime.Now;
                duration = after.Subtract(before);

                Debug.Log("Duration in milliseconds: " + duration.Milliseconds);
                if (len > 0)
                {
                    found = true;
                    Debug.Log("Found");
                }
                break;
            case "Knuth-Morris-Pratt":
                Debug.Log("Knuth-Morris-Pratt");

                before = DateTime.Now;

                len = KnuthMorrisPratt(SearchSubject.Substring(0, SearchSubject.Length - 1), SearchWord.Substring(0, SearchWord.Length - 1));

                after = DateTime.Now;
                duration = after.Subtract(before);

                Debug.Log("Total Duration in milliseconds: " + duration.Milliseconds);
                if (len > 0)
                {
                    found = true;
                    Debug.Log("Found");
                }
                break;
             case "BoyerMoore":
                Debug.Log("BoyerMoore");

                before = DateTime.Now;

                len = BoyerMoore(SearchSubject.Substring(0, SearchSubject.Length - 1), SearchWord.Substring(0, SearchWord.Length - 1));

                after = DateTime.Now;
                duration = after.Subtract(before);

                Debug.Log("Total Duration in milliseconds: " + duration.Milliseconds);
                if (len > 0)
                {
                    found = true;
                    Debug.Log("Found");
                }
                break;
        }

    }

    int StraighForward(String txt, String pat)
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
        Debug.Log("\tKnuth-Morris-Pratt Preprocessing");

        before = DateTime.Now;

        computeLPSArray(pat, M, lps);

        after = DateTime.Now;
        duration = after.Subtract(before);

        Debug.Log("\tDuration in milliseconds: " + duration.Milliseconds);

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
        Debug.Log("\tBoyerMoore Preprocessing");

        before = DateTime.Now;

        badCharHeuristic(pat, m, badchar);
        after = DateTime.Now;
        duration = after.Subtract(before);

        Debug.Log("Total Duration in milliseconds: " + duration.Milliseconds);

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

    public string SearchWord
    {
        get { return searchWord.text; }
    }
    public string SearchSubject
    {
        get { return searchSubject.text; }
    }

}
