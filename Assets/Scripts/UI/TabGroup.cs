using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
	public class TabGroup : MonoBehaviour
	{
		private List<TabButton> buttons = null;
		private TabButton selected = null;

		public void Subscribe(TabButton button)
		{
			if (buttons == null)
			{
				buttons = new List<TabButton>();
			}

			buttons.Add(button);
		}

		public void OnPointerEnter(TabButton button)
		{
			if (!button.Equals(selected))
			{
				button.Hover();
			}
		}

		public void OnPointerClick(TabButton button)
		{
			selected?.Deselect();
			selected = button;
			selected.Select();
		}

		public void OnPointerExit(TabButton button)
		{
			if (!button.Equals(selected))
			{
				button.Deselect();
			}
		}

		public void ResetTabs()
		{
			foreach (TabButton button in buttons)
			{
				if (!button.Equals(selected))
				{
					button.Deselect();
				}
			}
		}
	}
}
