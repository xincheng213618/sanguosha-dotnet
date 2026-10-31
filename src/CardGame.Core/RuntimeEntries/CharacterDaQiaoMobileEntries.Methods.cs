#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoMobileEntries
{
    internal bool AllowsPostCostSlashRedirectPayment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardLocation source, global::CardGame.Core.Card card) => _resolve().AllowsPostCostSlashRedirectPayment(frame, source, card);
    internal bool HasPostCostSlashRedirectPayment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeOwners.CardAttackHandle attack, int target) => _resolve().HasPostCostSlashRedirectPayment(frame, attack, target);
    internal bool IsPostCostSlashRedirectPaymentPair(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeOwners.CardAttackHandle attack, int target, global::CardGame.Core.CardLocation source, global::CardGame.Core.Card card) => _resolve().IsPostCostSlashRedirectPaymentPair(frame, attack, target, source, card);
    internal bool IsPostCostSlashRedirectTarget(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int owner, int target, int? excludedEquipment) => _resolve().IsPostCostSlashRedirectTarget(attack, owner, target, excludedEquipment);
    internal global::CardGame.Core.ProgramSkillFrame? PostCostSlashRedirectFrame(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int owner) => _resolve().PostCostSlashRedirectFrame(attack, owner);
    internal bool UsesPostCostSlashRedirectRange(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().UsesPostCostSlashRedirectRange(frame);
}
