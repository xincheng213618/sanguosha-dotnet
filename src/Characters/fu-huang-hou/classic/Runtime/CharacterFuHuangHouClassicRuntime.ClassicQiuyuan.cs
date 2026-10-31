#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFuHuangHouClassicRuntime
{
    internal static bool IsClassicQiuyuanAid(string skillId) => skillId == "classic:qiuyuan";
    internal static bool SameTypeAidIgnoresSlashDistance(string skillId) => IsClassicQiuyuanAid(skillId) || skillId == "boundary:qiuyuan-mobile";
    internal static IReadOnlyList<CardZoneKind> SameTypeAidPaymentZones(string skillId) => IsClassicQiuyuanAid(skillId) ? [CardZoneKind.Hand] : [CardZoneKind.Hand, CardZoneKind.Equipment];
    internal static bool IsSameTypeAidPaymentKind(string skillId, CardKind paid, CardKind effective) => IsClassicQiuyuanAid(skillId) ? global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(effective) && paid == CardKind.Dodge : global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime.IsSameTypeAidCard(paid, effective);
    internal static bool MatchesClassicQiuyuanAidContext(string skillId, int owner, ProgramSkillWindowContext context, SameTypeAidIdentity use) => !IsClassicQiuyuanAid(skillId) || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.EffectiveKind) && use.ActorSeat != owner && use.OriginalOwnerTarget == owner && context.TargetSeat == owner && (context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized ? context.CardUse?.EventTargetSeat == owner && context.Facts?.CardActionTargetIsOwner == true : context.Window == SkillProgramTriggerWindow.OtherActualUseTargeted && context.ActualUseTarget is { ActionId: null, LegacyProducerProgramId: not null } legacy && legacy.TargetSeat == owner);
}
