using System.Reflection;

namespace CardGame.Core;
internal enum ProgramContinuationKind
{
    Resume,
    Movement,
    MovementTail
}

// Register beside the implementation. Order is part of deterministic rule execution,
// not reflection discovery order. The handler retains its exact ownership checks.
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal sealed class ProgramContinuationAttribute(ProgramContinuationKind kind, int order) : Attribute
{
    internal ProgramContinuationKind Kind { get; } = kind;
    internal int Order { get; } = order;
}
