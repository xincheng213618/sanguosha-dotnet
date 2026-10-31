#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChenGongMouMobileRuntime))]
internal sealed partial class CharacterChenGongMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterChenGongMouMobileRuntime> _resolve;
    private CharacterChenGongMouMobileEntries(Func<EngineOwners.CharacterChenGongMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenGongMouMobileEntries Create(Func<EngineOwners.CharacterChenGongMouMobileRuntime> resolve) => new(resolve);
}
