using UnityEngine;

namespace Core
{
#if UNITY_EDITOR
	[ExecuteInEditMode]
#endif
	public abstract class RatioScaler : MonoBehaviour
	{
		private const float WIDE_SCREEN_RATIO = 1.7f;
#if UNITY_EDITOR
		[SerializeField] private bool _executeInEditor = true;
#endif
		[SerializeField, Range(0, 1)] private float _tabletMatch = 1f;
		[SerializeField, Range(0, 1)] private float _wideMatch = 0f;
		[SerializeField] private bool _portrait = true;

#if UNITY_EDITOR
		private float _ratio = 0f;

		private void Update()
		{
			if(_executeInEditor)
				Scale();
		}
#endif

		private void Awake() => Scale();

		private void Scale()
		{
			float width = Screen.width;
			float height = Screen.height;
			float ratio = _portrait
						? height / width
						: width / height;
#if UNITY_EDITOR
			if(ratio == _ratio)
				return;
			_ratio = ratio;
#endif
			SetActualRatioScale(ratio >= WIDE_SCREEN_RATIO ?
								_wideMatch :
								_tabletMatch);
		}

		protected abstract void SetActualRatioScale(in float match);
	}
}
