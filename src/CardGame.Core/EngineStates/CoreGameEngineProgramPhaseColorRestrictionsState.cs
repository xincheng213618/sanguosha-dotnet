#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineProgramPhaseColorRestrictionsState : Contracts.ICoreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess
{
    private List<ProgramPhaseColorRestriction> _programPhaseColorRestrictions = [];
    internal ref readonly List<ProgramPhaseColorRestriction> ReferenceToProgramPhaseColorRestrictions => ref _programPhaseColorRestrictions;

    ref readonly List<ProgramPhaseColorRestriction> Contracts.ICoreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions => ref _programPhaseColorRestrictions;
}
