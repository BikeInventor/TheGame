using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {	
	// Cдвиг при вращении
	public int RotationOffset = 0;	
	// Угол, на который поворачиваем руку
	private float rotZ = 0;
	// Update is called once per frame
	void Update () 
	{	
		// Вектор м/у позицией курсора и рукой
		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		difference.Normalize ();
		// Считаем на сколько повернуть
		rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		// Поворачиваем руку, если угол допустим при данном направлении игрока


		if (isValidAngle()) 
			transform.rotation = Quaternion.Euler (new Vector3(0f, 0f, rotZ + RotationOffset));


	}
	// Возвращает true, если руку можно повернуть на данный угол и false, если нельзя
	public bool isValidAngle()
	{

		// Направление игрока. Положительное - смотрит вправо, отрицательное - влево.
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
	}
}

