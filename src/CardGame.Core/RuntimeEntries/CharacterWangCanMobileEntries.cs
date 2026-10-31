#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangCanMobileRuntime))]
internal sealed partial class CharacterWangCanMobileEntries
{
    private readonly Func<EngineOwners.CharacterWangCanMobileRuntime> _resolve;
    private CharacterWangCanMobileEntries(Func<EngineOwners.CharacterWangCanMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangCanMobileEntries Create(Func<EngineOwners.CharacterWangCanMobileRuntime> resolve) => new(resolve);
}
