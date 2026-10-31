#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoCaoOlGodRuntime))]
internal sealed partial class CharacterCaoCaoOlGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoOlGodRuntime> _resolve;
    private CharacterCaoCaoOlGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoOlGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoCaoOlGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoOlGodRuntime> resolve) => new(resolve);
}
