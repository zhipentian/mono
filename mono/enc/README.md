
* Sample5: Swapped method calls another method that hasn't previously existed (i.e. add a new method, woot! only static though).
* Sample6: Swap a method that is currently on the stack (on-stack-replacement is not expected, but the execution should finish with the old method).
* Sample7: Swap a method that is currently on the stack and actually force the transformation of the new version on another thread.

