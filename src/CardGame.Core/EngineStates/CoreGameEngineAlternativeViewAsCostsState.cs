#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAlternativeViewAsCostsState : Contracts.ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess
{
    private Dictionary<int, (int Turn, HashSet<CardKind> Kinds)> _usedProgramBasicCardNames = [];
    internal ref readonly Dictionary<int, (int Turn, HashSet<CardKind> Kinds)> ReferenceToUsedProgramBasicCardNames => ref _usedProgramBasicCardNames;

    ref readonly Dictionary<int, (int Turn, HashSet<CardKind> Kinds)> Contracts.ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess._usedProgramBasicCardNames => ref _usedProgramBasicCardNames;
}
