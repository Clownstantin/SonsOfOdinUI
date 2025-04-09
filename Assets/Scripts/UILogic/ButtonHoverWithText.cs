using TMPro;
using UnityEngine;

namespace Core.UI
{
	public class ButtonHoverWithText : ButtonHover
	{
		[SerializeField] private TextMeshProUGUI _text;

		protected override void OnEnter()
		{
			base.OnEnter();
			_text.color = hoverColor;
		}

		protected override void OnExit()
		{
			base.OnExit();
			_text.color = baseColor;
		}
	}
}
