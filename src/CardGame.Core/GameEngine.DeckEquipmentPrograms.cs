namespace CardGame.Core;
public sealed record BeneficiarySuitShield(long Id, int BeneficiarySeat, Suit Suit, CardConversionSource Source);
public sealed record BeneficiarySuitShieldGrantedEvent(BeneficiarySuitShield Shield) : IGameEvent;
public sealed record RandomSkillGrantedEvent(int BeneficiarySeat, string SkillId, CardConversionSource Source) : IGameEvent;
