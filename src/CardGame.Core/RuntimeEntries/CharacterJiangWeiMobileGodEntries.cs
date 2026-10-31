#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime))]
internal sealed partial class CharacterJiangWeiMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime> _resolve;
    private CharacterJiangWeiMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangWeiMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime> resolve) => new(resolve);
}
