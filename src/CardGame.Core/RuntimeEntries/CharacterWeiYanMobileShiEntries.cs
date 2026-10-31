#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWeiYanMobileShiRuntime))]
internal sealed partial class CharacterWeiYanMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterWeiYanMobileShiRuntime> _resolve;
    private CharacterWeiYanMobileShiEntries(Func<EngineOwners.CharacterWeiYanMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWeiYanMobileShiEntries Create(Func<EngineOwners.CharacterWeiYanMobileShiRuntime> resolve) => new(resolve);
}
