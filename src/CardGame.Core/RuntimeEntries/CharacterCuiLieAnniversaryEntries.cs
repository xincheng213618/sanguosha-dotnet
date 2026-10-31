#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCuiLieAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> _resolve;
    private CharacterCuiLieAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCuiLieAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> resolve) => new(resolve);
}
