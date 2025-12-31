prompt:
	Claude 4.5 Sonnet(+thinking): https://claude.ai/share/cd1cbe10-9e21-47ad-9089-a474ed688757
	generation of `*.cs`: https://claude.ai/chat/413168f2-2f18-4ba0-b577-5544b7574f55


# To Answer:
	
	- does nested function call as arguement works ? // eg: _A(_A(1) + _A(_A(2) + _A(4)))
	- does nested loops works ? 
	- does recursion works and it has limits ?
	include complex test suite along and improvise addition to existing prompt where required.


### gen(latest: 2025-dec-27 13:18): https://claude.ai/share/7910bd45-cfb9-4036-8891-779470367535
### workflow prompt(gemini 3.5 pro): https://gemini.google.com/app/e042bc0371301487  ` via (seek.gpt.deep.research.0@gmail.com)`
### gen(latest: 2025-dec-27 14:18): https://claude.ai/share/d7420899-e500-4996-b8e3-30627a3b0bf1
### gen(latest: 2025-dec-27 15:08): https://claude.ai/share/124f4940-4c34-46f1-90ff-a622bc819fff
### gen(latest: 2025-dec-28 ~end o clock): https://claude.ai/share/124f4940-4c34-46f1-90ff-a622bc819fff (via seek.gpt.deep.research.5)



## required clarification or modification?
### error
	try-catch cannot inside the clause of IEnumerator in .Net 2.0(limiration) in unity 2020.3+
	Assets\Scripts\LOOP-2025 [NEW]\TestRunner.cs(147,25): error CS1626: Cannot yield a value in the body of a try block with a catch clause
	Assets\Scripts\LOOP-2025 [NEW]\TestRunner.cs(152,25): error CS1626: Cannot yield a value in the body of a try block with a catch clause
	Assets\Scripts\LOOP-2025 [NEW]\PythonInterpreter.cs(143,29): error CS1626: Cannot yield a value in the body of a try block with a catch clause
	Assets\Scripts\LOOP-2025 [NEW]\PythonInterpreter.cs(154,29): error CS1626: Cannot yield a value in the body of a try block with a catch clause
	Assets\Scripts\LOOP-2025 [NEW]\PythonInterpreter.cs(165,29): error CS1626: Cannot yield a value in the body of a try block with a catch clause
	Assets\Scripts\LOOP-2025 [NEW]\PythonInterpreter.cs(176,29): error CS1626: Cannot yield a value in the body of a try block with a catch clause
	Assets\Scripts\LOOP-2025 [NEW]\PythonInterpreter.cs(185,25): error CS1626: Cannot yield a value in the body of a try block with a catch clause
	Assets\Scripts\LOOP-2025 [NEW]\PythonInterpreter.cs(193,25): error CS1626: Cannot yield a value in the body of a try block with a catch clause
	Assets\Scripts\LOOP-2025 [NEW]\PythonInterpreter.cs(201,25): error CS1626: Cannot yield a value in the body of a try block with a catch clause
	Assets\Scripts\LOOP-2025 [NEW]\CoroutineRunner.cs(96,25): error CS1626: Cannot yield a value in the body of a try block with a catch clause
	Assets\Scripts\LOOP-2025 [NEW]\CoroutineRunner.cs(102,25): error CS1626: Cannot yield a value in the body of a try block with a catch clause
### also quick question:
		does the number handling handles not only the equals 1 == 1.0 but also the comparision ? such as 
		1 > 1.01 or 2.01 <= 3 or 2.001 < 4.1 and many more ? which also include complex conditions such as 
		1 == 1.0 and 2 <= 3 or not (2 > 3/2) similar to traditional python-like, btw what does int/int in python i guess still a float since its not 3 // 2.

	could you provide QUICK REFERENCE GUIDE now for all files and dependencies in `*.md`?



## after fix with the Check(test) Suite:
### gen(latest: 2025-dec-29 18:16): https://claude.ai/share/2bdfcf07-335a-4b91-a4a0-77d7d0187641 (via seek.gpt.deep.research.1)
### gen(latest: 2025-dec-29 20:11): https://claude.ai/share/04696a00-d0a1-400a-ad6e-f41c4436e326 (via seek.gpt.deep.research.1)

### required feature:
	- line number event based so what this event could be subscibed from somewhere else to know which line number is currently running,
		for example(just for an example): in future certail arra shall be placed on left of that certain line number any many more

	Limitations:
		Key parameter only accepts lambda functions, not user-defined functions (can add later if needed)
		i,e No .sort(key=, reverse=) kwargs yet - only positional: .sort(lambda_func, reverse_bool)
### gen(latest: 2025-dec-30 11:50): https://claude.ai/share/556ca343-0a19-406c-a8bd-8cb158101fcf (via seek.gpt.deep.research.5)

### gen(latest: 2025-dec-30 22:12): https://claude.ai/share/78f739c5-cd97-4956-85e3-624fdf122596 (via seek.gpt.deep.research.1)

```py
# do not use sleep inside factorial
# ienumerator/sleep() cannot be called inside ienumerator
def factorial(n):
	move("up")
    if n <= 1:
        return 1
    return n * factorial(n - 1)

print(factorial(5))  # Expected: 120
print(factorial(6))  # Expected: 720
```

even this
```py
# do not use sleep inside factorial
# ienumerator/sleep() cannot be called inside ienumerator
def factorial(n):
	for i in range(10):
		move("up")
	return 10

print(factorial(5))  # Expected: 120
print(factorial(6))  # Expected: 720
```

```
LoopLanguage.PythonInterpreter+<ExecuteFunctionBodyAsync>d__41
LoopLanguage.PythonInterpreter+<ExecuteFunctionBodyAsync>d__41
<color=#88cc00>[Execution complete]</color>
```

recursion depth limit fixed
### gen(latest: 2025-dec-31 end): https://claude.ai/share/2e430c92-7b92-40e7-853f-643b3cc48144 (via seek.gpt.deep.research.1)