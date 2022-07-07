using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Platform : NetworkBehaviour
{
    private List<ThirdPersonController> playersTrigers = new List<ThirdPersonController>();

    [Header("Platform Components")]
    public Rigidbody rb;
    public BoxCollider coll;


    [Header("Settings")] 
    public PlatformSettings settings;
    public float delayOnActivate = 0;
    public PlatformStartType startType = PlatformStartType.Automatic;
    public MovementType movement = MovementType.Position;
    [Header("Targets")]
    public Transform startPoint;
    public Transform endPoint;

    public bool isActive { get; private set; }
    public override void OnStartLocalPlayer()
    {
        gameObject.SetActive(true);
    }
    private void Awake()
    {
        isActive = false;
    }
    
    private void Start()
    {
        if (startType == PlatformStartType.Automatic)
            ActivePlatform();
    }
    public void ActivePlatform()
    {
        if (isActive)
            return;
        StartCoroutine(ActiveProcces());
    }

    private IEnumerator ActiveProcces()
    {
        isActive = true;
        yield return new WaitForSeconds(delayOnActivate);      
        MoveToEndPoint();
    }
    private void MoveToEndPoint()
    {
        //Apply platform Itween hash value to StartPoint
        if (movement == MovementType.Position)
            ApplyPlatformMovement(startPoint.position,endPoint.position,settings.delayOnEndRached,"MoveToStartPoint");
        else
            ApplyPlatformMovement(startPoint.eulerAngles,endPoint.eulerAngles,settings.delayOnEndRached,"MoveToStartPoint");
    }
    public void MoveToStartPoint()
    {
        //Apply platform Itween hash value to EndPoint
        if (movement == MovementType.Position)
            ApplyPlatformMovement(endPoint.position,startPoint.position,settings.delayOnStartRached,"MoveToEndPoint");
        else
            ApplyPlatformMovement(endPoint.eulerAngles, startPoint.eulerAngles, settings.delayOnStartRached, "MoveToEndPoint");


    }
    /// <summary>
    /// Apply Itween Hash Value To, update method called "UpdatePosition"
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="delay"></param>
    /// <param name="callback"></param>
    public void ApplyPlatformMovement(Vector3 from, Vector3 to, float delay, string callback)
    {
        var hash = iTween.Hash(
            "from",from,
            "to", to,
            "delay", delay,
            "time", settings.time,
            "easeType", settings.ease,
            "onComplete",callback,
            "onupdate","UpdatePosition" );
        iTween.ValueTo(gameObject,hash);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(startPoint.position, 0.1f);
        Gizmos.DrawLine(startPoint.position, endPoint.position);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(endPoint.position, 0.1f);
    }
    public void UpdatePosition( Vector3 newValue )
    {
        if (movement == MovementType.Position)
            rb.MovePosition(newValue);
        else
            rb.transform.eulerAngles = newValue;
    }
    private void FixedUpdate()
    {
        ApplyPlayersMovement();
    }
    private void CheckPlayerLeavePlatform()
    {
       
    }
    
    private void ApplyPlayersMovement()
    {
        foreach (var player in playersTrigers)
            player.move.SetExternalForce(rb.velocity);
    }
    private void OnTriggerEnter(Collider other)
    {
        var player = other.transform.GetComponent<ThirdPersonController>();
        if (player != null && playersTrigers.Contains(player) == false)
        {
            playersTrigers.Add(player);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var player = other.transform.GetComponent<ThirdPersonController>();
        if (player != null && playersTrigers.Contains(player) == true)
        {
            player.move.SetExternalForce(Vector3.zero);
            playersTrigers.Remove(player);
        }
    }

  
}
public enum MovementType
{
    Position,
    Rotation,
}
public enum PlatformStartType
{
    Automatic,
    Event,
}