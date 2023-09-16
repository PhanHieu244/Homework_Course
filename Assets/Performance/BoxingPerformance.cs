using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BoxingPerformance : MonoBehaviour
{
    public int testAmount = 10000000;

    private void Start()
    {
        var stopwatch = new Stopwatch();
        
        //Test Performance NoBoxing
        stopwatch.Start();
        NoBoxing();
        stopwatch.Stop();
        Debug.Log($"No Boxing: {stopwatch.ElapsedMilliseconds} ms");
        
        //Test Performance Boxing
        stopwatch.Reset();
        stopwatch.Start();
        Boxing();
        stopwatch.Stop();
        Debug.Log($"With Boxing: {stopwatch.ElapsedMilliseconds} ms");
        
        //Test Performance Boxing and Unboxing
        stopwatch.Reset();
        stopwatch.Start();
        BoxingAndUnboxing();
        stopwatch.Stop();
        Debug.Log($"With Boxing and Unboxing: {stopwatch.ElapsedMilliseconds} ms");
    }
    
    private void NoBoxing()
    {
        int valueType;
        for (int value = 0; value < testAmount; value++)
        {
            valueType = value;
        }
    }

    private void Boxing()
    {
        object referenceType;
        for (int value = 0; value < testAmount; value++)
        {
            //boxing
            referenceType = value;
        }
    }

    private void BoxingAndUnboxing()
    {
        object referenceType;
        int unbox;
        for (int value = 0; value < testAmount; value++)
        {
            //boxing
            referenceType = value;
            //unboxing
            unbox = (int) referenceType;
        } 
    }
}
