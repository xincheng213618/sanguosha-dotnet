#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentCardEnhancementsEntries
{
    internal ProgramSkillFrame ApplyCardEnhancement(ProgramSkillFrame frame, CurrentCardEnhancement enhancement, IReadOnlyList<int> extraTargets) => _resolve().ApplyCardEnhancement(frame, enhancement, extraTargets);
    internal SkillProgramStepOutcome ApplyProgramCurrentCardEnhancements(ProgramSkillFrame frame, int maximum) => _resolve().ApplyProgramCurrentCardEnhancements(frame, maximum);
    internal void CaptureProgramAdjustedSlashBaseDamage(NativeOwners.CardAttackHandle attack) => _resolve().CaptureProgramAdjustedSlashBaseDamage(attack);
    internal IReadOnlyList<PromptChoice> CardEnhancementChoices(ProgramSkillFrame frame) => _resolve().CardEnhancementChoices(frame);
    internal int[] CurrentCardExtraTargets(ProgramSkillFrame frame) => _resolve().CurrentCardExtraTargets(frame);
    internal CardUseFrame EnhancementCardUse(ProgramSkillFrame frame) => _resolve().EnhancementCardUse(frame);
    internal void FinishProgramCardEnhancements(ProgramSkillFrame frame) => _resolve().FinishProgramCardEnhancements(frame);
    internal bool HasCurrentCardEnhancement(long frameId, CurrentCardEnhancement flag) => _resolve().HasCurrentCardEnhancement(frameId, flag);
    internal bool HasRemainingEnhancedSlashTargets(long frameId) => _resolve().HasRemainingEnhancedSlashTargets(frameId);
    internal void PublishCardEnhancementPrompt(ProgramSkillFrame frame) => _resolve().PublishCardEnhancementPrompt(frame);
    internal void ResolveCardEnhancementChoice(PromptChoice selected) => _resolve().ResolveCardEnhancementChoice(selected);
    internal bool TryContinueEnhancedSlashTargets(NativeOwners.CardAttackHandle attack) => _resolve().TryContinueEnhancedSlashTargets(attack);
}
