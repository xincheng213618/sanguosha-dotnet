#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess : IProgramRuntimeOperations
{
    ref readonly List<ProgramPhaseColorRestriction> _programPhaseColorRestrictions { get; }
}
