using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vuforia;

public class KichBan : MonoBehaviour
{
    public GameObject goNurse;

    public enum AnimationState { XinChao, Idle, DoChieuCao, DoCanNang, DoNhietDo, DoSPO2, DoNhipTim, DoHuyetAp, NoiChuyen, HoanThanh }

    [Header("EventHandler")]
    [SerializeField] private DefaultObserverEventHandler DefaultObserverEventHandler;

    [Header("Startup")]
    private bool startHelloFirst = false;

    [Header("Debugging")]
    [SerializeField] private bool firstStart = false;

    /////
    public AnimatorController animatorController;
    public SoundManager soundManager;
    public MqttLibs mqttLibs;
    [SerializeField] private AnimationState currentState;
    private AnimationState prevState;
    public float timeSinceStartup;
    public float timerInterval = 0.1f;
    // private bool isFirstScan = true;
    public Button[] btnChange;
    public void Start()
    {
        // animator = GetComponent<Animator>(); // Assuming Animator is on this GameObject
        currentState = AnimationState.Idle;
        prevState = AnimationState.Idle;
    }

    public void Update()
    {
        Debug.Log($" {DefaultObserverEventHandler.GetTargetStatus().Status} - {DefaultObserverEventHandler.GetTargetStatus().StatusInfo}");
        timeSinceStartup += Time.deltaTime;

        if (timeSinceStartup >= timerInterval)
        {
            timerRun();
            fsmAction();
            timeSinceStartup -= timerInterval;
        }

        // Quet thanh cong ImageTarget
        if (DefaultObserverEventHandler.GetTargetStatus().Status == Status.TRACKED)
            HelloStatus();
        // Quet that bai ImageTarget
        //if (DefaultObserverEventHandler.GetTargetStatus().Status != Status.TRACKED)
        //soundManager.Clear();
        // if (goNurse.activeSelf)
        // {
        //     if (isFirstScan)
        //     {
        //         StartXinChao();
        //         isFirstScan = false;
        //     }
        // }

        // if (mqttLibs.objText.text != "")
        // {
        //     HandleMqttMessage(mqttLibs.objText.text);
        // }
    }

    void HelloStatus()
    {
        HandleMqttMessage("connguoi");
    }

    public void On_ChangeStateClick(int index)
    {


        if (index == 0)
        {
            HandleMqttMessage("connguoi");
        }
        if (index == 1)
        {
            HandleMqttMessage("can");
        }
        if (index == 2)
        {
            HandleMqttMessage("cao");
        }
        if (index == 3)
        {
            HandleMqttMessage("hoanThanh");
        }
    }
    public void HandleMqttMessage(string message)
    {
        switch (message.ToLower())
        {
            case "connguoi":
                StartXinChao();
                break;
            case "cao":
                StartDoChieuCao();
                break;
            case "can":
                StartDoCanNang();
                break;
            case "nhietdo":
                StartDoNhietDo();
                break;
            case "spo2":
                StartDoSPO2();
                break;
            case "nhiptim":
                StartDoNhipTim();
                break;
            case "ecg":
                StartDoHuyetAp();
                break;
            case "hoanthanh":
                StartHoanThanh();
                break;
            default:
                // Handle unexpected messages (optional)
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
                // chi xin chao mot lan
                if (startHelloFirst && !firstStart)
                    return;
                animatorController.TriggerAnim("XinChao");
                prevState = currentState;
                soundManager.PlaySound(0);
                startHelloFirst = true;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.DoChieuCao:
                animatorController.TriggerAnim("DoChieuCao"); // Assuming trigger name is "DoChieuCao"
                soundManager.PlaySound(2);
                prevState = currentState;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.DoCanNang:
                animatorController.TriggerAnim("DoCanNang"); // Assuming trigger name is "DoCanNang"
                soundManager.PlaySound(1);
                prevState = currentState;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.DoNhietDo:
                animatorController.TriggerAnim("DoNhietDo"); // Assuming trigger name is "DoNhietDo"
                soundManager.PlaySound(3);
                prevState = currentState;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.DoSPO2:
                animatorController.TriggerAnim("DoSPO2"); // Assuming trigger name is "DoSPO2"
                soundManager.PlaySound(4);
                prevState = currentState;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.DoNhipTim:
                animatorController.TriggerAnim("DoNhipTim"); // Assuming trigger name is "DoNhipTim"
                soundManager.PlaySound(5);
                prevState = currentState;
                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.DoHuyetAp:
                animatorController.TriggerAnim("DoHuyetAp"); // Assuming trigger name is "DoHuyetAp"
                soundManager.PlaySound(6);

                currentState = AnimationState.NoiChuyen;
                break;
            case AnimationState.HoanThanh:
                animatorController.TriggerAnim("XinChao");
                soundManager.PlaySound(3);
                prevState = currentState;
                currentState = AnimationState.NoiChuyen;

                break;
                /*ase AnimationState.NoiChuyen:
                    animatorController.TriggerAnim("NoiChuyen"); // Assuming trigger name is "DoHuyetAp"

                    if(prevState==AnimationState.XinChao){

                        StartCoroutine(WaitForSecondIdle(8f));
                    }
                    if(prevState==AnimationState.DoChieuCao){
                        StartCoroutine(WaitForSecondIdle(10f));
                    }
                    if(prevState==AnimationState.DoCanNang){
                        StartCoroutine(WaitForSecondIdle(7f));
                    }
                    if(prevState==AnimationState.HoanThanh){
                        StartCoroutine(WaitForSecondIdle(8f)); 
                    }
                    currentState = AnimationState.Idle;
                    break;  */
        }

        if (currentState == AnimationState.NoiChuyen)
        {
            animatorController.TriggerAnim("NoiChuyen"); // Assuming trigger name is "DoHuyetAp"

            if (prevState == AnimationState.XinChao)
            {
                StartCoroutine(WaitForSecondIdle(8f));
            }
            if (prevState == AnimationState.DoChieuCao)
            {
                StartCoroutine(WaitForSecondIdle(10f));
            }
            if (prevState == AnimationState.DoCanNang)
            {
                StartCoroutine(WaitForSecondIdle(7f));
            }
            if (prevState == AnimationState.HoanThanh)
            {
                StartCoroutine(WaitForSecondIdle(8f));
            }
            currentState = AnimationState.Idle;
        }
    }
    IEnumerator WaitForSecondIdle(float second)
    {
        animatorController.BoolAnimTrue("NoiChuyen");
        yield return new WaitForSecondsRealtime(second);
        animatorController.BoolAnimFalse("NoiChuyen");
        currentState = AnimationState.Idle;
    }
}