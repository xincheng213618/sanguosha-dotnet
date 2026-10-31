#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime))]
internal sealed partial class CharacterHanSongAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime> _resolve;
    private CharacterHanSongAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHanSongAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime> resolve) => new(resolve);
}
