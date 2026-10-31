#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime))]
internal sealed partial class CharacterWenChouMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> _resolve;
    private CharacterWenChouMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWenChouMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> resolve) => new(resolve);
}
