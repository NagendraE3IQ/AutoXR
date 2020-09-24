using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;

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
    public GameObject ContentLoadingPanel;

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

        //NumberOfAssignedCourses = UserData.Instance.UserDocument["assignedcourses"].AsBsonArray.ToList().Capacity;
        //Debug.Log(NumberOfAssignedCourses);

        //NumberOfPendingCourses = UserData.Instance.UserDocument["pendingcourses"].AsBsonArray.ToList().Capacity;
        //Debug.Log(NumberOfPendingCourses);

        //NumberOfCompletedCourses = UserData.Instance.UserDocument["completedcourses"].AsBsonArray.ToList().Capacity;
        //Debug.Log(NumberOfCompletedCourses);

        //ShowHomeScreenCourses();

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
            }
        }
        

        //foreach(Transform t in RecentlyAssignedCourses.transform)
        //{
        //    Destroy(t.gameObject);
        //}

        //for (int i = 1; i <= ShowNumberofRecentlyAssignedCourses; i++)
        //{
        //    if (AssignedCoursesContentPanel.transform.childCount >= i)
        //    {
        //        GameObject Card = (GameObject)Resources.Load("CourseAssignedCard");
        //        var button = Instantiate(Card, transform.position, transform.rotation);

        //        button.GetComponent<RectTransform>().SetParent(RecentlyAssignedCourses.transform);
        //        button.transform.localScale = new Vector3(1, 1, 1);

        //        button.name = AssignedCoursesContentPanel.transform.GetChild(AssignedCoursesContentPanel.transform.childCount - i).GetComponent<AssignedCardDataDetails>().CourseName;
        //        button.GetComponent<AssignedCardDataDetails>().CourseName = AssignedCoursesContentPanel.transform.GetChild(AssignedCoursesContentPanel.transform.childCount - i).GetComponent<AssignedCardDataDetails>().CourseName;
        //        button.GetComponent<AssignedCardDataDetails>().CourseDescription = AssignedCoursesContentPanel.transform.GetChild(AssignedCoursesContentPanel.transform.childCount - i).GetComponent<AssignedCardDataDetails>().CourseDescription;
        //        button.GetComponent<AssignedCardDataDetails>().CourseURL = AssignedCoursesContentPanel.transform.GetChild(AssignedCoursesContentPanel.transform.childCount - i).GetComponent<AssignedCardDataDetails>().CourseURL;
        //        button.GetComponent<AssignedCardDataDetails>().CourseIcon = AssignedCoursesContentPanel.transform.GetChild(AssignedCoursesContentPanel.transform.childCount - i).GetComponent<AssignedCardDataDetails>().CourseIcon;
        //        button.GetComponent<AssignedCardDataDetails>().CourseLevel.text = AssignedCoursesContentPanel.transform.GetChild(AssignedCoursesContentPanel.transform.childCount - i).GetComponent<AssignedCardDataDetails>().CourseLevel.text;
        //    }
        //}
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
        }     
    }
    public void LoadAllAssignedCourses()
    {
        int i = 0;
        foreach (var c in UserData.Instance.CourseDocumnets)
        {
            i++;
        }
        NumberOfAssignedCourses = i;
        NumberofAssignedCoursesText.text = "( " + i + " )";
        if (i > 0)
        {
            for (int j = 0; j < i; j++)
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

    //OLD Structure Code//
    //void ShowHomeScreenCourses()
    //{
    //    for (int i = 1; i <= ShowNumberofRecentlyAssignedCourses; i++)
    //    {
    //        if (NumberOfAssignedCourses >= i)
    //        {
    //            CourseName = UserData.Instance.UserDocument["assignedcourses"][NumberOfAssignedCourses - i]["name"].ToString();
    //            CourseDescription = UserData.Instance.UserDocument["assignedcourses"][NumberOfAssignedCourses - i]["description"].ToString();
    //            CourseURL = UserData.Instance.UserDocument["assignedcourses"][NumberOfAssignedCourses - i]["link"].ToString();
    //            CourseIconURL = UserData.Instance.UserDocument["assignedcourses"][NumberOfAssignedCourses - i]["icon"].ToString();

    //            GameObject Card = (GameObject)Resources.Load("CourseAssignedCard");
    //            var button = Instantiate(Card, transform.position, transform.rotation);

    //            button.GetComponent<RectTransform>().SetParent(RecentlyAssignedCourses.transform);
    //            button.transform.localScale = new Vector3(1, 1, 1);

    //            button.name = CourseName;
    //            button.GetComponent<AssignedCardDataDetails>().CourseName = CourseName;
    //            button.GetComponent<AssignedCardDataDetails>().CourseDescription = CourseDescription;
    //            button.GetComponent<AssignedCardDataDetails>().CourseURL = CourseURL;
    //            button.GetComponent<AssignedCardDataDetails>().CourseIcon = CourseIconURL;
    //        }
    //    }

    //    for (int i = 1; i <= ShowNumberofContinueLearningCourses; i++)
    //    {
    //        if (NumberOfPendingCourses >= i)
    //        {
    //            CourseName = UserData.Instance.UserDocument["pendingcourses"][NumberOfPendingCourses - i]["name"].ToString();
    //            CourseDescription = UserData.Instance.UserDocument["pendingcourses"][NumberOfPendingCourses - i]["description"].ToString();
    //            CourseURL = UserData.Instance.UserDocument["pendingcourses"][NumberOfPendingCourses - i]["link"].ToString();
    //            CourseIconURL = UserData.Instance.UserDocument["pendingcourses"][NumberOfPendingCourses - i]["icon"].ToString();

    //            GameObject Card = (GameObject)Resources.Load("CoursePendingCard");
    //            var button = Instantiate(Card, transform.position, transform.rotation);

    //            button.GetComponent<RectTransform>().SetParent(ContinueLearningCourses.transform);
    //            button.transform.localScale = new Vector3(1, 1, 1);

    //            button.name = CourseName;
    //            button.GetComponent<AssignedCardDataDetails>().CourseName = CourseName;
    //            button.GetComponent<AssignedCardDataDetails>().CourseDescription = CourseDescription;
    //            button.GetComponent<AssignedCardDataDetails>().CourseURL = CourseURL;
    //            button.GetComponent<AssignedCardDataDetails>().CourseIcon = CourseIconURL;
    //        }
    //    }
    //}

    //public void ShowAllContentCourses()
    //{
    //    GetAllCardsData("assignedcourses", NumberOfAssignedCourses, "AssignedScrollView", "CourseAssignedCard", AssignedCoursesPanel);
    //    GetAllCardsData("pendingcourses", NumberOfPendingCourses, "PendingScrollView", "CoursePendingCard", PendingCoursesPanel);
    //    GetAllCardsData("completedcourses", NumberOfCompletedCourses, "CompletedScrollView", "CourseCompletedCard", CompletedCoursesPanel);
    //}

    //void GetAllCardsData(string coursestype, int numberofcardsineachtype, string CoursescrollviewName, string coursescardtype, GameObject panel)
    //{
    //    for (int i = 0; i < numberofcardsineachtype; i++)
    //    {
    //        CourseName = UserData.Instance.UserDocument[coursestype][i]["name"].ToString();
    //        CourseDescription = UserData.Instance.UserDocument[coursestype][i]["description"].ToString();
    //        CourseURL = UserData.Instance.UserDocument[coursestype][i]["link"].ToString();
    //        CourseIconURL = UserData.Instance.UserDocument[coursestype][i]["icon"].ToString();

    //        ArrangeAllCards(CoursescrollviewName, coursescardtype, panel);

    //        SearchField.CoursesNames.Add(CourseName);
    //    }
    //}

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
}
