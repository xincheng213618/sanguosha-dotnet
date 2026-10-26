using System.Text.Json;
using CardGame.Core.Campaigns;

internal static class ShanheGuideTaskProgressChecks
{
    public static void TypedSourceEvidenceAccumulatesWithoutLocalDifficultyOrDuplicateCredit()
    {
        int[] sourceIds = [1, 2, 6, 11];
        var key = "expedition:" + Guid.Parse("a197e977-6b8c-44d2-92c3-31c404d924b9").ToString("N");
        ShanheGuideBattleCompletion[] battles =
        [
            new(1, "node-a", "fengxian-a", 2644, 5048, true),
            new(2, "node-b", "fengxian-b", 2645, 5048, true),
            new(3, "node-c", "unrelated-wu-prince", 5107, 201, true)
        ];
        var empty = ShanheGuideTaskProgressState.Empty;
        var unbound = ShanheGuideTaskProgress.Advance(empty, key, null, "classic:cao-cao", true, battles, sourceIds);
        var unknown = ShanheGuideTaskProgress.Advance(empty, key, 25, "classic:cao-cao", true, battles, sourceIds);
        Need(ReferenceEquals(empty, unbound) && ReferenceEquals(empty, unknown),
            "Local or unknown source difficulty identities do not satisfy source task conditions.");
        var progress = ShanheGuideTaskProgress.Advance(empty, key, 11, "classic:cao-cao", true, battles, sourceIds);
        Need(progress.Count(200352) == 2 && progress.Count(200357) == 1 && progress.Count(200358) == 0,
            "Only real matching battle identities count; battle 5107 is not city 5107 and cannot satisfy the city task.");
        progress = JsonSerializer.Deserialize<ShanheGuideTaskProgressState>(JsonSerializer.Serialize(progress))!;
        var repeat = ShanheGuideTaskProgress.Advance(progress, key, 11, "classic:cao-cao", true, battles, sourceIds);
        Need(ReferenceEquals(progress, repeat), "Cold replay of the same native ordinals cannot count a battle twice.");
        var nextKey = "expedition:" + Guid.Parse("379c5c54-f09d-428c-b87f-1f4ef9e7e2b4").ToString("N");
        progress = ShanheGuideTaskProgress.Advance(progress, nextKey, 11, "classic:yuan-shu", true,
            [new(1, "node-d", "source-battle", 22644, 5107, true)], sourceIds);
        Need(progress.Count(200352) == 3 && progress.Count(200347) == 1 && progress.Count(200358) == 1 &&
            progress.Receipts.Count == 6 && ShanheGuideTaskCatalog.Tasks.Where(task => task.RequiresScore).All(task => progress.Count(task.SourceId) == 0),
            "Distinct real runs accumulate three Fengxian victories and the typed city victory; score tasks remain unsupported and total progress stays bounded.");
        var failed = ShanheGuideTaskProgress.Advance(empty, key, 11, "classic:yuan-shu", false,
            [new(1, "node", "fengxian", 2644, 5107, false)], sourceIds);
        var below = ShanheGuideTaskProgress.Advance(empty, key, 1, "classic:cao-cao", true, battles, sourceIds);
        Need(failed.Receipts.Count == 0 && below.Receipts.Count == 0,
            "Losses and source difficulties below the individual threshold cannot create task progress.");
        var enemyVariant = ShanheGuideTaskProgress.Advance(empty, key, 11, "shanhe:s5-cao-cao-2679", true, battles, sourceIds);
        Need(enemyVariant.Count(200357) == 0,
            "A campaign enemy sharing Cao Cao's CharacterId does not match the source task's explicitly verified printed variants.");
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
