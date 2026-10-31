#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChengPuMobileShiRuntime))]
internal sealed partial class CharacterChengPuMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterChengPuMobileShiRuntime> _resolve;
    private CharacterChengPuMobileShiEntries(Func<EngineOwners.CharacterChengPuMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengPuMobileShiEntries Create(Func<EngineOwners.CharacterChengPuMobileShiRuntime> resolve) => new(resolve);
}
