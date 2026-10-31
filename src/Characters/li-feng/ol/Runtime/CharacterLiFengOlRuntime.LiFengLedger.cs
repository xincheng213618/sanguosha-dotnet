#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiFengOlRuntime
{
    internal const string JiyunInitialReason = "skill-program.li-feng.jiyun.initial", JiyunDrawReason = "skill-program.li-feng.jiyun.draw", JiyunCollectReason = "skill-program.li-feng.jiyun.collect", ShuliangGiftReason = "skill-program.li-feng.shuliang.gift";
    internal bool HasLiFengCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeJiyunCategoryGrain) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveJiyunPreparationDrawLeases) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.CollectJiyunCompletedUseEntities) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DistributeShuliangEndingGrain);

    internal static CardConversionSource LiFengSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_li_feng_ol_LiFengLedger.LiFengSource(f);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27591)]
    internal bool CanOfferLiFeng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [var effect] || !LiFengComposition.Handles(effect.Op))
            return true;
        if (!_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive)
            return false;
        if (effect.Op == SkillProgramEffectOp.InitializeJiyunCategoryGrain)
            return !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiFengStartedEvent>().Any(e => e.Op == effect.Op && e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillInstanceId == candidate.SkillInstanceId);
        if (effect.Op == SkillProgramEffectOp.DistributeShuliangEndingGrain)
            return _corePublicPileSourcesOperations.ReferencedPublicPileSources(candidate.OwnerSeat, "ol:jiyun", candidate.SkillInstanceId).Any(p => IsLiveJiyunPile(p) && _corePublicPileSourcesOperations.PublicPileCards(p).Count > 0);
        return effect.Op != SkillProgramEffectOp.ResolveJiyunPreparationDrawLeases || context.SourceSeat == candidate.OwnerSeat;
    }
}
