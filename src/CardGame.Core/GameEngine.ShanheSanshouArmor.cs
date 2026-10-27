namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheSanshouArmorSkill = "shanhe:sanshou-armor-3278";
    private static bool IsNativeSanshouSkill(string skillId) =>
        skillId is AnniversarySanshou or HappyZhangJiaoSanshou or ShanheSanshouArmorSkill;

    // Only the physical armor grant is filtered. A character's printed
    // anniversary/happy Sanshou remains a character skill under armor bypass.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 20776,
        SkillProgramEffectOp.ResolveAnniversaryZhangJiaoSanshou)]
    private bool CanOfferShanheSanshouArmor(ProgramTriggerLegalityContext context)
    {
        if (context.SkillId != ShanheSanshouArmorSkill) return true;
        return context.WindowContext is { Window: SkillProgramTriggerWindow.BeforeDamageApplied } window &&
            window.TargetSeat == context.OwnerSeat && !IsArmorIneffectiveForTurn(context.Owner) &&
            KongwuEffectiveEquipment(context.Owner).Any(card => card.Kind == CardKind.ShanheSanshou &&
                IsActiveProgramSourceEquipmentCard(context.OwnerSeat, context.SkillId, context.Candidate.SkillInstanceId, card)) &&
            CurrentDamageAttempt is { } damage && damage.TargetSeat == context.OwnerSeat &&
            !damage.IgnoresArmor && !HasDirectedCardArmorBypass(damage.ResolutionId, context.OwnerSeat);
    }
}
