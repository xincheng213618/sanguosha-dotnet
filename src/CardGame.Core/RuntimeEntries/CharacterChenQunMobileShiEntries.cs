#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChenQunMobileShiRuntime))]
internal sealed partial class CharacterChenQunMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterChenQunMobileShiRuntime> _resolve;
    private CharacterChenQunMobileShiEntries(Func<EngineOwners.CharacterChenQunMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenQunMobileShiEntries Create(Func<EngineOwners.CharacterChenQunMobileShiRuntime> resolve) => new(resolve);
}
