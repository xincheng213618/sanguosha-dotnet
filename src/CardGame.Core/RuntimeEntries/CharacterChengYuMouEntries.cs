#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime))]
internal sealed partial class CharacterChengYuMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> _resolve;
    private CharacterChengYuMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengYuMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> resolve) => new(resolve);
}
