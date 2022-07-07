using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerControllerInput
{
    private ThirdPersonController _controller;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    public Vector2 axis { get; private set; }
    public Vector2 look { get; private set; }
    public bool sprint { get; private set; }
    public bool jump;
    public inputEvent OnJumpPress;
    public delegate void inputEvent();


    private Vector3 _previousMousePosition;

    public bool enablePlayerHandler { get; private set; }
    public void SetHandlerEnable(bool value)
    {
        enablePlayerHandler = value;
    }

    public void Init(ThirdPersonController controller)
    {
        _controller = controller;
        _controller.StartCoroutine(Update());        
    }



    private IEnumerator Update()
    {
        while (true)
        {
            //loop infinite when this player handler is disabled
            while (enablePlayerHandler == false)
            {
                if (axis != Vector2.zero)
                    axis = Vector2.zero;
                yield return null;
            }

            sprint = Input.GetKey(sprintKey);
            axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Input.GetKeyDown(jumpKey))
            {
                if (OnJumpPress != null)
                    OnJumpPress.Invoke();
                jump = true;
            }

            // look = Input.mousePosition - _previousMousePosition;
            //  look = new Vector2(look.x , look.y * -1);

            if (Input.GetMouseButtonDown(0))
            {
                if (_controller.raycastsHandler.currentInteract)
                {
                    _controller.raycastsHandler.currentInteract.OnInteractRequest(_controller);
                }
            }
            look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * -1);
            _previousMousePosition = Input.mousePosition;

            if (Input.GetKeyDown(KeyCode.M))
            {
                UIManager.Instance.ToggleMap();
            } 
            if (Input.GetKeyDown(KeyCode.T))
            {                
                OnGameUIManager.Instance.SetActionsMenuActive(true);
            }
          /*  if (Input.GetKeyUp(KeyCode.T))
            {
                OnGameUIManager.Instance.SetActionsMenuActive(false);
            }*/
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                OnGameUIManager.Instance.ToggleMainMenuOnGame();
            }

            yield return null;
        }

    }
}

