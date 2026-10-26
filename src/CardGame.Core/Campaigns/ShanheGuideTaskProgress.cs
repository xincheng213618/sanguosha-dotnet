using System.Text.Json.Serialization;

namespace CardGame.Core.Campaigns;

/// <summary>Authored source identities captured only after independent replay of a completed native battle.</summary>
public sealed record ShanheGuideBattleCompletion(int EncounterOrdinal, string NodeId, string EncounterId,
    int? SourceBattleId, int? SourceCityId, bool Won);

/// <summary>One counted occurrence. A task retains at most its required number of occurrences.</summary>
public sealed record ShanheGuideTaskReceipt(int SourceTaskId, string ExpeditionKey, int EncounterOrdinal);

/// <summary>Bounded account progress, detached from replaceable expedition saves.</summary>
public sealed record ShanheGuideTaskProgressState
{
    [JsonConstructor]
    public ShanheGuideTaskProgressState(IReadOnlyList<ShanheGuideTaskReceipt> receipts)
    {
        ArgumentNullException.ThrowIfNull(receipts);
        Receipts = Array.AsReadOnly(receipts.ToArray());
    }

    public IReadOnlyList<ShanheGuideTaskReceipt> Receipts { get; }
    public static ShanheGuideTaskProgressState Empty { get; } = new([]);
    public int Count(int sourceTaskId) => Receipts.Count(receipt => receipt.SourceTaskId == sourceTaskId);
}

/// <summary>
/// Evaluates verified run facts, never scores or displayed names. The caller supplies the genuine
/// source difficulty directory; an unbound local difficulty cannot progress a source task.
/// </summary>
public static class ShanheGuideTaskProgress
{
    public static ShanheGuideTaskProgressState Advance(ShanheGuideTaskProgressState previous,
        string expeditionKey, int? sourceDifficultyId, string playerGeneralId, bool expeditionWon,
        IReadOnlyList<ShanheGuideBattleCompletion> completedBattles, IEnumerable<int> supportedSourceDifficultyIds)
    {
        ArgumentNullException.ThrowIfNull(previous);
        ArgumentNullException.ThrowIfNull(completedBattles);
        ArgumentNullException.ThrowIfNull(supportedSourceDifficultyIds);
        Validate(previous);
        if (!IsExpeditionKey(expeditionKey)) throw new ArgumentException("Task progress requires a canonical expedition identity.", nameof(expeditionKey));
        if (completedBattles.Any(battle => battle is null || battle.EncounterOrdinal <= 0 ||
                string.IsNullOrWhiteSpace(battle.NodeId) || battle.NodeId.Length > 256 ||
                string.IsNullOrWhiteSpace(battle.EncounterId) || battle.EncounterId.Length > 256 ||
                battle.SourceBattleId is <= 0 || battle.SourceCityId is <= 0) ||
            completedBattles.Select(battle => battle.EncounterOrdinal).Distinct().Count() != completedBattles.Count)
            throw new ArgumentException("Task battle evidence must identify distinct completed native encounters.", nameof(completedBattles));
        if (sourceDifficultyId is not { } difficulty || !supportedSourceDifficultyIds.Contains(difficulty)) return previous;

        var receipts = previous.Receipts.ToList();
        foreach (var task in ShanheGuideTaskCatalog.Tasks.Where(task => !task.RequiresScore && difficulty >= task.MinimumSourceDifficultyId))
        {
            if (task.Kind == ShanheGuideTaskKind.WinExpeditionWithGeneral)
            {
                // Explicitly verified printed versions, not the wider CharacterId family:
                // official character.sgs 100=庸肆/伪帝, 15=奸雄/护驾, 713=霸朝/覆载.
                // Boundary 311 currently has different local rules; gods, national-war
                // and campaign enemy variants sharing the same character are not aliases.
                var sourceGeneralId = playerGeneralId switch
                {
                    "classic:yuan-shu" => 100,
                    "classic:cao-cao" => 15,
                    "ol:mo-cao-cao" => 713,
                    _ => (int?)null
                };
                if (sourceGeneralId is { } generalId && task.SourceGeneralIds.Contains(generalId) && expeditionWon &&
                    completedBattles.Count > 0 && completedBattles.All(battle => battle.Won))
                    Add(task, completedBattles.Max(battle => battle.EncounterOrdinal));
                continue;
            }
            foreach (var battle in completedBattles.Where(battle => battle.Won).OrderBy(battle => battle.EncounterOrdinal))
            {
                var matches = task.Kind switch
                {
                    ShanheGuideTaskKind.WinSpecifiedBattle => battle.SourceBattleId is { } battleId && task.SourceTargetIds.Contains(battleId),
                    // Type 94 target 5107 is a city. The website's derived battle-5107 lookup
                    // names an unrelated Wu prince; only the original typed target is executable.
                    ShanheGuideTaskKind.WinBattleInSpecifiedCity => battle.SourceCityId is { } cityId && task.SourceTargetIds.Contains(cityId),
                    _ => false
                };
                if (matches) Add(task, battle.EncounterOrdinal);
            }
        }
        return receipts.Count == previous.Receipts.Count ? previous : new(receipts);

        void Add(ShanheGuideTaskDefinition task, int ordinal)
        {
            if (receipts.Count(receipt => receipt.SourceTaskId == task.SourceId) >= task.RequiredCount ||
                receipts.Any(receipt => receipt.SourceTaskId == task.SourceId && receipt.ExpeditionKey == expeditionKey &&
                    receipt.EncounterOrdinal == ordinal)) return;
            receipts.Add(new(task.SourceId, expeditionKey, ordinal));
        }
    }

    public static bool IsComplete(ShanheGuideTaskProgressState progress, int sourceTaskId) =>
        ShanheGuideTaskCatalog.BySourceId.TryGetValue(sourceTaskId, out var task) && !task.RequiresScore &&
        progress.Count(sourceTaskId) >= task.RequiredCount;

    public static void Validate(ShanheGuideTaskProgressState progress)
    {
        ArgumentNullException.ThrowIfNull(progress);
        var receipts = progress.Receipts;
        if (receipts.Count > ShanheGuideTaskCatalog.Tasks.Where(task => !task.RequiresScore).Sum(task => task.RequiredCount) ||
            receipts.Any(receipt => receipt is null || !ShanheGuideTaskCatalog.BySourceId.TryGetValue(receipt.SourceTaskId, out var task) ||
                task.RequiresScore || !IsExpeditionKey(receipt.ExpeditionKey) || receipt.EncounterOrdinal <= 0) ||
            receipts.Distinct().Count() != receipts.Count ||
            receipts.GroupBy(receipt => receipt.SourceTaskId).Any(group => group.Count() > ShanheGuideTaskCatalog.BySourceId[group.Key].RequiredCount))
            throw new ArgumentException("Invalid bounded chapter task progress.", nameof(progress));
    }

    private static bool IsExpeditionKey(string value) => value is { Length: 43 } && value.StartsWith("expedition:", StringComparison.Ordinal) &&
        Guid.TryParseExact(value[11..], "N", out var id) && id != Guid.Empty && value[11..] == id.ToString("N");
}
