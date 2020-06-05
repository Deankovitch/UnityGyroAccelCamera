using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCamera : MonoBehaviour
{
	private float m_initialYAngle = 0f;
	private float m_appliedGyroYAngle = 0f;
	private float m_calibrationYAngle = 0f;
	[SerializeField] private Transform m_rawGyroRotation;
	private float m_tempSmoothing;

	[SerializeField] private Transform m_swipeObjectContainer;
    public Transform SwipeObjectContainer { get { return m_swipeObjectContainer; } }

	[SerializeField] private float m_smoothing = 0.1f;

	public bool isActive = false;

	private IEnumerator Start()
	{
		m_initialYAngle = transform.eulerAngles.y;
        m_rawGyroRotation.position = transform.position;
        m_rawGyroRotation.rotation = transform.rotation;

        yield return new WaitForSeconds(1);
		StartCoroutine(CalibrateYAngle());
	}

	private void Update()
	{
		if (isActive == false)
		{
			return;
		}
		ApplyGyroRotation();
		ApplyCalibration();
		transform.rotation = Quaternion.Slerp(transform.rotation, m_rawGyroRotation.rotation, m_smoothing);
	}

	private IEnumerator CalibrateYAngle()
	{
		m_tempSmoothing = m_smoothing;
		m_smoothing = 1;
		m_calibrationYAngle = m_appliedGyroYAngle - m_initialYAngle; 
		yield return null;
		m_smoothing = m_tempSmoothing;
	}

	private void ApplyGyroRotation()
	{
		m_rawGyroRotation.rotation = Input.gyro.attitude;
        m_rawGyroRotation.Rotate(0f, 0f, 180f, Space.Self);
        m_rawGyroRotation.Rotate(90f, 180f, 0f, Space.World);
        m_appliedGyroYAngle = m_rawGyroRotation.eulerAngles.y;
	}

	private void ApplyCalibration()
	{
		m_rawGyroRotation.Rotate(0f, -m_calibrationYAngle, 0f, Space.World); 
	}

	public void SetEnabled(bool value)
	{
		enabled = true;
		StartCoroutine(CalibrateYAngle());
	}
}
