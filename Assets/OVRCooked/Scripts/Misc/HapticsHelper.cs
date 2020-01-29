using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticsHelper
{

    private static readonly float[] durations = { 0.05f, 0.1f, 0.25f, 0.4f, 0.55f };
    private static readonly float[] amplitudes = { 0.2f, 0.4f, 0.6f, 0.8f, 1f };
    private static readonly float[] frequencies = { 0.2f, 0.4f, 0.6f, 0.8f, 1f };

    public static void Vibration(MonoBehaviour coroutineOwner, OVRInput.Controller controller, float duration, float frequency, float amplitude)
    {
        coroutineOwner.StartCoroutine(
            VibrationCoroutine(
                controller, 
                duration, 
                frequency, 
                amplitude
            )
        );
    }

    public static void Vibration(MonoBehaviour coroutineOwner, OVRInput.Controller controller, Duration duration, Frequency frequency, Amplitude amplitude)
    {
        coroutineOwner.StartCoroutine(
            VibrationCoroutine(
                controller, 
                durations[(int)duration], 
                frequencies[(int)frequency], 
                amplitudes[(int)amplitude]
            )
        );
    }

    private static IEnumerator VibrationCoroutine(OVRInput.Controller controller, float duration, float frequency, float amplitude)
    {
        Debug.LogFormat("Haptics: {0}", controller.ToString());

        OVRInput.SetControllerVibration(frequency, amplitude, controller);

        yield return new WaitForSeconds(duration);

        OVRInput.SetControllerVibration(0f, 0f);
    }

    public enum Duration
    {
        VeryShort, Short, Medium, Long, VeryLong
    }

    public enum Frequency 
    {
        VeryLow, Low, Medium, High, VeryHigh
    }

    public enum Amplitude
    {
        VeryLow, Low, Medium, High, VeryHigh
    }
}
