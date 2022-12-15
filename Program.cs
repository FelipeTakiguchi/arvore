using System.Globalization;

string value = "20 - 36 - (1.4 * 3)";
var val = TokenizeExpression(value);

foreach(var v in val){
    Console.WriteLine(v);
}

List<object> TokenizeExpression(string expr)
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
// exp = highExp | medExp lowExp | NUM

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