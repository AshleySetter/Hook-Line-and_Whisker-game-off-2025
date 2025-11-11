

using System;
using System.Collections;
using UnityEngine;

public static class DoAfterDelayUtility
{
    public static IEnumerator DoAfterDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke(); // Safely invoke the function if it's not null
    }
}