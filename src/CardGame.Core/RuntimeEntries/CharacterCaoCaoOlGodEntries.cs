#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoCaoOlGodRuntime))]
internal sealed partial class CharacterCaoCaoOlGodEntries
{
    private readonly Func<EngineOwners.CharacterCaoCaoOlGodRuntime> _resolve;
    private CharacterCaoCaoOlGodEntries(Func<EngineOwners.CharacterCaoCaoOlGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoCaoOlGodEntries Create(Func<EngineOwners.CharacterCaoCaoOlGodRuntime> resolve) => new(resolve);
}
