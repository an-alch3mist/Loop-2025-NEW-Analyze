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
        
        #endregion
        
        #region Unity Lifecycle
        
        private void Awake()
        {
            gameBuiltins = new GameBuiltinMethods();
            interpreter = new PythonInterpreter(gameBuiltins);
            console = GetComponent<ConsoleManager>();
            
            if (console == null)
            {
                Debug.LogError("ConsoleManager not found! Add ConsoleManager component.");
            }
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
            console?.Clear();
            
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
                console?.WriteLine("[Execution stopped]");
            }
        }
        
        #endregion
        
        #region Coroutine Execution
        
        private IEnumerator ExecuteCode(string sourceCode)
        {
            try
            {
                // Lexical analysis
                Lexer lexer = new Lexer();
                var tokens = lexer.Tokenize(sourceCode);
                
                // Parsing
                Parser parser = new Parser();
                var ast = parser.Parse(tokens);
                
                // Execution
                IEnumerator execution = interpreter.Execute(ast);
                
                while (execution.MoveNext())
                {
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
                
                console?.WriteLine("[Execution complete]");
            }
            catch (LexerError e)
            {
                console?.WriteLine($"[LEXER ERROR] {e.Message}");
                Debug.LogError($"Lexer Error: {e.Message}");
            }
            catch (ParserError e)
            {
                console?.WriteLine($"[PARSER ERROR] {e.Message}");
                Debug.LogError($"Parser Error: {e.Message}");
            }
            catch (RuntimeError e)
            {
                console?.WriteLine($"[RUNTIME ERROR] {e.Message}");
                Debug.LogError($"Runtime Error: {e.Message}");
            }
            catch (Exception e)
            {
                console?.WriteLine($"[UNEXPECTED ERROR] {e.Message}\n{e.StackTrace}");
                Debug.LogError($"Unexpected Error: {e.Message}\n{e.StackTrace}");
            }
            finally
            {
                currentExecution = null;
            }
        }
        
        #endregion
    }
}
