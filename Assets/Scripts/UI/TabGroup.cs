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

		public void OnTabEnter(TabButton button)
		{
			ResetTabs();

			if (!button.Equals(selected))
			{
				button.Hover();
			}
		}

		public void OnTabSelected(TabButton button)
		{
			selected?.Deselect();
			selected = button;
			selected?.Select();
		}

		public void OnTabExit(TabButton button)
		{
			button.Deselect();
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
