#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime))]
internal sealed partial class CharacterHuangfuSongWumiaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> _resolve;
    private CharacterHuangfuSongWumiaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangfuSongWumiaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> resolve) => new(resolve);
}
