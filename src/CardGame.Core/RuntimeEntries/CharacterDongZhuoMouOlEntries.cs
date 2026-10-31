#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDongZhuoMouOlRuntime))]
internal sealed partial class CharacterDongZhuoMouOlEntries
{
    private readonly Func<EngineOwners.CharacterDongZhuoMouOlRuntime> _resolve;
    private CharacterDongZhuoMouOlEntries(Func<EngineOwners.CharacterDongZhuoMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongZhuoMouOlEntries Create(Func<EngineOwners.CharacterDongZhuoMouOlRuntime> resolve) => new(resolve);
}
