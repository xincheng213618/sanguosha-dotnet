#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AwaitedBlindHandTakeProgramCapability(Func<RuntimeEntries.CoreAwaitedBlindHandTakeProgramsEntries> getCoreAwaitedBlindHandTakeProgramsRuntime) : IAwaitedBlindHandTakeProgramHost
{
    public SkillProgramStepOutcome TakeRandomHandCardsAndAwait(ProgramSkillFrame frame, int amountPerTarget, CardMoveReason reason) => getCoreAwaitedBlindHandTakeProgramsRuntime().BeginAwaitedBlindHandTake(frame, amountPerTarget, reason);
}
