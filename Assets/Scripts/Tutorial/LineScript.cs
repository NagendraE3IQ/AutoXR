using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using Photon.Pun;

public class LineScript : MonoBehaviour
{
    //public
    public GameObject caster;
    public GameObject pointertip;
    public VRInputModule InputModule;
    public GameObject gameobj;
    public GameObject childobj;
    public Transform[] trs;
    public string[] PartNameOnGeomenty;
    public string[] PartNameOnBoard;
    public PhotonRoomManagerScript PRMReference;
   // public CheckOwnerScript CheckOwnerReference;

    public LineRenderer line;
    public LayerMask mask;

    public TMPro.TextMeshProUGUI PartName;

    public float maxdistance;
    RaycastHit hitInfo1;
    public float lineLength;

    public bool IsBeingUsedOnNetwork = false;

    // Start is called before the first frame update
    void Awake()
    {
        line = GameObject.Find("Line").GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //pointertip = GameObject.Find("Sphere");
        hitInfo1 = CreateRay(maxdistance, caster.transform);
        renderline(hitInfo1, maxdistance, caster, line, pointertip);
    }

    public void renderline(RaycastHit info, float maxlength, GameObject pointercaster, LineRenderer lr, GameObject tip)
    {
        // max length of the pointer
        PointerEventData data = InputModule.GetData();

        float maximumlength = data.pointerCurrentRaycast.distance == 0 ? maxlength : data.pointerCurrentRaycast.distance;

        // Default end position for the PointerLine

        Vector3 endposition = pointercaster.transform.position + pointercaster.transform.forward * maximumlength;
        lineLength = Vector3.Distance(caster.transform.position, pointertip.transform.position);

        // If hitting

        if (info.collider != null)
        {
            endposition = info.point;
            //Debug.Log(info.collider.gameObject.name);
        }

        // renderingline

        lr.SetPosition(0, pointercaster.transform.position);
        lr.SetPosition(1, endposition);
        // Adding PointerTip

        tip.transform.position = endposition;

        // Check Hover Events
        hover(info);
    }

    public RaycastHit CreateRay(float maxLength, Transform origin)
    {
        RaycastHit hitInfo;

        //LayerMask mask = LayerMask.GetMask("Point");

        Ray ray = new Ray(origin.position, origin.forward);
        Physics.Raycast(ray, out hitInfo, maxLength);
        if (IsBeingUsedOnNetwork && PRMReference.isjoined)
        {
          //  CheckOwnerScript.Instance.UpdateLineLength(lineLength);
        }


        return hitInfo;
    }

 public void hover(RaycastHit hit)
        {
     if (hit.transform != null && gameobj != null && hit.transform.gameObject == gameobj)
              {
                
              }
     if (hit.transform != null && gameobj != null && hit.transform.gameObject != gameobj)
               {
                  disabletext(gameobj);
                  gameobj = hit.transform.gameObject;
                     enabletext(gameobj);
               }

     if (hit.transform == null && gameobj != null)
               {
                  disabletext(gameobj);
                  gameobj = null;
               }

     if (hit.transform != null && gameobj == null)
               {
                   gameobj = hit.transform.gameObject;
                   enabletext(gameobj);
               }

     if (hit.transform == null && gameobj == null)
               {

               }
        }


    public void enabletext(GameObject obj)
        {
            string s = obj.name;
        trs = obj.GetComponentsInChildren<Transform>(true);

            foreach  (Transform t in trs)
                {
                    if (t.gameObject.name == obj.name + "Desc")
                        {
                            childobj = t.gameObject;
                            t.gameObject.SetActive(true);
                        }
                }
        }

    public void disabletext (GameObject obj)
        {
            if(childobj != null)
                {
                    childobj.SetActive(false);
                            
                    childobj = null;
                }
        }
}








 

            

