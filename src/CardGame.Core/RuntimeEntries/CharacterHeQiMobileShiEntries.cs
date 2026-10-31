#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHeQiMobileShiRuntime))]
internal sealed partial class CharacterHeQiMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterHeQiMobileShiRuntime> _resolve;
    private CharacterHeQiMobileShiEntries(Func<EngineOwners.CharacterHeQiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeQiMobileShiEntries Create(Func<EngineOwners.CharacterHeQiMobileShiRuntime> resolve) => new(resolve);
}
