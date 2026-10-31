#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterJiangWeiMouMobileRuntime))]
internal sealed partial class CharacterJiangWeiMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterJiangWeiMouMobileRuntime> _resolve;
    private CharacterJiangWeiMouMobileEntries(Func<EngineOwners.CharacterJiangWeiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangWeiMouMobileEntries Create(Func<EngineOwners.CharacterJiangWeiMouMobileRuntime> resolve) => new(resolve);
}
