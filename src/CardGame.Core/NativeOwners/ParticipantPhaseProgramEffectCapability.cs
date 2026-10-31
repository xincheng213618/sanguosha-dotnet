#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ParticipantPhaseProgramEffectCapability(Func<RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries> getCoreProgramParticipantPhaseOperationsRuntime) : IParticipantPhaseProgramEffectHost
{
    public void ConsumeTargetPhaseLedger(ProgramSkillFrame frame, string usageId) => getCoreProgramParticipantPhaseOperationsRuntime().ConsumeProgramTargetPhaseLedger(frame, usageId);
    public void GrantTurnBoundSuitUseProhibition(ProgramSkillFrame frame, string sourceBind) => getCoreProgramParticipantPhaseOperationsRuntime().GrantProgramTurnBoundSuitUseProhibition(frame, sourceBind);
}
