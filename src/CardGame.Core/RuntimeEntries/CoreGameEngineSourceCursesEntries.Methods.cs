#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceCursesEntries
{
    internal bool CanRunSourceCurseTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunSourceCurseTrigger(candidate, trigger, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SourceCurseSnapshot>? CreateSourceCurseSnapshots(int seat) => _resolve().CreateSourceCurseSnapshots(seat);
    internal global::CardGame.Core.SkillProgramStepOutcome DepositSourceCurse(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect) => _resolve().DepositSourceCurse(f, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawSourceCurse(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect) => _resolve().DrawSourceCurse(f, effect);
    internal global::CardGame.Core.SourceCurseLoss[] EligibleSourceCurseLosses(int owner, string skill, string instance, string hash) => _resolve().EligibleSourceCurseLosses(owner, skill, instance, hash);
    internal global::CardGame.Core.SourceCurseDeposit? EligibleSourceCurseUse(int owner, string skill, string instance, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramEffect effect) => _resolve().EligibleSourceCurseUse(owner, skill, instance, action, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteSourceCurse(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ExecuteSourceCurse(effect, supplied);
    internal void FinishSourceCurse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSourceCurse(f);
    internal bool IsSourceCurseMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsSourceCurseMovement(f, effect, pending);
    internal void ObserveSourceCurseMovement(global::CardGame.Core.CardMovementRecord movement) => _resolve().ObserveSourceCurseMovement(movement);
    internal bool ResumeSourceCurse(long id) => _resolve().ResumeSourceCurse(id);
    internal bool ReturnSourceCurseMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSourceCurseMovement(f);
    internal global::CardGame.Core.SourceCurseDeposit? SourceCurseAt(int seat) => _resolve().SourceCurseAt(seat);
    internal bool SourceCurseHasFinalGameEnd() => _resolve().SourceCurseHasFinalGameEnd();
    internal global::CardGame.Core.SkillProgramEffect? SourceCursePausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceCursePausedEffect(f);
    internal global::CardGame.Core.Card? TakeSourceCurseForJudgment(global::CardGame.Core.JudgmentFrame frame) => _resolve().TakeSourceCurseForJudgment(frame);
    internal bool HasSourceCurseActionColors { get => _resolve().HasSourceCurseActionColors; }
    internal bool HasSourceCurseCapability { get => _resolve().HasSourceCurseCapability; }
    internal long SourceCurseMovementSequence { get => _resolve().SourceCurseMovementSequence; }
}
