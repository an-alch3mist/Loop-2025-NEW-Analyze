tryna build a `farmer was replaced` clone for educational purpose
here's the error log report [pasted] from unity console(log reports) what you think is the issue ? provide the fix and let me know.

whats the file you gotta need to modify to fix that, 
make sure it wont break any the existing features that is successfully working

keep critical note in mind: .Net2.0 limitation cannot use `yield return null/value/anything inside try-catch block which is inside IEnumerator` .
All use try:/except: which requires adding exception handling grammar to your parser
if certain fix it requires to use try:/except use alternative approach inside IEnumerator try-catch clause

alright that works, 
## btw all components test runner, coroutineRunner, ConsoleManager are attached to same GameObject also with TextMeshProGUI linked but i do not see the print in that, print is visible only on debug console, fix that

## also what is your thoughts, do i need to log error in both unity console window and consoleText, or just consoleText so that if any error inside it shall just stop in consoleText ? feel free to provide your opinion after fixing the main mentioned issue.

## btw about the print() can it print anything of complex data types ? (multi dimensional list, dictionary, tupples with mixed data types) ?

for now required .sort behaviour similar to python3
```py
# A function that returns the length of the value:
def myFunc(e):
  return len(e)
def valFunc(v):
  return v**2
cars = ['Ford', 'Mitsubishi', 'BMW', 'VW']
vals = [0, 1, 2, 10, -1, 100]

cars.sort(key=myFunc)
cars.sort(myFunc)
cars.sort(key=lambda car: len(cars))
vals.sort() # with no args should also be supported
vals.sort(key=valFunc) # or with lambda or func as key

# .sort() or .sort(key=lambda v: v**2) or .sort(key=valFunc) all shou;d be supported just as traditional python 3
```
(handle edge cases such as .sort() can be called only for simple data types such as numbers, string for complex key is mandatory(just as similar to python), for now args can be empty with .sort() or .sort(key= you know what either lambda or valFunc) )

make sure its key=lambda x: x\*\*2 not just without key= (i,e any function anywhere can accept name inside arg too for example myFunc(e=1) myFunc(1))
provided the necessary fix, for now you can remove .sorted just focus on .sort