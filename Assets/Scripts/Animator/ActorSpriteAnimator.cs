using System;
using DG.Tweening;
using UnityEngine;

public class ActorSpriteAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Transform _transformSR;
    private IHasAnimations _actorData;
    private Action onAnimationComplete;

    private Sequence takeDamageSequence;

    [SerializeField, Range(0f, 60f)] private float framesPerSecond = 30f;
    private float frameTimer;

    private Sprite[] currentSprites;
    private int currentFrame;

    private bool isLooping = true;
    private bool isPlaying;

    private Color originalColor;
    private Vector3 originalPosition;


    private void Start() {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        _transformSR = _spriteRenderer.transform;

        originalColor = _spriteRenderer.color;
        originalPosition = _transformSR.localPosition;
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        if (currentSprites == null || currentSprites.Length == 0)
            return;

        frameTimer += Time.deltaTime;

        if (frameTimer >= 1f / framesPerSecond)
        {
            frameTimer = 0f;
            currentFrame++;

            if (currentFrame >= currentSprites.Length)
            {
                if (isLooping)
                {
                    currentFrame = 0;
                }
                else
                {   
                    currentFrame = currentSprites.Length - 1;

                    isPlaying = false;
                    onAnimationComplete?.Invoke();
                    return;
                }
            }

            _spriteRenderer.sprite = currentSprites[currentFrame];
        }
    }

    public void SetActorData(IHasAnimations actorData)
    {
        _actorData = actorData;
    }

    public void PlayAnim(AnimationType animType, bool loop = true, Action onComplete = null) 
    {
        currentSprites = _actorData.GetSpriteByType(animType);

        if (currentSprites == null || currentSprites.Length == 0) {
            Debug.LogWarning($"No sprites found for {animType}");
            return;
        }

        currentFrame = 0;
        frameTimer = 0f;
        isLooping = loop;
        isPlaying = true;
        onAnimationComplete = onComplete;

        _spriteRenderer.sprite = currentSprites[0];
    }

    public void TakeDamageAnim(Action onComplete = null)
    {
        if (takeDamageSequence != null && takeDamageSequence.IsActive())
        {
            takeDamageSequence.Kill(true);
        }

        takeDamageSequence = DOTween.Sequence();

        // Duration 0.1s mỗi lượt, lặp lại 2 lần, hiệu ứng yoyo (đảo ngược sau mỗi lượt)
        takeDamageSequence.Join(_spriteRenderer.DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo));
        // Duration: 0.2s, Vibrato: 5 (số lần rung), Elasticity: 1 (độ đàn hồi tối đa)
        takeDamageSequence.Join(_transformSR.DOPunchPosition(new Vector3(0, 0.2f, 0), 0.2f, vibrato: 5, elasticity: 1f));
        takeDamageSequence.OnComplete(() => {
            _spriteRenderer.color = originalColor;
            _transformSR.localPosition = originalPosition; 
        
            onComplete?.Invoke();
        });

    }

    public void StopAnim() 
    {
        isPlaying = false;
        onAnimationComplete = null;

        if (takeDamageSequence != null && takeDamageSequence.IsActive())
        {
            takeDamageSequence.Kill(true);
        }

    }

    public bool IsPlaying() 
    {
        return isPlaying;
    }
}
