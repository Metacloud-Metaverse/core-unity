using UnityEngine;
public class PlayerLookAtFollow : MonoBehaviour
{
    public float lerp = 5;
    public Vector3 offset;
    public Transform movementPlayer;
  
    void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            movementPlayer.position + offset,
            lerp *Time.deltaTime);
    }
}
