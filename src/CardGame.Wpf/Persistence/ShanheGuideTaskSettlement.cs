using System.IO;
using CardGame.Core.Campaigns;

namespace CardGame.Wpf.Persistence;

/// <summary>Prepares task progress, books and permanent claims for the host's single atomic profile write.</summary>
public static class ShanheGuideTaskSettlement
{
    private const string ClaimPrefix = "guide-task:";

    public static string ClaimKey(int sourceTaskId) => ClaimPrefix + sourceTaskId.ToString(System.Globalization.CultureInfo.InvariantCulture);

    public static bool HasClaimed(ShanheCodexProfile profile, int sourceTaskId) =>
        profile.ClaimedRewards.Contains(ClaimKey(sourceTaskId), StringComparer.Ordinal);

    /// <summary>Prepares every pending part or none; the caller writes the returned account exactly once.</summary>
    public static bool TryPrepareCombined(ShanheCodexProfile profile, ShanheCampaignSession session,
        int localDifficulty, IEnumerable<int> supportedSourceDifficultyIds, out ShanheCodexProfile next,
        string? playerGeneralName = null)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(supportedSourceDifficultyIds);
        next = profile;
        if (!IsValidProfile(profile)) return false;
        var prepared = profile;
        var changed = false;
        if (TryReadTaskChanges(profile, session, supportedSourceDifficultyIds, out var progress, out var completed))
        {
            if (!TryPrepareTasks(profile, progress, completed, out prepared)) return false;
            changed = true;
        }
        if (HasPendingBase(profile, session))
        {
            if (!ShanheCodexSettlement.TryPrepare(prepared, session, localDifficulty, out var terminal, playerGeneralName)) return false;
            prepared = terminal;
            changed = true;
        }
        if (!changed) return false;
        next = prepared;
        return true;
    }

    public static bool HasPending(ShanheCodexProfile profile, ShanheCampaignSession session,
        int localDifficulty, IEnumerable<int> supportedSourceDifficultyIds)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(supportedSourceDifficultyIds);
        // Pending is independent of current capacity: an overflow or full ledger
        // must not falsely publish a paid state or hide a retry behind the base claim.
        return IsValidProfile(profile) && (HasPendingBase(profile, session) ||
            TryReadTaskChanges(profile, session, supportedSourceDifficultyIds, out _, out _));
    }

    public static bool TryPrepare(ShanheCodexProfile profile, ShanheCampaignSession session,
        IEnumerable<int> supportedSourceDifficultyIds, out ShanheCodexProfile next)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(supportedSourceDifficultyIds);
        next = profile;
        return IsValidProfile(profile) && TryReadTaskChanges(profile, session, supportedSourceDifficultyIds,
            out var progress, out var completed) && TryPrepareTasks(profile, progress, completed, out next);
    }

    private static bool TryReadTaskChanges(ShanheCodexProfile profile, ShanheCampaignSession session,
        IEnumerable<int> supportedSourceDifficultyIds, out ShanheGuideTaskProgressState progress,
        out ShanheGuideTaskDefinition[] completed)
    {
        progress = profile.GuideTaskProgress ?? ShanheGuideTaskProgressState.Empty;
        completed = [];
        var options = session.CreateCheckpoint().Options;
        if (ShanheCodexSettlement.GetSettlementKey(session) is not { } expeditionKey ||
            options.SourceDifficultyId is not { } sourceDifficulty ||
            !ShanheCampaignDifficultyContract.IsSupported(session, sourceDifficulty)) return false;
        var state = session.Snapshot;
        progress = ShanheGuideTaskProgress.Advance(progress,
            expeditionKey, options.SourceDifficultyId, state.PlayerGeneralId,
            state.Phase == ShanheCampaignPhase.Won, state.CompletedBattles, supportedSourceDifficultyIds);
        var evaluated = progress;
        completed = ShanheGuideTaskCatalog.Tasks.Where(task => !task.RequiresScore &&
            ShanheGuideTaskProgress.IsComplete(evaluated, task.SourceId) && !HasClaimed(profile, task.SourceId)).ToArray();
        return !ReferenceEquals(progress, profile.GuideTaskProgress ?? ShanheGuideTaskProgressState.Empty) || completed.Length > 0;
    }

    private static bool TryPrepareTasks(ShanheCodexProfile profile, ShanheGuideTaskProgressState progress,
        ShanheGuideTaskDefinition[] completed, out ShanheCodexProfile next)
    {
        next = profile;
        var books = completed.Sum(task => task.BookReward);
        if (profile.Books > int.MaxValue - books || profile.ClaimedRewards.Length > 20000 - completed.Length) return false;
        next = profile with
        {
            Books = checked(profile.Books + books),
            GuideTaskProgress = progress,
            ClaimedRewards = [.. profile.ClaimedRewards, .. completed.Select(task => ClaimKey(task.SourceId))],
            RunRecords = [.. profile.RunRecords],
            UnlockedRewardIds = [.. profile.UnlockedRewardIds],
            EquippedTacticIds = [.. profile.EquippedTacticIds],
            LoadoutPlans = profile.LoadoutPlans?.Select(plan => plan with { EquippedTacticIds = [.. plan.EquippedTacticIds] }).ToArray()
        };
        return true;
    }

    private static bool HasPendingBase(ShanheCodexProfile profile, ShanheCampaignSession session) =>
        session.Snapshot.Phase is ShanheCampaignPhase.Won or ShanheCampaignPhase.Lost &&
        ShanheCodexSettlement.GetSettlementKey(session) is not null && !ShanheCodexSettlement.HasClaimed(profile, session);

    private static bool IsValidProfile(ShanheCodexProfile profile)
    {
        try { FileShanheCodexStore.Validate(profile); return true; }
        catch (InvalidDataException) { return false; }
    }

    /// <summary>Called by the profile store alongside its existing book and claim validation.</summary>
    public static void ValidateProfile(ShanheCodexProfile profile)
    {
        var progress = profile.GuideTaskProgress ?? ShanheGuideTaskProgressState.Empty;
        try { ShanheGuideTaskProgress.Validate(progress); }
        catch (ArgumentException exception) { throw new InvalidDataException("章节任务进度无效，原文件已保留。", exception); }
        if (profile.ClaimedRewards.Where(key => key.StartsWith(ClaimPrefix, StringComparison.Ordinal)).Any(key =>
            !ShanheGuideTaskCatalog.Tasks.Any(task => !task.RequiresScore && ClaimKey(task.SourceId) == key &&
                ShanheGuideTaskProgress.IsComplete(progress, task.SourceId))))
            throw new InvalidDataException("章节任务领奖收据无效，原文件已保留。");
    }
}
