#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime))]
internal sealed partial class CharacterYuanYinAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> _resolve;
    private CharacterYuanYinAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanYinAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> resolve) => new(resolve);
}
