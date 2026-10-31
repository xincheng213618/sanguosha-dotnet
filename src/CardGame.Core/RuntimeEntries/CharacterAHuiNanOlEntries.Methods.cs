#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterAHuiNanOlEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AHuiNanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AHuiNanChoices(f);
    internal bool AHuiNanEndingParent(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.DeferredTurnEndFrame? parent) => _resolve().AHuiNanEndingParent(c, context, out parent);
    internal bool AHuiNanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().AHuiNanFirstChild(f, child);
    internal global::CardGame.Core.AHuiNanBasicUseSnapshot[] AHuiNanHistory(int turn) => _resolve().AHuiNanHistory(turn);
    internal bool AHuiNanLegacyBasicUse(global::CardGame.Core.CardUseFrame use) => _resolve().AHuiNanLegacyBasicUse(use);
    internal global::CardGame.Core.ProgramSkillFrame? AHuiNanObserverRoot() => _resolve().AHuiNanObserverRoot();
    internal bool AHuiNanStartedBinding(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AHuiNanStartedBinding(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<int>> AHuiNanUseTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AHuiNanUseTargets(f);
    internal bool AdvanceAHuiNanChildren() => _resolve().AdvanceAHuiNanChildren();
    internal global::CardGame.Core.PromptChoice AiAHuiNan(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiAHuiNan(p, f);
    internal void AnswerAHuiNan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerAHuiNan(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAHuiNan(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginAHuiNan(input);
    internal void BeginAHuiNanJuemanUse(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.AHuiNanJuemanUseOrigin o, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().BeginAHuiNanJuemanUse(f, o, targets);
    internal void ContinueAHuiNan(long id) => _resolve().ContinueAHuiNan(id);
    internal bool ContinueAHuiNanJuemanSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueAHuiNanJuemanSimpleUse(id, continuation);
    internal bool ContinuePaidAHuiNan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidAHuiNan(f);
    internal bool DrainAHuiNan(long id) => _resolve().DrainAHuiNan(id);
    internal void ExpireAHuiNanJuemanTurnEndWine() => _resolve().ExpireAHuiNanJuemanTurnEndWine();
    internal void FinishAHuiNan(global::CardGame.Core.ProgramSkillFrame f, string outcome) => _resolve().FinishAHuiNan(f, outcome);
    internal bool FinishAHuiNanJuemanAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishAHuiNanJuemanAttack(attack, out paused);
    internal bool HasIssuedAHuiNanJuemanUse(long id) => _resolve().HasIssuedAHuiNanJuemanUse(id);
    internal bool IsAHuiNanJuemanEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsAHuiNanJuemanEndingCandidate(c, context);
    internal bool IsAHuiNanJuemanSlashQuery(global::CardGame.Core.CharacterState actor) => _resolve().IsAHuiNanJuemanSlashQuery(actor);
    internal bool IsAHuiNanJuemanUse(long id) => _resolve().IsAHuiNanJuemanUse(id);
    internal bool MatchesAHuiNanJueman(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.AHuiNanJuemanUseOrigin o) => _resolve().MatchesAHuiNanJueman(f, o);
    internal bool MatchesAHuiNanJuemanUseAction(long id, global::CardGame.Core.CardActionContext a, int cardId) => _resolve().MatchesAHuiNanJuemanUseAction(id, a, cardId);
    internal void ObserveAHuiNanBasicUses(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveAHuiNanBasicUses(payload);
    internal void PublishAHuiNan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishAHuiNan(f);
    internal bool ResumeAHuiNan(long id) => _resolve().ResumeAHuiNan(id);
    internal bool ReturnAHuiNanAttack() => _resolve().ReturnAHuiNanAttack();
    internal void ReturnAHuiNanUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnAHuiNanUse(use);
    internal bool ValidAHuiNan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidAHuiNan(f);
    internal void ValidateAHuiNanUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateAHuiNanUse(use);
    internal bool HasAHuiNanCapability { get => _resolve().HasAHuiNanCapability; }
}
