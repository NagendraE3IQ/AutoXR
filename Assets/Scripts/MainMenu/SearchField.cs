using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class SearchField : MonoBehaviour
{
    public List<string> CoursesNames;
    public GameObject Scrollview, SearchIcon, SearchCloseButton;
    public TMP_InputField IPF;

    bool IsDeselect = false;

    private void Update()
    {
        //if (IsDeselect)
        //{
        //    if(EventSystem.current.currentSelectedGameObject)
        //    {
                //if (EventSystem.current.currentSelectedGameObject.name != "searchitem(Clone)" && EventSystem.current.currentSelectedGameObject.name != "Scrollbar Vertical")
                //{
                    //ContentManager.Instance.SearchResults();
                    //SearchIcon.SetActive(false);
                    //IPF.text = "";
                //}
        //    }
        //    else
        //    {
        //        IPF.text = "";
        //        SearchIcon.SetActive(true);
        //        SearchCloseButton.SetActive(false);
        //    }        
        //}
    }

    string sceneName;
    GameObject g;
    public void OnValueChange()
    {
        g = Scrollview.GetComponentInChildren<GridLayoutGroup>().gameObject;

        for (int i = 0; i < g.transform.childCount; i++)
        {
            Destroy(g.transform.GetChild(i).transform.gameObject);
        }

        if (IPF.text.Length != 0)
        {
            foreach (var name in CoursesNames)
            {
                bool contains = name.IndexOf(IPF.text, StringComparison.CurrentCultureIgnoreCase) >= 0;

                if (contains)
                {
                    GameObject b = (GameObject)Resources.Load("searchitem");
                    GameObject button = Instantiate(b, transform.position, transform.rotation);
                    button.transform.SetParent(g.transform);
                    button.transform.localScale = new Vector3(1, 1, 1);
                    button.transform.GetChild(0).GetComponent<TMP_Text>().text = name;
                    IPF.text = name;
                }
            }
        }
    }

    public void OnSelect()
    {
        IsDeselect = false;
    }
    public void OnDeselect()
    {
        IsDeselect = true;
    }
}
