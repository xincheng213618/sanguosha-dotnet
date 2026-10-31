#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMuShunAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMuShunAnniversaryRuntime> _resolve;
    private CharacterMuShunAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMuShunAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMuShunAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMuShunAnniversaryRuntime> resolve) => new(resolve);
}
