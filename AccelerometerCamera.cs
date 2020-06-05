using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using AHRS;

public class AccelerometerCamera : MonoBehaviour
{
	private Vector3 flipingVector = Vector3.one;
	private Vector3 offsetVector = new Vector3(180f, 90f, 90f);

	private Quaternion flippingQuat = Quaternion.identity;
	private Quaternion offsetQuat = Quaternion.identity;

	public float rotationSpeed = 10.0f;
	static AHRS.MadgwickAHRS AHRS = new AHRS.MadgwickAHRS(1f / 20f, 0.975f);

	public bool isActive = false;


	void Update()
	{
		if (isActive == false){
			return;
		}

		Vector3 a = Input.acceleration;
		Vector3 g = Input.gyro.rotationRateUnbiased;

		AHRS.Update(g.x, g.y, g.z, a.x, a.y, a.z);
		Quaternion quat = new Quaternion(AHRS.Quaternion[2], -AHRS.Quaternion[1], AHRS.Quaternion[0], AHRS.Quaternion[3]);

		quat.eulerAngles = new Vector3(quat.eulerAngles.x * flipingVector.x, quat.eulerAngles.y * flipingVector.y, quat.eulerAngles.z * flipingVector.z );
		quat *= Quaternion.Euler(offsetVector);

		Quaternion inverseQuat = Quaternion.Inverse(quat);
		transform.rotation = Quaternion.Slerp(transform.rotation, inverseQuat , rotationSpeed*Time.deltaTime);
	}

	static float deg2rad(float degrees)
	{
		return (float)(Math.PI / 180.0f) * degrees;
	}

	static float rad2deg(float radians)
	{
		return (float)(1.0f / Math.PI) * radians;
	}

	private float AccelerometerUpdateInterval = 1.0f / 100.0f;
	private float LowPassKernelWidthInSeconds = 0.001f;
	private Vector3 lowPassValue = Vector3.zero;
	Vector3 lowpass()
	{
		float LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
		lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
		return lowPassValue;
	}
}
