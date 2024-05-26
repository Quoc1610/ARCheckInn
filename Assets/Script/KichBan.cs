using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vuforia;

public class KichBan : MonoBehaviour
{
    public GameObject goNurse;
    public GameObject goPosMove;
    public Vector3 posHuyetAp;
    public enum AnimationState { XinChao, Idle, DoChieuCao, DoCanNang, DoNhietDo, DoSPO2, DoNhipTim, DoHuyetAp,NoiChuyen, HoanThanh }

    [Header("-------------------------EventHandler--------------------------")]
    [SerializeField] private DefaultObserverEventHandler DefaultObserverEventHandler;

    [Header("-------------------------Startup------------------------------")]
    private bool startHelloFirst = false;
    public float timerInterval = 0.1f;
    private bool isFirstScan = true;
    private Quaternion prevRotation;
    [Header("-------------------------Variables----------------------------")]
    [SerializeField] private bool firstStart = false;
    [SerializeField] private bool isStartMoving= false;
    public AnimatorController animatorController;
    public SoundManager soundManager;
    public MqttLibs mqttLibs;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private AnimationState currentState;
    private AnimationState prevState;
    public float timeSinceStartup;
    public GameObject goImgTarget;   
    

    public bool isMoveHuyetAp;
    public void Awake()
    {
        currentState = AnimationState.Idle;
        prevState=AnimationState.Idle;
        mqttLibs.objText.text = "";
        posHuyetAp = new Vector3(1, 0f, 0);
        isMoveHuyetAp = false;
    
    }
    
    public void Update()
    {
        timeSinceStartup += Time.deltaTime;
        if (DefaultObserverEventHandler.GetTargetStatus().Status == Status.TRACKED)
        {   
            if (isFirstScan)
            {
                HandleMqttMessage("xinchao_va_can");
                isFirstScan = false;
                mqttLibs.Pub_WAIT();
                
            }
        }

        if (timeSinceStartup >= timerInterval)
        {
            timerRun();
            fsmAction();
            timeSinceStartup -= timerInterval;
        }

        if (isMoveHuyetAp)
        {
            if(isStartMoving==true){
                prevRotation = goNurse.transform.rotation;
                isStartMoving=false;
            }
            animatorController.animator.SetBool("Walking", true);
            
            Debug.Log("Rotation now"+prevRotation.x+" "+prevRotation.y+" "+prevRotation.z);
            Vector3 targetDirection = goPosMove.transform.position - goNurse.transform.position;
            targetDirection.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
    
            float rotationSpeed = 5.0f; 
            goNurse.transform.rotation = Quaternion.Slerp(goNurse.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            goNurse.transform.position = Vector3.MoveTowards(goNurse.transform.position, goPosMove.transform.position, 
                speed);

            if (Vector3.Distance(goNurse.transform.position, goPosMove.transform.position) < 0.1f)
            {
                isMoveHuyetAp = false;
                animatorController.animator.SetBool("Walking", false);
                goNurse.transform.rotation = prevRotation;
            }
        }



        if (mqttLibs.objText.text !="" && !isFirstScan)
        {

            HandleMqttMessage(mqttLibs.objText.text);
            mqttLibs.objText.text = "";
        }
    }

   
    public void On_ChangeStateClick(int index){
        if(index==0){
            HandleMqttMessage("xinchao_va_can");
        }
        if(index==1){
            HandleMqttMessage("do_chieu_cao");
        }
        if(index==2){
            HandleMqttMessage("do_nhiet_do");
        }
        if(index==3){
            HandleMqttMessage("spo2_va_nhip_tim");
        }
        if(index==4){
            HandleMqttMessage("do_huyet_ap");
        }
        if(index==5){
            HandleMqttMessage("hoan_thanh");
        }
    }
    public void HandleMqttMessage(string message)
    {

        switch (message.ToLower())
        {
            case "xinchao_va_can":
                StartXinChao();
                break;
            case "do_chieu_cao":
                StartDoChieuCao();
                break;
            case "do_nhiet_do":
                StartDoNhietDo();
                break;
            case "spo2_va_nhip_tim":
                StartDoSPO2vaNhipTim();
                break;
            case "do_huyet_ap":
                StartDoHuyetAp();
                break;
            case "hoan_thanh":
                StartHoanThanh();
                break;
            default:
                break;
        }
    }

    private void StartXinChao()
    {
        currentState = AnimationState.XinChao;
        SetTimer(1);
    }
    private void StartHoanThanh()
    {
        currentState = AnimationState.HoanThanh;
        SetTimer(1);
        // mqttLibs.Pub_WAIT();
    }

    private void StartDoChieuCao()
    {
        currentState = AnimationState.DoChieuCao;
        SetTimer(1);
    }

    private void StartDoCanNang()
    {
        currentState = AnimationState.DoCanNang;
        SetTimer(1);
    }

    private void StartDoNhietDo()
    {
        currentState = AnimationState.DoNhietDo;
        SetTimer(1);
    }

    private void StartDoSPO2()
    {
        currentState = AnimationState.DoSPO2;
        SetTimer(1);
    }

    private void StartDoNhipTim()
    {
        currentState = AnimationState.DoNhipTim;
        SetTimer(1);
    }

    private void StartDoHuyetAp()
    {
        currentState = AnimationState.DoHuyetAp;
        SetTimer(1);
    }

    private void StartDoSPO2vaNhipTim()
    {
        currentState = AnimationState.DoSPO2;
        SetTimer(1);
    }

    private void SetTimer(int duration)
    {
        timerInterval = duration * 0.1f; // Convert seconds to timer ticks
    }

    private void timerRun()
    {
        if (timerInterval > 0)
        {
            timerInterval -= Time.deltaTime;
            if (timerInterval <= 0)
            {
                currentState = AnimationState.Idle;
                timerInterval = 0.1f; // Reset default timer interval
            }
        }
    }

    private void fsmAction()
    {
        switch (currentState)
        {
            case AnimationState.XinChao:
                animatorController.TriggerAnim("XinChao");
                prevState=currentState;
                soundManager.PlaySound(0);
                // currentState = AnimationState.NoiChuyen;
                currentState = AnimationState.DoCanNang;
                break;
            case AnimationState.DoChieuCao:
                animatorController.TriggerAnim("DoChieuCao"); // Assuming trigger name is "DoChieuCao"
                soundManager.PlaySound(1);
                prevState=currentState;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.DoCanNang:
                prevState=currentState;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.DoNhietDo:
                animatorController.TriggerAnim("DoNhietDo"); // Assuming trigger name is "DoNhietDo"
                soundManager.PlaySound(2);
                prevState=currentState;
                currentState = AnimationState.NoiChuyen;
                
                break;
            case AnimationState.DoSPO2:
                animatorController.TriggerAnim("DoChieuCao"); // Assuming trigger name is "DoSPO2"
                soundManager.PlaySound(3);
                prevState=currentState;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.DoNhipTim:
                animatorController.TriggerAnim("DoNhipTim"); // Assuming trigger name is "DoNhipTim"
                soundManager.PlaySound(5);
                prevState=currentState;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.DoHuyetAp:
                animatorController.TriggerAnim("DoHuyetAp"); // Assuming trigger name is "DoHuyetAp"
                soundManager.PlaySound(4);
                prevState=currentState;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.HoanThanh:
                // animatorController.TriggerAnim("XinChao");
                soundManager.PlaySound(5);
                prevState = currentState;
                currentState = AnimationState.NoiChuyen;

                break;
            case AnimationState.NoiChuyen:
                animatorController.TriggerAnim("NoiChuyen"); // Assuming trigger name is "DoHuyetAp"
                if(prevState==AnimationState.DoNhietDo){
                    StartCoroutine(WaitForSecondNhietDo(6f));
                    
                }
                if(prevState==AnimationState.DoChieuCao){
                    StartCoroutine(WaitForSecondIdle(5f));
                }
                if(prevState==AnimationState.DoSPO2){
                    StartCoroutine(WaitForSecondIdle(5f));
                }
                if(prevState==AnimationState.DoHuyetAp){
                    StartCoroutine(WaitForSecondIdle(7f));
                }
                if(prevState==AnimationState.DoCanNang){
                    // StartCoroutine(WaitForSecondIdle(7f));
                    StartCoroutine(WaitForCan(3.5f));
                }
                if(prevState==AnimationState.HoanThanh){
                    // StartCoroutine(WaitForSecondIdle(10f)); 
                    StartCoroutine(WaitForHoanThanh(10f));
                }
                currentState = AnimationState.Idle;
                break;            
        }
    }
    IEnumerator WaitForSecondIdle(float second)
    {
        animatorController.BoolAnimTrue("NoiChuyen");
        yield return new WaitForSecondsRealtime(second);
        animatorController.BoolAnimFalse("NoiChuyen");
        currentState=AnimationState.Idle;
    }
    IEnumerator WaitForSecondNhietDo(float second)
    {
        animatorController.BoolAnimTrue("NoiChuyen");
        yield return new WaitForSecondsRealtime(second);
        animatorController.BoolAnimFalse("NoiChuyen");
        yield return new WaitForSecondsRealtime(2f);
        isMoveHuyetAp = true;
        isStartMoving=true;
        currentState=AnimationState.Idle;
    }
    IEnumerator WaitForCan(float second)
    {
        yield return new WaitForSecondsRealtime(second);
        animatorController.TriggerAnim("DoCanNang");
        animatorController.animator.SetBool("NoiChuyen",true);
        yield return new WaitForSecondsRealtime(second + 4f);
        animatorController.animator.SetBool("NoiChuyen",false);
        currentState=AnimationState.Idle;
        yield return new WaitForSecondsRealtime(2f);
        
    }

    IEnumerator WaitForHoanThanh(float second)
    {
        animatorController.BoolAnimTrue("NoiChuyen");
        yield return new WaitForSecondsRealtime(second);
        animatorController.BoolAnimFalse("NoiChuyen");
        animatorController.TriggerAnim("XinChao");
        currentState=AnimationState.Idle;
    }
}