#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiMobileEntries
{
    internal bool AllowsMobileOrdinaryQiangxiWeaponTarget(int owner, int weapon, int target) => _resolve().AllowsMobileOrdinaryQiangxiWeaponTarget(owner, weapon, target);
    internal void AssertMobileOrdinaryQiangxi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertMobileOrdinaryQiangxi(frame);
    internal void CaptureMobileOrdinaryQiangxi(global::CardGame.Core.ProgramSkillStartedEvent started) => _resolve().CaptureMobileOrdinaryQiangxi(started);
    internal void CaptureMobileOrdinaryQiangxiPayment(global::CardGame.Core.CardMovedEvent moved) => _resolve().CaptureMobileOrdinaryQiangxiPayment(moved);
    internal bool ContinuePaidMobileOrdinaryQiangxi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinuePaidMobileOrdinaryQiangxi(frame);
    internal bool MobileOrdinaryQiangxiPaid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().MobileOrdinaryQiangxiPaid(frame);
    internal bool MobileOrdinaryQiangxiRange(int owner, int target, int? excludedWeapon, scoped out int distance, scoped out int range, scoped out bool special) => _resolve().MobileOrdinaryQiangxiRange(owner, target, excludedWeapon, out distance, out range, out special);
    internal bool ReturnPaidMobileOrdinaryQiangxi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnPaidMobileOrdinaryQiangxi(frame);
}
