#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime))]
internal sealed partial class CharacterLuSuMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> _resolve;
    private CharacterLuSuMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuSuMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> resolve) => new(resolve);
}
