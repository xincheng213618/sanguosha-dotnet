#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterJiangWeiAnniversaryGodRuntime))]
internal sealed partial class CharacterJiangWeiAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiAnniversaryGodRuntime> _resolve;
    private CharacterJiangWeiAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangWeiAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiAnniversaryGodRuntime> resolve) => new(resolve);
}
