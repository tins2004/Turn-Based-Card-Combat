using UnityEngine;

public enum AnimationType 
{
  Attack,
  Dead,
  Idle
}

[System.Serializable]
public class AnimationData 
{
  [Required]
  public AnimationType animationType;
  [Required]
  public Sprite[] sprites;
}
