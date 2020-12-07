using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using TMPro;
using System.Threading.Tasks;
using System;
using Valve.VR;
using UnityEngine.XR;
using MongoDB.Bson;
using MongoDB.Driver;

public class AssignedCardDataDetails : MonoBehaviour
{
    public string CourseName, CourseDescription, CourseURL, CourseIcon;
    public Slider ProgressBar;
    public Image CourseIconImage;
    public TextMeshProUGUI CourseLevel, DownloadText, CourseNameText, CourseDescriptionText;
    public GameObject DownLoadBtn, DownLoadBtn1, DownLoadBtn2;
    public bool isAnyCourseOpened = false;
    public float ThisCoursePlaytimeInSeconds;
    public double RequiredInternetBandwidth;
    string CoursesStorePath;
    string CourseURLExtension;

    public virtual void Start()
    {
        CourseURLExtension = Path.GetExtension(CourseURL);

#if UNITY_EDITOR
        if (CourseURLExtension == ".zip" || CourseURLExtension == ".obj" || CourseURLExtension == ".fbx")
        {
            CoursesStorePath = @"D:\NagendraBuilds\Newfolder";
        }
        else
        {
            CoursesStorePath = Application.dataPath + "/Resources";
        }
#else
 if (CourseURLExtension == ".zip")
        {
            CoursesStorePath = Application.dataPath;
        }
        else
        {
            CoursesStorePath = Application.dataPath + "/Resources";
        }
#endif
        if (CourseURLExtension == ".zip")
        {
            if (Directory.Exists(CoursesStorePath + "/" + CourseName))
            {
                DownLoadBtn.SetActive(false);
                DownLoadBtn.transform.parent.GetComponent<Button>().onClick.AddListener(() => OnCourseOpenButtonClick(CourseName));
            }
            else
            {
                DownLoadBtn.SetActive(true);
            }
        }
        else
        {
            if (File.Exists(CoursesStorePath + "/" + CourseName + ".mp4"))
            {
                DownLoadBtn.SetActive(false);
                DownLoadBtn.transform.parent.GetComponent<Button>().onClick.AddListener(() => OnCourseOpenButtonClick(CourseName));
            }
            else
            {
                DownLoadBtn.SetActive(true);
            }
        }


        if (File.Exists(Application.dataPath + "/Resources/" + CourseName + ".png"))
        {
            StartCoroutine(LoadImage(CourseName, CourseDescription));
        }
        else
        {
            StartCoroutine(DownloadCourseIcon(CourseName, CourseDescription, CourseIcon));
        }
    }

    public void DownloadCourse()
    {
        StartCoroutine(Download(CourseName, CourseDescription, CourseURL));
        DownLoadBtn.gameObject.GetComponent<Button>().interactable = false;
        DownLoadBtn1.gameObject.GetComponent<Button>().interactable = false;
        DownLoadBtn2.gameObject.GetComponent<Button>().interactable = false;
    }

    IEnumerator Download(string cn, string cd, string cu)
    {
        DownloadText.text = "Downloading";
        DownloadText.color = new Color32(241, 94, 125, 255);
        Invoke("checkInternetspeedwhileDownloading", 5);
        var uwr = new UnityWebRequest(cu);
        uwr.method = UnityWebRequest.kHttpVerbGET;
        var resultFile = Path.Combine(CoursesStorePath, cn + CourseURLExtension);
        var dh = new DownloadHandlerFile(resultFile);
        dh.removeFileOnAbort = true;
        uwr.downloadHandler = dh;
        StartCoroutine(CourseDownloadProgress(uwr, ProgressBar));
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            UnityEngine.Debug.Log(uwr.error);
            DownLoadBtn.gameObject.GetComponent<Button>().interactable = true;
            DownLoadBtn1.gameObject.GetComponent<Button>().interactable = true;
            DownLoadBtn2.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            UnityEngine.Debug.Log("Download saved to: " + resultFile);
            if (CourseURLExtension == ".zip")
            {
                zipunzip(resultFile, cn);
            }
            else
            {
                DownLoadBtn.transform.parent.GetComponent<Button>().onClick.AddListener(() => OnCourseOpenButtonClick(cn));
                DownLoadBtn.gameObject.SetActive(false);
            }
        }
    }

    async void zipunzip(string rf, string cn)
    {
        await Task.Run(() => ZipFile.ExtractToDirectory(rf, CoursesStorePath));
        UnityEngine.Debug.Log(CoursesStorePath);
        File.Delete(rf);
        DownLoadBtn.transform.parent.GetComponent<Button>().onClick.AddListener(() => OnCourseOpenButtonClick(cn));
        DownLoadBtn.gameObject.SetActive(false);
    }

    IEnumerator CourseDownloadProgress(UnityWebRequest request, Slider ProgressBar)
    {
        while (!request.isDone)
        {
            ProgressBar.value = request.downloadProgress;
            UnityEngine.Debug.Log(request.downloadProgress);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator DownloadCourseIcon(string cn, string cd, string ci)
    {
        var uwr = new UnityWebRequest(ci);
        uwr.method = UnityWebRequest.kHttpVerbGET;
        var resultFile = Path.Combine(Application.dataPath + "/Resources", cn + ".png");
        var dh = new DownloadHandlerFile(resultFile);
        dh.removeFileOnAbort = true;
        uwr.downloadHandler = dh;
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            UnityEngine.Debug.Log(uwr.error);
        }
        else
        {
            UnityEngine.Debug.Log("Download saved to: " + resultFile);
            yield return new WaitForEndOfFrame();
            StartCoroutine(LoadImage(cn, cd));
        }
    }

    Texture2D t;
    IEnumerator LoadImage(string cn, string cd)
    {
        WWW www = new WWW(@Application.dataPath + "/Resources/" + cn + ".png");
        while (!www.isDone)
            yield return null;
        t = www.texture;
        Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);
        CourseIconImage.sprite = s;
        CourseIconImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(s.rect.width, s.rect.height);
        CourseNameText.text = cn;
        CourseDescriptionText.text = cd;
    }

    public void OnCourseOpenButtonClick(string cn)
    {
        //if (ContentManager.Instance.IsApplictionInstalled("SteamVR"))
        //{
            if (!isAnyCourseOpened)
            {
                if (CourseURLExtension == ".zip")
                {
                    ContentManager.Instance.ContentLoadingPanel.SetActive(true);
                    Process process = new Process();
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    UnityEngine.Debug.Log(CoursesStorePath + "/" + cn + "/" + cn + ".exe");
                    Process.Start(CoursesStorePath + "/" + cn + "/" + cn + ".exe");
                }
                else if (CourseURLExtension == ".mp4")
                {
                    GameObject g = Camera.main.gameObject;
                    this.StartCoroutine(this.LoadVisor("OpenVR", true, "360Video"));
                    g.GetComponent<SteamVR_LoadLevel>().levelName = "360Video";
                    g.GetComponent<SteamVR_LoadLevel>().enabled = true;
                    PlayerPrefs.SetString("ThreeDimVideo", cn);
                }
                else if (CourseURLExtension == ".obj" || CourseURLExtension == ".fbx")
                {
                    UnityEngine.Debug.Log(CourseURLExtension);
                    GameObject g = Camera.main.gameObject;
                    g.GetComponent<SteamVR_LoadLevel>().levelName = "3DViwer";
                    g.GetComponent<SteamVR_LoadLevel>().enabled = true;
                }


                isAnyCourseOpened = true;
                Invoke("HideContentLoadingPanel", 15);
            }
        //}
        //else
        //{
            //if (!ContentManager.Instance.IsApplictionInstalled("SteamVR"))
                //ContentManager.Instance.SteamVRAppNotInstalledPanel.SetActive(true);
        //}
    }

    void HideContentLoadingPanel()
    {
        ContentManager.Instance.ContentLoadingPanel.SetActive(false);

        DownLoadBtn.gameObject.GetComponent<Button>().interactable = true;
        DownLoadBtn1.gameObject.GetComponent<Button>().interactable = true;
        DownLoadBtn2.gameObject.GetComponent<Button>().interactable = true;
    }

    void checkInternetspeedwhileDownloading()
    {
        if (ContentManager.Instance.CheckSpeed() < RequiredInternetBandwidth && PlayerPrefs.GetInt("InterNetSpeedShow") == 0)
            ContentManager.Instance.InternetSpeedPanel.SetActive(true);
    }

    private IEnumerator LoadVisor(string StringVisor, bool BoolEnable, string scenename)
    {
        XRSettings.LoadDeviceByName(StringVisor);

        yield return null;

        if (BoolEnable == true)
        {
            XRSettings.enabled = true;

            InputTracking.disablePositionalTracking = false;
            InputTracking.Recenter();
            MainMenuManagerScript.Instance.SceneObjects.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            XRSettings.enabled = false;
        }
    }

    float hour, minutes, seconds;
    TimeSpan spendtime;
    private void OnApplicationFocus(bool focus)
    {
        if(isAnyCourseOpened)
        {
            if (focus)
            {
                ThisCoursePlaytimeInSeconds += ((DateTime.Now.Hour - hour) * 60) + ((DateTime.Now.Minute - minutes) * 60) + (DateTime.Now.Second - seconds);
                spendtime = TimeSpan.FromSeconds(ThisCoursePlaytimeInSeconds);

                string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                spendtime.Hours,
                                spendtime.Minutes,
                                spendtime.Seconds);

                UnityEngine.Debug.Log(answer);

                isAnyCourseOpened = false;
            }
            else
            {
                hour = DateTime.Now.Hour;
                minutes = DateTime.Now.Minute;
                seconds = DateTime.Now.Second;
            }

            SetSpendtimeOnCourse();
        }      
    }

    public void SetSpendtimeOnCourse()
    {
        BsonDocument doc = new BsonDocument
        {
            {CourseName, spendtime.Seconds },
        };

        var filter = Builders<BsonDocument>.Filter.Eq("email", UserData.Instance.UserDocument["email"]);
        var update = Builders<BsonDocument>.Update.AddToSet("Analytics", doc);

        UserData.Instance.UserDataCollection.UpdateOne(filter, update);
    }   
}