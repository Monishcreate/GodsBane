using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private CinemachineVirtualCamera Vcam;
    private float shakeIntensity = 4f;
    private float shakeTime = 0.1f;


    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    void Awake()
    {
        instance = this; 
       
        Vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin _cbmcp = Vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = shakeIntensity;

        timer = shakeTime;
    }

   
    // Start is called before the first frame update
    void Start()
    {
        CinemachineBasicMultiChannelPerlin _cbmcp = Vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0f;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            
            if(timer <= 0)
            {
                
                CinemachineBasicMultiChannelPerlin _cbmcp = Vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() ;
                _cbmcp.m_AmplitudeGain = 0f;
                timer = 0;
               
            }
            
        }
        
    }
}
