using Phenix.Unity.Camera;

public class HandleCamera
{
    public static void SlowMotion(bool isCritical, bool isFatal)
    {
        if (isCritical)
        {
            //CameraBehavior.Instance.ChangeTimeScale(0.25f, 0.5f);
            //CameraBehavior.Instance.ChangeFov(25, 0.5f);
            //CameraBehavior.Instance.Invoke("RestoreTimeScaleAndFov", 0.7f);
            CameraEffect.Instance.SlowMotion(UnityEngine.Camera.main, 0.25f, 25, 0.5f, 0.2f, 0.4f);
        }
        else if (isFatal)
        {
            //CameraBehavior.Instance.ChangeTimeScale(0.25f, 0.7f);
            //CameraBehavior.Instance.ChangeFov(25, 0.65f);
            //CameraBehavior.Instance.Invoke("RestoreTimeScaleAndFov", 0.8f);
            CameraEffect.Instance.SlowMotion(UnityEngine.Camera.main, 0.25f, 25, 0.7f, 0.1f, 0.4f);
        }
    }
}