#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCheZhouAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> _resolve;
    private CharacterCheZhouAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCheZhouAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> resolve) => new(resolve);
}
