#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoOlRuntime))]
internal sealed partial class CharacterZhugeGuoOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoOlRuntime> _resolve;
    private CharacterZhugeGuoOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeGuoOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoOlRuntime> resolve) => new(resolve);
}
