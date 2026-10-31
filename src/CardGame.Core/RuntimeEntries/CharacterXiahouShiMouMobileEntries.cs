#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiahouShiMouMobileRuntime))]
internal sealed partial class CharacterXiahouShiMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterXiahouShiMouMobileRuntime> _resolve;
    private CharacterXiahouShiMouMobileEntries(Func<EngineOwners.CharacterXiahouShiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouShiMouMobileEntries Create(Func<EngineOwners.CharacterXiahouShiMouMobileRuntime> resolve) => new(resolve);
}
