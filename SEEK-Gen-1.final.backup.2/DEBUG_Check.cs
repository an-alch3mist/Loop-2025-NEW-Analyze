using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SPACE_UTIL;
using LoopLanguage;

namespace SPACE_GAME
{
	public class DEBUG_Check: MonoBehaviour
	{
		private void Start()
		{
			this._runBtn.onClick.AddListener(() =>
			{
				string script = this._inputField.text;
				Debug.Log($"loaded script from inputField:\n {script}");
				_runner.Run(script);
			});
		}

		private void Update()
		{
			if(INPUT.K.HeldDown(KeyCode.LeftAlt) && INPUT.M.InstantDown(0))
			{
				StopAllCoroutines();
				StartCoroutine(STIMULATE());
			}
		}
		//
		IEnumerator STIMULATE()
		{
			#region frameRate
			yield return null;
			#endregion

			// this.checkLOG();
			yield return this.checkInterpreter();
			yield return null;
		}
		//
		[SerializeField] CoroutineRunner _runner;
		IEnumerator checkInterpreter()
		{
			// Load the script
			string script = LOG.LoadGameData(GameDataType.sampleScript);

			// Debug what was actually loaded
			Debug.Log(script);

			// Just use the runner's built-in method!
			_runner.Run(script);

			// Wait for completion
			yield return new WaitForSeconds(0.1f);
		}
		[SerializeField] TMP_InputField _inputField;
		[SerializeField] Button _runBtn;
		IEnumerator checkInterpreter__prev()
		{
			#region string script
			string script = @"
# Test: Large loop (tests instruction budget / time slicing)
sum = 0
for i in range(10000):
	if (i % 1000) == 0:
		print(i)
		sleep(1.0)
    sum += 1
print(sum)  # Expected: 1000
";
			// script = LOG.LoadGameData(GameDataType.sampleScript);
			Debug.Log(script);
			#endregion
			this._runner.Run(script);
			yield return null;
		}

		//
		void checkLOG()
		{
			LOG.AddLog("somthng", "txt");
		}
	}
}