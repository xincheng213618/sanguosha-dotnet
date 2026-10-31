#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime))]
internal sealed partial class CharacterGuanYuRuntimeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> _resolve;
    private CharacterGuanYuRuntimeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanYuRuntimeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> resolve) => new(resolve);
}
