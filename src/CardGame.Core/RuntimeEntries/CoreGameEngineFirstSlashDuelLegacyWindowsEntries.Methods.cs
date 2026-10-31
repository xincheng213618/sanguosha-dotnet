#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFirstSlashDuelLegacyWindowsEntries
{
    internal void AssertFirstSlashDuelLegacyWindows() => _resolve().AssertFirstSlashDuelLegacyWindows();
    internal bool CanRunFirstLegacyBenefit(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunFirstLegacyBenefit(c, context);
    internal void CompleteFirstLegacyBenefitCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CompleteFirstLegacyBenefitCandidate(c, context);
    internal void ContinueFirstSlashDuelLegacyWindow() => _resolve().ContinueFirstSlashDuelLegacyWindow();
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame DeferFirstSlashDuelCandidates(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().DeferFirstSlashDuelCandidates(frame);
    internal long? FirstSlashDuelLegacyProducer(global::CardGame.Core.CardUseFrame use) => _resolve().FirstSlashDuelLegacyProducer(use);
    internal bool HasFirstSlashDuelLegacyUseObserver(long id) => _resolve().HasFirstSlashDuelLegacyUseObserver(id);
    internal bool IsFirstLegacySlashUse(global::CardGame.Core.CardUseFrame use, long producer, bool requireSingle = true) => _resolve().IsFirstLegacySlashUse(use, producer, requireSingle);
    internal bool? ProveFirstSlashLegacy(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveFirstSlashLegacy(candidate, context);
    internal bool TryBeginFirstSlashDuelLegacyWindow(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginFirstSlashDuelLegacyWindow(attack);
}
