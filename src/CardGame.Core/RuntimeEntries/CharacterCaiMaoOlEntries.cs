#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaiMaoOlRuntime))]
internal sealed partial class CharacterCaiMaoOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiMaoOlRuntime> _resolve;
    private CharacterCaiMaoOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiMaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiMaoOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiMaoOlRuntime> resolve) => new(resolve);
}
