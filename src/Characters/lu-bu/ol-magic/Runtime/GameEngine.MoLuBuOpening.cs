namespace CardGame.Core;

public sealed partial class GameEngine
{
    public bool IsMoLuBuOpeningPhase =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime>().IsMoLuBuOpeningPhase;
}
