using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatorController : MonoBehaviour
{
    public Animator animator;
    public Button helloButton;
    public Button canButton;
    public Button thankButton;
    public Button bangDienButton;
    
    void Start()
    {
        
    }

    public void TriggerAnim(string trigger)
    {
        animator.SetTrigger(trigger);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    

}
