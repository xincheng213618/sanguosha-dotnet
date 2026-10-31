#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangHongMobileShiRuntime
{
    internal bool HasShiYhCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveShiYhJijian) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiYhYuanmo);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68640, SkillProgramEffectOp.ResolveShiYhYuanmo)]
    internal bool CanOfferShiYhYuanmo(ProgramTriggerLegalityContext c) => c.SkillId == ShiYangHongComposition.YuanmoSkill && c.Owner.IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && c.Context is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } or { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiYhYuanmoUsedEvent>().Count(entry => entry.OwnerSeat == c.OwnerSeat && entry.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber) < 2 && ShiYhFieldMovesExist();
}
