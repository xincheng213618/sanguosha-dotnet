#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangFeiOlMagicRuntime))]
internal sealed partial class CharacterZhangFeiOlMagicEntries
{
    private readonly Func<EngineOwners.CharacterZhangFeiOlMagicRuntime> _resolve;
    private CharacterZhangFeiOlMagicEntries(Func<EngineOwners.CharacterZhangFeiOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangFeiOlMagicEntries Create(Func<EngineOwners.CharacterZhangFeiOlMagicRuntime> resolve) => new(resolve);
}
