using UnityEngine;
using UnityEngine.UI;
using System.Text;

namespace LOOPLanguage
{
    /// <summary>
    /// Manages the in-game console for displaying print() output.
    /// </summary>
    public class ConsoleManager : MonoBehaviour
    {
        public static ConsoleManager Instance { get; private set; }
        
        [Header("UI References")]
        public Text consoleText;
        public ScrollRect scrollRect;
        
        [Header("Settings")]
        public int maxLines = 100;
        
        private StringBuilder buffer;
        private int lineCount;
        
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            buffer = new StringBuilder();
            lineCount = 0;
            
            Clear();
        }
        
        /// <summary>
        /// Writes a line to the console.
        /// </summary>
        public void WriteLine(string text)
        {
            buffer.AppendLine(text);
            lineCount++;
            
            // Trim old lines if exceeding max
            if (lineCount > maxLines)
            {
                string[] lines = buffer.ToString().Split('\n');
                buffer.Clear();
                
                int startIndex = lines.Length - maxLines;
                for (int i = startIndex; i < lines.Length; i++)
                {
                    if (i < lines.Length - 1) // Skip last empty line
                    {
                        buffer.AppendLine(lines[i]);
                    }
                }
                
                lineCount = maxLines;
            }
            
            UpdateDisplay();
        }
        
        /// <summary>
        /// Clears the console.
        /// </summary>
        public void Clear()
        {
            buffer.Clear();
            lineCount = 0;
            UpdateDisplay();
        }
        
        private void UpdateDisplay()
        {
            if (consoleText != null)
            {
                consoleText.text = buffer.ToString();
                
                // Scroll to bottom
                if (scrollRect != null)
                {
                    Canvas.ForceUpdateCanvases();
                    scrollRect.verticalNormalizedPosition = 0f;
                }
            }
        }
    }
}