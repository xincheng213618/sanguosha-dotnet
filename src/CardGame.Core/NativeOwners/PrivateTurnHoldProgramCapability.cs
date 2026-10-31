#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PrivateTurnHoldProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime>> getCoreGameEnginePrivateTurnHoldsRuntime) : IPrivateTurnHoldProgramHost
{
    public SkillProgramStepOutcome HoldOwnerHandUntilTurnEnd(ProgramSkillFrame f) => getCoreGameEnginePrivateTurnHoldsRuntime().Value.HoldOwnerHandUntilTurnEnd(f);
}
