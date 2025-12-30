tryna build a `farmer was replaced` clone for educational purpose
here's the error log report [pasted] from unity console(log reports) what you think is the issue ? provide the fix and let me know.

whats the file you gotta need to modify to fix that, 
make sure it wont break any the existing features that is successfully working


keep critical note in mind: .Net2.0 limitation cannot use `yield return null/value/anything inside try-catch block which is inside IEnumerator` .
All use try:/except: which requires adding exception handling grammar to your parser
if certain fix it requires to use try:/except use alternative approach inside IEnumerator try-catch clause