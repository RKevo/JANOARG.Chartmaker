namespace JANOARG.Chartmaker.Utils.Math
{
    // todo: Instr: op + b0 + b1 + b2 = uint
    internal enum Opcode : byte
    {
        // { R[0] <- R[0] @ R[1] Imm
        Add,
        Sub,
        Mul,
        Div,
        Mod,
        Pow,
        BAnd,
        BOr,
        BNeg,
        // }
        // { R[0]: addr(R[1 2]) Abs
        TJmp,
        NJmp,
        // }
        // todo: the rest opcode + functional lexer/parser + node alloc 

    }
}