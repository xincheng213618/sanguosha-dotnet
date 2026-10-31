#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterQuYiClassicRuntime))]
internal sealed partial class CharacterQuYiClassicEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuYiClassicRuntime> _resolve;
    private CharacterQuYiClassicEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuYiClassicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQuYiClassicEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuYiClassicRuntime> resolve) => new(resolve);
}
