using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabElement : MonoBehaviour
{
	[Header("State colors")]
    [SerializeField] private Color idleColor = Color.white;
    [SerializeField] private Color hoverColor = Color.grey;
    [SerializeField] private Color activeColor = Color.black;

	private Graphic graphic = null;

	protected virtual void Awake()
	{
		graphic = GetComponent<Graphic>();

		SetIdle();
	}

	public void SetIdle()
	{
		graphic.color = idleColor;
	}

    public void SetHover()
	{
		graphic.color = hoverColor;
	}

    public void SetActive()

    {
		graphic.color = activeColor;
	}
}
