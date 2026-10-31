#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuBuMouMobileRuntime))]
internal sealed partial class CharacterLuBuMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterLuBuMouMobileRuntime> _resolve;
    private CharacterLuBuMouMobileEntries(Func<EngineOwners.CharacterLuBuMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuBuMouMobileEntries Create(Func<EngineOwners.CharacterLuBuMouMobileRuntime> resolve) => new(resolve);
}
