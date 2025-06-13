using UnityEngine;
using UnityEngine.UI;

public class GodmodeController : MonoBehaviour
{
    public Image godmodeIcon; // Asigna en el Inspector
    private bool godmodeActive = false;
    private float hue = 0f;

    private GameObject muroGodMode;

    void Start()
    {
        SetIconVisible(false); // Asegura que empieza oculto
        muroGodMode = GameObject.FindGameObjectWithTag("MuroGodMode");
    }

    void Update()
    {
        muroGodMode = GameObject.FindGameObjectWithTag("MuroGodMode");

        if(muroGodMode != null)
        {
            godmodeActive = true;
            SetIconVisible(godmodeActive);
        }
        else
        {
            godmodeActive = false;
            SetIconVisible(godmodeActive);
        }


        if (godmodeActive)
        {
            AnimateRainbowEffect();
        }
    }

    public void SetIconVisible(bool visible)
    {
        Color c = godmodeIcon.color;
        c.a = visible ? 1f : 0f; // 1 = visible, 0 = transparente
        godmodeIcon.color = c;
    }

    private void AnimateRainbowEffect()
    {
        hue += Time.deltaTime * 0.5f;
        if (hue > 1f) hue = 0f;

        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);
        rainbowColor.a = 1f; // Asegura visibilidad completa
        godmodeIcon.color = rainbowColor;
    }
}
