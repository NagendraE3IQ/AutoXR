using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net.NetworkInformation;
using System.Net;

public class UserData : MonoBehaviour
{
    public string ServerConnectionURL;
    public string DatabseName;

    public string CourseDataCollectionName;
    public string DepartmentDataCollectionName;
    public string UserDataCollectionName;
    public string AdminDataCollectionName;

    IMongoDatabase DataBase;

    [HideInInspector]
    public IMongoCollection<BsonDocument> CourseDataCollection;
    [HideInInspector]
    public IMongoCollection<BsonDocument> DepartmentDataCollection;
    [HideInInspector]
    public IMongoCollection<BsonDocument> UserDataCollection;
    [HideInInspector]
    public IMongoCollection<BsonDocument> AdminDataCollection;

    [HideInInspector]
    public BsonDocument UserDocument;
    [HideInInspector]
    public BsonDocument DepartmentDocument;
    [HideInInspector]
    public List<BsonDocument> CourseDocumnets;

    public GameObject NoInternetConnectionPopup;
    public static UserData Instance;

    bool InternetConnection = false;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        OnInit();
        DontDestroyOnLoad(this);

        //InsertDoc();
    }

    public void OnInit()
    {
        if (chkconnection())
        {
            MongoClient DbClient = new MongoClient(ServerConnectionURL);
            DataBase = DbClient.GetDatabase(DatabseName);

            CourseDataCollection = DataBase.GetCollection<BsonDocument>(CourseDataCollectionName);
            DepartmentDataCollection = DataBase.GetCollection<BsonDocument>(DepartmentDataCollectionName);
            UserDataCollection = DataBase.GetCollection<BsonDocument>(UserDataCollectionName);
            AdminDataCollection = DataBase.GetCollection<BsonDocument>(AdminDataCollectionName);

            NoInternetConnectionPopup.SetActive(false);
        }
        else
        {
            NoInternetConnectionPopup.SetActive(true);
        }
    }

    bool chkconnection()
    {
        try
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://www.google.com"))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public void GetUserData()
    {
        CourseDocumnets.Clear();
        var departmentfilter = Builders<BsonDocument>.Filter.Eq("_id", UserDocument["department"]);
        DepartmentDocument = DepartmentDataCollection.Find(departmentfilter).FirstOrDefault();

        int totalDepartmentcourses = DepartmentDocument["courses"].AsBsonArray.ToList().Capacity;
        for (int i = 0; i < totalDepartmentcourses; i++)
        {
            var coursesfilter = Builders<BsonDocument>.Filter.Eq("_id", DepartmentDocument["courses"][i]);
            CourseDocumnets.Add(CourseDataCollection.Find(coursesfilter).FirstOrDefault());
        }
    }

    //TO TEST//
    public void InsertDoc()
    {
        var filter = Builders<BsonDocument>.Filter.Eq("email", "NagendraBabu.Ravilla@e3iq.com");
        UserDataCollection.DeleteOne(filter);
        UserDataCollection.InsertOne(new BsonDocument{
            { "name", "Nagendra Babu Ravilla"},
            { "organization", "E3IQ"},
            { "empid", "ACTV000000E20"},
            { "departMent", "VR Development"},
            { "email", "NagendraBabu.Ravilla@e3iq.com"},
            { "password", "xxxxxxxxxx"},
            { "numberofassignedcourses", 3},
            { "numberofcompletedcourses", 2},
            { "numberofpendingcourses", 1},
            { "assignedcourses",
                new BsonArray
                {
                new BsonDocument
                {
                { "level", "beginer" },
                { "name", "PCScene"},
                { "description", "Module Description"},
                { "icon", "https://autoxr-admin.s3-us-west-1.amazonaws.com/fireAndsafety.png"},
                { "link", "https://autoxr-admin.s3-us-west-1.amazonaws.com/PCScene.zip"},
                { "version", 1.0 },
                },
               new BsonDocument
                {
                { "level", "Intermidiate" },
                { "name", "PCScene"},
                { "description", "Module Description"},
                { "icon", "https://autoxr-admin.s3-us-west-1.amazonaws.com/fireAndsafety.png"},
                { "link", "https://autoxr-admin.s3-us-west-1.amazonaws.com/PCScene.zip"},
                { "version", 1.1 },
                },

                 new BsonDocument
                {
                { "level", "Expert" },
                { "name", "PCScene"},
                { "description", "Module Description"},
                { "icon", "https://autoxr-admin.s3-us-west-1.amazonaws.com/fireAndsafety.png"},
                { "link", "https://autoxr-admin.s3-us-west-1.amazonaws.com/PCScene.zip"},
                { "version", 1.1 },
                }
                }
            },
            { "pendingcourses",
                new BsonArray
                {
                new BsonDocument
                {
                { "level", "Intermidiate" },
                { "name", "PCScene"},
                { "description", "Module Description"},
                { "icon", "https://autoxr-admin.s3-us-west-1.amazonaws.com/fireAndsafety.png"},
                { "link", "https://autoxr-admin.s3-us-west-1.amazonaws.com/PCScene.zip"},
                { "version", 1.1 },
                },
               new BsonDocument
                {
                { "level", "Expert" },
                { "name", "PCScene"},
                { "description", "Module Description"},
                { "icon", "https://autoxr-admin.s3-us-west-1.amazonaws.com/fireAndsafety.png"},
                { "link", "https://autoxr-admin.s3-us-west-1.amazonaws.com/PCScene.zip"},
                { "version", 1.1 },
                }
                }
            },
            { "completedcourses",
                new BsonArray
                {
                new BsonDocument
                {
                { "level", "beginer" },
                { "name", "PCScene"},
                { "description", "Module Description"},
                { "icon", "https://autoxr-admin.s3-us-west-1.amazonaws.com/fireAndsafety.png"},
                { "link", "https://autoxr-admin.s3-us-west-1.amazonaws.com/PCScene.zip"},
                { "version", 1.0 },
                },
                }
            }
        });
    }
}
