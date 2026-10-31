#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaiFuRenOlRuntime))]
internal sealed partial class CharacterCaiFuRenOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiFuRenOlRuntime> _resolve;
    private CharacterCaiFuRenOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiFuRenOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiFuRenOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiFuRenOlRuntime> resolve) => new(resolve);
}
