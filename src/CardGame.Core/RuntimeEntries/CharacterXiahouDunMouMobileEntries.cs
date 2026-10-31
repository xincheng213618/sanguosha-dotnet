#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiahouDunMouMobileRuntime))]
internal sealed partial class CharacterXiahouDunMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterXiahouDunMouMobileRuntime> _resolve;
    private CharacterXiahouDunMouMobileEntries(Func<EngineOwners.CharacterXiahouDunMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouDunMouMobileEntries Create(Func<EngineOwners.CharacterXiahouDunMouMobileRuntime> resolve) => new(resolve);
}
