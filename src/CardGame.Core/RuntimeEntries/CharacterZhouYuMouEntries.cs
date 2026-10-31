#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime))]
internal sealed partial class CharacterZhouYuMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> _resolve;
    private CharacterZhouYuMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> resolve) => new(resolve);
}
