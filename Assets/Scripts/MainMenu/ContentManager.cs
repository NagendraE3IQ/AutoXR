using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;
using Microsoft.Win32;
using UnityEngine.XR;
using System.Net;
using System.Linq;

public class ContentManager : MonoBehaviour
{
    public GameObject RecentlyAssignedCourses, ContinueLearningCourses;
    [SerializeField]
    int ShowNumberofRecentlyAssignedCourses, ShowNumberofContinueLearningCourses;

    [Header("ASSIGNED COURSES")]
    public GameObject AssignedCoursesPanel;
    public GameObject AssignedCoursesContentPanel;
    public int NumberOfAssignedCourses;
    public TextMeshProUGUI NumberofAssignedCoursesText;

    [Header("PENDING COURSES")]
    public GameObject InProgressCoursesPanel;
    public GameObject InProgressCoursesContentPanel;
    public int NumberOfPendingCourses;

    [Header("COMPLETED COURSES")]
    public GameObject CompletedCoursesPanel;
    public GameObject CompletedCoursesContentPanel;
    public int NumberOfCompletedCourses;

    public SearchField SearchField;
    public GameObject ContentLoadingPanel, SteamVRAppNotInstalledPanel, VRDeviceConectedPanel, InternetSpeedPanel;

    int totalDepartmentcourses;
    string CourseName;
    string CourseDescription;
    string CourseURL;
    string CourseIconURL;
    string CourseLevel;

    public static ContentManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        LoadAllAssignedCourses();

        ShowAllHomeScreenCourses();
    }

    public void ShowAllHomeScreenCourses()
    {
        int i = AssignedCoursesContentPanel.transform.childCount;

        if(RecentlyAssignedCourses.transform.childCount == 0)
        {
            for (int j = 1; j < 5; j++)
            {
                AssignedCoursesContentPanel.transform.GetChild(i - j).transform.gameObject.GetComponent<RectTransform>().SetParent(RecentlyAssignedCourses.transform);
                RecentlyAssignedCourses.transform.GetChild(j - 1).transform.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void SetAllCourses()
    {
        int i = RecentlyAssignedCourses.transform.childCount;

        if(i > 0)
        {
            for (int j = 1; j < 5; j++)
            {
                RecentlyAssignedCourses.transform.GetChild(i - j).transform.gameObject.GetComponent<RectTransform>().SetParent(AssignedCoursesContentPanel.transform);
            }

            for(int a = 0; a < AssignedCoursesContentPanel.transform.childCount; a++)
            {
                AssignedCoursesContentPanel.transform.GetChild(a).transform.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }     
    }
    public void LoadAllAssignedCourses()
    {       
        NumberOfAssignedCourses = UserData.Instance.CourseDocumnets.Count;
        NumberofAssignedCoursesText.text = "( " + NumberOfAssignedCourses.ToString() + " )";
        if (NumberOfAssignedCourses > 0)
        {
            for (int j = 0; j < NumberOfAssignedCourses; j++)
            {
                CourseName = UserData.Instance.CourseDocumnets[j]["moduleName"].ToString();
                CourseDescription = UserData.Instance.CourseDocumnets[j]["description"].ToString();
                CourseURL = UserData.Instance.CourseDocumnets[j]["uploadFile"].ToString();
                CourseIconURL = UserData.Instance.CourseDocumnets[j]["thumbnail"].ToString();
                CourseLevel = UserData.Instance.CourseDocumnets[j]["level"].ToString();

                SearchField.CoursesNames.Add(CourseName);

                ArrangeAllCards(AssignedCoursesContentPanel, "CourseAssignedCard");
            }
        }
    }

    void ArrangeAllCards(GameObject g, string coursecardtype)
    {
        GameObject Card = (GameObject)Resources.Load(coursecardtype);
        var button = Instantiate(Card, transform.position, transform.rotation);

        button.GetComponent<RectTransform>().SetParent(g.transform);
        button.transform.localScale = new Vector3(1, 1, 1);

        if (g.name == "AssignedContent")
        {
            button.name = CourseName;
            button.GetComponent<AssignedCardDataDetails>().CourseName = CourseName;
            button.GetComponent<AssignedCardDataDetails>().CourseDescription = CourseDescription;
            button.GetComponent<AssignedCardDataDetails>().CourseURL = CourseURL;
            button.GetComponent<AssignedCardDataDetails>().CourseIcon = CourseIconURL;
            button.GetComponent<AssignedCardDataDetails>().CourseLevel.text = CourseLevel;
        }
        else if (g.name == "InProgressContent")
        {
            button.name = CourseName;
            button.GetComponent<PendingCardDataDetails>().CourseName = CourseName;
            button.GetComponent<PendingCardDataDetails>().CourseDescription = CourseDescription;
            button.GetComponent<PendingCardDataDetails>().CourseURL = CourseURL;
            button.GetComponent<PendingCardDataDetails>().CourseIcon = CourseIconURL;
        }
        else if (g.name == "CompletedContent")
        {
            button.name = CourseName;
            button.GetComponent<CompletedCardDataDetails>().CourseName = CourseName;
            button.GetComponent<CompletedCardDataDetails>().CourseDescription = CourseDescription;
            button.GetComponent<CompletedCardDataDetails>().CourseURL = CourseURL;
            button.GetComponent<CompletedCardDataDetails>().CourseIcon = CourseIconURL;
        }
    }

    public void OnAssignedTabBtn()
    {
        DisableAllTaabs();
        AssignedCoursesPanel.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnPendingTabBtn()
    {
        DisableAllTaabs();
        InProgressCoursesPanel.transform.localScale = new Vector3(1, 1, 1);
    }
    public void OnCompletedTabBtn()
    {
        DisableAllTaabs();
        CompletedCoursesPanel.transform.localScale = new Vector3(1, 1, 1);
    }

    void DisableAllTaabs()
    {
        AssignedCoursesPanel.transform.localScale = new Vector3(0, 0, 0);
        InProgressCoursesPanel.transform.localScale = new Vector3(0, 0, 0);
        CompletedCoursesPanel.transform.localScale = new Vector3(0, 0, 0);
    }

    List<Transform> tobject = new List<Transform>();
    List<int> sibling = new List<int>();
    public void OnSearchButtonClicked()
    {
        if (SearchField.IPF.text.Length != 0)
        {
            foreach (Transform g in AssignedCoursesContentPanel.transform)
            {
                bool contains = g.transform.gameObject.name.IndexOf(SearchField.IPF.text, StringComparison.CurrentCultureIgnoreCase) >= 0;
                if (contains)
                {
                    g.transform.gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    g.transform.gameObject.transform.localScale = new Vector3(0, 0, 0);
                    tobject.Add(g);
                    sibling.Add(g.transform.GetSiblingIndex());
                }
            }

            for (int i = 0; i < tobject.Count; i++)
            {
                tobject[i].transform.parent = null;
            }

            SearchField.SearchCloseButton.SetActive(true);
        }
    }

    public void OnSearchCloseBtnClicked()
    {
        for (int i = 0; i < tobject.Count; i++)
        {
            tobject[i].transform.gameObject.GetComponent<RectTransform>().SetParent(AssignedCoursesContentPanel.transform);
            tobject[i].transform.gameObject.transform.localScale = new Vector3(1, 1, 1);
            tobject[i].SetSiblingIndex(sibling[i]);
        }
       
        tobject.Clear();
        sibling.Clear();
        SearchField.SearchIcon.SetActive(true);
        SearchField.SearchCloseButton.SetActive(false);
        SearchField.IPF.text = "";
    }

    int NewAssignedCourses;
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            UserData.Instance.GetUserData();
            int i = 0;

            foreach (var c in UserData.Instance.CourseDocumnets)
            {
                i++;
            }
            if (NumberOfAssignedCourses < i)
            {
                NumberofAssignedCoursesText.text = "( " + i + " )";
                NewAssignedCourses = i - NumberOfAssignedCourses;

                for (int j = 0; j < NewAssignedCourses; j++)
                {
                    CourseName = UserData.Instance.CourseDocumnets[NumberOfAssignedCourses + j]["moduleName"].ToString();
                    CourseDescription = UserData.Instance.CourseDocumnets[NumberOfAssignedCourses + j]["description"].ToString();
                    CourseURL = UserData.Instance.CourseDocumnets[NumberOfAssignedCourses + j]["uploadFile"].ToString();
                    CourseIconURL = UserData.Instance.CourseDocumnets[NumberOfAssignedCourses + j]["thumbnail"].ToString();
                    CourseLevel = UserData.Instance.CourseDocumnets[NumberOfAssignedCourses + j]["level"].ToString();

                    SearchField.CoursesNames.Add(CourseName);

                    ArrangeAllCards(AssignedCoursesContentPanel, "CourseAssignedCard");
                }
                NumberOfAssignedCourses = i;
                ShowAllHomeScreenCourses();
            }
        }
    }

    public void RetryAppInstalledorNot()
    {
        //if (IsApplictionInstalled("SteamVR"))
        SteamVRAppNotInstalledPanel.SetActive(false);
    }

    public bool IsApplictionInstalled(string p_name)
    {
        string displayName;
        RegistryKey key;

        // search in: CurrentUser
        key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
        foreach (String keyName in key.GetSubKeyNames())
        {
            RegistryKey subkey = key.OpenSubKey(keyName);
            displayName = subkey.GetValue("DisplayName") as string;
            if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
        }

        // search in: LocalMachine_32
        key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
        foreach (String keyName in key.GetSubKeyNames())
        {
            RegistryKey subkey = key.OpenSubKey(keyName);
            displayName = subkey.GetValue("DisplayName") as string;
            if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
        }

        // search in: LocalMachine_64
        key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
        foreach (String keyName in key.GetSubKeyNames())
        {
            RegistryKey subkey = key.OpenSubKey(keyName);
            displayName = subkey.GetValue("DisplayName") as string;
            if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
        }

        // NOT FOUND
        return false;
    }

    public void InterNetSpeed()
    {
        PlayerPrefs.SetInt("InterNetSpeedShow", 1);
        InternetSpeedPanel.SetActive(false);
    }

    public double CheckSpeed()
    {
        double[] speeds = new double[5];
        for (int i = 0; i < 5; i++)
        {
            int FileSize = 21; //Size of File in KB.
            WebClient client = new WebClient();
            DateTime startTime = DateTime.Now;
            client.DownloadFile("https://autoxr-admin.s3.us-west-1.amazonaws.com/AssembleLearn.png", @Application.dataPath + "/speedtest.png");
            DateTime endTime = DateTime.Now;
            speeds[i] = Math.Round((FileSize / (endTime - startTime).TotalSeconds));
        }
        Debug.Log(string.Format("Download Speed: {0}MB/s", speeds.Average() / 1000));
        return speeds.Average() / 1000;
    }
}
