#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed partial class CharacterDiaoChanLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _resolve;
    private CharacterDiaoChanLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDiaoChanLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> resolve) => new(resolve);
}
