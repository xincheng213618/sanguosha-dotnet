#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuLiAnniversaryEntries
{
    internal bool AdvanceLiuLiSubtree() => _resolve().AdvanceLiuLiSubtree();
    internal IEnumerable<RuleQueryContribution> AnniversaryLiuLiHandLimitContributions(CharacterState p) => _resolve().AnniversaryLiuLiHandLimitContributions(p);
    internal void AnswerLiuLi(PromptChoice c) => _resolve().AnswerLiuLi(c);
    internal SkillProgramStepOutcome BeginLiuLi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginLiuLi(supplied, op);
    internal bool CanStartLiuLiDehua(ProgramTriggerLegalityContext g) => _resolve().CanStartLiuLiDehua(g);
    internal bool CanStartLiuLiFuli(CharacterState owner) => _resolve().CanStartLiuLiFuli(owner);
    internal void CompleteLiuLiNative(ProgramSkillFrame f, bool? hadAttack = default) => _resolve().CompleteLiuLiNative(f, hadAttack);
    internal void CompleteLiuLiReveal(ProgramSkillFrame f) => _resolve().CompleteLiuLiReveal(f);
    internal bool ContinueLiuLi(long id) => _resolve().ContinueLiuLi(id);
    internal bool DrainLiuLi(ProgramSkillFrame f) => _resolve().DrainLiuLi(f);
    internal void EnsureLiuLiDamagePool(int owner) => _resolve().EnsureLiuLiDamagePool(owner);
    internal void FinishLiuLi(ProgramSkillFrame f) => _resolve().FinishLiuLi(f);
    internal bool IsAnniversaryLiuLiHandLimitExempt(int seat, Card card) => _resolve().IsAnniversaryLiuLiHandLimitExempt(seat, card);
    internal bool IsAnniversaryLiuLiVirtualUse(CardUseFrame use) => _resolve().IsAnniversaryLiuLiVirtualUse(use);
    internal void IssueLiuLiSlash(ProgramSkillFrame f) => _resolve().IssueLiuLiSlash(f);
    internal void IssueLiuLiUse(ProgramSkillFrame f) => _resolve().IssueLiuLiUse(f);
    internal bool KeepPaidLiuLi(ProgramSkillFrame f) => _resolve().KeepPaidLiuLi(f);
    internal PromptChoice LiuLiAi(PendingDecision p) => _resolve().LiuLiAi(p);
    internal bool LiuLiCanDiscard(int seat, IReadOnlyList<Card> cards) => _resolve().LiuLiCanDiscard(seat, cards);
    internal Card[][] LiuLiCategories(int seat) => _resolve().LiuLiCategories(seat);
    internal IReadOnlyList<PromptChoice> LiuLiChoices(ProgramSkillFrame f) => _resolve().LiuLiChoices(f);
    internal bool LiuLiChosenProof(AnniversaryLiuLiNameChosenEvent e) => _resolve().LiuLiChosenProof(e);
    internal bool LiuLiDiscardProof(ProgramSkillFrame f) => _resolve().LiuLiDiscardProof(f);
    internal bool LiuLiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LiuLiFirstChild(f, child);
    internal bool LiuLiHandNameForbidden(int owner, CardKind kind, IEnumerable<int> ids) => _resolve().LiuLiHandNameForbidden(owner, kind, ids);
    internal bool LiuLiMaterialsForbidden(ProgramCardMaterialQuery q) => _resolve().LiuLiMaterialsForbidden(q);
    internal bool LiuLiNameChosen(int owner, CardKind kind) => _resolve().LiuLiNameChosen(owner, kind);
    internal ProgramSkillFrame? LiuLiObserverRoot() => _resolve().LiuLiObserverRoot();
    internal bool LiuLiOriginal(ProgramSkillFrame f) => _resolve().LiuLiOriginal(f);
    internal IReadOnlyList<AnniversaryLiuLiPlan> LiuLiPlans(int owner) => _resolve().LiuLiPlans(owner);
    internal bool LiuLiProjectedUseForbidden(ProgramLegalActionQuery q) => _resolve().LiuLiProjectedUseForbidden(q);
    internal RuleQueryEvaluation LiuLiRange(CharacterState p, RuleQueryEvaluation ordinary) => _resolve().LiuLiRange(p, ordinary);
    internal bool LiuLiRangeActive(AnniversaryLiuLiRangeIssuedEvent e) => _resolve().LiuLiRangeActive(e);
    internal CardConversionSource LiuLiSource(ProgramSkillFrame f) => _resolve().LiuLiSource(f);
    internal void PublishLiuLi(ProgramSkillFrame f) => _resolve().PublishLiuLi(f);
    internal bool ReturnLiuLiAttack() => _resolve().ReturnLiuLiAttack();
    internal void ReturnLiuLiDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw) => _resolve().ReturnLiuLiDraw(supplied, draw);
    internal bool ReturnLiuLiMovement(ProgramSkillFrame f) => _resolve().ReturnLiuLiMovement(f);
    internal void SettleLiuLiName(ProgramSkillFrame f) => _resolve().SettleLiuLiName(f);
    internal bool TryReturnAnniversaryLiuLiVirtualUse(CardUseFrame use) => _resolve().TryReturnAnniversaryLiuLiVirtualUse(use);
    internal bool ValidLiuLi(ProgramSkillFrame f) => _resolve().ValidLiuLi(f);
    internal void ValidateLiuLiNativeHandUse(IGameEvent payload) => _resolve().ValidateLiuLiNativeHandUse(payload);
    internal void ValidateLiuLiSubmittedUse(ProgramSubmittedUseQuery q) => _resolve().ValidateLiuLiSubmittedUse(q);
}
