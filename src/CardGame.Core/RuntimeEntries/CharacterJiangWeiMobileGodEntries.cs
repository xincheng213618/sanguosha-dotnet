#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterJiangWeiMobileGodRuntime))]
internal sealed partial class CharacterJiangWeiMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterJiangWeiMobileGodRuntime> _resolve;
    private CharacterJiangWeiMobileGodEntries(Func<EngineOwners.CharacterJiangWeiMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangWeiMobileGodEntries Create(Func<EngineOwners.CharacterJiangWeiMobileGodRuntime> resolve) => new(resolve);
}
