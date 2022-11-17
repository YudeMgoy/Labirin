using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]Ball ball;
    [SerializeField]GameObject arrow;
    [SerializeField]Image aim;
    [SerializeField]LineRenderer line;
    [SerializeField]TMP_Text shootCountText;
    [SerializeField]LayerMask ballLayer;
    [SerializeField]LayerMask rayLayer;
    [SerializeField]FollowBall cameraPivot;
    [SerializeField]Camera cam;
    [SerializeField]Vector2 camSensitivity;
    [SerializeField]float shootForce;

    bool isShooting;
    Vector3 lastMousePosition;
    float ballDistance;

    float forceFactor;
    Vector3 forceDir;
    
    Renderer[] arrowRends;
    Color[] arrowOriginalColors;

    int shootCount=0;
    public int ShootCount {get=> shootCount;}
    void Start()
    {
        ballDistance = Vector3.Distance(cam.transform.position,ball.Position)+1;
        arrowRends = arrow.GetComponentsInChildren<Renderer>();
        arrowOriginalColors = new Color[arrowRends.Length];
        for(int i=0;i<arrowRends.Length;i++)
        {
            arrowOriginalColors[i]= arrowRends[i].material.color;
        }
        arrow.SetActive(false);
        shootCountText.text = "Shoot Count: "+shootCount;
    }
    void Update()
    {
        if(ball.IsMoving || ball.IsTeleporting)
            return;
        // if(!cameraPivot.IsMoving && aim.gameObject.activeInHierarchy ==false)
        // {
            aim.gameObject.SetActive(true);
            var rectx = aim.GetComponent<RectTransform>();
            rectx.anchoredPosition = cam.WorldToScreenPoint(ball.Position);
        // }

        if(this.transform.position!=ball.Position)
        {
            this.transform.position=ball.Position;
            aim.gameObject.SetActive(true);
            var rect = aim.GetComponent<RectTransform>();
            rect.anchoredPosition = cam.WorldToScreenPoint(ball.Position);
        }

        if(Input.GetMouseButtonDown(0))
        {
            
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,ballDistance,ballLayer))
            {
                
                Debug.Log("Ball");
                isShooting = true;
                arrow.SetActive(true);
                Debug.Log(arrow.activeInHierarchy);
                line.enabled=true;
            }

        }

        //shoot mode
        if(Input.GetMouseButton(0) && isShooting==true)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,ballDistance*2,rayLayer))
            {
                Debug.DrawLine(ball.Position,hit.point);
                // Debug.Log(hit.point);

                var forceVector = ball.Position-hit.point;
                forceVector = new Vector3(forceVector.x,0,forceVector.z);
                forceDir = forceVector.normalized;
                var forceMagnitude = forceVector.magnitude; 
                // Debug.Log(forceMagnitude);
                forceMagnitude = Mathf.Clamp(forceMagnitude,0,5);
                forceFactor = forceMagnitude/5;

            }
            // Debug.DrawRay(ray.origin,ray.direction*100);

            //arrow
            this.transform.LookAt(this.transform.position + forceDir);
            arrow.transform.localScale=new Vector3(
                0.3f * forceFactor,
                0.3f * forceFactor,
                1 * forceFactor);

            for(int i=0;i<arrowRends.Length;i++)
            {
                arrowRends[i].material.color=Color.Lerp(arrowOriginalColors[i],Color.red,forceFactor);
            }
            // foreach(var rend in arrowRends)
            // {
            //     rend.material.color = Color.Lerp(Color.white,Color.red,forceFactor);
            // }

            //aim
            var rect = aim.GetComponent<RectTransform>();
            rect.anchoredPosition = Input.mousePosition;

            //line
            var ballScrPos = cam.WorldToScreenPoint(ball.Position);
            line.SetPositions(new Vector3[]
            {
                ballScrPos,
                Input.mousePosition
            }
            );
        }
    

        //camera mode
        if(Input.GetMouseButton(0)&&isShooting==false)
        {
            var current = cam.ScreenToViewportPoint(Input.mousePosition);
            var last = cam.ScreenToViewportPoint(lastMousePosition);
            var delta = current - last;

            //rotate horizontal
            cameraPivot.transform.RotateAround(
                ball.Position,
                Vector3.up,
                delta.x*camSensitivity.x);

            //rotate vertical
            cameraPivot.transform.RotateAround(
                ball.Position,
                cam.transform.right,
                -delta.y*camSensitivity.y);

            //var angle
            var angle = Vector3.SignedAngle(
                Vector3.up,
                cam.transform.up,
                cam.transform.right);


            //jika lewat batas maka diputar balik
            if(angle<3)
            {
                cameraPivot.transform.RotateAround(
                ball.Position,
                cam.transform.right,
                3-angle);
            }
            else if(angle>65)
            {
                cameraPivot.transform.RotateAround(
                ball.Position,
                cam.transform.right,
                65-angle);
            }
        }

        if(Input.GetMouseButtonUp(0) && isShooting)
        {
            ball.AddForce(forceDir*shootForce*forceFactor);
            shootCount+=1;
            shootCountText.text = "Shoot Count: "+shootCount;
            forceFactor=0;
            forceDir=Vector3.zero;
            isShooting=false;
            arrow.SetActive(false);

            aim.gameObject.SetActive(false);
            line.enabled=false;

        }
        lastMousePosition= Input.mousePosition;
    }
}
