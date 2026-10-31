namespace CardGame.Core;
public sealed record BeneficiarySuitShield(long Id, int BeneficiarySeat, Suit Suit, CardConversionSource Source);
public sealed record BeneficiarySuitShieldGrantedEvent(BeneficiarySuitShield Shield) : IGameEvent;
public sealed record RandomSkillGrantedEvent(int BeneficiarySeat, string SkillId, CardConversionSource Source) : IGameEvent;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.BeneficiarySuitShield> _beneficiarySuitShields => ref _deckEquipmentPrograms.ReferenceToBeneficiarySuitShields;
    private ref long _beneficiarySuitShieldSequence => ref _deckEquipmentPrograms.ReferenceToBeneficiarySuitShieldSequence;
}
