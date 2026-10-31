#nullable enable
using System.Collections.ObjectModel;

namespace CardGame.Core.NativeOwners;
internal sealed class ParticipantPhaseProgramEffectCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries> getCoreProgramParticipantPhaseOperationsRuntime) : IParticipantPhaseProgramEffectHost
{
    public void ConsumeTargetPhaseLedger(ProgramSkillFrame frame, string usageId) => getCoreProgramParticipantPhaseOperationsRuntime().ConsumeProgramTargetPhaseLedger(frame, usageId);
    public void GrantTurnBoundSuitUseProhibition(ProgramSkillFrame frame, string sourceBind) => getCoreProgramParticipantPhaseOperationsRuntime().GrantProgramTurnBoundSuitUseProhibition(frame, sourceBind);
}
