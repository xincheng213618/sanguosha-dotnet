#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceCursesEntries
{
    internal bool CanRunSourceCurseTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunSourceCurseTrigger(candidate, trigger, context);
    internal IReadOnlyList<SourceCurseSnapshot>? CreateSourceCurseSnapshots(int seat) => _resolve().CreateSourceCurseSnapshots(seat);
    internal SkillProgramStepOutcome DepositSourceCurse(ProgramSkillFrame f, SkillProgramEffect effect) => _resolve().DepositSourceCurse(f, effect);
    internal SkillProgramStepOutcome DrawSourceCurse(ProgramSkillFrame f, SkillProgramEffect effect) => _resolve().DrawSourceCurse(f, effect);
    internal SourceCurseLoss[] EligibleSourceCurseLosses(int owner, string skill, string instance, string hash) => _resolve().EligibleSourceCurseLosses(owner, skill, instance, hash);
    internal SourceCurseDeposit? EligibleSourceCurseUse(int owner, string skill, string instance, CardActionContext action, SkillProgramEffect effect) => _resolve().EligibleSourceCurseUse(owner, skill, instance, action, effect);
    internal SkillProgramStepOutcome ExecuteSourceCurse(SkillProgramEffect effect, ProgramSkillFrame supplied) => _resolve().ExecuteSourceCurse(effect, supplied);
    internal void FinishSourceCurse(ProgramSkillFrame f) => _resolve().FinishSourceCurse(f);
    internal bool IsSourceCurseMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsSourceCurseMovement(f, effect, pending);
    internal void ObserveSourceCurseMovement(CardMovementRecord movement) => _resolve().ObserveSourceCurseMovement(movement);
    internal bool ResumeSourceCurse(long id) => _resolve().ResumeSourceCurse(id);
    internal bool ReturnSourceCurseMovement(ProgramSkillFrame f) => _resolve().ReturnSourceCurseMovement(f);
    internal SourceCurseDeposit? SourceCurseAt(int seat) => _resolve().SourceCurseAt(seat);
    internal bool SourceCurseHasFinalGameEnd() => _resolve().SourceCurseHasFinalGameEnd();
    internal SkillProgramEffect? SourceCursePausedEffect(ProgramSkillFrame f) => _resolve().SourceCursePausedEffect(f);
    internal Card? TakeSourceCurseForJudgment(JudgmentFrame frame) => _resolve().TakeSourceCurseForJudgment(frame);
    internal bool HasSourceCurseActionColors { get => _resolve().HasSourceCurseActionColors; }
    internal bool HasSourceCurseCapability { get => _resolve().HasSourceCurseCapability; }
    internal long SourceCurseMovementSequence { get => _resolve().SourceCurseMovementSequence; }
}
