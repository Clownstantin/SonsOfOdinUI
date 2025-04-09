using UnityEngine;
using UnityEngine.UI;

namespace Core
{
	public class ExitAppManager : MonoBehaviour
	{
		[SerializeField] private Button _exitButton;

		private void OnEnable() => _exitButton.onClick.AddListener(OnExitButtonClicked);

		private void OnDisable() => _exitButton.onClick.RemoveListener(OnExitButtonClicked);

		private void OnExitButtonClicked() => Application.Quit();
	}
}
