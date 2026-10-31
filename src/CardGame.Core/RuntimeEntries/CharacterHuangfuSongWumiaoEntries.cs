#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangfuSongWumiaoRuntime))]
internal sealed partial class CharacterHuangfuSongWumiaoEntries
{
    private readonly Func<EngineOwners.CharacterHuangfuSongWumiaoRuntime> _resolve;
    private CharacterHuangfuSongWumiaoEntries(Func<EngineOwners.CharacterHuangfuSongWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangfuSongWumiaoEntries Create(Func<EngineOwners.CharacterHuangfuSongWumiaoRuntime> resolve) => new(resolve);
}
