
using DG.Tweening;
using SP.Core.GEventSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class WActionButton : MonoBehaviour, IPointerClickHandler
{
    private readonly Vector3 POSITION_SHOW_OFFSET = new Vector3(0f, -1f, 0f);
    private readonly Vector3 POSITION_HIDE_OFFSET = new Vector3(0f, -1f, 0f);
    
    private float SHOW_ANIMATION_DURATION = 0.5f;
    private float HIDE_ANIMATION_DURATION = 0.2f;
    
    private float SHOW_SCALE = 1f;
    private float HIDE_SCALE = 0.2f;
    
    private readonly Color HIDE_COLOR = new Color(1f, 1f, 1f, 0f);
    private readonly Color SHOW_COLOR = new Color(1f, 1f, 1f, 1f);
    
    [SerializeField] private SpriteRenderer _bgRenderer;
    [SerializeField] private SpriteRenderer _iconRenderer;

    private Collider2D _buttonCollider;

    private Vector3 _pos;

    public int gActionId;

    private bool _disabled = false;

    private bool _isShowed = false;
    public bool IsShowed => _isShowed;
    
    public bool IsInitialized { get; private set; }
    
    // --------------------------------------------------------------------------------------------------
    public void Awake()
    {
        Initialize();
    }
    
    // --------------------------------------------------------------------------------------------------
    public void Show()
    {
        if (_isShowed || _disabled) return;
        _isShowed = true;

        _bgRenderer.enabled = true;
        _iconRenderer.enabled = true;
        
        transform.localPosition = _pos + POSITION_SHOW_OFFSET;
        _bgRenderer.material.color = HIDE_COLOR;
        _iconRenderer.material.color = HIDE_COLOR;

        transform.localScale = Vector3.one * HIDE_SCALE;
        transform.DOScale(SHOW_SCALE, SHOW_ANIMATION_DURATION);
        
        transform.DOLocalMove(_pos, SHOW_ANIMATION_DURATION);
        _bgRenderer.material.DOColor(SHOW_COLOR, SHOW_ANIMATION_DURATION);
        _iconRenderer.material.DOColor(SHOW_COLOR, SHOW_ANIMATION_DURATION);

        if (_buttonCollider) _buttonCollider.enabled = true;
    }
    
    // --------------------------------------------------------------------------------------------------
    public void Hide()
    {
        if (!_isShowed || _disabled) return;
        _isShowed = false;
        
        transform.DOLocalMove(_pos + POSITION_HIDE_OFFSET, HIDE_ANIMATION_DURATION);
        _bgRenderer.material.DOColor(HIDE_COLOR, HIDE_ANIMATION_DURATION);
        _iconRenderer.material.DOColor(HIDE_COLOR, HIDE_ANIMATION_DURATION);
        
        transform.DOScale(HIDE_SCALE, HIDE_ANIMATION_DURATION).OnComplete(_onHideAnimationPlayed);
        
        if (_buttonCollider) _buttonCollider.enabled = false;
    }
    
    // --------------------------------------------------------------------------------------------------
    private void _onHideAnimationPlayed()
    {
        _bgRenderer.enabled = false;
        _iconRenderer.enabled = false;
    }
    
    // --------------------------------------------------------------------------------------------------
    public void OnPointerClick(PointerEventData eventData)
    {
        GEvent.UI_BUTTON_CLICKED_WAction.Invoke(new GEventData_WActionButton(this));
    }
    
    // --------------------------------------------------------------------------------------------------
    public void DisableButton()
    {
        Initialize();
        _disabled = true;
        _bgRenderer.enabled = false;
        _iconRenderer.enabled = false;
    }
    // --------------------------------------------------------------------------------------------------
    private void Initialize()
    {
        if (IsInitialized)
            return;
        
        _pos = transform.localPosition;
        
        _bgRenderer.enabled = false;
        _iconRenderer.enabled = false;

        _buttonCollider = GetComponent<Collider2D>();
        _buttonCollider.enabled = false;

        IsInitialized = true;
    }
}
