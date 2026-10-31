#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreAwaitedBlindHandTakeProgramsEntries
{
    internal void AssertAwaitedBlindHandTake(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertAwaitedBlindHandTake(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAwaitedBlindHandTake(global::CardGame.Core.ProgramSkillFrame frame, int amountPerTarget, global::CardGame.Core.CardMoveReason reason) => _resolve().BeginAwaitedBlindHandTake(frame, amountPerTarget, reason);
    internal bool ResumeAwaitedBlindHandTake(long frameId) => _resolve().ResumeAwaitedBlindHandTake(frameId);
}
