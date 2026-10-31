#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuHongOlRuntime
{
    internal void ArmProgramGameDamageBonus(ProgramSkillFrame frame, string skillId, int ownerSeat, int amount)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var bindingId = skillId == active.SkillId ? CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active) : _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program!.Triggers.Single(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.TuxingArmGameDamage)).Id;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramGameDamageBonusArmedEvent(active.Id, skillId, bindingId, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId), ownerSeat, amount));
    }

    // Shared consumption: every damage the armed seat deals carries the armed
    // amount, whatever the card kind and including chain propagation — the OL
    // text has no non-transferred-damage restriction.
    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 40)]
    internal IReadOnlyList<(CardUseEffectSource Source, int Amount)> GetArmedGameDamageBonuses(GameEngine.IDamageAttempt attack)
    {
        if (attack.IsSourceLess || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive)
            return[];
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramGameDamageBonusArmedEvent>().Where(fact => fact.OwnerSeat == attack.SourceSeat).Select(fact => (new CardUseEffectSource(fact.SkillId, fact.BindingId, fact.OwnerSeat, fact.SkillInstanceId), fact.Amount)).ToArray();
    }

    SkillProgramStepOutcome ILiuHongProgramHost.YujueResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => YujueProgramResolve(frame, effect);
    SkillProgramStepOutcome ILiuHongProgramHost.ZhihuExpire(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhihuProgramExpire(frame, effect);
    SkillProgramStepOutcome ILiuHongProgramHost.TuxingArmGameDamage(ProgramSkillFrame frame, SkillProgramEffect effect) => TuxingProgramArmGameDamage(frame, effect);
}
