#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairObtainShownGiftsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome AwaitPairBenefitChildren(long frameId) => _resolve().AwaitPairBenefitChildren(frameId);
    internal bool CanActivatePairObtain(global::CardGame.Core.CharacterState owner, string skill, string instance, global::CardGame.Core.SkillProgramActivation activation, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().CanActivatePairObtain(owner, skill, instance, activation, targets);
    internal global::CardGame.Core.SkillProgramStepOutcome GiveShownCardToLeastOriginalTarget(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().GiveShownCardToLeastOriginalTarget(supplied, effect);
    internal bool HasPayablePairObtainTarget(int owner, string skill, string instance, int target) => _resolve().HasPayablePairObtainTarget(owner, skill, instance, target);
    internal global::CardGame.Core.SkillProgramStepOutcome ObtainOneFromEachSelectedTarget(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ObtainOneFromEachSelectedTarget(supplied, effect);
    internal string PairBenefitMoveReason(global::CardGame.Core.ProgramSkillFrame f, bool gift) => _resolve().PairBenefitMoveReason(f, gift);
    internal global::CardGame.Core.CardConversionSource PairBenefitSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PairBenefitSource(f);
    internal bool PairBenefitSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PairBenefitSourceCurrent(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PairObtainChoices(global::CardGame.Core.ProgramSkillFrame f, int target, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones) => _resolve().PairObtainChoices(f, target, zones);
    internal void PublishPairBenefitPrompt(global::CardGame.Core.ProgramSkillFrame f, string message, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, int? target) => _resolve().PublishPairBenefitPrompt(f, message, choices, target);
    internal void ResolvePairObtainChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePairObtainChoice(choice);
    internal void ResolveShownPairGiftChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveShownPairGiftChoice(choice);
    internal global::CardGame.Core.PromptChoice SelectAiPairBenefit(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiPairBenefit(decision, f);
    internal bool TryReturnPairBenefitMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TryReturnPairBenefitMovement(f);
    internal long PairBenefitSequence { get => _resolve().PairBenefitSequence; }
}
