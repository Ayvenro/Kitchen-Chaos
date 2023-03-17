using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    [SerializeField] private Slider soundEffectVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Image soundEffectVolumeFill;
    [SerializeField] private Image musicVolumeFill;
    [SerializeField] private Button closeButton;
    [Header("Назначение клавиш")]
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI useText;
    [SerializeField] private TextMeshProUGUI useAlternativeText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Button useAlternativeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Transform pressToRebindKeyTransform;

    private Action onCloseButtonAction;
    private void Awake()
    {
        Instance = this;
        soundEffectVolumeSlider.onValueChanged.AddListener((value) => 
        {
            SoundManager.Instance.ChangeVolume(value);
            UpdateVisual();
        });
        musicVolumeSlider.onValueChanged.AddListener((value) =>
        {
            MusicManager.Instance.ChangeVolume(value);
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });
        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveUp);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveDown);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveLeft);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveRight);
        });
        useButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Use);
        });
        useAlternativeButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.UseAlternative);
        });
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Pause);
        });
    }

    private void Start()
    {
        soundEffectVolumeSlider.value = SoundManager.Instance.GetVolume();
        musicVolumeSlider.value = MusicManager.Instance.GetVolume();
        Hide();
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectVolumeFill.color = Color.Lerp(Color.red, Color.green, soundEffectVolumeSlider.value);
        musicVolumeFill.color = Color.Lerp(Color.red, Color.green, musicVolumeSlider.value);
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        useText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Use);
        useAlternativeText.text = GameInput.Instance.GetBindingText(GameInput.Binding.UseAlternative);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });

    }
}
