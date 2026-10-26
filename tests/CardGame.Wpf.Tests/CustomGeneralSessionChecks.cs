using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class CustomGeneralSessionChecks
{
    private const string GeneralId = "custom:general-d471596a55b14e019bb52f586b737b81";
    private const string ActiveSkillId = "custom:skill-ec1801832ad44e51a2f186795ad42fcb";
    private const string TriggerSkillId = "custom:skill-54c4a4c596d8438ba5b9d810f0454910";
    private const string OriginalName = "会话自定义武将";

    public static void EditorChangesAffectNextGameWhileSavedCustomMatchRestoresExactly()
    {
        var library = new MemoryCustomGeneralLibraryStore(Library());
        var saves = new MemorySaveStore();
        using var vm = new MainViewModel(false, 721019, true, saves, useExpandedContent: true,
            historyStore: new MemoryMatchHistoryStore(), preferencesStore: new MemoryPlayerPreferencesStore(),
            contentRegistry: StandardContentRegistry.CreateWithClassicGenerals(),
            wushuangSaveStore: new MemorySaveStore(), customGeneralStore: library);
        vm.SelectedTableMode = vm.TableModes.Single(m => m.ModeId == "identity:classic-5");
        vm.SelectedStartingRole = vm.StartingRoles.Single(r => r.Role == Role.Lord);
        vm.StartNewGameCommand.Execute(null);
        var originalGame = Program.Engine(vm);
        Need(originalGame.ModeId == "identity:classic-5" && originalGame.State.Players.Count == 5 &&
            vm.IsGeneralSelectionPending, "The injected library must enter a real five-player identity draft.");
        OpenRoster(vm);
        var custom = vm.CustomGeneralChoices.Single(c => c.GeneralId == GeneralId);
        var beforePreview = originalGame.Revision;
        vm.PreviewGeneralChoiceCommand.Execute(custom);
        Need(ReferenceEquals(vm.SelectedGeneralChoice, custom) && originalGame.Revision == beforePreview &&
            custom.Portrait.ArtworkId == "classic:zhao-yun" && custom.SkillName.Contains("咆哮", StringComparison.Ordinal) &&
            custom.SkillName.Contains("会话摸牌", StringComparison.Ordinal) && custom.SkillName.Contains("会话回复", StringComparison.Ordinal),
            "Full roster must preview the custom general, inherited skill, both authored skills and chosen portrait without committing a selection.");
        vm.SaveGameCommand.Execute(null);
        Need(!vm.HasSaveError, vm.SaveStatus);
        var draftSave = saves.Read(GameSaveSlot.Manual);
        var originalHash = originalGame.ContentRegistry.ContentHash;
        var draftState = State(vm);
        Need(draftSave.CustomGenerals is { } originalLibrary && originalLibrary.Generals.Single().Name == OriginalName &&
            originalLibrary.Generals.Single().CustomSkills.Count == 2 && draftSave.Checkpoint.ContentHash == originalHash,
            "The real VM save command must attach its original custom library to the match checkpoint.");

        var editor = vm.CreateCustomGeneralEditor();
        editor.SelectedGeneral = editor.Generals.Single(g => g.Id == GeneralId);
        editor.SelectedGeneral.Name = "下一局改名";
        editor.SelectedGeneral.CustomSkills.Single(s => s.Id == ActiveSkillId).Effects.Single().Amount = 3;
        editor.SaveCommand.Execute(null);
        Need(!editor.HasErrors && !editor.HasUnsavedChanges && library.Read().Generals.Single().Name == "下一局改名" &&
            ReferenceEquals(Program.Engine(vm), originalGame) && State(vm) == draftState &&
            originalGame.ContentRegistry.ContentHash == originalHash &&
            draftSave.CustomGenerals!.Generals.Single().Name == OriginalName &&
            draftSave.CustomGenerals.Generals.Single().CustomSkills.Single(s => s.Id == ActiveSkillId).Effects.Single().Amount == 2,
            "Saving real editor changes must publish the next-game library while preserving the current engine and previously saved nested snapshot.");
        vm.OpenGeneralGalleryCommand.Execute(null);
        var nextEntry = vm.GeneralGalleryEntries.Single(g => g.GeneralId == GeneralId);
        Need(nextEntry.Name == "下一局改名" && nextEntry.HasPortrait && nextEntry.Portrait.ArtworkId == "classic:zhao-yun",
            "The gallery must refresh to the edited next-game definition and its selected appearance.");
        vm.CloseGeneralGalleryCommand.Execute(null);

        var oldSelection = Program.Engine(vm);
        vm.ChangeStartingIdentityCommand.Execute(vm.StartingRoles.Single(r => r.Role == Role.Loyalist));
        Need(!ReferenceEquals(Program.Engine(vm), oldSelection) &&
            Program.Engine(vm).ContentRegistry.ContentHash == originalHash &&
            Program.Engine(vm).ContentRegistry.Generals[GeneralId].Name == OriginalName &&
            Program.Engine(vm).CreateCheckpoint().Options.HumanRole == Role.Loyalist,
            "Changing the current opening identity must rebuild with the active original registry, not import the recently edited library.");

        vm.LoadManualGameCommand.Execute(null);
        Need(!vm.HasSaveError && State(vm) == draftState && Program.Engine(vm).ContentRegistry.ContentHash == originalHash,
            "The original pre-selection custom save must restore through the real manual-load command.");
        OpenRoster(vm);
        custom = vm.CustomGeneralChoices.Single(c => c.GeneralId == GeneralId);
        vm.PreviewGeneralChoiceCommand.Execute(custom);
        vm.ConfirmGeneralChoiceCommand.Execute(null);
        Need(Program.Engine(vm).CreateSnapshot(0).Players.Single(p => p.Seat == 0).GeneralId == GeneralId &&
            vm.Seats.Single(s => s.Seat == 0) is { HasPortrait: true, Portrait.ArtworkId: "classic:zhao-yun" },
            "Confirming the published custom choice must commit the real custom general and keep its active portrait.");
        vm.SaveGameCommand.Execute(null);
        Need(!vm.HasSaveError, vm.SaveStatus);
        var selectedSave = saves.Read(GameSaveSlot.Manual);
        var selectedState = State(vm);
        var selectedEngine = Program.Engine(vm);

        var deletion = vm.CreateCustomGeneralEditor();
        deletion.SelectedGeneral = deletion.Generals.Single(g => g.Id == GeneralId);
        deletion.DeleteCommand.Execute(null);
        deletion.SaveCommand.Execute(null);
        Need(!deletion.HasErrors && !deletion.HasUnsavedChanges && library.Read().Generals.Count == 0 &&
            ReferenceEquals(Program.Engine(vm), selectedEngine) && State(vm) == selectedState &&
            selectedSave.CustomGenerals!.Generals.Single().Name == OriginalName,
            "Deleting and saving the authoring draft must keep the selected current general and its saved original content intact.");
        vm.StartNewGameCommand.Execute(null);
        Need(!Program.Engine(vm).ContentRegistry.Generals.ContainsKey(GeneralId) &&
            Program.Engine(vm).ContentRegistry.ContentHash != originalHash && vm.IsGeneralSelectionPending,
            "A newly started match must use the current empty library instead of carrying the previous match's custom content forward.");
        OpenRoster(vm);
        Need(vm.CustomGeneralChoices.All(c => c.GeneralId != GeneralId), "The deleted custom general must be absent from the next game's full roster.");

        vm.LoadManualGameCommand.Execute(null);
        Need(!vm.HasSaveError && State(vm) == selectedState &&
            Program.Engine(vm).ContentRegistry.ContentHash == selectedSave.Checkpoint.ContentHash &&
            Program.Engine(vm).ContentRegistry.Generals[GeneralId].Name == OriginalName && library.Read().Generals.Count == 0,
            "A selected custom match must recover its exact state and fingerprint even after its authoring entry is deleted.");
        vm.SaveGameCommand.Execute(null);
        Need(!vm.HasSaveError && saves.Read(GameSaveSlot.Manual).CustomGenerals!.Generals.Single().Name == OriginalName,
            "Saving the restored custom session must retain its active snapshot instead of reading the now-empty editor library.");

        var tampered = selectedSave.CustomGenerals! with
        {
            Generals = [selectedSave.CustomGenerals!.Generals.Single() with { Name = "篡改的快照" }]
        };
        foreach (var invalid in new[] { selectedSave with { CustomGenerals = null }, selectedSave with { CustomGenerals = tampered } })
        {
            saves.Write(GameSaveSlot.Manual, invalid);
            var writes = saves.WriteCount;
            vm.LoadManualGameCommand.Execute(null);
            Need(vm.HasSaveError && vm.SaveStatus.Contains("读取失败", StringComparison.Ordinal) &&
                !Program.Engine(vm).IsFaulted && Program.Engine(vm).AcceptedCommands is [StartGameCommand] &&
                vm.FlushPendingSave() && saves.WriteCount == writes && ReferenceEquals(saves.Read(GameSaveSlot.Manual), invalid),
                "Missing or fingerprint-tampered custom content must reject restoration, initialize a healthy fresh match and preserve the original source save.");
        }
    }

    private static void OpenRoster(MainViewModel vm)
    {
        if (!vm.IsCustomGeneralSelectionOpen) vm.ToggleGeneralRosterCommand.Execute(null);
        Need(vm.IsCustomGeneralSelectionOpen, "The native full-roster command must expose the current match's legal general list.");
    }

    private static string State(MainViewModel vm) => string.Join("|", Enumerable.Range(0, 5)
        .Select(seat => SnapshotJson.Serialize(Program.Engine(vm).CreateSnapshot(seat))));

    private static CustomGeneralLibrary Library() => new()
    {
        Generals = [new()
        {
            Id = GeneralId, Name = OriginalName, FactionId = "shu", BaseHp = 4,
            PortraitKey = "classic:zhao-yun", SkillIds = ["classic:paoxiao"], CustomSkills =
            [
                new() { Id = ActiveSkillId, Name = "会话摸牌", Trigger = CustomSkillTrigger.Active,
                    Effects = [new() { Kind = CustomSkillEffectKind.Draw, Amount = 2 }] },
                new() { Id = TriggerSkillId, Name = "会话回复", Trigger = CustomSkillTrigger.TurnEnd,
                    Effects = [new() { Kind = CustomSkillEffectKind.Recover, Amount = 1 }] }
            ]
        }]
    };

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }
}
