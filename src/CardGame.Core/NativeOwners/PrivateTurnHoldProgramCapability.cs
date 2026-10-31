#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PrivateTurnHoldProgramCapability(Func<RuntimeEntries.CoreGameEnginePrivateTurnHoldsEntries> getCoreGameEnginePrivateTurnHoldsRuntime) : IPrivateTurnHoldProgramHost
{
    public SkillProgramStepOutcome HoldOwnerHandUntilTurnEnd(ProgramSkillFrame f) => getCoreGameEnginePrivateTurnHoldsRuntime().HoldOwnerHandUntilTurnEnd(f);
}
