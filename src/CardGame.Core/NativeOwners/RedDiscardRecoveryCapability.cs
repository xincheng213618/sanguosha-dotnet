#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RedDiscardRecoveryCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRedDiscardRecoveryEntries> getCoreGameEngineRedDiscardRecoveryRuntime) : IRedDiscardRecoveryHost
{
    public SkillProgramStepOutcome OfferRedDiscardRecoveryChoice(ProgramSkillFrame f) => getCoreGameEngineRedDiscardRecoveryRuntime().BeginRedDiscardRecovery(f);
}
