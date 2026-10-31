#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterChengPuMobileShiRuntime))]
internal sealed partial class CharacterChengPuMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuMobileShiRuntime> _resolve;
    private CharacterChengPuMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengPuMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuMobileShiRuntime> resolve) => new(resolve);
}
