#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSanshouArmorRuntime
{
    internal const string ShanheSanshouArmorSkill = "shanhe:sanshou-armor-3278";
    internal static bool IsNativeSanshouSkill(string skillId) => skillId is "anniversary-god:sanshou" or "happy:sanshou" or ShanheSanshouArmorSkill;
    // Only the physical armor grant is filtered. A character's printed
    // anniversary/happy Sanshou remains a character skill under armor bypass.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 20776, SkillProgramEffectOp.ResolveAnniversaryZhangJiaoSanshou)]
    internal bool CanOfferShanheSanshouArmor(ProgramTriggerLegalityContext context)
    {
        if (context.SkillId != ShanheSanshouArmorSkill)
            return true;
        return context.WindowContext is { Window: SkillProgramTriggerWindow.BeforeDamageApplied } window && window.TargetSeat == context.OwnerSeat && !_coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(context.Owner) && _characterHuCheErMouOperations.KongwuEffectiveEquipment(context.Owner).Any(card => card.Kind == CardKind.ShanheSanshou && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(context.OwnerSeat, context.SkillId, context.Candidate.SkillInstanceId, card)) && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } damage && damage.TargetSeat == context.OwnerSeat && !damage.IgnoresArmor && !_coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(damage.ResolutionId, context.OwnerSeat);
    }
}
