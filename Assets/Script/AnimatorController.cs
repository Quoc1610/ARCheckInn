using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatorController : MonoBehaviour
{
    public Animator animator;

    public void TriggerAnim(string trigger)
    {
        animator.SetTrigger(trigger);
    }
}
