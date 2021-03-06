﻿using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {	
	// Cдвиг при вращении
	public int RotationOffset = 0;	
	// Угол, на который поворачиваем руку
	private float rotZ = 0;

	void Update () 
	{	
		// Вектор м/у позицией курсора и рукой
		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		difference.Normalize ();
		// Считаем на сколько повернуть
		rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;

		if (isValidAngle()) 
			transform.rotation = Quaternion.Euler (new Vector3(0f, 0f, rotZ + RotationOffset));
	}
	// Возвращает true, если руку можно повернуть на данный угол
	public bool isValidAngle()
	{
		/*
		float playerDirection = GameObject.Find ("Graphics").transform.localScale.x;

		if (playerDirection > 0) // Вправо
		{ 
			if (rotZ > -70 && rotZ < 70) 
				return true;
		}
		else // Влево
		{
			if ((rotZ > 100 && rotZ < 180) || (rotZ > -180 && rotZ < -130 ))
				return true;
		}
		return false;
		*/
		return true;
	}
}

