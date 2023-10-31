using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class PhotoApp : MonoBehaviour
{
    [SerializeField]
    /// <summary>
    /// The menu that is displayed when the app is first opened
    /// </summary>
    private GameObject selectMenu;
    [SerializeField]
    /// <summary>
    /// The menu that is displayed when a photo is selected to edit
    /// </summary>
    private GameObject editMenu;
    [SerializeField]
    private GameObject displayImage;
    [SerializeField]
    private InputManager inputManager;
    /// <summary>
    /// The list of photos that can be edited and viewed
    /// </summary>
    [SerializeField]
    private List<Texture2D> photos;
    /// <summary>
    /// The photo that is currently being edited or viewed
    /// </summary>
    private Image photo;
    /// <summary>
    /// The index of the photo that is currently being edited or viewed
    /// </summary>
    private int photoIndex = 0;
    /// <summary>
    /// The slider that controls the brightnes of the photo
    /// </summary>
    private Slider brightnesSlider;
    /// <summary>
    /// The slider that controls the contrast of the photo
    /// </summary>
    private Slider contrastSlider;
    /// <summary>
    /// The toggle that controls the red color of the photo
    /// </summary>
    private Toggle redToggle;
    /// <summary>
    /// The toggle that controls the green color of the photo
    /// </summary>
    private Toggle greenToggle;
    /// <summary>
    /// The toggle that controls the blue color of the photo
    /// </summary>
    private Toggle blueToggle;
    /// <summary>
    /// The texture that is outputted after all adjustments are made
    /// </summary>
    private Texture2D outputTexture;

    private Animator imgAnimator;
    private bool isFullscreen = false;

    void OnEnable()
    {
        if(selectMenu == null || editMenu == null)
        {
            Debug.LogError("One of the menu variables is null");
        }
        else
        {
            selectMenu.SetActive(true);
            editMenu.SetActive(false);
        }

        if(inputManager == null)
            Debug.LogError("Input manager is null");
        
        if(displayImage == null)
            Debug.LogError("Photo is null or does not have an animator component");
        
        imgAnimator = displayImage.GetComponent<Animator>();
        photo = displayImage.GetComponent<Image>();

        photoIndex = 0;
        if(photos.Count == 0)
            Debug.LogError("No photos in list");
        else
            SetImage(photos[photoIndex]);
    }
    /// <summary>
    /// Makes the photo fullscreen
    /// </summary>
     public void ViewPhoto()
    {
        imgAnimator.SetTrigger("FsEnter");
        isFullscreen = true;
    }
    /// <summary>
    /// Returns the photo to its original size
    /// </summary>
    public void ExitFullscreen()
    {
        imgAnimator.SetTrigger("FsExit");
    }
    /// <summary>
    /// Opens the edit menu and closes the select menu
    /// </summary>
    public void OpenEditMenu()
    {
        selectMenu.SetActive(false);
        editMenu.SetActive(true);
        imgAnimator.SetTrigger("EdtEnter");
        // finds the sliders and toggles and sets them to their respective variables
        brightnesSlider = GameObject.Find("BrightnesSlider").GetComponent<Slider>();
        contrastSlider = GameObject.Find("ContrastSlider").GetComponent<Slider>();
        redToggle = GameObject.Find("RedToggle").GetComponent<Toggle>();
        greenToggle = GameObject.Find("GreenToggle").GetComponent<Toggle>();
        blueToggle = GameObject.Find("BlueToggle").GetComponent<Toggle>();
    }
    /// <summary>
    /// Closes the edit menu and opens the select menu
    /// </summary>
    public void CloseEditMenu()
    {
        imgAnimator.SetTrigger("EdtExit");
        selectMenu.SetActive(true);
        editMenu.SetActive(false);
    }
    /// <summary>
    /// Sets the image being displayed to the next photo in the list and loops back to the first photo if the last photo is reached
    /// </summary>
    public void NextPhoto()
    {
        photoIndex = (photoIndex + 1) % photos.Count;
        if(photoIndex < photos.Count)
            SetImage(photos[photoIndex]);
        else
            Debug.LogError("Photo index out of range");
    }
    /// <summary>
    /// Sets the image being displayed to the previous photo in the list and loops back to the last photo if the first photo is reached
    /// </summary>
    public void PreviousPhoto()
    {
        photoIndex = photoIndex == 0 ? photos.Count - 1 : photoIndex - 1;
        if(photoIndex < photos.Count)
            SetImage(photos[photoIndex]);
        else
            Debug.LogError("Photo index out of range");
    }
    /// <summary>
    /// Sets the image being displayed to the given texture
    /// </summary>
    /// <param name="texture"></param>
    void SetImage(Texture2D texture)
    {
        photo.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
    /// <summary>
    /// Saves the current photo as a new photo in the list
    /// </summary>
    public void SavePhotoAsNew()
    {
        photos.Add(outputTexture);
        photoIndex = photos.Count - 1;
        SetImage(photos[photoIndex]);
    }
    /// <summary>
    /// Saves the current photo over the current photo in the list
    /// </summary>
    public void SavePhoto()
    {
        photos[photoIndex] = outputTexture;
        SetImage(photos[photoIndex]);
    }

    /// <summary>
    /// Calls all of the adjustment functions
    /// </summary>
    public void AdjustmentCall()
    {
        outputTexture = photos[photoIndex];
        outputTexture = InvertColors(outputTexture);
        outputTexture = SetBrightnes(outputTexture);
        outputTexture = SetContrast(outputTexture);
        SetImage(outputTexture);
    }
    /// <summary>
    /// Inverts the colors of the given texture
    /// </summary>
    /// <param name="texture"></param>
    /// <returns> The inverted texture
    /// </returns>
    private Texture2D InvertColors(Texture2D texture)
    {   
        // Checks if any of the color toggles are null or if none of them are on
        if(redToggle == null || greenToggle == null || blueToggle == null)
        {
            Debug.LogError("One of the color toggles is null");
            return texture;
        }
        if (!redToggle.isOn && !greenToggle.isOn && !blueToggle.isOn)
        {
            return texture;
        }
        Texture2D invTexture = new Texture2D(texture.width, texture.height);
        Color[] pixels = texture.GetPixels();
        Color[] newPixels = new Color[pixels.Length];
        // Loops through all of the pixels and inverts the colors based on the toggles
        for (int i = 0; i < pixels.Length; i++)
        {
            if (redToggle.isOn)
            {
                newPixels[i].r = 1 - pixels[i].r;
            }
            if (greenToggle.isOn)
            {
                newPixels[i].g = 1 - pixels[i].g;
            }
            if (blueToggle.isOn)
            {
                newPixels[i].b = 1 - pixels[i].b;
            }
        }
        invTexture.SetPixels(newPixels);
        invTexture.Apply();
        return invTexture;
    }
    /// <summary>
    /// Sets the brightnes of the given texture
    /// </summary>
    /// <param name="texture"></param>
    /// <returns> The texture with the brightnes adjusted
    /// </returns>
    private Texture2D SetBrightnes(Texture2D texture)
    {
        // Checks if the brightnes slider is null
        if(brightnesSlider == null)
        {
            Debug.LogError("Brightnes slider is null");
            return texture;
        }
        float brightnes = brightnesSlider.value;
        Texture2D brightTexture = new Texture2D(texture.width, texture.height);
        Color[] pixels = texture.GetPixels();
        Color[] newPixels = new Color[pixels.Length];
        // Loops through all of the pixels and adjusts the brightnes
        for (int i = 0; i < pixels.Length; i++)
        {
            newPixels[i] = new Color(pixels[i].r + brightnes, pixels[i].g + brightnes, pixels[i].b + brightnes);
        }
        brightTexture.SetPixels(newPixels);
        brightTexture.Apply();
        return brightTexture;
    }
    /// <summary>
    /// Sets the contrast of the given texture
    /// </summary>
    /// <param name="texture"></param>
    /// <returns> The texture with the contrast adjusted
    /// </returns>
    private Texture2D SetContrast(Texture2D texture)
     {
        // Checks if the contrast slider is null
        if(contrastSlider == null)
        {
            Debug.LogError("Contrast slider is null");
            return texture;
        }
        float contrast = contrastSlider.value;
        Texture2D contrastTexture = new Texture2D(texture.width, texture.height);
        Color[] pixels = texture.GetPixels();
        Color[] newPixels = new Color[pixels.Length];
        // Loops through all of the pixels and adjusts the contrast within the range of 0 to 1
        for(int i = 0; i < pixels.Length; i++)
        {
            float red = pixels[i].r/1;
            red -= 0.05f;
            red *= contrast;
            red += 0.05f;
            if(red > 1)
            {
                red = 1;
            }
            else if(red < 0)
            {
                red = 0;
            }
            float green = pixels[i].g/1;
            green -= 0.05f;
            green *= contrast;
            green += 0.05f;
            if(green > 1)
            {
                green = 1;
            }
            else if(green < 0)
            {
                green = 0;
            }
            float blue = pixels[i].b/1;
            blue -= 0.05f;
            blue *= contrast;
            blue += 0.05f;
            if(blue > 1)
            {
                blue = 1;
            }
            else if(blue < 0)
            {
                blue = 0;
            }
            newPixels[i] = new Color(red, green, blue, pixels[i].a);
        }
        contrastTexture.SetPixels(newPixels);
        contrastTexture.Apply();
        return contrastTexture;

    }
    /// <summary>
    /// Closes the app
    /// </summary>
    public void CloseApp()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(inputManager.GetIsEscPressed() && isFullscreen)
        {
            isFullscreen = false;
            ExitFullscreen();
        }
    }
}
