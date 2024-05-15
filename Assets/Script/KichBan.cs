using UnityEngine;

public class KichBan : MonoBehaviour
{
    public MqttLibs mqttLibs;
    public TestAnim animator;
    public PlayAudio audioSound;
    public float interval = .1f;
    private float nextUpdateTime;
    public const int idle = 0;
    public const int greeting = 1;
    public const int weight_manual = 2;
    public int status_fsm = idle;
    int[] timer_counter = new int[10];
    int[] timer_flag = new int[10];

    private void Start()
    {
        nextUpdateTime = Time.realtimeSinceStartup + 1f;
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup >= nextUpdateTime)
        {
            Debug.Log("1s");
            timerRun();
            fsm_action();
            nextUpdateTime += interval;
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
                setTimer(0, 100);
                status_fsm = greeting;

                

                break;
            case greeting:
                if (timer_flag[0] == 1)
                {
                    setTimer(0, 150);
                    
                    //TODO FOR GREETING
                    Debug.Log("GREETING");
                    animator.TriggerAnim("Hello");
                    audioSound.PlaySound(1);
                    
                    status_fsm = weight_manual;
                }
                break;
            case weight_manual:
                if (timer_flag[0] == 1)
                {
                    setTimer(0, 200);
                    //TODO FOR WEIGHT MANUAL
                    Debug.Log("Weight");
                    animator.TriggerAnim("Can");
                    audioSound.PlaySound(2);
                    
                    status_fsm = greeting;
                    
                }
                break;
            default:
                break;
        }
    }
}