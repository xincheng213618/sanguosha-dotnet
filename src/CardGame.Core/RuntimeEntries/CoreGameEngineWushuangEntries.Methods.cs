#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangEntries
{
    internal WushuangIntermissionFrame? ActiveWushuangWindow(int seat) => _resolve().ActiveWushuangWindow(seat);
    internal void AdvanceWushuangShopSeat(WushuangIntermissionFrame frame) => _resolve().AdvanceWushuangShopSeat(frame);
    internal void BuyWushuangOffer(int seat, WushuangShopOffer offer, string? replacement, bool opening = false) => _resolve().BuyWushuangOffer(seat, offer, replacement, opening);
    internal bool CanUseWushuangShop(int seat) => _resolve().CanUseWushuangShop(seat);
    internal void ChangeWushuangTokens(int seat, int delta, string reason) => _resolve().ChangeWushuangTokens(seat, delta, reason);
    internal void CompleteWushuangShopAction(long frameId) => _resolve().CompleteWushuangShopAction(frameId);
    internal bool IsWushuangSkillGrantQualified(SkillGrant grant) => _resolve().IsWushuangSkillGrantQualified(grant);
    internal void RefreshWushuangOffers(int seat, bool opening = false) => _resolve().RefreshWushuangOffers(seat, opening);
    internal CommandResult RejectMissingWushuangFeature() => _resolve().RejectMissingWushuangFeature();
    internal CommandResult SubmitBuyWushuangOffer(BuyWushuangOfferCommand command) => _resolve().SubmitBuyWushuangOffer(command);
    internal CommandResult SubmitFinishWushuangShop(FinishWushuangShopCommand command) => _resolve().SubmitFinishWushuangShop(command);
    internal CommandResult SubmitRerollWushuangShop(RerollWushuangShopCommand command) => _resolve().SubmitRerollWushuangShop(command);
    internal CommandResult SubmitToggleWushuangShopLock(ToggleWushuangShopLockCommand command) => _resolve().SubmitToggleWushuangShopLock(command);
    internal CommandResult SubmitUpgradeWushuangShop(UpgradeWushuangShopCommand command) => _resolve().SubmitUpgradeWushuangShop(command);
    internal string? ValidateWushuangPurchase(int seat, WushuangShopOffer? offer, string? replacement) => _resolve().ValidateWushuangPurchase(seat, offer, replacement);
    internal IWushuangModeFeature RequireWushuangFeature { get => _resolve().RequireWushuangFeature; }
    internal IWushuangModeFeature? WushuangFeature { get => _resolve().WushuangFeature; }
}
