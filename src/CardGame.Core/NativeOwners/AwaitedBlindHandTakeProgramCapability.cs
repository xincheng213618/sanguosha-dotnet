#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AwaitedBlindHandTakeProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreAwaitedBlindHandTakeProgramsRuntime>> getCoreAwaitedBlindHandTakeProgramsRuntime) : IAwaitedBlindHandTakeProgramHost
{
    public SkillProgramStepOutcome TakeRandomHandCardsAndAwait(ProgramSkillFrame frame, int amountPerTarget, CardMoveReason reason) => getCoreAwaitedBlindHandTakeProgramsRuntime().Value.BeginAwaitedBlindHandTake(frame, amountPerTarget, reason);
}
