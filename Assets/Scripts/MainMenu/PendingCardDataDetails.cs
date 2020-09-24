using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using System.Collections;

public class PendingCardDataDetails : AssignedCardDataDetails
{
    public Slider CourseCompletionProgressBar;

    public override void Start()
    {
        base.Start();
        CourseCompletionProgressBar.value = 0.5f;
    }
}
