#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuanJieMobileShiRuntime))]
internal sealed partial class CharacterHuanJieMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterHuanJieMobileShiRuntime> _resolve;
    private CharacterHuanJieMobileShiEntries(Func<EngineOwners.CharacterHuanJieMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuanJieMobileShiEntries Create(Func<EngineOwners.CharacterHuanJieMobileShiRuntime> resolve) => new(resolve);
}
