#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentCardEnhancementsEntries
{
    internal global::CardGame.Core.ProgramSkillFrame ApplyCardEnhancement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CurrentCardEnhancement enhancement, global::System.Collections.Generic.IReadOnlyList<int> extraTargets) => _resolve().ApplyCardEnhancement(frame, enhancement, extraTargets);
    internal global::CardGame.Core.SkillProgramStepOutcome ApplyProgramCurrentCardEnhancements(global::CardGame.Core.ProgramSkillFrame frame, int maximum) => _resolve().ApplyProgramCurrentCardEnhancements(frame, maximum);
    internal void AssertCurrentCardEnhancementDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertCurrentCardEnhancementDraft(frame, paused);
    internal void CaptureProgramAdjustedSlashBaseDamage(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CaptureProgramAdjustedSlashBaseDamage(attack);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CardEnhancementChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CardEnhancementChoices(frame);
    internal int[] CurrentCardExtraTargets(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CurrentCardExtraTargets(frame);
    internal global::CardGame.Core.CardUseFrame EnhancementCardUse(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().EnhancementCardUse(frame);
    internal void FinishProgramCardEnhancements(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishProgramCardEnhancements(frame);
    internal bool HasCurrentCardEnhancement(long frameId, global::CardGame.Core.CurrentCardEnhancement flag) => _resolve().HasCurrentCardEnhancement(frameId, flag);
    internal bool HasRemainingEnhancedSlashTargets(long frameId) => _resolve().HasRemainingEnhancedSlashTargets(frameId);
    internal void PublishCardEnhancementPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishCardEnhancementPrompt(frame);
    internal void ResolveCardEnhancementChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveCardEnhancementChoice(selected);
    internal bool TryContinueEnhancedSlashTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryContinueEnhancedSlashTargets(attack);
}
