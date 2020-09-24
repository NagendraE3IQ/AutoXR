using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine.UI;

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
    public TMP_InputField NewUserMangerID;
    public TextMeshProUGUI RequestSentMessage;

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
    }

    public void OnLogin()
    {
        var filter = Builders<BsonDocument>.Filter.Eq("email", UserName.text);
        userdocument = UserData.Instance.UserDataCollection.Find(filter).FirstOrDefault();

        if (userdocument != null)
        {
            if (userdocument["email"] == UserName.text && userdocument["password"] == Password.text)
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

    //SEND REQUEST To MANAGER
    public void OnSignUp()
    {
        if (!string.IsNullOrEmpty(NewUserName.text) && !string.IsNullOrEmpty(NewUSerEmail.text) && !string.IsNullOrEmpty(NewUserEmpID.text) && !string.IsNullOrEmpty(NewUserDepartment.text) && !string.IsNullOrEmpty(NewUserOrganization.text) && !string.IsNullOrEmpty(NewUserMangerID.text))
        {
            BsonDocument doc = new BsonDocument
        {
            {"name", NewUserName.text },
            {"email", NewUSerEmail.text },
            {"empid", NewUserEmpID.text},
            {"department", NewUserDepartment.text},
            {"organization", NewUserOrganization.text},
        };

            var filter = Builders<BsonDocument>.Filter.Eq("email", NewUserMangerID.text);
            var update = Builders<BsonDocument>.Update.Set(NewUserEmpID.text + "  Request", doc);

            UserData.Instance.AdminDataCollection.UpdateOne(filter, update);

            RequestSentMessage.text = "Your Request Sent";
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
}
