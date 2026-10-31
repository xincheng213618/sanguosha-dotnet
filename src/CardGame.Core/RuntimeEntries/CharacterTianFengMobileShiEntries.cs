#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTianFengMobileShiRuntime))]
internal sealed partial class CharacterTianFengMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterTianFengMobileShiRuntime> _resolve;
    private CharacterTianFengMobileShiEntries(Func<EngineOwners.CharacterTianFengMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTianFengMobileShiEntries Create(Func<EngineOwners.CharacterTianFengMobileShiRuntime> resolve) => new(resolve);
}
