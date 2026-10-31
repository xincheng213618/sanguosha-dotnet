#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineProgramPhaseColorRestrictionsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.ProgramPhaseColorRestriction> _programPhaseColorRestrictions = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramPhaseColorRestriction> ReferenceToProgramPhaseColorRestrictions => ref _programPhaseColorRestrictions;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramPhaseColorRestriction> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions => ref _programPhaseColorRestrictions;
}
