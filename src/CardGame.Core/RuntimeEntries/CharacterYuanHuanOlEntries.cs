#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYuanHuanOlRuntime))]
internal sealed partial class CharacterYuanHuanOlEntries
{
    private readonly Func<EngineOwners.CharacterYuanHuanOlRuntime> _resolve;
    private CharacterYuanHuanOlEntries(Func<EngineOwners.CharacterYuanHuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanHuanOlEntries Create(Func<EngineOwners.CharacterYuanHuanOlRuntime> resolve) => new(resolve);
}
