using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButtonPlatform : InteractableWorld
{
    public Platform platformToActivate;
    public override void NetworkMe()
    {
        platformToActivate.ActivePlatform();
    }

   
}
