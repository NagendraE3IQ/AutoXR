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

public class AssignedCardDataDetails : MonoBehaviour
{
    public string CourseName, CourseDescription, CourseURL, CourseIcon;
    public Slider ProgressBar;
    public Image CourseIconImage;
    public TextMeshProUGUI CourseLevel, DownloadText, CourseNameText, CourseDescriptionText;
    public GameObject DownLoadBtn, DownLoadBtn1, DownLoadBtn2;
    public bool isAnyCourseOpened = false;
    public float ThisCoursePlaytimeInSeconds;
    string CoursesStorePath;

    public virtual void Start()
    {
#if UNITY_EDITOR
        CoursesStorePath = @"D:\NagendraBuilds\Newfolder";
#else
        CoursesStorePath = Application.dataPath;
#endif
        if (Directory.Exists(CoursesStorePath + "/" + CourseName))
        {
            DownLoadBtn.SetActive(false);
            DownLoadBtn.transform.parent.GetComponent<Button>().onClick.AddListener(() => OnCourseOpenButtonClick(CourseName));
        }
        else
        {
            DownLoadBtn.SetActive(true);
        }

        if (File.Exists(Application.dataPath + "/Resources/" + CourseName + ".png"))
        {
            StartCoroutine(LoadImage(CourseName, CourseDescription));
            UnityEngine.Debug.Log(CourseName);
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
        var uwr = new UnityWebRequest(cu);
        uwr.method = UnityWebRequest.kHttpVerbGET;
        var resultFile = Path.Combine(CoursesStorePath, cn + ".zip");
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
            zipunzip(resultFile, cn);
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
            StartCoroutine(LoadImage(cn,  cd));
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
        if(!isAnyCourseOpened)
        {
            ContentManager.Instance.ContentLoadingPanel.SetActive(true);
            Process process = new Process();
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            UnityEngine.Debug.Log(CoursesStorePath + "/" + cn + "/" + cn + ".exe");
            Process.Start(CoursesStorePath + "/" + cn + "/" + cn + ".exe");

            isAnyCourseOpened = true;
            Invoke("HideContentLoadingPanel", 15);
        }      
    }

    void HideContentLoadingPanel()
    {
        ContentManager.Instance.ContentLoadingPanel.SetActive(false);

        DownLoadBtn.gameObject.GetComponent<Button>().interactable = true;
        DownLoadBtn1.gameObject.GetComponent<Button>().interactable = true;
        DownLoadBtn2.gameObject.GetComponent<Button>().interactable = true;
    }

    float hour, minutes, seconds;
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
             ThisCoursePlaytimeInSeconds += ((DateTime.Now.Hour - hour) * 60) + ((DateTime.Now.Minute - minutes) * 60) + (DateTime.Now.Second - seconds);
            TimeSpan t = TimeSpan.FromSeconds(ThisCoursePlaytimeInSeconds);

            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);

            UnityEngine.Debug.Log(answer);

            isAnyCourseOpened = false;
        }
        else
        {
            hour = DateTime.Now.Hour;
            minutes = DateTime.Now.Minute;
            seconds = DateTime.Now.Second;
        }
    }
}