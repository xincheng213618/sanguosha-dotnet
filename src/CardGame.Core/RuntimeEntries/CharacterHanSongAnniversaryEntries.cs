#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHanSongAnniversaryRuntime))]
internal sealed partial class CharacterHanSongAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterHanSongAnniversaryRuntime> _resolve;
    private CharacterHanSongAnniversaryEntries(Func<EngineOwners.CharacterHanSongAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHanSongAnniversaryEntries Create(Func<EngineOwners.CharacterHanSongAnniversaryRuntime> resolve) => new(resolve);
}
