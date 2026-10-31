#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangRuntime
{
    internal const string WushuangGrantSource = WushuangSkillSources.Shop;
    internal IWushuangModeFeature? WushuangFeature => _modeRuntimeOperations._modeRuntime.Wushuang;
    internal IWushuangModeFeature RequireWushuangFeature => WushuangFeature ?? throw new InvalidOperationException("The current match has no Wushuang feature.");

    internal bool IsWushuangSkillGrantQualified(SkillGrant grant) => !_coreWushuangModeProjectionOperations.IsWushuangMode || _contentRegistryOperations._contentRegistry.Skills.TryGetValue(grant.SkillId, out var skill) && WushuangCatalogue.IsSkillAllowed(skill);
    internal WushuangIntermissionFrame? ActiveWushuangWindow(int seat) => WushuangFeature?.ActiveWushuangWindow(seat);
    internal bool HasWushuangIntermissionBoundary() => WushuangFeature?.HasWushuangIntermissionBoundary() ?? false;
    internal bool CanUseWushuangShop(int seat) => WushuangFeature?.CanUseWushuangShop(seat) ?? false;
    internal void AdvanceWushuangShopSeat(WushuangIntermissionFrame frame) => RequireWushuangFeature.AdvanceWushuangShopSeat(frame);
    internal void ChangeWushuangTokens(int seat, int delta, string reason) => RequireWushuangFeature.ChangeWushuangTokens(seat, delta, reason);
    internal void RefreshWushuangOffers(int seat, bool opening = false) => RequireWushuangFeature.RefreshWushuangOffers(seat, opening);
    internal CommandResult SubmitBuyWushuangOffer(BuyWushuangOfferCommand command) => WushuangFeature?.SubmitBuyWushuangOffer(command) ?? RejectMissingWushuangFeature();
    internal string? ValidateWushuangPurchase(int seat, WushuangShopOffer? offer, string? replacement) => RequireWushuangFeature.ValidateWushuangPurchase(seat, offer, replacement);
    internal void BuyWushuangOffer(int seat, WushuangShopOffer offer, string? replacement, bool opening = false) => RequireWushuangFeature.BuyWushuangOffer(seat, offer, replacement, opening);
    internal CommandResult SubmitRerollWushuangShop(RerollWushuangShopCommand command) => WushuangFeature?.SubmitRerollWushuangShop(command) ?? RejectMissingWushuangFeature();
    internal CommandResult SubmitUpgradeWushuangShop(UpgradeWushuangShopCommand command) => WushuangFeature?.SubmitUpgradeWushuangShop(command) ?? RejectMissingWushuangFeature();
    internal CommandResult SubmitFinishWushuangShop(FinishWushuangShopCommand command) => WushuangFeature?.SubmitFinishWushuangShop(command) ?? RejectMissingWushuangFeature();
    internal CommandResult SubmitToggleWushuangShopLock(ToggleWushuangShopLockCommand command) => WushuangFeature?.SubmitToggleWushuangShopLock(command) ?? RejectMissingWushuangFeature();
    internal void CompleteWushuangShopAction(long frameId) => RequireWushuangFeature.CompleteWushuangShopAction(frameId);
    internal CommandResult RejectMissingWushuangFeature() => _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "当前场次没有虎符商店。");
}
