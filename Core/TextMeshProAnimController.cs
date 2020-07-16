using System;
using DG.Tweening;
using SP.Localization;
using TMPro;
using UnityEngine;

public enum TriggerTextAnimationMode
{
    ALWAYS_SHOW,
    ALWAYS_HIDE,
    SHOW_ON_ENTER,
    HIDE_ON_ENTER,
}

[Serializable]
public class TriggerTextAnimationParams
{
    [SerializeField] public TriggerTextAnimationMode mode;
    [SerializeField] public float timeShow = 0.5f;
    [SerializeField] public float timeHide = 0.2f;
    [SerializeField] public Vector2 textShowOffset = new Vector2(0f, -1f);
    [SerializeField] public Vector2 textHideOffset = new Vector2(0f, -1f);
    [SerializeField] public float textHideScale = 0.2f;
    [SerializeField] public bool opacityAnimation = false;
}

public class TextMeshProAnimController : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    private MeshRenderer _meshRenderer;
    private LocalizedText _localizedText;

    private bool _initialized = false;

    private Vector3 _pos;
    
    private Vector3 pos
    {
        get
        {
            if(!_initialized) _initialize();
            return _pos;
        }
        set { _pos = value; }
    }

    private TriggerTextAnimationParams animParams;
    
    private bool _textEnabled = true;
    public bool TextEnabled
    {
        get { return _textEnabled; }
        set
        {
            if (_textEnabled != value)
            {
                _textEnabled = value;

                if (!_initialized)
                {
                    _initialize();
                }

                if (_textMeshPro) _textMeshPro.enabled = value;
                if (_meshRenderer) _meshRenderer.enabled = value;
                if (_localizedText) _localizedText.enabled = value;
            }
        }
    }
    
    // --------------------------------------------------------------------------------------------------
    private void _initialize()
    {
        _initialized = true;
        _pos = gameObject.transform.localPosition;
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _localizedText = GetComponentInChildren<LocalizedText>();
    }
    
    // --------------------------------------------------------------------------------------------------
    public void SetAnimParams(TriggerTextAnimationParams animParams)
    {
        this.animParams = animParams;

        switch (animParams.mode)
        {
            case TriggerTextAnimationMode.ALWAYS_HIDE:
                TextEnabled = false;
                break;
            case TriggerTextAnimationMode.SHOW_ON_ENTER:
                gameObject.transform.localScale = Vector3.one * animParams.textHideScale;
                gameObject.transform.localPosition = pos + new Vector3(animParams.textShowOffset.x, animParams.textShowOffset.y,0);
                if (animParams.opacityAnimation)
                    _textMeshPro.color = new Color(0f,0f,0f,0f);
                TextEnabled = false;
                break;
            case TriggerTextAnimationMode.HIDE_ON_ENTER:
                TextEnabled = true;
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localPosition = pos;
                if (animParams.opacityAnimation)
                    _textMeshPro.color = new Color(1f,1f,1f,1f);
                break;
        }
    }
    
    // --------------------------------------------------------------------------------------------------
    public void OnTriggerEnter()
    {
        Debug.Log("ontriggerenter");
        if (animParams.mode.Equals(TriggerTextAnimationMode.SHOW_ON_ENTER))
        {
            _playShowAnimation();
        }
        else if(animParams.mode.Equals(TriggerTextAnimationMode.HIDE_ON_ENTER))
        {
            _playHideAnimation();
        }
    }
    
    // --------------------------------------------------------------------------------------------------  
    public void OnTriggerExit()
    {
        if (animParams.mode.Equals(TriggerTextAnimationMode.SHOW_ON_ENTER))
        {
            _playHideAnimation();
        }
        else if(animParams.mode.Equals(TriggerTextAnimationMode.HIDE_ON_ENTER))
        {
            _playShowAnimation();
        }
    }
    
    // -------------------------------------------------------------------------------------------------- 
    private void _playShowAnimation()
    {
        TextEnabled = true;
        
        transform.DOLocalMove(pos, animParams.timeShow);
        transform.DOScale(1, animParams.timeShow);

        if (animParams.opacityAnimation)
            _textMeshPro.DOColor(new Color(1f, 1f, 1f, 1f), animParams.timeShow);
    }
    
    // --------------------------------------------------------------------------------------------------
    private void _playHideAnimation()
    {
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = pos;
        gameObject.transform.DOLocalMove(pos + new Vector3(animParams.textHideOffset.x, animParams.textHideOffset.y, 0), animParams.timeHide);
        gameObject.transform.DOScale(animParams.textHideScale, animParams.timeHide).OnComplete(_onTextHided);

        if (animParams.opacityAnimation)
            _textMeshPro.DOColor(new Color(0f, 0f, 0f, 0f), animParams.timeHide);
    }
    
    // --------------------------------------------------------------------------------------------------
    private void _onTextHided()
    {
        TextEnabled = false;
    }
}
