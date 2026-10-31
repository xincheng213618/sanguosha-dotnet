#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuoYuanMobileShiRuntime))]
internal sealed partial class CharacterGuoYuanMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterGuoYuanMobileShiRuntime> _resolve;
    private CharacterGuoYuanMobileShiEntries(Func<EngineOwners.CharacterGuoYuanMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoYuanMobileShiEntries Create(Func<EngineOwners.CharacterGuoYuanMobileShiRuntime> resolve) => new(resolve);
}
