#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime))]
internal sealed partial class CharacterCaoAngMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> _resolve;
    private CharacterCaoAngMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoAngMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> resolve) => new(resolve);
}
