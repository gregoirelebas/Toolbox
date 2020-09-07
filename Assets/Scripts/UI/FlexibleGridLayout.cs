using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toolbox
{
	public class FlexibleGridLayout : LayoutGroup
	{
		[SerializeField] private Vector2 spacing = Vector2.zero;
		
		private int rows = 1;
		private int columns = 1;
		private Vector2 cellSize = Vector2.one;

		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();

			float sqrt = Mathf.Sqrt(transform.childCount);
			rows = Mathf.CeilToInt(sqrt);
			columns = Mathf.CeilToInt(sqrt);

			float parentWidth = rectTransform.rect.width;
			float parentHeight = rectTransform.rect.height;

			cellSize.x = (parentWidth / columns) - (spacing.x / columns * 2.0f) - (padding.left / columns) - (padding.right / columns);
			cellSize.y = (parentHeight / rows) - (spacing.y / rows * 2.0f) - (padding.top / rows) - (padding.bottom / rows); ;

			int rowCount;
			int columnCount;

			for (int i = 0; i < rectChildren.Count; i++)
			{
				rowCount = i / columns;
				columnCount = i % columns;

				RectTransform item = rectChildren[i];
				float xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
				float yPos = cellSize.y * rowCount + (spacing.y * rowCount) + padding.top;

				SetChildAlongAxis(item, 0, xPos, cellSize.x);
				SetChildAlongAxis(item, 1, yPos, cellSize.y);
			}
		}

		public override void CalculateLayoutInputVertical()
		{
			
		}

		public override void SetLayoutHorizontal()
		{
			
		}

		public override void SetLayoutVertical()
		{
			
		}
	}
}
