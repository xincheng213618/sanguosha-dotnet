namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static bool IsClassicQiuyuanAid(string skillId) => skillId == "classic:qiuyuan";
    private static bool SameTypeAidIgnoresSlashDistance(string skillId) =>
        IsClassicQiuyuanAid(skillId) || skillId == "boundary:qiuyuan-mobile";
    private static IReadOnlyList<CardZoneKind> SameTypeAidPaymentZones(string skillId) =>
        IsClassicQiuyuanAid(skillId) ? [CardZoneKind.Hand] : [CardZoneKind.Hand, CardZoneKind.Equipment];
    private static bool IsSameTypeAidPaymentKind(string skillId, CardKind paid, CardKind effective) =>
        IsClassicQiuyuanAid(skillId) ? IsSlashCard(effective) && paid == CardKind.Dodge : IsSameTypeAidCard(paid, effective);
    private static bool MatchesClassicQiuyuanAidContext(string skillId, int owner, ProgramSkillWindowContext context, SameTypeAidIdentity use) =>
        !IsClassicQiuyuanAid(skillId) || IsSlashCard(use.EffectiveKind) && use.ActorSeat != owner && use.OriginalOwnerTarget == owner &&
        context.TargetSeat == owner && (context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized
            ? context.CardUse?.EventTargetSeat == owner && context.Facts?.CardActionTargetIsOwner == true
            : context.Window == SkillProgramTriggerWindow.OtherActualUseTargeted && context.ActualUseTarget is
                { ActionId: null, LegacyProducerProgramId: not null } legacy && legacy.TargetSeat == owner);
}
