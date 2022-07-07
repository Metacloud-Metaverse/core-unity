using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseButton : MonoBehaviour
{
    void OnMouseDown()
    {
        EnterHouseRequestMessage.Send();
    }
}
