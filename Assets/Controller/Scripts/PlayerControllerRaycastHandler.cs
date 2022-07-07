using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerControllerRaycastHandler
{
    public InteractableWorld currentInteract;

    public RaycastPlayerType type;
    private ThirdPersonController _controller;

   public void Init(ThirdPersonController controler)
   {
        _controller = controler;
    }
    public void Update()
    {
        RaycastHit hit;
        Ray ray = _controller.components.camera.ScreenPointToRay(Input.mousePosition);
        if (type == RaycastPlayerType.ToScreenCenter)
            ray = _controller.components.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if(Physics.Raycast(ray, out hit, 100,_controller.physics.ignorePlayerLayer))
        {
            var interact = hit.transform.GetComponent<InteractableWorld>();
            if (interact)
            {
                if (interact != currentInteract)
                {
                    if (currentInteract)
                    {
                        ExitInteract();
                    }
                    currentInteract = interact;
                    interact.OnInteractEnter();
                    _controller.cursorHandler.SetUICursorInteractable();
                }

            }
            else
            {
                if (currentInteract)
                {
                    ExitInteract();
                }
            }
        }
        else
        {
            if (currentInteract)
            {
                ExitInteract();
            }
        }
    }

    private void ExitInteract()
    {
        currentInteract.OnInteractExit();
        currentInteract = null;
        _controller.cursorHandler.SetUICursorNomal();
    }
}

public enum RaycastPlayerType
{
    ToMousePosition,
    ToScreenCenter,
}
