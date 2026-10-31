#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWangKuangOlRuntime))]
internal sealed partial class CharacterWangKuangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangKuangOlRuntime> _resolve;
    private CharacterWangKuangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangKuangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangKuangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangKuangOlRuntime> resolve) => new(resolve);
}
