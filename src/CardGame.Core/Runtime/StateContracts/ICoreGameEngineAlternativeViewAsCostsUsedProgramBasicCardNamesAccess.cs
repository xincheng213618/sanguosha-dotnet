#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, (int Turn, HashSet<CardKind> Kinds)> _usedProgramBasicCardNames { get; }
}
