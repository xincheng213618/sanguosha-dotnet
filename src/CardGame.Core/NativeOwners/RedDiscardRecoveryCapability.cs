#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RedDiscardRecoveryCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime>> getCoreGameEngineRedDiscardRecoveryRuntime) : IRedDiscardRecoveryHost
{
    public SkillProgramStepOutcome OfferRedDiscardRecoveryChoice(ProgramSkillFrame f) => getCoreGameEngineRedDiscardRecoveryRuntime().Value.BeginRedDiscardRecovery(f);
}
