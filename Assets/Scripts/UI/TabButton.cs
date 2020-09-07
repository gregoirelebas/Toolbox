using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Toolbox
{
	[RequireComponent(typeof(Image))]
	public class TabButton : TabElement, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
	{		
		[Header("External references")]
		[SerializeField] private TabGroup group = null;
		[SerializeField] private GameObject panel = null;

		private List<TabElement> elements = new List<TabElement>();

		protected override void Awake()
		{
			base.Awake();

			foreach (TabElement tabElement in GetComponentsInChildren<TabElement>())
			{
				elements.Add(tabElement);
			}
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

		public void Select()
		{
			foreach (TabElement element in elements)
			{
				element.SetActive();
			}

			panel.SetActive(true);
		}

		public void Hover()
		{
			foreach (TabElement element in elements)
			{
				element.SetHover();
			}
		}

		public void Deselect()
		{
			foreach (TabElement element in elements)
			{
				element.SetIdle();
			}

			panel.SetActive(false);
		}
	}
}
