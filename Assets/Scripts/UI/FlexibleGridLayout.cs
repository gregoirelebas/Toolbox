using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toolbox
{
	public class FlexibleGridLayout : LayoutGroup
	{
		[SerializeField] private int rows = 1;
		[SerializeField] private int cols = 1;
		[SerializeField] private Vector2 cellSize = Vector2.one;

		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();

			float sqrt = Mathf.Sqrt(transform.childCount);
			rows = Mathf.CeilToInt(sqrt);
			cols = Mathf.CeilToInt(sqrt);

			float parentWidth = rectTransform.rect.width;
			float parentHeight = rectTransform.rect.height;

			cellSize.x = parentWidth / rows;
			cellSize.y = parentHeight / cols;

			int rowCount;
			int colCount;

			for (int i = 0; i < rectChildren.Count; i++)
			{
				rowCount = i / cols;
				colCount = i % cols;

				RectTransform item = rectChildren[i];
				float xPos = cellSize.x * colCount;
				float yPos = cellSize.y * rowCount;

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
