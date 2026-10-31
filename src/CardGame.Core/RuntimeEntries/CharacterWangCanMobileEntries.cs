#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWangCanMobileRuntime))]
internal sealed partial class CharacterWangCanMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanMobileRuntime> _resolve;
    private CharacterWangCanMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangCanMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanMobileRuntime> resolve) => new(resolve);
}
