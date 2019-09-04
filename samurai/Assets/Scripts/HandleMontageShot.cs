using UnityEngine;

// 蒙太奇镜头数据
public class MontageShotData
{    
    public bool isCritical;
    public bool isFatalAttack;    
}

public class HandleMontageShot
{
    public static void MontageShotHandler(MontageShotData montageData)
    {
        if (montageData.isCritical)
        {
            CameraBehavior.Instance.ChangeTimeScale(0.25f, 0.5f);
            CameraBehavior.Instance.ChangeFov(25, 0.5f);
            CameraBehavior.Instance.Invoke("RestoreTimeScaleAndFov", 0.7f);
        }
        else if (montageData.isFatalAttack)
        {
            CameraBehavior.Instance.ChangeTimeScale(0.25f, 0.7f);
            CameraBehavior.Instance.ChangeFov(25, 0.65f);
            CameraBehavior.Instance.Invoke("RestoreTimeScaleAndFov", 0.8f);
        }
    }
}