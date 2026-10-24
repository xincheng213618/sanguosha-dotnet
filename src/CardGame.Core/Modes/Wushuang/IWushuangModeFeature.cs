namespace CardGame.Core;

public static class WushuangSkillSources
{
    public const string Shop = "mode:wushuang-shop";
    public const string OriginalReplacement = "mode:wushuang-replaced-original";
}

/// <summary>The optional duel service exposes queries and controlled actions, never mutable player progress.</summary>
public interface IWushuangModeFeature : IGameModeFeature
{
    int ElapsedMilliseconds { get; set; }
    int RemainingMilliseconds { get; }
    IReadOnlyList<int> TacticIds(int seat);
    bool HasPurchasedOffer(int seat, string offerId, int? tacticId = null);
    void ToggleShopLock(int seat);
    WushuangIntermissionFrame? ActiveWushuangWindow(int seat);
    bool HasWushuangIntermissionBoundary();
    WushuangSnapshot? CreateWushuangSnapshot(int viewerSeat);
    bool CanUseWushuangShop(int seat);
    void InitializeWushuangShops(GameStartedEvent started);
    void ProgressWushuangEconomy(TurnStartedEvent started);
    void ProgressWushuangRound(RoundStartedEvent started);
    bool TryBeginWushuangOpening();
    bool TryBeginWushuangShopping(int endedSeat, bool skipped);
    void ContinueWushuangIntermission(long frameId);
    void AnswerWushuangChoice(PromptChoice choice);
    void AdvanceWushuangShopSeat(WushuangIntermissionFrame frame);
    void ChangeWushuangTokens(int seat, int delta, string reason);
    void ChangeWushuangFreeRerolls(int seat, int delta, string reason);
    void RefreshWushuangOffers(int seat, bool opening = false);
    CommandResult SubmitBuyWushuangOffer(BuyWushuangOfferCommand command);
    string? ValidateWushuangPurchase(int seat, WushuangShopOffer? offer, string? replacement);
    void BuyWushuangOffer(int seat, WushuangShopOffer offer, string? replacement, bool opening = false);
    CommandResult SubmitRerollWushuangShop(RerollWushuangShopCommand command);
    CommandResult SubmitUpgradeWushuangShop(UpgradeWushuangShopCommand command);
    CommandResult SubmitFinishWushuangShop(FinishWushuangShopCommand command);
    CommandResult SubmitToggleWushuangShopLock(ToggleWushuangShopLockCommand command);
    void CompleteWushuangShopAction(long frameId);
    bool HasWushuangTactic(int seat, int id);
    bool IsWushuangHandLimitExempt(int seat, CardKind kind);
    IEnumerable<RuleQueryContribution> WushuangNumericRuleContributions(int seat, SkillRuleQuery query);
    RuleQueryEvaluation ApplyWushuangEnemyQuota(int seat, RuleQueryEvaluation ordinary, SkillRuleQuery query);
    int WushuangSourceDamageBonus(WushuangDamageFacts attack);
}
