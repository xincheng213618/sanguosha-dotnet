#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ParticipantPreventionCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineParticipantPreventionEntries> getCoreGameEngineParticipantPreventionRuntime) : IParticipantPreventionHost
{
    public SkillProgramStepOutcome PreventParticipantDamageDrawActualSource(ProgramSkillFrame f) => getCoreGameEngineParticipantPreventionRuntime().PreventParticipantDamageDrawActualSource(f);
}
