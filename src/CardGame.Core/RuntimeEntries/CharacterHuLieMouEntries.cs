#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime))]
internal sealed partial class CharacterHuLieMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> _resolve;
    private CharacterHuLieMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuLieMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> resolve) => new(resolve);
}
