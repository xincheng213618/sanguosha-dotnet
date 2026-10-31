#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetBenefitWindowsEntries
{
    internal bool CanRunSlashTargetBenefitCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunSlashTargetBenefitCandidate(c, context);
    internal void CompleteSlashTargetBenefitBinding(ProgramSkillFrame f) => _resolve().CompleteSlashTargetBenefitBinding(f);
    internal void ContinueSlashAfterTargetBenefitAdditions(NativeOwners.CardAttackHandle attack, bool legacy) => _resolve().ContinueSlashAfterTargetBenefitAdditions(attack, legacy);
    internal void ContinueSlashTargetBenefitWindowCore() => _resolve().ContinueSlashTargetBenefitWindowCore();
    internal bool? DecideIssuedTrigger720(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger720(candidate, context);
    internal ActualUseTargetIdentity? FreezeSlashTargetBenefitUse(NativeOwners.CardAttackHandle attack, int? designatedTarget = default) => _resolve().FreezeSlashTargetBenefitUse(attack, designatedTarget);
    internal bool MatchesSlashTargetBenefitUse(ActualUseTargetIdentity identity) => _resolve().MatchesSlashTargetBenefitUse(identity);
    internal void ReplaceSlashTargetBenefit(SlashTargetBenefitReceipt receipt) => _resolve().ReplaceSlashTargetBenefit(receipt);
    internal long? SlashBenefitLegacyProducer(CardUseFrame use, int target, bool issued = false) => _resolve().SlashBenefitLegacyProducer(use, target, issued);
    internal SlashTargetBenefitReceipt? SlashTargetBenefitCurrent(SlashTargetBenefitReceipt identity) => _resolve().SlashTargetBenefitCurrent(identity);
    internal bool TryBeginDodgeCancelledSlashBenefits(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginDodgeCancelledSlashBenefits(attack);
    internal bool TryBeginSlashTargetBenefits(NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false) => _resolve().TryBeginSlashTargetBenefits(attack, legacy, afterActualTargets);
    internal bool HasSlashTargetBenefitCapability { get => _resolve().HasSlashTargetBenefitCapability; }
}
