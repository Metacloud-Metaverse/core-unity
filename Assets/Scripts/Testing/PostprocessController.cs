using UnityEngine;
using UnityEngine.UI;

public class PostprocessController : MonoBehaviour
{
    public Toggle activatePostprocess;
    public MonoBehaviour volume;

    public void TogglePostprocess(bool enabled)
    {
        volume.enabled = enabled;
    }
}
