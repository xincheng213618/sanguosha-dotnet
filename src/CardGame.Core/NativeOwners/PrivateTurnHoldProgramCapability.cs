#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PrivateTurnHoldProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateTurnHoldsEntries> getCoreGameEnginePrivateTurnHoldsRuntime) : IPrivateTurnHoldProgramHost
{
    public SkillProgramStepOutcome HoldOwnerHandUntilTurnEnd(ProgramSkillFrame f) => getCoreGameEnginePrivateTurnHoldsRuntime().HoldOwnerHandUntilTurnEnd(f);
}
