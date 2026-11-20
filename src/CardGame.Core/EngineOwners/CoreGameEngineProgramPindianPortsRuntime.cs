#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramPindianPortsRuntime(
    RuntimeEntries.CoreGameEnginePindianEntries _corePindianOperations,
    RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations,
    RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations,
    RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries _coreForeignEquipmentDiscardPoliciesOperations,
    RuntimeEntries.CoreGameEngineEquipmentSkillsEntries _coreEquipmentSkillsOperations,
    EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess)
    : IProgramPindianPolicyQueries, IProgramPindianProducer
{
}
