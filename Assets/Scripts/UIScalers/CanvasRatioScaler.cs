using UnityEngine;
using UnityEngine.UI;

namespace Core
{
	public class CanvasRatioScaler : RatioScaler
	{
		[SerializeField] private CanvasScaler[] _scalers = default;

		protected override void SetActualRatioScale(in float match)
		{
			foreach(CanvasScaler scaler in _scalers)
				if(scaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
					scaler.matchWidthOrHeight = match;
		}
	}
}
