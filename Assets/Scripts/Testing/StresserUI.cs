using UnityEngine;
using UnityEngine.UI;
public class StresserUI : MonoBehaviour
{
    public InputField countInput;
    public InputField columnInput;
    public InputField distanceInput;
    public Stresser stresser;
    public PostprocessController postprocessController;
    public Text fpsText;
    public Toggle activatePostprocess;
    public Toggle isMotion;

    private float _deltaTime;

    void Start()
    {
        countInput.text = stresser.count.ToString();
        columnInput.text = stresser.rowCount.ToString();
        distanceInput.text = stresser.distance.ToString();
    }

    private void Update()
    {
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
        
    }

    public void Refresh()
    {
        var count = int.Parse(countInput.text);
        var column = int.Parse(columnInput.text);
        var distance = float.Parse(distanceInput.text);

        stresser.Refresh(count, column, distance);
    }

    public void SetMotion()
    {
        stresser.SetMotion(isMotion.isOn);
    }

    public void SetPostprocess()
    {
        postprocessController.TogglePostprocess(activatePostprocess.isOn);
    }
}
