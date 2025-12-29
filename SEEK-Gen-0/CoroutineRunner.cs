using System.Collections;
using UnityEngine;

namespace LOOPLanguage
{
    /// <summary>
    /// Provides safe coroutine execution with stop capability.
    /// Prevents coroutine leaks and allows graceful shutdown.
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
        private Coroutine currentCoroutine;
        
        /// <summary>
        /// Starts a new coroutine, stopping any existing one.
        /// </summary>
        public void RunScript(IEnumerator routine)
        {
            StopCurrentScript();
            currentCoroutine = StartCoroutine(routine);
        }
        
        /// <summary>
        /// Stops the currently running script.
        /// </summary>
        public void StopCurrentScript()
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
        }
        
        /// <summary>
        /// Checks if a script is currently running.
        /// </summary>
        public bool IsRunning()
        {
            return currentCoroutine != null;
        }
        
        void OnDestroy()
        {
            StopCurrentScript();
        }
    }
}