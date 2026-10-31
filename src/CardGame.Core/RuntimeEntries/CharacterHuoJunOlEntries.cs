#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuoJunOlRuntime))]
internal sealed partial class CharacterHuoJunOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuoJunOlRuntime> _resolve;
    private CharacterHuoJunOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuoJunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuoJunOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuoJunOlRuntime> resolve) => new(resolve);
}
