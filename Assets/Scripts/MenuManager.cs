using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public string LevelName;
    public Image image;
    public TextMeshProUGUI text;

    private float elapsedTime = 0f;
    private bool isFading = false;
    private bool isFading2 = false;
    public float duration = 2f;

    private void Start()
    {
        image.gameObject.SetActive(false);
    }

    private void Update()
    {
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        if (isFading)
        {
            image.gameObject.SetActive (true);
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);

            Color color = image.color;
            color.a = alpha;
            image.color = color;

            if (elapsedTime >= duration)
            {
                isFading = false;
                elapsedTime = 0f;
            }
        }
        if (isFading2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - elapsedTime / duration);

            Color color = text.color;
            color.a = alpha;
            text.color = color;

            if (elapsedTime >= duration)
            {
                isFading2 = false;
                elapsedTime = 0f;
                LoadNewGame();
            }
        }
    }

    public void SetFadingTrue() => isFading = true;
    public void SetFading2True() => isFading2 = true;

    public void LoadExistsGame(string level)
    {

    }

    public void LoadNewGame()
    {
        if (LevelName.Length > 0) SceneManager.LoadScene(LevelName);
    }
    
    public void TextUnAppearence()
    {
        while (text.color != new Color(1f, 1f, 1f, 0f)) text.color -= new Color(0f, 0f, 0f, Time.deltaTime * 0.1f);
    }
}
