using System.IO;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf.Persistence;

internal static class CustomGeneralPersistenceChecks
{
    private const string GeneralId = "custom:general-3a753a5e7e1a4acbb5921527db674a21";
    private const string SkillId = "custom:skill-f763a85bd8624f4ba3c588eac3f172e9";

    public static void LibraryRoundTripsAndFailedWritesPreserveOriginal(string output) =>
        WithDirectory(output, "library", directory =>
        {
            var path = Path.Combine(directory, "custom-generals.json");
            var store = new FileCustomGeneralLibraryStore(path);
            Need(store.Read().Generals.Count == 0 && !File.Exists(path), "A missing custom library is empty without writing a file.");
            var library = Library();
            store.Write(library);
            var original = File.ReadAllText(path);
            var json = JsonNode.Parse(original)!;
            Need(json["formatVersion"]!.GetValue<int>() == CustomGeneralLibrary.CurrentFormatVersion &&
                json["generals"]![0]!["gender"]!.GetValue<string>() == "female" &&
                json["generals"]![0]!["customSkills"]![0]!["trigger"]!.GetValue<string>() == "active" &&
                store.Read().Generals.Single() is { Id: GeneralId, Name: "存档自定义武将", Gender: GeneralGender.Female } read &&
                read.CustomSkills.Single().Effects.Single().Amount == 2,
                "The authoring library must round-trip names, physical identity and nested skills with camelCase and string enums.");

            ExpectInvalid(() => store.Write(library with { FormatVersion = int.MaxValue }));
            Need(File.ReadAllText(path) == original, "Validation must finish before an invalid library can replace the saved file.");
            using (var locked = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var failed = false;
                try { store.Write(library with { Generals = [library.Generals.Single() with { Name = "尚未写入的改名" }] }); }
                catch (Exception error) when (error is IOException or UnauthorizedAccessException) { failed = true; }
                Need(failed && File.ReadAllText(path) == original, "A failed atomic replacement must leave the original library intact.");
            }
            Need(!Directory.EnumerateFiles(directory, "*.tmp").Any(), "A failed library write must clean its own temporary file.");

            foreach (JsonNode invalidGender in new JsonNode[] { JsonValue.Create(1)!, JsonValue.Create("unknown")! })
            {
                json["generals"]![0]!["gender"] = invalidGender;
                var invalid = json.ToJsonString();
                File.WriteAllText(path, invalid);
                ExpectInvalid(() => store.Read());
                Need(File.ReadAllText(path) == invalid, "Invalid enum input must be reported and preserved without a silent rewrite.");
            }
            File.WriteAllText(path, "{ broken library");
            ExpectInvalid(() => store.Read());
            Need(File.ReadAllText(path) == "{ broken library", "An unreadable custom library must remain recoverable.");
        });

    public static void SaveSnapshotRestoresAfterCurrentLibraryChanges(string output) =>
        WithDirectory(output, "snapshot", directory =>
        {
            var effects = new List<CustomSkillEffect> { new() { Kind = CustomSkillEffectKind.Draw, Amount = 2 } };
            var skills = new List<CustomSkillDraft> { new() { Id = SkillId, Name = "当局摸牌", Effects = effects } };
            var printed = new List<string> { "standard:yingzi" };
            var generals = new List<CustomGeneralDraft> { new()
            {
                Id = GeneralId, Name = "存档自定义武将", Gender = GeneralGender.Female,
                SkillIds = printed, CustomSkills = skills
            } };
            var mutable = new CustomGeneralLibrary { Generals = generals };
            var libraryStore = new MemoryCustomGeneralLibraryStore();
            libraryStore.Write(mutable);
            var baseline = StandardContentRegistry.Create();
            var registry = CustomGeneralContent.CreateRegistry(baseline, libraryStore.Read());
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = 721019, PlayerCount = 5, HumanSeat = 0, HumanRole = Role.Lord,
                UseInteractiveSetup = true, AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, registry);
            Accept(game, new StartGameCommand());
            Need(game.PendingDecision is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 }, "Custom persistence starts through real general selection.");
            Accept(game, new RequestGeneralRosterCommand(0, true, game.Revision, game.PendingDecision!.PromptId));
            Need(game.PendingDecision!.ValidContentIds.Contains(GeneralId), "The extended native mode must expose the saved custom identity.");
            Accept(game, new SelectGeneralCommand(0, GeneralId, game.Revision, game.PendingDecision.PromptId));
            var save = new GameSaveFile(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint())
                { CustomGenerals = mutable };

            effects[0] = new() { Kind = CustomSkillEffectKind.Draw, Amount = 5 };
            skills.Clear(); printed.Clear(); generals.Clear();
            Need(libraryStore.Read().Generals.Single().CustomSkills.Single().Effects.Single().Amount == 2 &&
                save.CustomGenerals!.Generals.Single().SkillIds.SequenceEqual(["standard:yingzi"]) &&
                save.CustomGenerals.Generals.Single().CustomSkills.Single().Effects.Single().Amount == 2,
                "Memory writes and save assignment must detach every nested authoring collection from later editor mutations.");
            var readOnly = false;
            try { ((IList<CustomGeneralDraft>)save.CustomGenerals.Generals).Clear(); }
            catch (NotSupportedException) { readOnly = true; }
            Need(readOnly, "The stored match's authoring snapshot must expose a read-only general collection.");
            libraryStore.Write(CustomGeneralLibrary.Empty);
            var file = new FileGameSaveStore(directory);
            file.Write(GameSaveSlot.Manual, save);
            var saveJson = JsonNode.Parse(File.ReadAllText(file.GetPath(GameSaveSlot.Manual)))!;
            Need(saveJson["CustomGenerals"]!["generals"]![0]!["gender"]!.GetValue<string>() == "female" &&
                saveJson["Checkpoint"] is not null,
                "Only the embedded authoring payload uses its shared camelCase and string-enum contract; the save envelope remains unchanged.");
            var loaded = file.Read(GameSaveSlot.Manual);
            var recoveredRegistry = CustomGeneralContent.CreateRegistry(baseline, loaded.CustomGenerals!);
            var restored = GameReplay.Restore(loaded.Checkpoint, recoveredRegistry);
            Need(libraryStore.Read().Generals.Count == 0 && recoveredRegistry.ContentHash == game.ContentRegistry.ContentHash &&
                restored.CreateSnapshot(0).Players.Single(p => p.Seat == 0).GeneralId == GeneralId &&
                Enumerable.Range(0, 5).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
                "A saved custom match must rebuild and replay its exact original content after the live authoring library has been cleared.");
            Accept(game, new AdvanceOneStepCommand(game.Revision));
            Accept(restored, new AdvanceOneStepCommand(restored.Revision));
            Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(restored.CreateSnapshot(0)),
                "The restored original custom match must continue through the same native boundary.");

            saveJson["CustomGenerals"]!["generals"]![0]!["gender"] = 0;
            var invalidSave = saveJson.ToJsonString();
            File.WriteAllText(file.GetPath(GameSaveSlot.Manual), invalidSave);
            ExpectInvalid(() => file.Read(GameSaveSlot.Manual));
            Need(File.ReadAllText(file.GetPath(GameSaveSlot.Manual)) == invalidSave,
                "The embedded custom snapshot must also reject numeric enums without rewriting its source save.");
        });

    private static CustomGeneralLibrary Library() => new()
    {
        Generals = [new()
        {
            Id = GeneralId, Name = "存档自定义武将", Gender = GeneralGender.Female, SkillIds = ["standard:yingzi"],
            CustomSkills = [new() { Id = SkillId, Name = "当局摸牌", Effects = [new() { Kind = CustomSkillEffectKind.Draw, Amount = 2 }] }]
        }]
    };

    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Custom persistence command failed.");
    }

    private static void ExpectInvalid(Action action)
    {
        try { action(); }
        catch (Exception error) when (error is InvalidDataException or System.Text.Json.JsonException) { return; }
        throw new InvalidOperationException("Invalid custom authoring data was accepted.");
    }

    private static void WithDirectory(string output, string name, Action<string> action)
    {
        var outputRoot = Path.GetFullPath(output);
        Directory.CreateDirectory(outputRoot);
        var directory = Path.GetFullPath(Path.Combine(outputRoot, "custom-general-persistence-" + name));
        Need(directory.StartsWith(outputRoot.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) &&
            !Directory.Exists(directory) && !File.Exists(directory), "The focused custom-persistence fixture requires a new task-owned output directory.");
        Directory.CreateDirectory(directory);
        try { action(directory); }
        finally
        {
            Need((File.GetAttributes(directory) & FileAttributes.ReparsePoint) == 0 && !Directory.EnumerateDirectories(directory).Any(),
                "The owned flat persistence fixture must not follow linked or nested directories during cleanup.");
            foreach (var file in Directory.EnumerateFiles(directory)) File.Delete(file);
            Directory.Delete(directory);
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }
}
