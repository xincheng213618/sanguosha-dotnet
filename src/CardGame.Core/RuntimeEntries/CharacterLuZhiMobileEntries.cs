#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLuZhiMobileRuntime))]
internal sealed partial class CharacterLuZhiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMobileRuntime> _resolve;
    private CharacterLuZhiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuZhiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMobileRuntime> resolve) => new(resolve);
}
