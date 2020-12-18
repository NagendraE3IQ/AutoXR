using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class LoginManagerScript : MonoBehaviour
{
    [Header("SIGNIN")]
    public TMP_InputField UserName;
    public TMP_InputField Password;
    public TextMeshProUGUI ErrorMessage;
    public Toggle RemindmeToggle;

    [Header("SIGNUP")]
    public TMP_InputField NewUserName;
    public TMP_InputField NewUSerEmail;
    public TMP_InputField NewUserEmpID;
    public TMP_InputField NewUserDepartment;
    public TMP_InputField NewUserOrganization;
    public TMP_InputField NewUserPassword;
    public TextMeshProUGUI RequestSentMessage;
    public TMP_Dropdown NewUserOrganizations, NewUserDepartments;
    public TextMeshProUGUI UserExistMessage;


    List<string> AdminNames = new List<string>();
    List<string> DepartmentNames = new List<string>();
    int m_Org, m_Dep;
    bool IsUserAlreadyExist = false;

    BsonDocument userdocument;

    private void Start()
    {
        if (PlayerPrefs.HasKey("UserName"))
        {
            UserName.text = PlayerPrefs.GetString("UserName");
        }
        if (PlayerPrefs.HasKey("UserPassword"))
        {
            Password.text = PlayerPrefs.GetString("UserPassword");
        }

        AdminNames.Add("None");
        foreach (var document in UserData.Instance.AdminDataCollection.AsQueryable())
        {
            AdminNames.Add(document["name"].ToString());
        }

        NewUserOrganizations.ClearOptions();
        for (int i = 0; i < AdminNames.Count; i++)
        {
            TMP_Dropdown.OptionData temp = new TMP_Dropdown.OptionData();
            m_Org = i;
            temp.text = AdminNames[i];
            NewUserOrganizations.options.Insert(m_Org, temp);
        }

        Invoke("SetTextforDropdowns", 0.1f);
    }

    void SetTextforDropdowns()
    {
        NewUserOrganizations.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Select Your Organization";
        NewUserDepartments.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Select Your Department";
    }

    public void OnOrganizationValueChange()
    {
        DepartmentNames.Clear();

        if (NewUserOrganizations.value == 0)
        {
            NewUserOrganizations.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Select Your Organization";
        }
        else
        {
            var af = Builders<BsonDocument>.Filter.Eq("name", NewUserOrganizations.options[NewUserOrganizations.value].text);
            BsonDocument admindoc = UserData.Instance.AdminDataCollection.Find(af).FirstOrDefault();

            DepartmentNames.Add("None");
            foreach (var document in UserData.Instance.DepartmentDataCollection.AsQueryable())
            {
                if (document["organization"] == admindoc["_id"])
                    DepartmentNames.Add(document["name"].ToString());
            }
            NewUserDepartments.ClearOptions();
            for (int i = 0; i < DepartmentNames.Count; i++)
            {
                TMP_Dropdown.OptionData temp = new TMP_Dropdown.OptionData();
                m_Org = i;
                temp.text = DepartmentNames[i];
                NewUserDepartments.options.Insert(m_Org, temp);
            }

        }
        NewUserDepartments.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Select Your Department";

    }

    public void OnDepartmentValueChange()
    {
        if (NewUserDepartments.value == 0)
        {
            NewUserDepartments.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Select Your Department";
        }
    }

    public void OnLogin()
    {
        var filter = Builders<BsonDocument>.Filter.Eq("email", UserName.text.ToLower());
        userdocument = UserData.Instance.UserDataCollection.Find(filter).FirstOrDefault();

        if (userdocument != null)
        {
            string e = userdocument["email"].ToString();
            Debug.Log(e);
            if (string.Equals(e, UserName.text, StringComparison.CurrentCultureIgnoreCase) && userdocument["password"] == Password.text)
            {
                SceneManager.LoadScene("MainMenu");
                UserData.Instance.UserDocument = UserData.Instance.UserDataCollection.Find(filter).FirstOrDefault();
                UserData.Instance.GetUserData();

                if (RemindmeToggle.isOn)
                {
                    PlayerPrefs.SetString("UserName", UserName.text);
                    PlayerPrefs.SetString("UserPassword", Password.text);
                }
                else
                {
                    PlayerPrefs.SetString("UserName", "");
                    PlayerPrefs.SetString("UserPassword", "");
                }
            }
            else
            {
                ErrorMessage.text = "Wrong Credentials";
            }
        }
        else
        {
            ErrorMessage.text = "Wrong Credentials";
        }
    }

    public void RevealPassword()
    {
        Password.contentType = TMP_InputField.ContentType.Standard;
        Password.DeactivateInputField();
        Password.ActivateInputField();
    }
    public void HidePassword()
    {
        Password.contentType = TMP_InputField.ContentType.Password;
        Password.DeactivateInputField();
        Password.ActivateInputField();
    }

    //SEND REQUEST To MANAGER
    public void OnSignUp()
    {
        RequestSentMessage.text = "";
        UserExistMessage.text = "";

        foreach (var document in UserData.Instance.UserDataCollection.AsQueryable())
        {
            if (document["email"] == NewUSerEmail.text)
            {
                IsUserAlreadyExist = true;
            }
        }

        if(!IsUserAlreadyExist)
        {
            var af = Builders<BsonDocument>.Filter.Eq("name", NewUserOrganizations.options[NewUserOrganizations.value].text);
            BsonDocument admindoc = UserData.Instance.AdminDataCollection.Find(af).FirstOrDefault();

            var df = Builders<BsonDocument>.Filter.Eq("name", NewUserDepartments.options[NewUserDepartments.value].text);
            BsonDocument departmentdoc = UserData.Instance.DepartmentDataCollection.Find(df).FirstOrDefault();

            if (!string.IsNullOrEmpty(NewUserName.text) && !string.IsNullOrEmpty(NewUSerEmail.text) && !string.IsNullOrEmpty(NewUserEmpID.text) && !string.IsNullOrEmpty(NewUserDepartments.options[NewUserDepartments.value].text) && !string.IsNullOrEmpty(NewUserOrganizations.options[NewUserOrganizations.value].text) && !string.IsNullOrEmpty(NewUserPassword.text))
            {
                BsonDocument doc = new BsonDocument
        {
            {"organization", admindoc["_id"]},
            {"isVerified", false},
            {"name", NewUserName.text },
            {"empid", NewUserEmpID.text},
            {"email", NewUSerEmail.text.ToLower()},
            {"password", NewUserPassword.text },
            {"department", departmentdoc["_id"]}
        };

                //var filter = Builders<BsonDocument>.Filter.Eq("email", NewUserPassword.text);
                //var update = Builders<BsonDocument>.Update.Set(NewUserEmpID.text + "  Request", doc);

                //UserData.Instance.AdminDataCollection.UpdateOne(filter, update);

                UserData.Instance.UserDataCollection.InsertOne(doc);

                RequestSentMessage.text = "Your Request Sent";
            }
        }
        else
        {
            UserExistMessage.text = "User Already Exists";
            IsUserAlreadyExist = false;
        }
            
    }

    public void NewUserRevealPassword()
    {
        NewUserPassword.contentType = TMP_InputField.ContentType.Standard;
        NewUserPassword.DeactivateInputField();
        NewUserPassword.ActivateInputField();
    }
    public void NewUserHidePassword()
    {
        NewUserPassword.contentType = TMP_InputField.ContentType.Password;
        NewUserPassword.DeactivateInputField();
        NewUserPassword.ActivateInputField();
    }


    //Delete PlayerPrefs
    public void DeleteCache()
    {
        PlayerPrefs.DeleteAll();
    }
}
