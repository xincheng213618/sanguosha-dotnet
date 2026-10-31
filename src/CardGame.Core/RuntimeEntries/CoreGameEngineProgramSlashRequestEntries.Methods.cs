#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramSlashRequestEntries
{
    internal void CommitProgramChoiceResult(long frameId, string bind, string optionId, int chooserSeat, string label) => _resolve().CommitProgramChoiceResult(frameId, bind, optionId, chooserSeat, label);
    internal int[] GetNearestLivingCharacterSeats(int seat) => _resolve().GetNearestLivingCharacterSeats(seat);
    internal SkillProgramStepOutcome RequestProgramSlashByNearest(ProgramSkillFrame frame, int participantSeat, int hpAmount, bool keepCurrentRequest = false) => _resolve().RequestProgramSlashByNearest(frame, participantSeat, hpAmount, keepCurrentRequest);
    internal SkillProgramStepOutcome RequestProgramSlashByTarget(ProgramSkillFrame frame, int targetSeat, string resultBind, bool keepCurrentRequest = false) => _resolve().RequestProgramSlashByTarget(frame, targetSeat, resultBind, keepCurrentRequest);
    internal void ResolveProgramRequestSlashByNearestChoice(PromptChoice selected) => _resolve().ResolveProgramRequestSlashByNearestChoice(selected);
    internal void ResolveProgramRequestSlashChoice(PromptChoice selected) => _resolve().ResolveProgramRequestSlashChoice(selected);
    internal PromptChoice SelectAiProgramRequestSlash(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiProgramRequestSlash(decision, frame);
    internal PromptChoice SelectAiProgramRequestSlashByNearest(PendingDecision decision) => _resolve().SelectAiProgramRequestSlashByNearest(decision);
}
