#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterJiaXuMouRuntime))]
internal sealed partial class CharacterJiaXuMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiaXuMouRuntime> _resolve;
    private CharacterJiaXuMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiaXuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiaXuMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiaXuMouRuntime> resolve) => new(resolve);
}
