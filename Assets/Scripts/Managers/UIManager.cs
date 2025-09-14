using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public abstract class UIManager : MonoBehaviour
{
    [Header("Main Canvas & Buttons")]
    [SerializeField] protected GameObject _mainCanvas;
    [SerializeField] protected List<GameObject> _buttonsToManage;

    [Header("Audio")]
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _hoverClip;
    [SerializeField] protected AudioClip _clickClip;

    [Header("Button Animation")]
    [SerializeField] protected float _scaleFactor = 1.1f;
    [SerializeField] protected float _lerpSpeed = 12f;

    protected Dictionary<GameObject, ButtonData> _buttonData = new();

    protected class ButtonData
    {
        public RectTransform Rect;
        public Vector3 BaseScale;
        public bool Hovering;
    }

    protected virtual void Awake()
    {
        if (_mainCanvas == null)
            _mainCanvas = GameObject.Find("Canvas");

        SetupButtons();
    }

    protected virtual void SetupButtons()
    {
        _buttonData.Clear();

        foreach (GameObject btn in _buttonsToManage)
        {
            if (btn == null) continue;
            var rect = btn.GetComponent<RectTransform>();
            if (!rect) continue;

            _buttonData[btn] = new ButtonData
            {
                Rect = rect,
                BaseScale = rect.localScale,
                Hovering = false
            };

            AddEvent(btn, EventTriggerType.PointerEnter, _ => OnHover(btn, true));
            AddEvent(btn, EventTriggerType.PointerExit, _ => OnHover(btn, false));
            AddEvent(btn, EventTriggerType.PointerClick, _ => OnClick(btn));
        }
    }

    protected void AddEvent(GameObject go, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>() ?? go.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    protected virtual void OnHover(GameObject btn, bool entering)
    {
        if (_buttonData.TryGetValue(btn, out var data))
        {
            data.Hovering = entering;
            if (entering && _hoverClip && _audioSource)
                _audioSource.PlayOneShot(_hoverClip);
        }
    }

    protected virtual void OnClick(GameObject btn)
    {
        if (_clickClip && _audioSource)
            _audioSource.PlayOneShot(_clickClip);
    }

    protected virtual void Update()
    {
        UpdateButtonAnimations();
    }

    protected virtual void UpdateButtonAnimations()
    {
        foreach (var kvp in _buttonData)
        {
            var data = kvp.Value;
            float targetScaleX = data.Hovering ? data.BaseScale.x * _scaleFactor : data.BaseScale.x;
            Vector3 target = new Vector3(targetScaleX, data.BaseScale.y, data.BaseScale.z);
            data.Rect.localScale = Vector3.Lerp(data.Rect.localScale, target, Time.deltaTime * _lerpSpeed);
        }
    }

    // Método para limpiar y volver a configurar botones si cambian
    public void RegisterButtons(List<GameObject> buttons)
    {
        _buttonsToManage = buttons;
        SetupButtons();
    }

    public void ClearButtons()
    {
        _buttonData.Clear();
        _buttonsToManage.Clear();
    }
}
