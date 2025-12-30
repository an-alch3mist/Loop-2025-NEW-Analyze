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


tryna build a `farmer was replaced` clone for educational purpose
here's the error log report [pasted] from unity console(log reports) what you think is the issue ? provide the fix and let me know.

whats the file you gotta need to modify to fix that, 
make sure it wont break any the existing features that is successfully working

keep critical note in mind: .Net2.0 limitation cannot use `yield return null/value/anything inside try-catch block which is inside IEnumerator` .
All use try:/except: which requires adding exception handling grammar to your parser
if certain fix it requires to use try:/except use alternative approach inside IEnumerator try-catch clause


## 0.fix the function issue
when ran follwing it works as intended
```py
for i in range(10):
    print("somthng")
    # sleep(0.2)
list = [00, 10, 1.0, 100]
print(list)

```

```consoleTMPOutput
somthng
somthng
somthng
somthng
somthng
somthng
somthng
somthng
somthng
somthng
[0, 10, 1, 100]
<color=#88cc00>[Execution complete]</color>
```

but when ran this it isnt as intended why is that, provide the neccessary fix, without breaking any of successull cases already(its 100%  from comprehensive TestSuite, Demo script now)
```py
def _A():
    for i in range(10):
        print("somthng")
        # sleep(0.2)
    list = [00, 10, 1.0, 100]
    print(list)
_A()
```

```consoleTMPOutput
somthng
somthng
somthng
somthng
somthng
somthng
somthng
somthng
somthng
somthng
<color=#88cc00>[Execution complete]</color>
```

## 1. list error still exist when ran
```py
# fail <color=red>[PARSER ERROR] Parse error at line 3: Unexpected token: </color>

list = [
    1,
    2, 3]
print(list)

# fail <color=red>[PARSER ERROR] Parse error at line 2: Unexpected token: </color>
list = [ 1,
    2, 3]

# fail <color=red>[PARSER ERROR] Parse error at line 4: Unexpected token: </color>
list = [
    0,
    1, 2
    ]  

# fail <color=red>[PARSER ERROR] Parse error at line 4: Unexpected token: </color>
list = [
    0,
    1, 2, 3
    ]  

# the only successful is
# success
list = [1,
2, 3]

# success
list = [
    0,
    1,
    2
] 
# success
list = [
    0,
    1, 2
] 

  
```
do an indepth desearch on dictionary, list in python 3 syntax and make sure these errors are fixed.

```output
<color=red>[PARSER ERROR] Parse error at line 3: Unexpected token: 
</color>
```