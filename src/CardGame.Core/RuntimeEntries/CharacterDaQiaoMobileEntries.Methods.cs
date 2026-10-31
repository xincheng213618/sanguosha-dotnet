#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoMobileEntries
{
    internal bool AllowsPostCostSlashRedirectPayment(ProgramSkillFrame frame, CardLocation source, Card card) => _resolve().AllowsPostCostSlashRedirectPayment(frame, source, card);
    internal bool HasPostCostSlashRedirectPayment(ProgramSkillFrame frame, NativeOwners.CardAttackHandle attack, int target) => _resolve().HasPostCostSlashRedirectPayment(frame, attack, target);
    internal bool IsPostCostSlashRedirectPaymentPair(ProgramSkillFrame frame, NativeOwners.CardAttackHandle attack, int target, CardLocation source, Card card) => _resolve().IsPostCostSlashRedirectPaymentPair(frame, attack, target, source, card);
    internal bool IsPostCostSlashRedirectTarget(NativeOwners.CardAttackHandle attack, int owner, int target, int? excludedEquipment) => _resolve().IsPostCostSlashRedirectTarget(attack, owner, target, excludedEquipment);
    internal ProgramSkillFrame? PostCostSlashRedirectFrame(NativeOwners.CardAttackHandle attack, int owner) => _resolve().PostCostSlashRedirectFrame(attack, owner);
    internal bool UsesPostCostSlashRedirectRange(ProgramSkillFrame frame) => _resolve().UsesPostCostSlashRedirectRange(frame);
}
