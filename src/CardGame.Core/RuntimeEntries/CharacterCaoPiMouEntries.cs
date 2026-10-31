#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoPiMouRuntime))]
internal sealed partial class CharacterCaoPiMouEntries
{
    private readonly Func<EngineOwners.CharacterCaoPiMouRuntime> _resolve;
    private CharacterCaoPiMouEntries(Func<EngineOwners.CharacterCaoPiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoPiMouEntries Create(Func<EngineOwners.CharacterCaoPiMouRuntime> resolve) => new(resolve);
}
