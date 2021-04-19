using UnityEngine.UI;

public class CustomSlider : Slider
{
    public void SetValue(float newValue)
    {
        Set(newValue, false);
    }
}
