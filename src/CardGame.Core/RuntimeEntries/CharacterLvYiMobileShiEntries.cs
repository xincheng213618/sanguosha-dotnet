#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLvYiMobileShiRuntime))]
internal sealed partial class CharacterLvYiMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterLvYiMobileShiRuntime> _resolve;
    private CharacterLvYiMobileShiEntries(Func<EngineOwners.CharacterLvYiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvYiMobileShiEntries Create(Func<EngineOwners.CharacterLvYiMobileShiRuntime> resolve) => new(resolve);
}
