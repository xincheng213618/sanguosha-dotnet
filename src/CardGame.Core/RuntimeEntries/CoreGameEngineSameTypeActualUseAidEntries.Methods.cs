#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSameTypeActualUseAidEntries
{
    internal void AddSameTypeAidTarget(ProgramSkillFrame f, int target) => _resolve().AddSameTypeAidTarget(f, target);
    internal void AppendSameTypeAidFinalizedCandidates(long useId, CardActionContext action, int newTarget) => _resolve().AppendSameTypeAidFinalizedCandidates(useId, action, newTarget);
    internal SkillProgramStepOutcome BeginSameTypeAid(ProgramSkillFrame frame) => _resolve().BeginSameTypeAid(frame);
    internal bool CanAddSameTypeAidTarget(SameTypeAidIdentity identity, int seat, bool ignoreSlashDistance = false) => _resolve().CanAddSameTypeAidTarget(identity, seat, ignoreSlashDistance);
    internal bool CanRunSameTypeAid(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunSameTypeAid(candidate, trigger, context);
    internal SameTypeAidIdentity? FreezeSameTypeAidUse(int ownerSeat, ProgramSkillWindowContext context) => _resolve().FreezeSameTypeAidUse(ownerSeat, context);
    internal bool HasSameTypeAidTargetTail(CardUseFrame use) => _resolve().HasSameTypeAidTargetTail(use);
    internal bool IsSameTypeAidTargetFact(CardUseFrame use, SameTypeAidTargetAddedEvent fact) => _resolve().IsSameTypeAidTargetFact(use, fact);
    internal bool MatchesSameTypeAidLegacyProducer(CardUseFrame use, long producer) => _resolve().MatchesSameTypeAidLegacyProducer(use, producer);
    internal bool MatchesSameTypeAidUse(SameTypeAidIdentity identity) => _resolve().MatchesSameTypeAidUse(identity);
    internal void PublishSameTypeAidPrompt(ProgramSkillFrame f) => _resolve().PublishSameTypeAidPrompt(f);
    internal void ResolveSameTypeAidChoice(PromptChoice selected) => _resolve().ResolveSameTypeAidChoice(selected);
    internal bool ResumeSameTypeAid(long frameId) => _resolve().ResumeSameTypeAid(frameId);
    internal IReadOnlyList<PromptChoice> SameTypeAidChoices(ProgramSkillFrame f) => _resolve().SameTypeAidChoices(f);
    internal bool SameTypeAidIssuedSlashIgnoresDistance(CardUseFrame use) => _resolve().SameTypeAidIssuedSlashIgnoresDistance(use);
    internal long? SameTypeAidLegacyCurrentProducer(CardUseFrame use, int target) => _resolve().SameTypeAidLegacyCurrentProducer(use, target);
    internal Dictionary<string, string> SameTypeAidParameters(ProgramSkillFrame f, string option) => _resolve().SameTypeAidParameters(f, option);
    internal int[] SameTypeAidRecipients(SameTypeAidIdentity identity, string skillId) => _resolve().SameTypeAidRecipients(identity, skillId);
    internal CardConversionSource SameTypeAidSource(ProgramSkillFrame f) => _resolve().SameTypeAidSource(f);
    internal bool SameTypeAidUnpaidSourceValid(ProgramSkillFrame f) => _resolve().SameTypeAidUnpaidSourceValid(f);
    internal PromptChoice SelectAiSameTypeAid(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiSameTypeAid(decision, f);
    internal bool HasSameTypeActualUseAid { get => _resolve().HasSameTypeActualUseAid; }
}
