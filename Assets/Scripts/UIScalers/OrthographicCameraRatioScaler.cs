using UnityEngine;

namespace Kartsan.Core
{
	public class OrthographicCameraRatioScaler : RatioScaler
	{
		[SerializeField] private OrthographicScaler _scalers = default;

		protected override void SetActualRatioScale(in float match)
		=> _scalers.SetMatch(match);
	}
}
