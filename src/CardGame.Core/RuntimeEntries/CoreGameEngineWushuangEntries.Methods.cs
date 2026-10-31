#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangEntries
{
    internal global::CardGame.Core.WushuangIntermissionFrame? ActiveWushuangWindow(int seat) => _resolve().ActiveWushuangWindow(seat);
    internal void AdvanceWushuangShopSeat(global::CardGame.Core.WushuangIntermissionFrame frame) => _resolve().AdvanceWushuangShopSeat(frame);
    internal void BuyWushuangOffer(int seat, global::CardGame.Core.WushuangShopOffer offer, string? replacement, bool opening = false) => _resolve().BuyWushuangOffer(seat, offer, replacement, opening);
    internal bool CanUseWushuangShop(int seat) => _resolve().CanUseWushuangShop(seat);
    internal void ChangeWushuangTokens(int seat, int delta, string reason) => _resolve().ChangeWushuangTokens(seat, delta, reason);
    internal void CompleteWushuangShopAction(long frameId) => _resolve().CompleteWushuangShopAction(frameId);
    internal bool HasWushuangIntermissionBoundary() => _resolve().HasWushuangIntermissionBoundary();
    internal bool IsWushuangSkillGrantQualified(global::CardGame.Core.SkillGrant grant) => _resolve().IsWushuangSkillGrantQualified(grant);
    internal void RefreshWushuangOffers(int seat, bool opening = false) => _resolve().RefreshWushuangOffers(seat, opening);
    internal global::CardGame.Core.CommandResult RejectMissingWushuangFeature() => _resolve().RejectMissingWushuangFeature();
    internal global::CardGame.Core.CommandResult SubmitBuyWushuangOffer(global::CardGame.Core.BuyWushuangOfferCommand command) => _resolve().SubmitBuyWushuangOffer(command);
    internal global::CardGame.Core.CommandResult SubmitFinishWushuangShop(global::CardGame.Core.FinishWushuangShopCommand command) => _resolve().SubmitFinishWushuangShop(command);
    internal global::CardGame.Core.CommandResult SubmitRerollWushuangShop(global::CardGame.Core.RerollWushuangShopCommand command) => _resolve().SubmitRerollWushuangShop(command);
    internal global::CardGame.Core.CommandResult SubmitToggleWushuangShopLock(global::CardGame.Core.ToggleWushuangShopLockCommand command) => _resolve().SubmitToggleWushuangShopLock(command);
    internal global::CardGame.Core.CommandResult SubmitUpgradeWushuangShop(global::CardGame.Core.UpgradeWushuangShopCommand command) => _resolve().SubmitUpgradeWushuangShop(command);
    internal string? ValidateWushuangPurchase(int seat, global::CardGame.Core.WushuangShopOffer? offer, string? replacement) => _resolve().ValidateWushuangPurchase(seat, offer, replacement);
    internal global::CardGame.Core.IWushuangModeFeature RequireWushuangFeature { get => _resolve().RequireWushuangFeature; }
    internal global::CardGame.Core.IWushuangModeFeature? WushuangFeature { get => _resolve().WushuangFeature; }
}
