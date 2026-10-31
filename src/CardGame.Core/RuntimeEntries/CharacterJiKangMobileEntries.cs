#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterJiKangMobileRuntime))]
internal sealed partial class CharacterJiKangMobileEntries
{
    private readonly Func<EngineOwners.CharacterJiKangMobileRuntime> _resolve;
    private CharacterJiKangMobileEntries(Func<EngineOwners.CharacterJiKangMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiKangMobileEntries Create(Func<EngineOwners.CharacterJiKangMobileRuntime> resolve) => new(resolve);
}
