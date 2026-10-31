#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RedDiscardRecoveryCapability(Func<RuntimeEntries.CoreGameEngineRedDiscardRecoveryEntries> getCoreGameEngineRedDiscardRecoveryRuntime) : IRedDiscardRecoveryHost
{
    public SkillProgramStepOutcome OfferRedDiscardRecoveryChoice(ProgramSkillFrame f) => getCoreGameEngineRedDiscardRecoveryRuntime().BeginRedDiscardRecovery(f);
}
