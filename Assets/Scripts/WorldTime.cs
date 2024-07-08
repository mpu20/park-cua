using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public static int MinuteInDay = 1440;
    public event EventHandler<TimeSpan> WorldTimeChanged;

    [SerializeField]
    private float _dayLength; // in seconds
    private TimeSpan _currentTime;
    private float _minuteLength => _dayLength / MinuteInDay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AddMinute());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator AddMinute()
    {
        _currentTime += TimeSpan.FromMinutes(1);
        WorldTimeChanged?.Invoke(this, _currentTime);
        yield return new WaitForSeconds(_minuteLength);
        StartCoroutine(AddMinute());
    }
}
