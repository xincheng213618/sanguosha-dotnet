#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairObtainShownGiftsEntries
{
    internal SkillProgramStepOutcome AwaitPairBenefitChildren(long frameId) => _resolve().AwaitPairBenefitChildren(frameId);
    internal bool CanActivatePairObtain(CharacterState owner, string skill, string instance, SkillProgramActivation activation, IReadOnlyList<int> targets) => _resolve().CanActivatePairObtain(owner, skill, instance, activation, targets);
    internal SkillProgramStepOutcome GiveShownCardToLeastOriginalTarget(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().GiveShownCardToLeastOriginalTarget(supplied, effect);
    internal bool HasPayablePairObtainTarget(int owner, string skill, string instance, int target) => _resolve().HasPayablePairObtainTarget(owner, skill, instance, target);
    internal SkillProgramStepOutcome ObtainOneFromEachSelectedTarget(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ObtainOneFromEachSelectedTarget(supplied, effect);
    internal string PairBenefitMoveReason(ProgramSkillFrame f, bool gift) => _resolve().PairBenefitMoveReason(f, gift);
    internal CardConversionSource PairBenefitSource(ProgramSkillFrame f) => _resolve().PairBenefitSource(f);
    internal bool PairBenefitSourceCurrent(ProgramSkillFrame f) => _resolve().PairBenefitSourceCurrent(f);
    internal IReadOnlyList<PromptChoice> PairObtainChoices(ProgramSkillFrame f, int target, IReadOnlyList<CardZoneKind> zones) => _resolve().PairObtainChoices(f, target, zones);
    internal void PublishPairBenefitPrompt(ProgramSkillFrame f, string message, IReadOnlyList<PromptChoice> choices, int? target) => _resolve().PublishPairBenefitPrompt(f, message, choices, target);
    internal void ResolvePairObtainChoice(PromptChoice choice) => _resolve().ResolvePairObtainChoice(choice);
    internal void ResolveShownPairGiftChoice(PromptChoice choice) => _resolve().ResolveShownPairGiftChoice(choice);
    internal PromptChoice SelectAiPairBenefit(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiPairBenefit(decision, f);
    internal bool TryReturnPairBenefitMovement(ProgramSkillFrame f) => _resolve().TryReturnPairBenefitMovement(f);
    internal long PairBenefitSequence { get => _resolve().PairBenefitSequence; }
}
