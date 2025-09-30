using System;
using System.Runtime.CompilerServices;

namespace JANOARG.Chartmaker.Utils
{
    public readonly struct Result<T, E>
    {
        readonly T value { init; get; }
        readonly E error { init; get; }
        readonly bool valid { init; get; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<T, E> Ok(T value)
        {
            return new Result<T, E>
            {
                value = value,
                error = default,
                valid = true,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<T, E> Err(E err)
        {
            return new Result<T, E>
            {
                value = default,
                error = err,
                valid = false,
            };
        }

        public readonly bool IsOk => valid;
        public readonly bool IsErr => !valid;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IfOk(Action<T> fv)
        {
            if (valid)
            {
                fv.Invoke(value);
            }
            return valid;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IfErr(Action<E> fe)
        {
            if (!valid)
            {
                fe.Invoke(error);
            }
            return valid;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Handle(Action<T> fv, Action<E> fe)
        {
            if (valid)
            {
                fv.Invoke(value);
                return;
            }
            fe.Invoke(error);
        }
    }

    public readonly struct Maybe<T>
    {
        readonly T value { init; get; }
        readonly bool valid { init; get; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Maybe<T> Some(T value)
        {
            return new Maybe<T>
            {
                value = value,
                valid = true,
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Maybe<T> None()
        {
            return new Maybe<T>
            {
                value = default,
                valid = true,
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IfSome(Action<T> fv)
        {
            if (valid)
            {
                fv.Invoke(value);
            }
            return valid;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IfNone(Action f)
        {
            if (!valid)
            {
                f.Invoke();
            }
            return valid;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Handle(Action<T> fv, Action f)
        {
            if (valid)
            {
                fv.Invoke(value);
                return;
            }
            f.Invoke();
        }
        
    }
}