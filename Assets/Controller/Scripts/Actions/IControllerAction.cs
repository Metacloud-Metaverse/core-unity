using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllerAction 
{
    void Init(ThirdPersonController controller);
    void Update();
    void FixedUpdate();
}
