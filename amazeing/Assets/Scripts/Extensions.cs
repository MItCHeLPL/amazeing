using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ExtendedMathf
{
	public static float Map(float value, float oldMinValue, float oldMaxValue, float newMinValue, float newMaxValue)
	{
		return newMinValue + (value - oldMinValue) * (newMaxValue - newMinValue) / (oldMaxValue - oldMinValue); //Map value from old range onto a new range
	}
	public static int Map(int value, int oldMinValue, int oldMaxValue, int newMinValue, int newMaxValue)
	{
		return newMinValue + (value - oldMinValue) * (newMaxValue - newMinValue) / (oldMaxValue - oldMinValue); //Map value from old range onto a new range
	}

	public static float MapFrom01(float value, float newMinValue, float newMaxValue)
	{
		return newMinValue + (value - 0) * (newMaxValue - newMinValue) / (1 - 0); //Map value from old range onto a new range
	}
	public static float MapTo01(float value, float oldMinValue, float oldMaxValue)
	{
		return 0 + (value - oldMinValue) * (1 - 0) / (oldMaxValue - oldMinValue); //Map value from old range onto a new range
	}
}