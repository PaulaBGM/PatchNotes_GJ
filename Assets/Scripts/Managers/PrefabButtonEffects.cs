using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PrefabButtonEffects : MonoBehaviour
{
    [Header("Botones")]
    [SerializeField] private Button _buttonA;
    [SerializeField] private Button _buttonB;

    [Header("Audio")]
    [SerializeField] private AudioClip _hoverClip;
    [SerializeField] private AudioClip _clickClip;
    [SerializeField] private AudioSource _audioSource;

    [Header("Animación")]
    [SerializeField] private float _scaleAmount = 1.1f;
    [SerializeField] private float _scaleSpeed = 8f;

    private Vector3 _originalScaleA;
    private Vector3 _originalScaleB;
    private bool _hoveringA;
    private bool _hoveringB;

    private void Start()
    {
        _originalScaleA = _buttonA.transform.localScale;
        _originalScaleB = _buttonB.transform.localScale;

        AddEvents(_buttonA, () => _hoveringA = true, () => _hoveringA = false);
        AddEvents(_buttonB, () => _hoveringB = true, () => _hoveringB = false);
    }

    private void Update()
    {
        AnimateButton(_buttonA.transform, _originalScaleA, _hoveringA);
        AnimateButton(_buttonB.transform, _originalScaleB, _hoveringB);
    }

    private void AnimateButton(Transform button, Vector3 originalScale, bool hovering)
    {
        Vector3 targetScale = hovering
            ? new Vector3(originalScale.x * _scaleAmount, originalScale.y, originalScale.z)
            : originalScale;

        button.localScale = Vector3.Lerp(button.localScale, targetScale, Time.deltaTime * _scaleSpeed);
    }

    private void AddEvents(Button button, System.Action onEnter, System.Action onExit)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        // Pointer Enter
        EventTrigger.Entry enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        enter.callback.AddListener((_) =>
        {
            onEnter?.Invoke();
            _audioSource.PlayOneShot(_hoverClip);
        });
        trigger.triggers.Add(enter);

        // Pointer Exit
        EventTrigger.Entry exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        exit.callback.AddListener((_) => onExit?.Invoke());
        trigger.triggers.Add(exit);

        // Pointer Click
        EventTrigger.Entry click = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        click.callback.AddListener((_) => _audioSource.PlayOneShot(_clickClip));
        trigger.triggers.Add(click);
    }
}