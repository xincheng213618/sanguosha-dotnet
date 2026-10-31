#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangChangMobileShiRuntime))]
internal sealed partial class CharacterWangChangMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterWangChangMobileShiRuntime> _resolve;
    private CharacterWangChangMobileShiEntries(Func<EngineOwners.CharacterWangChangMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangChangMobileShiEntries Create(Func<EngineOwners.CharacterWangChangMobileShiRuntime> resolve) => new(resolve);
}
