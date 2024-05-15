using System;
using UnityEngine;
using UnityEngine.Serialization;
using Vuforia;

public class KichBan : MonoBehaviour
{
    public GameObject goNurse;
    public CustomObserverEventHandler observerEventHandler;
    public MqttLibs mqttLibs;
    public AnimatorController animatorController;
    public SoundManager soundManager;
    public int countFirstTrack;
    public float interval = .1f;
    private float nextUpdateTime;
    public enum animationState {idle, greeting, weight_manual, height_manual};
    public const int idle = 0;
    public const int greeting = 1;
    public const int weight_manual = 2;
    public const int height_manual = 3;
    public int status_fsm = idle;
    int[] timer_counter = new int[10];
    int[] timer_flag = new int[10];

    private void Start()
    {
        countFirstTrack = 0;
    }
    
    private void Update()
    {
        if (Time.realtimeSinceStartup >= nextUpdateTime)
        {
            //Debug.Log("1s");
            timerRun();
            fsm_action();
            nextUpdateTime += interval;
        }
        if (goNurse.gameObject.activeSelf)
        {
            if (countFirstTrack == 0)
            {
                setTimer(0,1);
                status_fsm = greeting;
                countFirstTrack++;
            }
        }

        if (mqttLibs.objText.text == "can")
        {
            setTimer(0, 1);
            Debug.Log("can");
            status_fsm = weight_manual;
        }
        else if (mqttLibs.objText.text == "do")
        {
            setTimer(0, 1);
            Debug.Log("do");
            status_fsm = height_manual;

        }
    }

    void setTimer(int index, int count)
    {
        timer_counter[index] = count;
        timer_flag[index] = 0;
    }

    void timerRun()
    {
        for (int i = 0; i < 10; i++)
        {
            if (timer_counter[i] > 0)
            {
                timer_counter[i]--;

                if (timer_counter[i] == 0)
                {
                    timer_flag[i] = 1;
                }
            }
        }
    }

    void fsm_action()
    {
        switch (status_fsm)
        {
            case idle:
                
                break;
            case greeting:
                if (timer_flag[0] == 1)
                {
                    setTimer(0, 1);

                    //TODO FOR GREETING
                    Debug.Log("GREETING");
                    animatorController.TriggerAnim("Hello");
                    soundManager.PlaySound(1);

                    status_fsm = weight_manual;
                }

                break;
            case weight_manual:
                if (timer_flag[0] == 1)
                {
                    setTimer(0, 1);
                    //TODO FOR WEIGHT MANUAL
                    Debug.Log("Weight");
                    animatorController.TriggerAnim("Can");
                    soundManager.PlaySound(2);
                }

                break;
            case height_manual:
                if (timer_flag[0] == 1)
                {
                    setTimer(0, 1);
                    Debug.Log("Height");
                    animatorController.TriggerAnim("DoChieuCao");
                    soundManager.PlaySound(2);
                }

                break;
            default:
                break;
        }
    }
    
}

   