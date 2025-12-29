using System.Collections;
using UnityEngine;

namespace LoopLanguage
{
    /// <summary>
    /// Automated test runner that executes all test cases from DemoScripts.
    /// Can be triggered via Unity Inspector or script.
    /// Reports results to console.
    /// </summary>
    public class TestRunner : MonoBehaviour
    {
        #region Inspector Fields
        
        [Header("Test Configuration")]
        [SerializeField] private bool runOnStart = false;
        [SerializeField] private float delayBetweenTests = 0.5f;
        
        #endregion
        
        #region Fields
        
        private CoroutineRunner runner;
        private int testsRun = 0;
        private int testsPassed = 0;
        private int testsFailed = 0;
        
        #endregion
        
        #region Unity Lifecycle
        
        private void Start()
        {
            runner = GetComponent<CoroutineRunner>();
            
            if (runner == null)
            {
                Debug.LogError("TestRunner: CoroutineRunner component not found!");
                return;
            }
            
            if (runOnStart)
            {
                StartCoroutine(RunAllTests());
            }
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Runs all test cases from DemoScripts
        /// </summary>
        [ContextMenu("Run All Tests")]
        public void RunAllTestsButton()
        {
            StartCoroutine(RunAllTests());
        }
        
        /// <summary>
        /// Runs a single specific test by index
        /// </summary>
        public void RunTest(int index)
        {
            string[] allTests = DemoScripts.GetAllTests();
            
            if (index >= 0 && index < allTests.Length)
            {
                StartCoroutine(RunSingleTestByIndex(index));
            }
            else
            {
                Debug.LogError($"Invalid test index: {index}. Valid range: 0-{allTests.Length - 1}");
            }
        }
        
        private IEnumerator RunSingleTestByIndex(int index)
        {
            string[] allTests = DemoScripts.GetAllTests();
            yield return RunSingleTest(index, allTests[index]);
        }
        
        #endregion
        
        #region Test Execution
        
        private IEnumerator RunAllTests()
        {
            // Get combined test suite (original + comprehensive)
            string[] allTests = DemoScripts.GetAllTests();
            
            Debug.Log("========================================");
            Debug.Log("STARTING COMPREHENSIVE TEST SUITE");
            Debug.Log($"Total tests: {allTests.Length}");
            Debug.Log("(35 original + 45 extended tests)");
            Debug.Log("========================================");
            
            testsRun = 0;
            testsPassed = 0;
            testsFailed = 0;
            
            for (int i = 0; i < allTests.Length; i++)
            {
                yield return RunSingleTest(i, allTests[i]);
                yield return new WaitForSeconds(delayBetweenTests);
            }
            
            Debug.Log("========================================");
            Debug.Log("TEST SUITE COMPLETE");
            Debug.Log($"Tests Run: {testsRun}");
            Debug.Log($"Passed: {testsPassed}");
            Debug.Log($"Failed: {testsFailed}");
            Debug.Log($"Success Rate: {(testsPassed * 100.0 / testsRun):F1}%");
            Debug.Log("========================================");
        }
        
        private IEnumerator RunSingleTest(int testIndex, string testScript)
        {
            testsRun++;
            
            string testName = ExtractTestName(testScript);
            
            Debug.Log($"\n[TEST {testsRun}] Running: {testName}");
            
            try
            {
                // Create a temporary interpreter for this test
                GameBuiltinMethods gameBuiltins = new GameBuiltinMethods();
                PythonInterpreter interpreter = new PythonInterpreter(gameBuiltins);
                
                // Lexical analysis
                Lexer lexer = new Lexer();
                var tokens = lexer.Tokenize(testScript);
                
                // Parsing
                Parser parser = new Parser();
                var ast = parser.Parse(tokens);
                
                // Execution
                IEnumerator execution = interpreter.Execute(ast);
                
                while (execution.MoveNext())
                {
                    if (interpreter.ShouldYield())
                    {
                        yield return null;
                    }
                    
                    if (execution.Current != null)
                    {
                        yield return execution.Current;
                    }
                }
                
                testsPassed++;
                Debug.Log($"[TEST {testsRun}] ✓ PASSED: {testName}");
            }
            catch (System.Exception e)
            {
                testsFailed++;
                Debug.LogError($"[TEST {testsRun}] ✗ FAILED: {testName}");
                Debug.LogError($"Error: {e.Message}");
            }
        }
        
        private string ExtractTestName(string script)
        {
            // Extract test name from first comment line
            int commentIndex = script.IndexOf("# Test:");
            if (commentIndex == -1)
            {
                return "Unnamed Test";
            }
            
            int endIndex = script.IndexOf('\n', commentIndex);
            if (endIndex == -1)
            {
                endIndex = script.Length;
            }
            
            string commentLine = script.Substring(commentIndex, endIndex - commentIndex);
            return commentLine.Replace("# Test:", "").Trim();
        }
        
        #endregion
    }
}
