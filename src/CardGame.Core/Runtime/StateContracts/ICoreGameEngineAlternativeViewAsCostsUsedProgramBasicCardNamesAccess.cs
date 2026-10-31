#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, (int Turn, global::System.Collections.Generic.HashSet<global::CardGame.Core.CardKind> Kinds)> _usedProgramBasicCardNames { get; }
}
