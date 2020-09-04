using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Toolbox
{
	[RequireComponent(typeof(Image))]
	public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
	{
		[HideInInspector] public Image background = null;
		
		[SerializeField] private TabGroup group = null;
		[SerializeField] private GameObject panel = null;

		[SerializeField] private UnityEvent onTabSelected = null;
		[SerializeField] private UnityEvent onTabDeselected = null;
		
		private void Awake()
		{
			background = GetComponent<Image>();
		}

		private void Start()
		{
			group.Subscribe(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			group.OnTabEnter(this);
		}
		
		public void OnPointerClick(PointerEventData eventData)
		{
			group.OnTabSelected(this);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			group.OnTabExit(this);
		}

		public void SetPanelActive(bool active)
		{
			panel.SetActive(active);
		}

		public void Select()
		{
			onTabSelected?.Invoke();
		}

		public void Deselect()
		{
			onTabDeselected?.Invoke();
		}
	}
}
