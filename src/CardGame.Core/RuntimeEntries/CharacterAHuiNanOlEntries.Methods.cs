#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterAHuiNanOlEntries
{
    internal IReadOnlyList<PromptChoice> AHuiNanChoices(ProgramSkillFrame f) => _resolve().AHuiNanChoices(f);
    internal bool AHuiNanEndingParent(ProgramTriggerCandidate c, ProgramSkillWindowContext context, scoped out DeferredTurnEndFrame? parent) => _resolve().AHuiNanEndingParent(c, context, out parent);
    internal bool AHuiNanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().AHuiNanFirstChild(f, child);
    internal AHuiNanBasicUseSnapshot[] AHuiNanHistory(int turn) => _resolve().AHuiNanHistory(turn);
    internal bool AHuiNanLegacyBasicUse(CardUseFrame use) => _resolve().AHuiNanLegacyBasicUse(use);
    internal ProgramSkillFrame? AHuiNanObserverRoot() => _resolve().AHuiNanObserverRoot();
    internal bool AHuiNanStartedBinding(ProgramSkillFrame f) => _resolve().AHuiNanStartedBinding(f);
    internal IReadOnlyList<IReadOnlyList<int>> AHuiNanUseTargets(ProgramSkillFrame f) => _resolve().AHuiNanUseTargets(f);
    internal bool AdvanceAHuiNanChildren() => _resolve().AdvanceAHuiNanChildren();
    internal PromptChoice AiAHuiNan(PendingDecision p, ProgramSkillFrame f) => _resolve().AiAHuiNan(p, f);
    internal void AnswerAHuiNan(PromptChoice choice) => _resolve().AnswerAHuiNan(choice);
    internal SkillProgramStepOutcome BeginAHuiNan(ProgramSkillFrame input) => _resolve().BeginAHuiNan(input);
    internal void BeginAHuiNanJuemanUse(ProgramSkillFrame f, AHuiNanJuemanUseOrigin o, IReadOnlyList<int> targets) => _resolve().BeginAHuiNanJuemanUse(f, o, targets);
    internal void ContinueAHuiNan(long id) => _resolve().ContinueAHuiNan(id);
    internal bool ContinueAHuiNanJuemanSimpleUse(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueAHuiNanJuemanSimpleUse(id, continuation);
    internal bool ContinuePaidAHuiNan(ProgramSkillFrame f) => _resolve().ContinuePaidAHuiNan(f);
    internal bool DrainAHuiNan(long id) => _resolve().DrainAHuiNan(id);
    internal void ExpireAHuiNanJuemanTurnEndWine() => _resolve().ExpireAHuiNanJuemanTurnEndWine();
    internal void FinishAHuiNan(ProgramSkillFrame f, string outcome) => _resolve().FinishAHuiNan(f, outcome);
    internal bool FinishAHuiNanJuemanAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishAHuiNanJuemanAttack(attack, out paused);
    internal bool HasIssuedAHuiNanJuemanUse(long id) => _resolve().HasIssuedAHuiNanJuemanUse(id);
    internal bool IsAHuiNanJuemanEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsAHuiNanJuemanEndingCandidate(c, context);
    internal bool IsAHuiNanJuemanSlashQuery(CharacterState actor) => _resolve().IsAHuiNanJuemanSlashQuery(actor);
    internal bool IsAHuiNanJuemanUse(long id) => _resolve().IsAHuiNanJuemanUse(id);
    internal bool MatchesAHuiNanJueman(ProgramSkillFrame f, AHuiNanJuemanUseOrigin o) => _resolve().MatchesAHuiNanJueman(f, o);
    internal bool MatchesAHuiNanJuemanUseAction(long id, CardActionContext a, int cardId) => _resolve().MatchesAHuiNanJuemanUseAction(id, a, cardId);
    internal void ObserveAHuiNanBasicUses(IGameEvent payload) => _resolve().ObserveAHuiNanBasicUses(payload);
    internal void PublishAHuiNan(ProgramSkillFrame f) => _resolve().PublishAHuiNan(f);
    internal bool ResumeAHuiNan(long id) => _resolve().ResumeAHuiNan(id);
    internal bool ReturnAHuiNanAttack() => _resolve().ReturnAHuiNanAttack();
    internal void ReturnAHuiNanUse(CardUseFrame use) => _resolve().ReturnAHuiNanUse(use);
    internal bool ValidAHuiNan(ProgramSkillFrame f) => _resolve().ValidAHuiNan(f);
    internal void ValidateAHuiNanUse(CardUseFrame use) => _resolve().ValidateAHuiNanUse(use);
    internal bool HasAHuiNanCapability { get => _resolve().HasAHuiNanCapability; }
}
