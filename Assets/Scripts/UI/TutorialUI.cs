using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyUseText;
    [SerializeField] private TextMeshProUGUI keyUseAlternativeText;
    [SerializeField] private TextMeshProUGUI keyPauseText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStart())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        keyUseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Use);
        keyUseAlternativeText.text = GameInput.Instance.GetBindingText(GameInput.Binding.UseAlternative);
        keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
