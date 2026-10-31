#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLeiBoAnniversaryRuntime))]
internal sealed partial class CharacterLeiBoAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLeiBoAnniversaryRuntime> _resolve;
    private CharacterLeiBoAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLeiBoAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLeiBoAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLeiBoAnniversaryRuntime> resolve) => new(resolve);
}
