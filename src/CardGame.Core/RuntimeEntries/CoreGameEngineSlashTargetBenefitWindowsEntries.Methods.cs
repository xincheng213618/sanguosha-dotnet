#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetBenefitWindowsEntries
{
    internal bool CanRunSlashTargetBenefitCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunSlashTargetBenefitCandidate(c, context);
    internal void CompleteSlashTargetBenefitBinding(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteSlashTargetBenefitBinding(f);
    internal void ContinueSlashAfterTargetBenefitAdditions(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy) => _resolve().ContinueSlashAfterTargetBenefitAdditions(attack, legacy);
    internal void ContinueSlashTargetBenefitWindowCore() => _resolve().ContinueSlashTargetBenefitWindowCore();
    internal bool? DecideIssuedTrigger720(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger720(candidate, context);
    internal global::CardGame.Core.ActualUseTargetIdentity? FreezeSlashTargetBenefitUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int? designatedTarget = default) => _resolve().FreezeSlashTargetBenefitUse(attack, designatedTarget);
    internal bool MatchesSlashTargetBenefitUse(global::CardGame.Core.ActualUseTargetIdentity identity) => _resolve().MatchesSlashTargetBenefitUse(identity);
    internal void ReplaceSlashTargetBenefit(global::CardGame.Core.SlashTargetBenefitReceipt receipt) => _resolve().ReplaceSlashTargetBenefit(receipt);
    internal long? SlashBenefitLegacyProducer(global::CardGame.Core.CardUseFrame use, int target, bool issued = false) => _resolve().SlashBenefitLegacyProducer(use, target, issued);
    internal global::CardGame.Core.SlashTargetBenefitReceipt? SlashTargetBenefitCurrent(global::CardGame.Core.SlashTargetBenefitReceipt identity) => _resolve().SlashTargetBenefitCurrent(identity);
    internal bool TryBeginDodgeCancelledSlashBenefits(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginDodgeCancelledSlashBenefits(attack);
    internal bool TryBeginSlashTargetBenefits(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false) => _resolve().TryBeginSlashTargetBenefits(attack, legacy, afterActualTargets);
    internal bool HasSlashTargetBenefitCapability { get => _resolve().HasSlashTargetBenefitCapability; }
}
