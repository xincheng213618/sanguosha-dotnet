#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFirstSlashDuelLegacyWindowsEntries
{
    internal bool CanRunFirstLegacyBenefit(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunFirstLegacyBenefit(c, context);
    internal void CompleteFirstLegacyBenefitCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CompleteFirstLegacyBenefitCandidate(c, context);
    internal void ContinueFirstSlashDuelLegacyWindow() => _resolve().ContinueFirstSlashDuelLegacyWindow();
    internal ProgramCardTriggerWindowFrame DeferFirstSlashDuelCandidates(ProgramCardTriggerWindowFrame frame) => _resolve().DeferFirstSlashDuelCandidates(frame);
    internal long? FirstSlashDuelLegacyProducer(CardUseFrame use) => _resolve().FirstSlashDuelLegacyProducer(use);
    internal bool IsFirstLegacySlashUse(CardUseFrame use, long producer, bool requireSingle = true) => _resolve().IsFirstLegacySlashUse(use, producer, requireSingle);
    internal bool? ProveFirstSlashLegacy(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveFirstSlashLegacy(candidate, context);
    internal bool TryBeginFirstSlashDuelLegacyWindow(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginFirstSlashDuelLegacyWindow(attack);
}
