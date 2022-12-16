using System.Globalization;
using System.Text.RegularExpressions;

// string value = "20 - 36 - (1.4 * 3)";
string value = "4 * (1 + 2)";
var val = SplitExpression(value);

var tokens = TokenizeExp(val);
var tokensCpy = TokenizeExp(val);

foreach(var v in val){
    Console.Write(v);
}

Console.WriteLine();

Regex rgx = new Regex(@"[A-Z]+\s?[A-Z]+\s?[A-Z]+");

var condiction = new object[] {Token.HIGHEXP, Token.MEDEXP, Token.LOWEXP, Token.NUM};

for (int i = 0; i < tokens.Count(); i++)
{
    if(condiction.Contains(tokens[i])){
        tokens[i] = Token.EXP;
    }
}

for (int i = 0; i <= tokens.Count() - 2; i++)
{
    List<Token> SomeTokens = new List<Token>();
    SomeTokens.Add(tokens[i]);
    SomeTokens.Add(tokens[i+1]);
    SomeTokens.Add(tokens[i+2]);
 
    if(SomeTokens[0] == Token.EXP && (SomeTokens[1] == Token.OPSUM || SomeTokens[1] == Token.OPSUB) && SomeTokens[2] == Token.EXP){
        
        Console.WriteLine($"{tokens[i+1]} {tokens[i+2]}");
        tokens.RemoveAt(i+1);
        tokens.RemoveAt(i+1);
        if(tokens.Count() > 2)
            i=-1;
    }

    if(SomeTokens[0] == Token.EXP && (SomeTokens[1] == Token.OPMUL || SomeTokens[1] == Token.OPDIV) && SomeTokens[2] == Token.EXP){
        tokens.RemoveAt(i+1);
        tokens.RemoveAt(i+1);
        if(tokens.Count() > 2)
            i=-1;
    }

    if(SomeTokens[0] == Token.OPENPARENTHESIS && SomeTokens[1] == Token.EXP && SomeTokens[2] == Token.CLOSEPARENTHESIS){
        tokens[i] = Token.EXP;
        tokens.RemoveAt(i+1);
        tokens.RemoveAt(i+1);
        if(tokens.Count() > 2)
            i=-1;

    }
    
// highExp = OPENPARENTHESIS exp CLOSEPARENTHESIS
// medExp = exp OPMUL exp | exp OPDIV exp
// lowExp = exp OPSUM exp | exp OPSUB exp
// exp = highExp | medExp | lowExp | NUM

}

foreach(var obj in tokens)
{
    Console.WriteLine(obj);
}

List<object> SplitExpression(string expr)
{
    // TODO Add all your delimiters here
    var delimiters = new[] { '(', '+', '-', '*', '/', ')' };
    var buffer = string.Empty;
    var ret = new List<object>();
    expr = expr.Replace(" ", "");
    expr = expr.Replace(".", ",");
    foreach (var c in expr)
    {
        if (delimiters.Contains(c))
        {
            if (buffer.Length > 0)
            {
                ret.Add(float.Parse(buffer));
                buffer = string.Empty;
            }
            ret.Add(c.ToString());
        }
        else
        {
            buffer += c;
        }
    }
    return ret;
}

List<Token> TokenizeExp(List<object> list)
{
    List<Token> tokenList = new List<Token>();

    foreach(var obj in list)
    {
        switch(obj)
        {
            case "+":
                tokenList.Add(Token.OPSUM);
                break;
            case "-":
                tokenList.Add(Token.OPSUB);
                break;
            case "*":
                tokenList.Add(Token.OPMUL);
                break;
            case "/":
                tokenList.Add(Token.OPDIV);
                break;
            case "(":
                tokenList.Add(Token.OPENPARENTHESIS);
                break;
            case ")":
                tokenList.Add(Token.CLOSEPARENTHESIS);
                break;
            default:
                tokenList.Add(Token.NUM);
                break;
        }
    }

    return tokenList;
}


public enum Token {
    NUM,
    OPSUM,
    OPSUB,
    OPMUL,
    OPDIV,
    OPENPARENTHESIS,
    CLOSEPARENTHESIS,
    LOWEXP,
    MEDEXP,
    HIGHEXP,
    EXP
}

// NUM
// OPSUM
// OPSUB
// OPMUL
// OPDIV
// OPENPARENTHESIS
// CLOSEPARENTHESIS

// highExp = OPENPARENTHESIS exp CLOSEPARENTHESIS
// medExp = exp OPMUL exp | exp OPDIV exp
// lowExp = exp OPSUM exp | exp OPSUB exp
// exp = highExp | medExp | lowExp | NUM

// 4 * (1 + 2)
// NUM OPMUL OPENPARENTHESIS NUM OPSUM NUM CLOSEPARENTHESIS
// exp OPMUL OPENPARENTHESIS exp OPSUM exp CLOSEPARENTHESIS
// exp OPMUL OPENPARENTHESIS lowExp CLOSEPARENTHESIS
// exp OPMUL OPENPARENTHESIS exp CLOSEPARENTHESIS
// exp OPMUL highExp
// exp OPMUL exp
// medExp
// exp

// 1 + 2 * 3
// NUM OPSUM NUM OPMUL NUM
// exp OPSUM exp OPMUL exp
// exp OPSUM medExp
// exp OPSUM exp
// lowExp
// exp