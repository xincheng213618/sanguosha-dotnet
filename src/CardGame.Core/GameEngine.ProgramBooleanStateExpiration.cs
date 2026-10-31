namespace CardGame.Core;
public sealed partial class GameEngine
{
    internal sealed record ProgramBooleanStateLease(CardUseEffectSource Source, string StateId, int TurnNumber, int TurnSeat, SkillProgramDamageModifierExpiration Expiration);
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramBooleanStateLease> _programBooleanStateLeases => ref _programBooleanStateExpiration.ReferenceToProgramBooleanStateLeases;
}
