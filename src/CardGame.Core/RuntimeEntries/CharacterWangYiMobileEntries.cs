#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime))]
internal sealed partial class CharacterWangYiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime> _resolve;
    private CharacterWangYiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangYiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime> resolve) => new(resolve);
}
