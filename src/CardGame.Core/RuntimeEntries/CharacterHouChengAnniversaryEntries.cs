#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHouChengAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHouChengAnniversaryRuntime> _resolve;
    private CharacterHouChengAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHouChengAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHouChengAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHouChengAnniversaryRuntime> resolve) => new(resolve);
}
