using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestAnim : MonoBehaviour
{
    public Animator animator;
    public Button helloButton;
    public Button canButton;
    public Button thankButton;
    public Button bangDienButton;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnHello_Clicked(){
        animator.SetTrigger("Hello");
    }
    public void OnCan_Clicked(){
        animator.SetTrigger("Can");
    }
    public void OnThank_Clicked(){
        animator.SetTrigger("Thank");
    }
    public void OnBangDien_Clicked(){
        animator.SetTrigger("BangDien");
    }

}
