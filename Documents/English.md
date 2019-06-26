# About TrueRegex
This is library for [Regular Language](https://en.wikipedia.org/wiki/Regular_language) defined in Computer Science. This library have Expression abstract class(for operator overload, it isn't interface). It construct a directed graph to match a string.  
Constructed graph is not NFA but directed graph defined below.
- each vertex have bool value that means that vertex is accept state or not
- edge have ε or Func\<char, bool\>
- there is one vertex that express initial state

But it is not problem because we can construct NFA from that graph.

Worst time complexity for match is O(N\*K^2\*logK) (where N is string length and K is number of vertexes of above graph). \[K^2\*logK\] is upper bound I know, so real worst time complexity may be smaller than this.
# class and static property
classes are all in "namespace TrueRegex", and classes and properties that help you in "static class Predefined".
## public abstract class Expression
Abstract class that express Regular Language.This class have below functions.
### public bool Match(IEnumerable\<char\> str)
Check given character sequence(normally, that is string) matches Expression. if that sequence matches then return true, otherwise return false.
### public int? FirstMatch(IEnumerable\<char\> str)
Return length of "shortest continuous subsequence include first character" that matches Expression. If there is not that continuous subsequence, return null.
### public int? LastMatch(IEnumerable<char> str)
Return length of "longest continuous subsequence include first character" that matches Expression. If there is not that continuous subsequence, return null.

---
## public class Atomic : Expression
The Expression that have "Func\<char, bool\> func" and string matches when length of that string is one and func(c) is true where that string is \[c\]. This class have below function.
### public Atomic(Func\<char, bool\> func)
Constructor.
### public static Atomic Create(Func\<char, bool\> func)
Static function that call constructor of Atomic. You can use this static function when you don't want to write "new Atomic(func)".
### Example
In all example include this, the code includes "using static System.Console;".
```csharp
static void Main(string[] args)
{
    var expr = Atomic.Create(char.IsNumber);
    WriteLine(expr.Match("1"));
    WriteLine(expr.Match("a"));
    WriteLine(expr.Match("ab"));
    WriteLine(expr.Match(""));
}
```

---
## public class ZeroRepeat : Expression
The Expression that express repeat zero or more repetitions. You can use unary operator~ to construct this class instance. This class have below function.
### public ZeroRepeat(Expression expr)
Constructor.
### Example
```csharp
static void Main(string[] args)
{
    var atomic = Atomic.Create(char.IsNumber);

    var expr0 = new ZeroRepeat(atomic);
    WriteLine(expr0.Match("123"));
    WriteLine(expr0.Match("12b"));
    WriteLine(expr0.Match(""));

    var expr1 = ~atomic;
    WriteLine(expr1.Match("123"));
    WriteLine(expr1.Match("12b"));
    WriteLine(expr1.Match(""));
}
```

---
## public class OneRepeat : Expression
The Expression that express repeat one or more repetitions. You can use unary operator+ to construct this class instance. This class have below function.
### public OneRepeat(Expression expr)
Constuctor.
### Example
```csharp
static void Main(string[] args)
{
    var atomic = Atomic.Create(char.IsNumber);

    var expr0 = new OneRepeat(atomic);
    WriteLine(expr0.Match("123"));
    WriteLine(expr0.Match("12b"));
    WriteLine(expr0.Match(""));

    var expr1 = +atomic;
    WriteLine(expr1.Match("123"));
    WriteLine(expr1.Match("12b"));
    WriteLine(expr1.Match(""));
}
```
## public class Sequence : Expression
The Expression that express sequence of 2 expressions. You can use binary operator+ to construct this class instance. This class have below function.
### public Sequence(Expression lhs, Expression rhs)
Constructor.
### Example
```csharp
static void Main(string[] args)
{
    var atomic = Atomic.Create(char.IsNumber);

    var expr0 = new Sequence(atomic, atomic);
    WriteLine(expr0.Match("12"));
    WriteLine(expr0.Match("12b"));
    WriteLine(expr0.Match("1"));
    WriteLine(expr0.Match(""));

    var expr1 = atomic + atomic;
    WriteLine(expr0.Match("12"));
    WriteLine(expr0.Match("12b"));
    WriteLine(expr0.Match("1"));
    WriteLine(expr0.Match(""));
}
```

---
## public class Optional : Expression
The Expression that express optional expression. You can use unary operator- to construct this class instance. This class have below function.
### public Optional(Expression expr)
Constructor.
### Example
```csharp
static void Main(string[] args)
{
    var atomic = Atomic.Create(char.IsNumber);

    var expr0 = new Optional(atomic);
    WriteLine(expr0.Match("0"));
    WriteLine(expr0.Match("a"));
    WriteLine(expr0.Match(""));

    var expr1 = -atomic;
    WriteLine(expr1.Match("0"));
    WriteLine(expr1.Match("a"));
    WriteLine(expr1.Match(""));
}
```

---
## public class Select : Expression
The Expression that express union of 2 expressions. You can use binary operator | to construct this class instance. This class have below function.
### public Select(Expression lhs, Expression rhs)
Constructor.
### Example
```csharp
static void Main(string[] args)
{
    var atomic0 = Atomic.Create(char.IsNumber);
    var atomic1 = Atomic.Create(char.IsLetter);

    var expr0 = new Select(atomic0, atomic1);
    WriteLine(expr0.Match("1"));
    WriteLine(expr0.Match("a"));
    WriteLine(expr0.Match(""));

    var expr1 = atomic0 | atomic1;
    WriteLine(expr1.Match("1"));
    WriteLine(expr1.Match("a"));
    WriteLine(expr1.Match(""));
}
```

---
## public class Not : Expression
The Expression that express negate of expression. You can use unary operator ! to construct this class instance. This class have below function.
### public Not(Expression expr)
Constructor.
### Example
```csharp
static void Main(string[] args)
{
    var expr = +Atomic.Create(char.IsNumber);

    var expr0 = new Not(expr);
    WriteLine(expr0.Match("123"));
    WriteLine(expr0.Match("12a"));
    WriteLine(expr0.Match(""));

    var expr1 = !expr;
    WriteLine(expr1.Match("123"));
    WriteLine(expr1.Match("12a"));
    WriteLine(expr1.Match(""));
}
```

---
## public class Predefined.Chars : Expression
The Expression that have set of character and string matches when length of that string is one, and \[c\] in that set where that string is \[c\]. This class have below functions.
### public Chars(IEnumerable\<char\> chars)
Constructor.
### public static Chars Create(params char[] cs)
Static function that call constructor of Chars. You can use this static function when you assert charater set explicitly or you don't want to write "new Predefined.Chars(cs)".
### Example
```csharp
static void Main(string[] args)
{
    var expr0 = new Predefined.Chars("abc");
    WriteLine(expr0.Match("a"));
    WriteLine(expr0.Match("b"));
    WriteLine(expr0.Match("c"));
    WriteLine(expr0.Match("d"));

    var expr1 = Predefined.Chars.Create('a', 'b', 'c');
    WriteLine(expr1.Match("a"));
    WriteLine(expr1.Match("b"));
    WriteLine(expr1.Match("c"));
    WriteLine(expr1.Match("d"));
}
```

---
## public static OneRepeat Predefined.Number { get; }
The Expression that express sequence of numbers. This is equal to "+Atomic.Create(char.IsNumber)".
### Example
```csharp
static void Main(string[] args)
{
    WriteLine(Predefined.Number.Match("123"));
    WriteLine(Predefined.Number.Match("12a"));
    WriteLine(Predefined.Number.Match(""));
}
```

---
## public static OneRepeat Name { get; }
The Expression that express name(include constructed by only numbers). This is equal to "+Atomic.Create(char.IsLetterOrDigit)".
### Example
```csharp
static void Main(string[] args)
{
    WriteLine(Predefined.Name.Match("abc"));
    WriteLine(Predefined.Name.Match("123"));
    WriteLine(Predefined.Name.Match("cd4"));
    WriteLine(Predefined.Name.Match("a,c"));
    WriteLine(Predefined.Name.Match(""));
}
```

---
## public class Predefined.String : Expression
The Expression that express specific string.
### public String(string str)
Constructor.
### public static String Create(string str)
Static function that call constructor of Atomic. You can use this static function when you don't want to write "new Predefined.String(str)".
### Example
```csharp
static void Main(string[] args)
{
    var expr0 = new Predefined.String("123");
    WriteLine(expr0.Match("123"));
    WriteLine(expr0.Match("12a"));
    WriteLine(expr0.Match("12"));
    WriteLine(expr0.Match(""));

    var expr1 = Predefined.String.Create("123");
    WriteLine(expr1.Match("123"));
    WriteLine(expr1.Match("12a"));
    WriteLine(expr1.Match("12"));
    WriteLine(expr1.Match(""));
}
```

---
## Example of "FirstMatch" and "LastMatch"
```csharp
static void Main(string[] args)
{
    var expr = 
        +Atomic.Create(char.IsLetter) +
        +Atomic.Create(char.IsNumber);

    var str0 = "name123";
    WriteLine(expr.FirstMatch(str0)?.ToString() ?? "null");
    WriteLine(expr.LastMatch(str0)?.ToString() ?? "null");

    var str1 = "name123suffix";
    WriteLine(expr.FirstMatch(str1)?.ToString() ?? "null");
    WriteLine(expr.LastMatch(str1)?.ToString() ?? "null");

    var str2 = "123";
    WriteLine(expr.FirstMatch(str2)?.ToString() ?? "null");
    WriteLine(expr.LastMatch(str2)?.ToString() ?? "null");
}
```