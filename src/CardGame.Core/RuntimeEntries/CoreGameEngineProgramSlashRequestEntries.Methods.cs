#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramSlashRequestEntries
{
    internal void CommitProgramChoiceResult(long frameId, string bind, string optionId, int chooserSeat, string label) => _resolve().CommitProgramChoiceResult(frameId, bind, optionId, chooserSeat, label);
    internal int[] GetNearestLivingCharacterSeats(int seat) => _resolve().GetNearestLivingCharacterSeats(seat);
    internal global::CardGame.Core.SkillProgramStepOutcome RequestProgramSlashByNearest(global::CardGame.Core.ProgramSkillFrame frame, int participantSeat, int hpAmount, bool keepCurrentRequest = false) => _resolve().RequestProgramSlashByNearest(frame, participantSeat, hpAmount, keepCurrentRequest);
    internal global::CardGame.Core.SkillProgramStepOutcome RequestProgramSlashByTarget(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string resultBind, bool keepCurrentRequest = false) => _resolve().RequestProgramSlashByTarget(frame, targetSeat, resultBind, keepCurrentRequest);
    internal void ResolveProgramRequestSlashByNearestChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramRequestSlashByNearestChoice(selected);
    internal void ResolveProgramRequestSlashChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramRequestSlashChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiProgramRequestSlash(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiProgramRequestSlash(decision, frame);
    internal global::CardGame.Core.PromptChoice SelectAiProgramRequestSlashByNearest(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiProgramRequestSlashByNearest(decision);
}
