#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuYanOlRuntime))]
internal sealed partial class CharacterLiuYanOlEntries
{
    private readonly Func<EngineOwners.CharacterLiuYanOlRuntime> _resolve;
    private CharacterLiuYanOlEntries(Func<EngineOwners.CharacterLiuYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuYanOlEntries Create(Func<EngineOwners.CharacterLiuYanOlRuntime> resolve) => new(resolve);
}
