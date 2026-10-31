#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime))]
internal sealed partial class CharacterCheZhouAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> _resolve;
    private CharacterCheZhouAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCheZhouAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> resolve) => new(resolve);
}
