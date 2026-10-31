#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterJiKangMobileRuntime))]
internal sealed partial class CharacterJiKangMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiKangMobileRuntime> _resolve;
    private CharacterJiKangMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiKangMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiKangMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiKangMobileRuntime> resolve) => new(resolve);
}
