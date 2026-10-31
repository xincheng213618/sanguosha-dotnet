#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHeQiMobileRuntime))]
internal sealed partial class CharacterHeQiMobileEntries
{
    private readonly Func<EngineOwners.CharacterHeQiMobileRuntime> _resolve;
    private CharacterHeQiMobileEntries(Func<EngineOwners.CharacterHeQiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeQiMobileEntries Create(Func<EngineOwners.CharacterHeQiMobileRuntime> resolve) => new(resolve);
}
