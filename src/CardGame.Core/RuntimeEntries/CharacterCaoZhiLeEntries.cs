#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed partial class CharacterCaoZhiLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _resolve;
    private CharacterCaoZhiLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhiLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> resolve) => new(resolve);
}
