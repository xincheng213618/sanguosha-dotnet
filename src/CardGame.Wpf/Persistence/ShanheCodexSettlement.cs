using System.IO;
using CardGame.Core.Campaigns;

namespace CardGame.Wpf.Persistence;

/// <summary>Prepares one account credit from a trusted, fully completed campaign session.</summary>
public static class ShanheCodexSettlement
{
    // The verified normal-1 sample is ten books. All three local difficulty
    // tiers use this same local completion floor, not an inferred official table.
    public const int NormalOneCompletionBooks = 10;
    public const int MaximumRunRecords = 100;

    /// <summary>The confirmed award before checking whether this account has already claimed it.</summary>
    public static int ConfirmedBookAmount(ShanheCampaignSession session, int difficulty)
    {
        ArgumentNullException.ThrowIfNull(session);
        return CanSettle(session, difficulty) && session.Snapshot.Phase == ShanheCampaignPhase.Won
            ? NormalOneCompletionBooks : 0;
    }

    public static bool CanSettle(ShanheCampaignSession session, int difficulty) =>
        difficulty is >= 1 and <= 3 && session.Snapshot.Phase is ShanheCampaignPhase.Won or ShanheCampaignPhase.Lost &&
        GetSettlementKey(session) is not null;

    public static string? GetSettlementKey(ShanheCampaignSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        var options = session.CreateCheckpoint().Options;
        return options.UseCodexRewardPool && Guid.TryParse(options.ExpeditionId, out var expeditionId) && expeditionId != Guid.Empty
            ? "expedition:" + expeditionId.ToString("N") : null;
    }

    public static bool HasClaimed(ShanheCodexProfile profile, ShanheCampaignSession session)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return GetSettlementKey(session) is { } claimKey && profile.ClaimedRewards?.Contains(claimKey, StringComparer.Ordinal) == true;
    }

    public static int EligibleBookAmount(ShanheCodexProfile profile, ShanheCampaignSession session, int difficulty)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(session);
        if (profile.Books > int.MaxValue - NormalOneCompletionBooks ||
            profile.ClaimedRewards is not { Length: < 20000 } || HasClaimed(profile, session)) return 0;
        try { FileShanheCodexStore.Validate(profile); }
        catch (InvalidDataException) { return 0; }
        return ConfirmedBookAmount(session, difficulty);
    }

    public static bool TryPrepare(ShanheCodexProfile profile, ShanheCampaignSession session, int difficulty,
        out ShanheCodexProfile next, string? playerGeneralName = null)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(session);
        next = profile;
        if (!CanSettle(session, difficulty) || HasClaimed(profile, session) ||
            profile.ClaimedRewards is not { Length: < 20000 } || GetSettlementKey(session) is not { } claimKey) return false;
        try { FileShanheCodexStore.Validate(profile); }
        catch (InvalidDataException) { return false; }
        var amount = ConfirmedBookAmount(session, difficulty);
        if (profile.Books > int.MaxValue - amount) return false;
        var state = session.Snapshot;
        var record = new ShanheRunRecord(claimKey, state.CampaignId, state.CampaignName, state.PlayerGeneralId,
            string.IsNullOrWhiteSpace(playerGeneralName) ? state.PlayerGeneralId : playerGeneralName,
            difficulty, state.Phase == ShanheCampaignPhase.Won, amount, state.CompletedNodeIds.Count,
            state.EncounterOrdinal, DateTimeOffset.UtcNow, state.EndingText);
        next = profile with
        {
            Books = checked(profile.Books + amount),
            ClaimedRewards = [.. profile.ClaimedRewards, claimKey],
            RunRecords = [record, .. profile.RunRecords.Take(MaximumRunRecords - 1)],
            UnlockedRewardIds = [.. profile.UnlockedRewardIds],
            EquippedTacticIds = [.. profile.EquippedTacticIds],
            LoadoutPlans = profile.LoadoutPlans?.Select(plan => plan with { EquippedTacticIds = [.. plan.EquippedTacticIds] }).ToArray()
        };
        return true;
    }

    internal static void ValidateRecords(ShanheCodexProfile profile)
    {
        if (profile.RunRecords is not { Length: <= MaximumRunRecords } records ||
            records.Any(r => r is null || string.IsNullOrWhiteSpace(r.ExpeditionKey) ||
                !profile.ClaimedRewards.Contains(r.ExpeditionKey, StringComparer.Ordinal) ||
                string.IsNullOrWhiteSpace(r.CampaignId) || string.IsNullOrWhiteSpace(r.CampaignName) ||
                string.IsNullOrWhiteSpace(r.PlayerGeneralId) || string.IsNullOrWhiteSpace(r.PlayerGeneralName) ||
                r.CampaignId.Length > 256 || r.CampaignName.Length > 256 || r.PlayerGeneralId.Length > 256 ||
                r.PlayerGeneralName.Length > 256 || r.Difficulty is < 1 or > 3 ||
                r.BooksEarned != (r.Won ? NormalOneCompletionBooks : 0) || r.CompletedNodes < 0 || r.Battles < 0 ||
                r.CompletedAtUtc == default || r.EndingText is null || r.EndingText.Length > 4000) ||
            records.Select(r => r.ExpeditionKey).Distinct(StringComparer.Ordinal).Count() != records.Length)
            throw new InvalidDataException("山河征程记录无效，原文件已保留。");
    }
}
