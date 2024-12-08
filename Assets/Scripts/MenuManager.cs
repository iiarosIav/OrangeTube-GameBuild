using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public string LevelName;
    public Image image;
    public TextMeshProUGUI text;

    [Header("NicknameInputField")]
    public TMP_InputField NickInputField;
    public TextMeshProUGUI InputFieldText;

    [Header("Buttons")]
    public Button StartNew;
    public Button Continue;

    private float elapsedTime = 0f;
    private bool isFading = false;
    private bool isFading2 = false;
    private float duration = 2f;
    private bool canStart = false;

    private void Start()
    {
        image.gameObject.SetActive(false);
    }

    public void SetNick()
    {
        if (NickInputField.text.Length < 1)
        {
            NickInputField.text = "Введите никнейм";
            InputFieldText.color = Color.red;
            canStart = false;
            return;
        }
        else
        {
            if (NickInputField.text == "Введите никнейм" || NickInputField.text == "Несуществующий игрок") return;
            PlayerData.SetNickname(NickInputField.text);
            canStart = true;
        }
    }

    private void Update()
    {
        if(!canStart) { return; }
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
                LoadNewGame(LevelName);
            }
        }
    }

    public void SetFadingTrue() => isFading = true;
    public void SetFading2True() => isFading2 = true;

    public void LoadExistsGame(string level)
    {
        if (!NickInputField) return;
        if (!Progress.isExistPlayer(NickInputField.text)) NickInputField.text = "Несуществующий игрок";
        if(!canStart || !Progress.isExistPlayer(NickInputField.text)) return;
        if (level.Length > 0) SceneManager.LoadScene(level);
        PlayerData.SetContinue(true);
    }

    public void LoadNewGame(string level)
    {
        if (level.Length > 0) SceneManager.LoadScene(level);
    }
    
    public void TextUnAppearence()
    {
        while (text.color != new Color(1f, 1f, 1f, 0f)) text.color -= new Color(0f, 0f, 0f, Time.deltaTime * 0.1f);
    }
}
