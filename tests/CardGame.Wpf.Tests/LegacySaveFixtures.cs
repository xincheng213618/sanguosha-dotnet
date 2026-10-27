using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

/// <summary>
/// Offline checkpoint fixtures for retained replay, privacy and UI regression checks.
/// These do not re-enable the retired save/restore actions in the desktop product.
/// </summary>
internal static class LegacySaveFixtures
{
    private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;

    public static void Save(MainViewModel model, GameSaveSlot slot = GameSaveSlot.Manual)
    {
        if (model.IsShanheBattleSession || model.IsShanheOpen)
        {
            model.SaveGameCommand.Execute(null);
            return;
        }
        foreach (var method in new[] { "FlushWushuangClock", "FlushNianClock" })
            typeof(MainViewModel).GetMethod(method, Private)!.Invoke(model, null);
        var game = Program.Engine(model);
        var store = (IGameSaveStore)typeof(MainViewModel).GetField(model.IsWushuangTable ? "_wushuangSaveStore" : "_saveStore", Private)!.GetValue(model)!;
        var custom = (CustomGeneralLibrary)typeof(MainViewModel).GetField("_activeCustomGenerals", Private)!.GetValue(model)!;
        store.Write(slot, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, model.IsAutoAdvance, game.CreateCheckpoint())
        {
            PlaybackSpeedId = model.SelectedPlaybackSpeed.Id,
            CustomGenerals = game.ContentRegistry!.Packages.Any(package => package.Id == CustomGeneralContent.PackageId) ? custom : null
        });
    }

    public static void Load(MainViewModel model, GameSaveSlot slot = GameSaveSlot.Manual, bool? wushuang = null) =>
        typeof(MainViewModel).GetMethod("LoadGame", Private)!.Invoke(model, [slot, wushuang ?? model.IsWushuangTable]);
}
