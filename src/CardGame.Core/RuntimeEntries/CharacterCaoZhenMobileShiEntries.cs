#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoZhenMobileShiRuntime))]
internal sealed partial class CharacterCaoZhenMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterCaoZhenMobileShiRuntime> _resolve;
    private CharacterCaoZhenMobileShiEntries(Func<EngineOwners.CharacterCaoZhenMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhenMobileShiEntries Create(Func<EngineOwners.CharacterCaoZhenMobileShiRuntime> resolve) => new(resolve);
}
