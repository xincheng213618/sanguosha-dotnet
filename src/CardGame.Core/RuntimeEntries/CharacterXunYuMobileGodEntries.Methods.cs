#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXunYuMobileGodEntries
{
    internal void AddQizhengNaturalActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddQizhengNaturalActions(actions, actor, playable, selectedPhysicalCardId);
    internal bool AdvanceQizhengAi() => _resolve().AdvanceQizhengAi();
    internal void AnswerDinghanEdit(PromptChoice selected) => _resolve().AnswerDinghanEdit(selected);
    internal void AnswerQizheng(PromptChoice selected) => _resolve().AnswerQizheng(selected);
    internal void AssertQizheng(CardUseFrame use) => _resolve().AssertQizheng(use);
    internal bool CanLingce(int seat, CardActionContext? a) => _resolve().CanLingce(seat, a);
    internal bool CanTriggerShenXunYu(ProgramTriggerLegalityContext gate) => _resolve().CanTriggerShenXunYu(gate);
    internal PromptChoice ChooseDinghanEdit(PendingDecision prompt) => _resolve().ChooseDinghanEdit(prompt);
    internal void ContinueQizheng(long id) => _resolve().ContinueQizheng(id);
    internal IReadOnlyList<PromptChoice> DinghanEditChoices(ProgramSkillFrame f) => _resolve().DinghanEditChoices(f);
    internal HashSet<CardKind> DinghanNames(int seat) => _resolve().DinghanNames(seat);
    internal bool DinghanRequiresFinalizedTrickWindow(ProgramTargetCardWindowQuery query) => _resolve().DinghanRequiresFinalizedTrickWindow(query);
    internal void FinishQizhengWithoutDamage(CardUseFrame supplied) => _resolve().FinishQizhengWithoutDamage(supplied);
    internal bool FinishVirtualQizhengAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishVirtualQizhengAttack(attack, out paused);
    internal bool HasDinghanCancellation(CardUseFrame use) => _resolve().HasDinghanCancellation(use);
    internal bool HasQizhengPrompt() => _resolve().HasQizhengPrompt();
    internal bool IsDinghanCancellationFact(CardUseFrame use, DinghanTargetCanceledEvent e, IReadOnlyList<int> before) => _resolve().IsDinghanCancellationFact(use, e, before);
    internal void PayQizhengSingleResponse(long id, CharacterState responder, Card card, CardKind kind) => _resolve().PayQizhengSingleResponse(id, responder, card, kind);
    internal void PublishDinghanEdit(ProgramSkillFrame f) => _resolve().PublishDinghanEdit(f);
    internal void PublishQizheng(CardUseFrame use) => _resolve().PublishQizheng(use);
    internal IReadOnlyList<PromptChoice> QizhengChoices(CardUseFrame use) => _resolve().QizhengChoices(use);
    internal Dictionary<string, string> QizhengParameters(string branch, string value = "") => _resolve().QizhengParameters(branch, value);
    internal void ResolveQizhengAfterResponse(long id) => _resolve().ResolveQizhengAfterResponse(id);
    internal void ResolveQizhengXiangshengEffect(NullificationWindowFrame pending) => _resolve().ResolveQizhengXiangshengEffect(pending);
    internal SkillProgramStepOutcome ResolveShenXunYu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ResolveShenXunYu(supplied, op);
    internal bool ResumeDinghanEdit(long id) => _resolve().ResumeDinghanEdit(id);
    internal CardActionContext? ShenXunYuAction(ProgramSkillWindowContext? context) => _resolve().ShenXunYuAction(context);
    internal bool TryCompleteQizhengAttack(NativeOwners.CardAttackHandle attack) => _resolve().TryCompleteQizhengAttack(attack);
    internal bool TryContinueQizhengResponse(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().TryContinueQizhengResponse(attack, action, continuation);
    internal bool TryExecuteQizhengNaturalUse(CharacterState actor, Card card, LegalAction action) => _resolve().TryExecuteQizhengNaturalUse(actor, card, action);
    internal bool HasDinghanCapability { get => _resolve().HasDinghanCapability; }
}
