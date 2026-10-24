namespace CardGame.Core;

public interface IBossEncounterFlowFeature : IGameModeFeature { }
public interface IShenwuBattleFeature : IGameModeFeature
{
    bool IsPlayerSlot(int seat);
    int ReplacementDrawBonus(int seat);
    bool AllowsTierTrigger(int seat, string skillId);
}

public sealed record ModeDamageCapPlan(int OwnerSeat, string SkillId, string BindingId, int Maximum);

public interface IJiangeBattleFeature : IGameModeFeature
{
    ModeDamageCapPlan? AlliedDamageCap(int targetSeat);
}
public interface IShanheBattleFeature : IGameModeFeature { }
