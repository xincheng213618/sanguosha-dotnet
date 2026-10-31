#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouYuMouRuntime))]
internal sealed partial class CharacterZhouYuMouEntries
{
    private readonly Func<EngineOwners.CharacterZhouYuMouRuntime> _resolve;
    private CharacterZhouYuMouEntries(Func<EngineOwners.CharacterZhouYuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuMouEntries Create(Func<EngineOwners.CharacterZhouYuMouRuntime> resolve) => new(resolve);
}
