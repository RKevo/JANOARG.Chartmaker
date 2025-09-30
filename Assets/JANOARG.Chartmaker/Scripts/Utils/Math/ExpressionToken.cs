

namespace JANOARG.Chartmaker.Utils.Math
{
    internal abstract class ExpressionToken
    {
        public override string ToString()
        {
            return $"unidentified tokens";
        }
    }

    internal class ConstantExpressionToken : ExpressionToken
    {
        public double Value;
        public override string ToString()
        {
            return $"numeric literal '{Value}'";
        }
    }

    internal class OperatorExpressionToken : ExpressionToken
    {
        public string Operator;
        public override string ToString()
        {
            return $"operator '{Operator}'";
        }
    }

    internal class StartExpressionToken : ExpressionToken
    {
        public override string ToString()
        {
            return "'('";
        }
    }

    internal class EndExpressionToken : ExpressionToken
    {
        public override string ToString()
        {
            return "')'";
        }
    }

    // Expanded. In the hope to be usable as general storyboard scripting.
    internal enum TokenKind
    {
        // Numeric
        RtParen,
        LParen,
        Plus,
        Minus,
        Mul,
        Div,
        Modulo,
        Exponent,
        BitwiseAnd,
        BitwiseOr,
        BitwiseNegate,

        // Logical
        LogicalAnd,
        LogicalOr,
        LogicalNot,
        Eq,
        NEq,
        GtEq,
        LtEq,
        Gt,
        Lt,

        // Constants
        Identifier,
        Number, // float?
    }

    internal struct SourceToken
    {
        public TokenKind Kind;
        readonly Range tokenRange;
        readonly int Begin => tokenRange.Begin;
        readonly int End => tokenRange.End;

        public struct Range
        {
            readonly public int Begin;
            readonly public int End;
        }
    }
}