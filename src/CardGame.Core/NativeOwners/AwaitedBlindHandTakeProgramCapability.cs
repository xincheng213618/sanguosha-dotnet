#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AwaitedBlindHandTakeProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreAwaitedBlindHandTakeProgramsEntries> getCoreAwaitedBlindHandTakeProgramsRuntime) : IAwaitedBlindHandTakeProgramHost
{
    public SkillProgramStepOutcome TakeRandomHandCardsAndAwait(ProgramSkillFrame frame, int amountPerTarget, CardMoveReason reason) => getCoreAwaitedBlindHandTakeProgramsRuntime().BeginAwaitedBlindHandTake(frame, amountPerTarget, reason);
}
