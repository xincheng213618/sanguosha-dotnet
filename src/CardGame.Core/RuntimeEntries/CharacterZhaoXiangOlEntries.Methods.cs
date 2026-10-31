#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoXiangOlEntries
{
    internal bool CanContinueZhaoXiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueZhaoXiang(f);
    internal bool CanGainMeiYingFromDamage(int owner, global::CardGame.Core.SkillProgramTrigger trigger, int source, int target, bool sourceLess) => _resolve().CanGainMeiYingFromDamage(owner, trigger, source, target, sourceLess);
    internal bool CanOfferMeiYingConversionDraw(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMeiYingConversionDraw(candidate, trigger, context);
    internal bool CanOfferZhaoXiangExchange(int owner, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().CanOfferZhaoXiangExchange(owner, trigger);
    internal bool CanPayMeiYingConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source) => _resolve().CanPayMeiYingConversion(owner, source);
    internal bool DrainZhaoXiangChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainZhaoXiangChildren(f);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawAfterPaidMeiYingConversion(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().DrawAfterPaidMeiYingConversion(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome ExchangeFiveShuBySpentMeiYing(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ExchangeFiveShuBySpentMeiYing(supplied);
    internal void FinishZhaoXiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishZhaoXiang(f);
    internal void GainMeiYingAfterActualSlashDamage(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().GainMeiYingAfterActualSlashDamage(supplied);
    internal bool IsMeiYingConversion(global::CardGame.Core.CardConversionSource source) => _resolve().IsMeiYingConversion(source);
    internal bool IsZhaoXiangMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsZhaoXiangMovement(f, e, p);
    internal bool MeiYingConversionUnavailable(global::CardGame.Core.ProgramViewAsSourceQuery query) => _resolve().MeiYingConversionUnavailable(query);
    internal void ObserveAcceptedMeiYingConversion(global::CardGame.Core.CardActionAcceptedEvent accepted) => _resolve().ObserveAcceptedMeiYingConversion(accepted);
    internal void PayMeiYingConversion(global::CardGame.Core.CardActionContext action) => _resolve().PayMeiYingConversion(action);
    internal void PublishZhaoXiangExchange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhaoXiangExchange(f);
    internal void ResolveZhaoXiangExchangeChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveZhaoXiangExchangeChoice(choice);
    internal bool ResumeZhaoXiang(long id) => _resolve().ResumeZhaoXiang(id);
    internal bool ReturnZhaoXiangMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhaoXiangMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiZhaoXiangExchange(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiZhaoXiangExchange(p, f);
    internal bool TryAdvanceZhaoXiangSubtree() => _resolve().TryAdvanceZhaoXiangSubtree();
    internal bool ValidZhaoXiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidZhaoXiang(f);
    internal global::CardGame.Core.ZhaoXiangConversionPayment? ZhaoXiangActionPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhaoXiangActionPayment(f);
    internal string[] ZhaoXiangAvailableShuGenerals(int owner) => _resolve().ZhaoXiangAvailableShuGenerals(owner);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame? ZhaoXiangConversionParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhaoXiangConversionParent(f);
    internal int ZhaoXiangCumulativeRemoved(int owner) => _resolve().ZhaoXiangCumulativeRemoved(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhaoXiangExchangeChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhaoXiangExchangeChoices(f);
    internal bool ZhaoXiangFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhaoXiangFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? ZhaoXiangObserverRoot() => _resolve().ZhaoXiangObserverRoot();
    internal global::CardGame.Core.SkillProgramEffect? ZhaoXiangPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhaoXiangPausedEffect(f);
    internal bool ZhaoXiangPreparationParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhaoXiangPreparationParentMatches(f);
    internal bool HasMeiYingConversionCapability { get => _resolve().HasMeiYingConversionCapability; }
}
