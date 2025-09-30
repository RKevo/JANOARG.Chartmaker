using System;
using Unity.VisualScripting;
using conerr = JANOARG.Chartmaker.Utils.Math.SyntaxConstructionError;

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}

namespace JANOARG.Chartmaker.Utils.Math
{
    internal readonly struct TokenRange
    {
        public readonly int Begin;
        public readonly int End;
    }
    internal interface ISyntaxNode
    {

    }

    internal readonly struct BinaryOp : ISyntaxNode
    {
        public readonly Op Operator { init; get; }
        /// Indices of the other <see cref="ISyntaxNode"/>s
        public readonly int RIndex { init; get; }
        public readonly int LIndex { init; get; }

        public static Result<BinaryOp, conerr> From(int left, int right, Op op)
        {
            switch (op)
            {
                case Op.Add:
                case Op.Sub:
                case Op.Mul:
                case Op.Mod:
                case Op.Exp:
                case Op.BAnd:
                case Op.BOr:
                    return Result<BinaryOp, conerr>.Ok(new BinaryOp
                    {
                        RIndex = right,
                        LIndex = left,
                        Operator = op
                    });
                default:
                    return Result<BinaryOp, conerr>.Err(conerr.InvalidBinaryOp);
            }

        }
    }

    internal readonly struct UnaryOp : ISyntaxNode
    {
        public readonly Op Operator { init; get; }
        public readonly int UnitIndex { init; get; }

        public static Result<UnaryOp, conerr> From(int unit, Op op)
        {
            switch (op)
            {
                case Op.Add:
                case Op.Sub:
                case Op.Mul:
                case Op.Mod:
                case Op.Exp:
                case Op.BAnd:
                case Op.BOr:
                    return Result<UnaryOp, conerr>.Ok(new UnaryOp
                    {
                        UnitIndex = unit,
                        Operator = op
                    });
                default:
                    return Result<UnaryOp, conerr>.Err(conerr.InvalidBinaryOp);
            }

        }
    }

    internal readonly struct NumberLiteral : ISyntaxNode
    {
        public readonly double Value { init; get; }
        public static NumberLiteral From(double value)
            => new()
            {
                Value = value
            };
    }

    internal readonly struct Identifier : ISyntaxNode
    {
        public readonly string Value { init; get; }
        public static Identifier From(string value)
            => new()
            {
                Value = value
            };
    }

    internal readonly struct BoolLiteral : ISyntaxNode
    {
        public readonly bool Value { init; get; }
        public static BoolLiteral From(bool value)
            => new()
            {
                Value = value
            };
    }

    

    internal enum SyntaxConstructionError
    {
        InvalidBinaryOp,
        InvalidUnaryOp,
    }



    internal enum Op
    {
        Add, Sub, Mul, Div, Mod, Exp, BAnd, BOr, BNeg,
        LAnd, LOr, LNot, Eq, Neq, GtEq, LtEq, Gt, Lt
    }

    internal static class SynUtils
    {
        public static Maybe<Op> MapOp(TokenKind kind)
        {
            return kind switch
            {
                TokenKind.Plus => Maybe<Op>.Some(Op.Add),
                TokenKind.Minus => Maybe<Op>.Some(Op.Sub),
                TokenKind.Mul => Maybe<Op>.Some(Op.Mul),
                TokenKind.Div => Maybe<Op>.Some(Op.Div),
                TokenKind.Modulo => Maybe<Op>.Some(Op.Mod),
                TokenKind.Exponent => Maybe<Op>.Some(Op.Exp),
                TokenKind.BitwiseAnd => Maybe<Op>.Some(Op.BAnd),
                TokenKind.BitwiseOr => Maybe<Op>.Some(Op.BOr),
                TokenKind.BitwiseNegate => Maybe<Op>.Some(Op.BNeg),
                TokenKind.LogicalAnd => Maybe<Op>.Some(Op.LAnd),
                TokenKind.LogicalOr => Maybe<Op>.Some(Op.LOr),
                TokenKind.LogicalNot => Maybe<Op>.Some(Op.LNot),
                TokenKind.Eq => Maybe<Op>.Some(Op.Eq),
                TokenKind.NEq => Maybe<Op>.Some(Op.Neq),
                TokenKind.GtEq => Maybe<Op>.Some(Op.GtEq),
                TokenKind.LtEq => Maybe<Op>.Some(Op.LtEq),
                TokenKind.Gt => Maybe<Op>.Some(Op.Gt),
                TokenKind.Lt => Maybe<Op>.Some(Op.Lt),
                _ => Maybe<Op>.None(),
            };
        }
    }
}
