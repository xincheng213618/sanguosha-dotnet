#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterMuShunAnniversaryRuntime))]
internal sealed partial class CharacterMuShunAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMuShunAnniversaryRuntime> _resolve;
    private CharacterMuShunAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMuShunAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMuShunAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMuShunAnniversaryRuntime> resolve) => new(resolve);
}
