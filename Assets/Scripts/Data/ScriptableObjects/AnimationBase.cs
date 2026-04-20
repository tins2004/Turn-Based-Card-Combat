using UnityEngine;

public enum AnimationType 
{
  Attack,
  Dead,
  Idle,
  Init
}

[System.Serializable]
public class AnimationData 
{
  [Required]
  public AnimationType animationType;
  [Required]
  public Sprite[] sprites;
}
