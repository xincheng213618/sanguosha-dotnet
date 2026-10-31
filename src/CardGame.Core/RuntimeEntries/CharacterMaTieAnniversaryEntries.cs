#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaTieAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> _resolve;
    private CharacterMaTieAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaTieAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> resolve) => new(resolve);
}
