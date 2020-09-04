using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
	public class TabGroup : MonoBehaviour
	{
		[Header("State sprites")]
		[SerializeField] private Sprite idleTabSprite = null;
		[SerializeField] private Sprite hoverTabSprite = null;
		[SerializeField] private Sprite activeTabSprite = null;

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
				button.background.sprite = hoverTabSprite;
			}
		}

		public void OnTabSelected(TabButton button)
		{
			selected?.Deselect();
			selected = button;
			selected?.Select();

			ResetTabs();

			button.background.sprite = activeTabSprite;

			foreach (TabButton currentButton in buttons)
			{
				currentButton.SetPanelActive(false);
			}

			button.SetPanelActive(true);
		}

		public void OnTabExit(TabButton button)
		{
			ResetTabs();
		}

		public void ResetTabs()
		{
			foreach (TabButton button in buttons)
			{
				if (!button.Equals(selected))
				{
					button.background.sprite = idleTabSprite;
				}
			}
		}
	}
}
