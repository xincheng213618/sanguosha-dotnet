#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSimaShiMouRuntime))]
internal sealed partial class CharacterSimaShiMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaShiMouRuntime> _resolve;
    private CharacterSimaShiMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaShiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaShiMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaShiMouRuntime> resolve) => new(resolve);
}
