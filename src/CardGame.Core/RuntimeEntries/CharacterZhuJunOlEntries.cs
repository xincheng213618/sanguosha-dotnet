#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhuJunOlRuntime))]
internal sealed partial class CharacterZhuJunOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuJunOlRuntime> _resolve;
    private CharacterZhuJunOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuJunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuJunOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuJunOlRuntime> resolve) => new(resolve);
}
