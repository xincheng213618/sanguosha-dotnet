#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoCaoOlMagicRuntime))]
internal sealed partial class CharacterCaoCaoOlMagicEntries
{
    private readonly Func<EngineOwners.CharacterCaoCaoOlMagicRuntime> _resolve;
    private CharacterCaoCaoOlMagicEntries(Func<EngineOwners.CharacterCaoCaoOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoCaoOlMagicEntries Create(Func<EngineOwners.CharacterCaoCaoOlMagicRuntime> resolve) => new(resolve);
}
