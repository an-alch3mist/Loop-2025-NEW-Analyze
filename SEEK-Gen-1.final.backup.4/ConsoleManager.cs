using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LoopLanguage
{
    /// <summary>
    /// Manages in-game console UI for print() output and errors.
    /// Displays all interpreter output to the player.
    /// </summary>
    public class ConsoleManager : MonoBehaviour
    {
        #region Inspector Fields
        
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI consoleText;
		[SerializeField] private Button copyButton;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private int maxLines = 1000;
        
        #endregion
        
        #region Fields
        
        private string consoleContent = "";
        private int lineCount = 0;
        
        #endregion
        
        #region Unity Lifecycle
        
        private void Awake()
        {
            if (consoleText == null)
            {
                Debug.LogError("ConsoleManager: consoleText reference not set!");
            }
            
            Clear();
        }

		private void Start()
		{
			this.copyButton.onClick.AddListener(() =>
			{
				Debug.Log("copied console text to clipBoard");
				GUIUtility.systemCopyBuffer = this.consoleContent;
			});
		}
		#endregion

		#region Public Methods

		/// <summary>
		/// Writes a line to the console with optional color
		/// </summary>
		public void WriteLine(string message, bool isError = false)
		{
			if (isError)
				consoleContent += "<color=red>" + message + "</color>\n";
			else
				consoleContent += message + "\n";

			lineCount++;

			// Trim old lines if exceeding max
			if (lineCount > maxLines)
			{
				int firstNewlineIndex = consoleContent.IndexOf('\n');
				if (firstNewlineIndex != -1)
				{
					consoleContent = consoleContent.Substring(firstNewlineIndex + 1);
					lineCount--;
				}
			}

			UpdateDisplay();
		}

		/// <summary>
		/// Writes text without newline
		/// </summary>
		/// not required for now
		private void Write(string message)
        {
            consoleContent += message;
            UpdateDisplay();
        }
        
        /// <summary>
        /// Clears all console content
        /// </summary>
        public void Clear()
        {
            consoleContent = "";
            lineCount = 0;
            UpdateDisplay();
        }
        
        #endregion
        
        #region Private Methods
        
        private void UpdateDisplay()
        {
            if (consoleText != null)
            {
                consoleText.text = consoleContent;
				this.ScrollToBottom();
            }
        }

		private void ScrollToBottom()
		{
			// Scroll to bottom
			if (scrollRect != null)
			{
				Canvas.ForceUpdateCanvases();
				scrollRect.verticalNormalizedPosition = 0f;
			}
		}
        
        #endregion
    }
}
