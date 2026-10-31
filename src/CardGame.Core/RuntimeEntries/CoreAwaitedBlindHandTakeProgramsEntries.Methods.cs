#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreAwaitedBlindHandTakeProgramsEntries
{
    internal SkillProgramStepOutcome BeginAwaitedBlindHandTake(ProgramSkillFrame frame, int amountPerTarget, CardMoveReason reason) => _resolve().BeginAwaitedBlindHandTake(frame, amountPerTarget, reason);
    internal bool ResumeAwaitedBlindHandTake(long frameId) => _resolve().ResumeAwaitedBlindHandTake(frameId);
}
