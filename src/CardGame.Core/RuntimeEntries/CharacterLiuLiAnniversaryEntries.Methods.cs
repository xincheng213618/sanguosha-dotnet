#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuLiAnniversaryEntries
{
    internal bool AdvanceLiuLiSubtree() => _resolve().AdvanceLiuLiSubtree();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> AnniversaryLiuLiHandLimitContributions(global::CardGame.Core.CharacterState p) => _resolve().AnniversaryLiuLiHandLimitContributions(p);
    internal void AnswerLiuLi(global::CardGame.Core.PromptChoice c) => _resolve().AnswerLiuLi(c);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLiuLi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginLiuLi(supplied, op);
    internal bool CanStartLiuLiDehua(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanStartLiuLiDehua(g);
    internal bool CanStartLiuLiFuli(global::CardGame.Core.CharacterState owner) => _resolve().CanStartLiuLiFuli(owner);
    internal void CompleteLiuLiNative(global::CardGame.Core.ProgramSkillFrame f, bool? hadAttack = default) => _resolve().CompleteLiuLiNative(f, hadAttack);
    internal void CompleteLiuLiReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteLiuLiReveal(f);
    internal bool ContinueLiuLi(long id) => _resolve().ContinueLiuLi(id);
    internal bool DrainLiuLi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainLiuLi(f);
    internal void EnsureLiuLiDamagePool(int owner) => _resolve().EnsureLiuLiDamagePool(owner);
    internal void FinishLiuLi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishLiuLi(f);
    internal bool IsAnniversaryLiuLiHandLimitExempt(int seat, global::CardGame.Core.Card card) => _resolve().IsAnniversaryLiuLiHandLimitExempt(seat, card);
    internal bool IsAnniversaryLiuLiVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsAnniversaryLiuLiVirtualUse(use);
    internal void IssueLiuLiSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueLiuLiSlash(f);
    internal void IssueLiuLiUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueLiuLiUse(f);
    internal bool KeepPaidLiuLi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidLiuLi(f);
    internal global::CardGame.Core.PromptChoice LiuLiAi(global::CardGame.Core.PendingDecision p) => _resolve().LiuLiAi(p);
    internal bool LiuLiCanDiscard(int seat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().LiuLiCanDiscard(seat, cards);
    internal global::CardGame.Core.Card[][] LiuLiCategories(int seat) => _resolve().LiuLiCategories(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LiuLiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuLiChoices(f);
    internal bool LiuLiChosenProof(global::CardGame.Core.AnniversaryLiuLiNameChosenEvent e) => _resolve().LiuLiChosenProof(e);
    internal bool LiuLiDiscardProof(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuLiDiscardProof(f);
    internal bool LiuLiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().LiuLiFirstChild(f, child);
    internal bool LiuLiHandNameForbidden(int owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IEnumerable<int> ids) => _resolve().LiuLiHandNameForbidden(owner, kind, ids);
    internal bool LiuLiMaterialsForbidden(global::CardGame.Core.ProgramCardMaterialQuery q) => _resolve().LiuLiMaterialsForbidden(q);
    internal bool LiuLiNameChosen(int owner, global::CardGame.Core.CardKind kind) => _resolve().LiuLiNameChosen(owner, kind);
    internal global::CardGame.Core.ProgramSkillFrame? LiuLiObserverRoot() => _resolve().LiuLiObserverRoot();
    internal bool LiuLiOriginal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuLiOriginal(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.AnniversaryLiuLiPlan> LiuLiPlans(int owner) => _resolve().LiuLiPlans(owner);
    internal bool LiuLiProjectedUseForbidden(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().LiuLiProjectedUseForbidden(q);
    internal global::CardGame.Core.RuleQueryEvaluation LiuLiRange(global::CardGame.Core.CharacterState p, global::CardGame.Core.RuleQueryEvaluation ordinary) => _resolve().LiuLiRange(p, ordinary);
    internal bool LiuLiRangeActive(global::CardGame.Core.AnniversaryLiuLiRangeIssuedEvent e) => _resolve().LiuLiRangeActive(e);
    internal global::CardGame.Core.CardConversionSource LiuLiSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuLiSource(f);
    internal void PublishLiuLi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishLiuLi(f);
    internal bool ReturnLiuLiAttack() => _resolve().ReturnLiuLiAttack();
    internal void ReturnLiuLiDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnLiuLiDraw(supplied, draw);
    internal bool ReturnLiuLiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLiuLiMovement(f);
    internal void SettleLiuLiName(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleLiuLiName(f);
    internal bool TryReturnAnniversaryLiuLiVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().TryReturnAnniversaryLiuLiVirtualUse(use);
    internal bool ValidLiuLi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidLiuLi(f);
    internal void ValidateLiuLiNativeHandUse(global::CardGame.Core.IGameEvent payload) => _resolve().ValidateLiuLiNativeHandUse(payload);
    internal void ValidateLiuLiSubmittedUse(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().ValidateLiuLiSubmittedUse(q);
}
