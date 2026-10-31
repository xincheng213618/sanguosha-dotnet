#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime))]
internal sealed partial class CharacterZouShiLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> _resolve;
    private CharacterZouShiLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZouShiLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> resolve) => new(resolve);
}
