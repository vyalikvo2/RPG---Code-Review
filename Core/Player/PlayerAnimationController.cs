using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
   public delegate void AnimationCallbackEvent();
   public event AnimationCallbackEvent OnMoveStarted;

   private Animator _animator;

   private bool _isRunning = false;
   private bool _isWalking = false;
   
   void Awake()
   {
      _animator = GetComponent<Animator>();
      _animator.speed = 1.0f;
   }

   public void SetIsWalking(bool value)
   {
      if (_isWalking == value) return;
      _isWalking = value;
      _animator.SetBool("IsWalking", _isWalking);
   }
   
     public void SetIsRunning(bool value)
     {
        if (_isRunning == value) return;
        _isRunning = value;
        _animator.SetBool("IsRunning", _isRunning);
      }

   public void Animation_MoveStarted()
   {
      if (OnMoveStarted != null) OnMoveStarted();
   }
}
