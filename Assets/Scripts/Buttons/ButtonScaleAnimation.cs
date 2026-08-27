using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;


public class ButtonScaleAnimation : ButtonBase
{
  private GameObject button;
  private const float OriginaScale = 1.0f;

  private Vector3 PreviousButtonSize;
  [SerializeField] private float Scale;

  [SerializeField] private float TimeDuration;

  [Header("Shake Settings")]
  [Tooltip("The duration of the shake animation.")]
  [SerializeField] private float shakeDuration = 0.2f;

  [Tooltip("The strength/magnitude of the shake.")]
  [SerializeField] private float shakeStrength = 0.15f;

  [Tooltip("How many times the button should shake.")]
  [SerializeField] private int shakeVibrato = 10;

  [Tooltip("The randomness of the shake.")]
  [SerializeField] private float shakeRandomness = 90f;

  void Awake()
  {
    button = gameObject;
    PreviousButtonSize = button.transform.localScale;
  }

  public override void OnDeselect(BaseEventData eventData)
  {
    Left();
  }

  public override void OnPointerClick(PointerEventData eventData)
  {

    Clicked();
  }

  public override void OnPointerEnter(PointerEventData eventData)
  {

    Entered();

  }

  public override void OnPointerExit(PointerEventData eventData)
  {

    Left();
  }

  public override void OnSelect(BaseEventData eventData)
  {
    Entered();
  }

  public override void OnSubmit(BaseEventData eventData)
  {
    Clicked();

  }

  public override void Entered()
  {
    SoundPlayer.PlaySound("UIbutton");
    transform.DOScale(Scale * PreviousButtonSize, TimeDuration).SetEase(Ease.InOutSine).SetLink(gameObject).SetUpdate(true);
  }

  public override void Left()
  {
    transform.DOScale(OriginaScale * PreviousButtonSize, TimeDuration).SetEase(Ease.InOutSine).SetLink(gameObject).SetUpdate(true);
  }

  public override void Clicked()
  {
    // throw new System.NotImplementedException();

    SoundPlayer.PlaySound("UIbuttonClicked");

    transform.DOShakeScale(
        shakeDuration,      // Duration
        shakeStrength,      // Strength (as a float, 0.15f means 15% scale variation)
        shakeVibrato,       // Vibrato (frequency of the shake)
        shakeRandomness,    // Randomness
        false              // FadeOut (set to false for a uniform shake
    )
    // 3. Chain a sequence to ensure the scale always returns to its original state.
    .OnComplete(() =>
    {
      transform.localScale = PreviousButtonSize;
    }).SetLink(gameObject).SetUpdate(true);
    // Debug.Log("--- SHAKE EXECUTED ---");
  }
}
