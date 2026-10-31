#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoCaoOlMagicRuntime))]
internal sealed partial class CharacterCaoCaoOlMagicEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoOlMagicRuntime> _resolve;
    private CharacterCaoCaoOlMagicEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoCaoOlMagicEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoOlMagicRuntime> resolve) => new(resolve);
}
