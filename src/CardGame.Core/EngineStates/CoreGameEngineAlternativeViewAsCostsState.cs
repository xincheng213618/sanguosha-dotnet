#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAlternativeViewAsCostsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess
{
    private global::System.Collections.Generic.Dictionary<int, (int Turn, global::System.Collections.Generic.HashSet<global::CardGame.Core.CardKind> Kinds)> _usedProgramBasicCardNames = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, (int Turn, global::System.Collections.Generic.HashSet<global::CardGame.Core.CardKind> Kinds)> ReferenceToUsedProgramBasicCardNames => ref _usedProgramBasicCardNames;

    ref readonly global::System.Collections.Generic.Dictionary<int, (int Turn, global::System.Collections.Generic.HashSet<global::CardGame.Core.CardKind> Kinds)> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess._usedProgramBasicCardNames => ref _usedProgramBasicCardNames;
}
