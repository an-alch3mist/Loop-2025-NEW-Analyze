using System;
using System.Collections;
using UnityEngine;

namespace LoopLanguage
{
    /// <summary>
    /// Safely executes interpreter coroutines with error handling.
    /// Catches exceptions and displays them to console.
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
        #region Fields
        private PythonInterpreter interpreter;
        private GameBuiltinMethods gameBuiltins;
        private ConsoleManager console;
        private Coroutine currentExecution;

		[Header("Console Manager")]
		[SerializeField] ConsoleManager _consoleManager;
		[Header("Error Display")]
		[SerializeField] private bool showErrorsInUnityConsole = true;  // Toggle in Inspector
		#endregion

		#region Unity Lifecycle

		// Replace the Awake() method
		private void Awake()
		{
			gameBuiltins = new GameBuiltinMethods();

			console = (this._consoleManager == null) ? GetComponent<ConsoleManager>() : this._consoleManager;
			if (console == null)
			{
				Debug.LogError("ConsoleManager not found! Add ConsoleManager component.");
			}

			// Pass console to interpreter so print() works
			interpreter = new PythonInterpreter(gameBuiltins, console);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Runs Python source code as a coroutine
		/// </summary>
		public void Run(string sourceCode)
        {
            // Stop any existing execution
            if (currentExecution != null)
            {
                StopCoroutine(currentExecution);
            }
            
            // Reset interpreter state
            interpreter.Reset();
            console.Clear();
            
            // Start new execution
            currentExecution = StartCoroutine(ExecuteCode(sourceCode));
        }
        
        /// <summary>
        /// Stops current execution
        /// </summary>
        public void Stop()
        {
            if (currentExecution != null)
            {
                StopCoroutine(currentExecution);
                currentExecution = null;
                console.WriteLine("<color=#cc8800> [Execution stopped]</color>");
            }
        }

		#endregion

		#region Coroutine Execution
		private IEnumerator ExecuteCode(string sourceCode)
		{
			// .NET 2.0 Compliance: Cannot yield inside try-catch
			// Solution: Get execution routine outside try-catch, then yield
			IEnumerator execution = null;
			bool hasError = false;
			string errorType = "";
			string errorMessage = "";

			try
			{
				// Lexical analysis
				Lexer lexer = new Lexer();
				var tokens = lexer.Tokenize(sourceCode);

				// Parsing
				Parser parser = new Parser();
				var ast = parser.Parse(tokens);

				// Get execution routine (don't start yet)
				execution = interpreter.Execute(ast);
			}
			catch (LexerError e)
			{
				hasError = true;
				errorType = "LEXER ERROR";
				errorMessage = e.Message;
			}
			catch (ParserError e)
			{
				hasError = true;
				errorType = "PARSER ERROR";
				errorMessage = e.Message;
			}
			catch (RuntimeError e)
			{
				hasError = true;
				errorType = "RUNTIME ERROR";
				errorMessage = e.Message;
			}
			catch (Exception e)
			{
				hasError = true;
				errorType = "UNEXPECTED ERROR";
				errorMessage = $"{e.Message}\n{e.StackTrace}";
			}

			// Handle errors before execution
			if (hasError)
			{
				string fullError = $"[{errorType}] {errorMessage}";

				// ★ CHANGED: Log to BOTH Unity console and ConsoleManager
				Debug.LogError($"{errorType}: {errorMessage}");

				if (console != null)
					console.WriteLine(fullError, isError: true);

				currentExecution = null;
				yield break;
			}

			// Execute outside try-catch (safe to yield here)
			if (execution != null)
			{
				bool executionError = false;
				string executionErrorType = "";
				string executionErrorMessage = "";

				while (true)
				{
					bool hasMore = false;

					try
					{
						hasMore = execution.MoveNext();
					}
					catch (RuntimeError e)
					{
						executionError = true;
						executionErrorType = "RUNTIME ERROR";
						executionErrorMessage = e.Message;
						break;
					}
					catch (BreakException)
					{
						executionError = true;
						executionErrorType = "CONTROL FLOW ERROR";
						executionErrorMessage = "break statement used outside loop";
						break;
					}
					catch (ContinueException)
					{
						executionError = true;
						executionErrorType = "CONTROL FLOW ERROR";
						executionErrorMessage = "continue statement used outside loop";
						break;
					}
					catch (Exception e)
					{
						executionError = true;
						executionErrorType = "UNEXPECTED ERROR";
						executionErrorMessage = $"{e.Message}\n{e.StackTrace}";
						break;
					}

					if (!hasMore) break;

					// Check if we should yield for frame budget
					if (interpreter.ShouldYield())
					{
						yield return null;
					}

					// Yield any game commands
					if (execution.Current != null)
					{
						yield return execution.Current;
					}
				}

				// ★ CHANGED: Display execution errors to BOTH Unity console and ConsoleManager
				if (executionError)
				{
					string fullError = $"[{executionErrorType}] {executionErrorMessage}";

					Debug.LogError($"{executionErrorType}: {executionErrorMessage}");

					if (console != null)
						console.WriteLine(fullError, isError: true);
				}
				else
				{
					if (console != null)
						console.WriteLine("<color=#88cc00>[Execution complete]</color>");
				}
			}

			currentExecution = null;
		}
		#endregion
	}
}
