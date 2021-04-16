using UnityEngine;

public class BeatTestManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Animator animator;
    public CustomSlider slider;
    private void Start()
    {
        slider = GetComponent<CustomSlider>();
        print(slider);
        slider.maxValue = audioSource.clip.length;
    }

    private void Update()
    {
        slider.SetValue(audioSource.time);
    }

    public void SetTime(float value)
    {
        audioSource.time = value;
        animator.Play("FireAnim", 0, value / audioSource.clip.length);
        
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] myAnimatorClip = animator.GetCurrentAnimatorClipInfo(0);
        float time = myAnimatorClip[0].clip.length * animationState.normalizedTime;
        
        print(time);
    }
}
