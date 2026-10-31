#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangQuanMobileShiRuntime))]
internal sealed partial class CharacterHuangQuanMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterHuangQuanMobileShiRuntime> _resolve;
    private CharacterHuangQuanMobileShiEntries(Func<EngineOwners.CharacterHuangQuanMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangQuanMobileShiEntries Create(Func<EngineOwners.CharacterHuangQuanMobileShiRuntime> resolve) => new(resolve);
}
