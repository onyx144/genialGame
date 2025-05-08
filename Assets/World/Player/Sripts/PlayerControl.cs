using UnityEngine;

[SelectionBase]
public class PlayerControl : MonoBehaviour
{
    #region EditorData
    [Header("MovementAttribute")]
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Dependence")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private GameObject _character; // Укажи сюда Player/Visual/Character
    [SerializeField] private SpriteRenderer _spriteRenderer; // Укажи сюда SpriteRenderer персонажа
    [SerializeField] private Sprite _idleSprite; // Укажи сюда спрайт для покоя
    #endregion

    #region InternalData
    private Vector2 _moveDir = Vector2.zero;
    private Animator _animator;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (_character != null)
            _animator = _character.GetComponent<Animator>();
        else
            Debug.LogError("Character reference not assigned in PlayerControl.");
    }

    private void Update()
    {
        GetInput();
       // UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Move();
        UpdateAnimation();
    }
    #endregion

    #region Input
    private void GetInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }
    #endregion

    #region Movement
    private void Move()
    {
if (_moveDir.sqrMagnitude > 0.01f)
        _rb.linearVelocity = _moveDir.normalized * _moveSpeed * Time.fixedDeltaTime;
    else
        _rb.linearVelocity = Vector2.zero;    }
    #endregion

    #region Animation
    private void UpdateAnimation()
    {
        if (_animator == null || _spriteRenderer == null || _idleSprite == null) return;

        bool isMoving = _rb.linearVelocity.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            if (!_animator.enabled)
            {
                _animator.enabled = true;
            }
            _animator.SetBool("isMoving", true);
        }
        else
        {
            if (_animator.enabled)
            {
                _animator.enabled = false;
            }
            _spriteRenderer.sprite = _idleSprite;
        }
    }
    #endregion
}
