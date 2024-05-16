using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vuforia;

public class KichBan : MonoBehaviour
{
    public GameObject goNurse;
    
    public enum AnimationState { XinChao, Idle, DoChieuCao, DoCanNang, DoNhietDo, DoSPO2, DoNhipTim, DoHuyetAp }

    public AnimatorController animatorController;
    public SoundManager soundManager;
    public MqttLibs mqttLibs;
    [SerializeField] private AnimationState currentState;
    public float timeSinceStartup;
    public float timerInterval = 0.1f;
    // private bool isFirstScan = true;

    public void Start()
    {
        // animator = GetComponent<Animator>(); // Assuming Animator is on this GameObject
        currentState = AnimationState.Idle;
    }

    public void Update()
    {
        timeSinceStartup += Time.deltaTime;

        if (timeSinceStartup >= timerInterval)
        {
            timerRun();
            fsmAction();
            timeSinceStartup -= timerInterval;
        }

        // if (goNurse.activeSelf)
        // {
        //     if (isFirstScan)
        //     {
        //         StartXinChao();
        //         isFirstScan = false;
        //     }
        // }

        if (mqttLibs.objText.text != "")
        {
            HandleMqttMessage(mqttLibs.objText.text);
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
                animatorController.TriggerAnim("XinChao"); // Assuming trigger name is "XinChao"
                soundManager.PlaySound(1);
                currentState = AnimationState.Idle;
                break;
            case AnimationState.DoChieuCao:
                animatorController.TriggerAnim("DoChieuCao"); // Assuming trigger name is "DoChieuCao"
                soundManager.PlaySound(2);
                currentState = AnimationState.Idle;
                break;
            case AnimationState.DoCanNang:
                animatorController.TriggerAnim("DoCanNang"); // Assuming trigger name is "DoCanNang"
                soundManager.PlaySound(2);
                currentState = AnimationState.Idle;
                break;
            case AnimationState.DoNhietDo:
                animatorController.TriggerAnim("DoNhietDo"); // Assuming trigger name is "DoNhietDo"
                soundManager.PlaySound(2);
                currentState = AnimationState.Idle;
                break;
            case AnimationState.DoSPO2:
                animatorController.TriggerAnim("DoSPO2"); // Assuming trigger name is "DoSPO2"
                soundManager.PlaySound(2);
                currentState = AnimationState.Idle;
                break;
            case AnimationState.DoNhipTim:
                animatorController.TriggerAnim("DoNhipTim"); // Assuming trigger name is "DoNhipTim"
                soundManager.PlaySound(2);
                currentState = AnimationState.Idle;
                break;
            case AnimationState.DoHuyetAp:
                animatorController.TriggerAnim("DoHuyetAp"); // Assuming trigger name is "DoHuyetAp"
                soundManager.PlaySound(2);
                currentState = AnimationState.Idle;
                break;
        }
    }
}