#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoXiangOlEntries
{
    internal bool CanContinueZhaoXiang(ProgramSkillFrame f) => _resolve().CanContinueZhaoXiang(f);
    internal bool CanGainMeiYingFromDamage(int owner, SkillProgramTrigger trigger, int source, int target, bool sourceLess) => _resolve().CanGainMeiYingFromDamage(owner, trigger, source, target, sourceLess);
    internal bool CanOfferMeiYingConversionDraw(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferMeiYingConversionDraw(candidate, trigger, context);
    internal bool CanOfferZhaoXiangExchange(int owner, SkillProgramTrigger trigger) => _resolve().CanOfferZhaoXiangExchange(owner, trigger);
    internal bool CanPayMeiYingConversion(CharacterState owner, CardConversionSource source) => _resolve().CanPayMeiYingConversion(owner, source);
    internal bool DrainZhaoXiangChildren(ProgramSkillFrame f) => _resolve().DrainZhaoXiangChildren(f);
    internal SkillProgramStepOutcome DrawAfterPaidMeiYingConversion(ProgramSkillFrame supplied) => _resolve().DrawAfterPaidMeiYingConversion(supplied);
    internal SkillProgramStepOutcome ExchangeFiveShuBySpentMeiYing(ProgramSkillFrame supplied) => _resolve().ExchangeFiveShuBySpentMeiYing(supplied);
    internal void FinishZhaoXiang(ProgramSkillFrame f) => _resolve().FinishZhaoXiang(f);
    internal void GainMeiYingAfterActualSlashDamage(ProgramSkillFrame supplied) => _resolve().GainMeiYingAfterActualSlashDamage(supplied);
    internal bool IsMeiYingConversion(CardConversionSource source) => _resolve().IsMeiYingConversion(source);
    internal bool IsZhaoXiangMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsZhaoXiangMovement(f, e, p);
    internal bool MeiYingConversionUnavailable(ProgramViewAsSourceQuery query) => _resolve().MeiYingConversionUnavailable(query);
    internal void ObserveAcceptedMeiYingConversion(CardActionAcceptedEvent accepted) => _resolve().ObserveAcceptedMeiYingConversion(accepted);
    internal void PayMeiYingConversion(CardActionContext action) => _resolve().PayMeiYingConversion(action);
    internal void PublishZhaoXiangExchange(ProgramSkillFrame f) => _resolve().PublishZhaoXiangExchange(f);
    internal void ResolveZhaoXiangExchangeChoice(PromptChoice choice) => _resolve().ResolveZhaoXiangExchangeChoice(choice);
    internal bool ResumeZhaoXiang(long id) => _resolve().ResumeZhaoXiang(id);
    internal bool ReturnZhaoXiangMovement(ProgramSkillFrame f) => _resolve().ReturnZhaoXiangMovement(f);
    internal PromptChoice SelectAiZhaoXiangExchange(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiZhaoXiangExchange(p, f);
    internal bool TryAdvanceZhaoXiangSubtree() => _resolve().TryAdvanceZhaoXiangSubtree();
    internal bool ValidZhaoXiang(ProgramSkillFrame f) => _resolve().ValidZhaoXiang(f);
    internal ZhaoXiangConversionPayment? ZhaoXiangActionPayment(ProgramSkillFrame f) => _resolve().ZhaoXiangActionPayment(f);
    internal string[] ZhaoXiangAvailableShuGenerals(int owner) => _resolve().ZhaoXiangAvailableShuGenerals(owner);
    internal ProgramCardTriggerWindowFrame? ZhaoXiangConversionParent(ProgramSkillFrame f) => _resolve().ZhaoXiangConversionParent(f);
    internal int ZhaoXiangCumulativeRemoved(int owner) => _resolve().ZhaoXiangCumulativeRemoved(owner);
    internal IReadOnlyList<PromptChoice> ZhaoXiangExchangeChoices(ProgramSkillFrame f) => _resolve().ZhaoXiangExchangeChoices(f);
    internal bool ZhaoXiangFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhaoXiangFirstChild(f, child);
    internal ProgramSkillFrame? ZhaoXiangObserverRoot() => _resolve().ZhaoXiangObserverRoot();
    internal SkillProgramEffect? ZhaoXiangPausedEffect(ProgramSkillFrame f) => _resolve().ZhaoXiangPausedEffect(f);
    internal bool ZhaoXiangPreparationParentMatches(ProgramSkillFrame f) => _resolve().ZhaoXiangPreparationParentMatches(f);
    internal bool HasMeiYingConversionCapability { get => _resolve().HasMeiYingConversionCapability; }
}
