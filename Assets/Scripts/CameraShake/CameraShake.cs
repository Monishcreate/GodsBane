using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    
    private CinemachineVirtualCamera Vcam;
    private float shakeIntensity = 1f;
    private float shakeTime = 0.2f;
    private bool shake;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    void Awake()
    {
       
        shake = false;
        Vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera()
    {
        shake = true;
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
        
        if (shake)
        {
            CinemachineBasicMultiChannelPerlin _cbmcp = Vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _cbmcp.m_AmplitudeGain = shakeIntensity;

            timer = shakeTime;
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            
            if(timer <= 0)
            {
                shake = false;
                CinemachineBasicMultiChannelPerlin _cbmcp = Vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() ;
                _cbmcp.m_AmplitudeGain = 0f;
                timer = 0;
            }
        }
        
    }
}
