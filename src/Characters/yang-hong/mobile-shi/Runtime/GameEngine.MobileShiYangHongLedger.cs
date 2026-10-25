namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HasShiYhCapability =>
        _contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveShiYhJijian) ||
        _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiYhYuanmo);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68640, SkillProgramEffectOp.ResolveShiYhYuanmo)]
    private bool CanOfferShiYhYuanmo(ProgramTriggerLegalityContext c) =>
        c.SkillId == ShiYangHongComposition.YuanmoSkill && c.Owner.IsAlive && _winner == Winner.None &&
        c.Context is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } or
            { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } &&
        ProgramEventHistory<ShiYhYuanmoUsedEvent>().Count(entry =>
            entry.OwnerSeat == c.OwnerSeat && entry.Turn == _turnNumber) < 2 &&
        ShiYhFieldMovesExist();
}
