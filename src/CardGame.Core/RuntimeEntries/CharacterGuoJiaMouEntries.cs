#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime))]
internal sealed partial class CharacterGuoJiaMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> _resolve;
    private CharacterGuoJiaMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoJiaMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> resolve) => new(resolve);
}
