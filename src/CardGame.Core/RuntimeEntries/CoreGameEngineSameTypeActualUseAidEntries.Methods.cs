#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSameTypeActualUseAidEntries
{
    internal void AddSameTypeAidTarget(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().AddSameTypeAidTarget(f, target);
    internal void AppendSameTypeAidFinalizedCandidates(long useId, global::CardGame.Core.CardActionContext action, int newTarget) => _resolve().AppendSameTypeAidFinalizedCandidates(useId, action, newTarget);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSameTypeAid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginSameTypeAid(frame);
    internal bool CanAddSameTypeAidTarget(global::CardGame.Core.SameTypeAidIdentity identity, int seat, bool ignoreSlashDistance = false) => _resolve().CanAddSameTypeAidTarget(identity, seat, ignoreSlashDistance);
    internal bool CanRunSameTypeAid(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunSameTypeAid(candidate, trigger, context);
    internal global::CardGame.Core.SameTypeAidIdentity? FreezeSameTypeAidUse(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().FreezeSameTypeAidUse(ownerSeat, context);
    internal bool HasSameTypeAidTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasSameTypeAidTargetTail(use);
    internal bool IsSameTypeAidTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SameTypeAidTargetAddedEvent fact) => _resolve().IsSameTypeAidTargetFact(use, fact);
    internal bool MatchesSameTypeAidLegacyProducer(global::CardGame.Core.CardUseFrame use, long producer) => _resolve().MatchesSameTypeAidLegacyProducer(use, producer);
    internal bool MatchesSameTypeAidUse(global::CardGame.Core.SameTypeAidIdentity identity) => _resolve().MatchesSameTypeAidUse(identity);
    internal void PublishSameTypeAidPrompt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSameTypeAidPrompt(f);
    internal void ResolveSameTypeAidChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveSameTypeAidChoice(selected);
    internal bool ResumeSameTypeAid(long frameId) => _resolve().ResumeSameTypeAid(frameId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SameTypeAidChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SameTypeAidChoices(f);
    internal bool SameTypeAidIssuedSlashIgnoresDistance(global::CardGame.Core.CardUseFrame use) => _resolve().SameTypeAidIssuedSlashIgnoresDistance(use);
    internal long? SameTypeAidLegacyCurrentProducer(global::CardGame.Core.CardUseFrame use, int target) => _resolve().SameTypeAidLegacyCurrentProducer(use, target);
    internal global::System.Collections.Generic.Dictionary<string, string> SameTypeAidParameters(global::CardGame.Core.ProgramSkillFrame f, string option) => _resolve().SameTypeAidParameters(f, option);
    internal int[] SameTypeAidRecipients(global::CardGame.Core.SameTypeAidIdentity identity, string skillId) => _resolve().SameTypeAidRecipients(identity, skillId);
    internal global::CardGame.Core.CardConversionSource SameTypeAidSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SameTypeAidSource(f);
    internal bool SameTypeAidUnpaidSourceValid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SameTypeAidUnpaidSourceValid(f);
    internal global::CardGame.Core.PromptChoice SelectAiSameTypeAid(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiSameTypeAid(decision, f);
    internal bool HasSameTypeActualUseAid { get => _resolve().HasSameTypeActualUseAid; }
}
