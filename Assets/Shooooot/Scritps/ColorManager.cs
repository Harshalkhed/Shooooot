using UnityEngine;
using UnityEngine.Serialization;


public class ColorManager : MonoBehaviour
{
    
    public enum ColorMode
    {
        Solid, Random
    }
    
    public ColorMode colorMode;
    public Color solidColor;
    
    public GameObject floor;
    public GameObject walls;

    private Color primaryColor;
    private Color secondaryColor;

    
    private void Start()
    {
        SetColors();
    }


    private void SetColors()
    {
        switch (colorMode)
        {
            case ColorMode.Random:
                SetRandomColors();
                break;
            case ColorMode.Solid:
                SetSolidColors();
                break;
        }
        ApplyColors();
    }
    
    
    private void SetRandomColors()
    {
        // Generate the primary color randomly
        float randomHue = Random.Range(0, 1f);
        primaryColor = Color.HSVToRGB(randomHue, 0.5f, 0.5f);
        
        // Convert RGB of primaryColor to HSV and create a secondary color
        Color.RGBToHSV(primaryColor, out float hue, out float saturation, out float value);
        secondaryColor = Color.HSVToRGB(hue + 0.02f, saturation + 0.1f, value + 0.1f);
    }

    
    private void SetSolidColors()
    {
        primaryColor = secondaryColor = solidColor;
    }

    
    private void ApplyColors()
    {
        // Apply colors to floor
        Renderer floorRenderer = floor.GetComponent<Renderer>();
        floorRenderer.material.SetColor("_ColorBottom", primaryColor);
        floorRenderer.material.SetColor("_ColorTop", secondaryColor);

        // Apply colors to each wall
        for (int i = 0; i < walls.transform.childCount; i++)
        {
            Renderer wallRenderer = walls.transform.GetChild(i).GetComponent<Renderer>();
            wallRenderer.material.SetColor("_ColorBottom", secondaryColor);
            wallRenderer.material.SetColor("_ColorTop", primaryColor);
        }
    }
    
}
