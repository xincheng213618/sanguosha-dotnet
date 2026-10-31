#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuXunMouMobileRuntime))]
internal sealed partial class CharacterLuXunMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterLuXunMouMobileRuntime> _resolve;
    private CharacterLuXunMouMobileEntries(Func<EngineOwners.CharacterLuXunMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuXunMouMobileEntries Create(Func<EngineOwners.CharacterLuXunMouMobileRuntime> resolve) => new(resolve);
}
