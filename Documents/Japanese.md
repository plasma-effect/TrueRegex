# TrueRegexの概要
コンピューターサイエンスの文脈で定義される[正規言語](https://ja.wikipedia.org/wiki/%E6%AD%A3%E8%A6%8F%E8%A8%80%E8%AA%9E)を表すExpressionクラス(演算子オーバーロードを定義するためにインターフェースではない)を提供するライブラリである。実際に言語をマッチさせるために内部では有向グラフを構成する。  
構成されるグラフは非決定性有限オートマトンではなく以下で定義されるグラフである。
- 頂点は終了状態かどうかを判別するbool値を持つ
- 辺はεかFunc\<char, bool\>のどちらかを持つ
- 開始状態を表す頂点がただ1つ存在する

ただしこれに対応する非決定性有限オートマトンを構成することができるため問題ない。  

マッチのチェックの最悪時間計算量はO(N\*K^2\*logK)である(Nは文字列のサイズ、Kは上のグラフの頂点数)。この内K^2\*logKは現状わかっている上界であり、実際はこれより小さい可能性がある。
# 提供されるクラスと定数
提供されるクラスは全てnamespace TrueRegex内にあるが、static class Predefined内にこのライブラリの使用を補助するようなクラスや静的プロパティが定義されている。
## public abstract class Expression
正規言語を表現するクラスである。これ自体は抽象クラスであり、したがってインスタンスを生成することはできない。このクラスでは以下の関数が定義されている。
### public bool Match(IEnumerable\<char\> str)
与えられた文字の列(通常はstringであろう)がExpressionと完全にマッチするかをチェックする。マッチした場合trueを返し、そうでない場合falseを返す。
### public int? FirstMatch(IEnumerable\<char\> str)
マッチする「先頭の文字を含む最小の部分文字列」の長さを返す。そのような部分文字列が存在しない場合nullを返す。
### public int? LastMatch(IEnumerable<char> str)
マッチする「先頭の文字を含む最長の部分文字列」の長さを返す。そのような部分文字列が存在しない場合nullを返す。

---
## public class Atomic : Expression
Func\<char, bool\> funcを持ち「文字列が1文字で、かつその文字をcとしてfunc(c)がtrue」の場合のみマッチするExpression。以下の関数を持つ
### public Atomic(Func\<char, bool\> func)
コンストラクタ。
### public static Atomic Create(Func\<char, bool\> func)
そのままコンストラクタを呼ぶstatic関数。newを書きたくないときに使う。
### 使用例
以下使用例ではusing static System.Consoleをしているものとする。
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
0回以上の繰り返しを表すExpression。コンストラクタから生成する以外に、Expressionに対し単項演算子~を適用することでも生成できる。以下の関数を持つ。
### public ZeroRepeat(Expression expr)
コンストラクタ。
### 使用例
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
1回以上の繰り返しを表すExpression。コンストラクタから生成する以外に、Expressionに対し単項演算子+を適用することでも生成できる。以下の関数を持つ。
### public OneRepeat(Expression expr)
コンストラクタ。
### 使用例
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
2つのExpressionの連結を表すExpression。コンストラクタから生成する以外に、2つのExpressionに対し二項演算子+を適用することでも生成できる。以下の関数を持つ。
### public Sequence(Expression lhs, Expression rhs)
コンストラクタ。
### 使用例
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
省略可能を表すExpression。コンストラクタから生成する以外に、Expressionに対し単項演算子-を適用することでも生成できる。以下の関数を持つ。
### public Optional(Expression expr)
コンストラクタ。
### 使用例
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
2つのExpressionの和を表すExpression。コンストラクタから生成する以外に2つのExpressionに対し二項演算子 | を適用することでも生成できる。以下の関数を持つ。
### public Select(Expression lhs, Expression rhs)
コンストラクタ。
### 使用例
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
Expressionの否定を表すExpression。コンストラクタから生成する以外にExpressionに対し単項演算子 ! を適用することでも生成できる。以下の関数を持つ。
### public Not(Expression expr)
コンストラクタ。
### 使用例
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
文字の集合setを持ち「文字列が1文字で、かつその文字をcとしてcがsetに含まれる」の場合のみマッチするExpression。以下の関数を持つ。
### public Chars(IEnumerable\<char\> chars)
コンストラクタ。
### public static Chars Create(params char[] cs)
そのままコンストラクタを呼ぶstatic関数。より明示的にどの文字のときマッチするか表明したり、単にnewを書きたくなかったりするときに使う。
### 使用例
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
1文字以上の数字の列にマッチするExpression。+Atomic.Create(char.IsNumber)と同じである。
### 使用例
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
1文字以上の名前(数字のみのものを含む)にマッチするExpression。+Atomic.Create(char.IsLetterOrDigit)と同じである。
### 使用例
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
特定の文字列にマッチするExpression。以下の関数を持つ。
### public String(string str)
コンストラクタ。
### public static String Create(string str)
そのままコンストラクタを呼ぶstatic関数。newを書きたくないときに使う。
### 使用例
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
## FirstMatch関数とLastMatch関数の使用例
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