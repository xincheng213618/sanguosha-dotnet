#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    internal bool IsMoQuanyuAddedTargetFact(CardUseFrame use, MoQuanyuTargetAddedEvent fact) => use.Action is { Type: CardActionType.Use } a && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && fact.UseId == use.Id && fact.ActionId == a.ActionId && fact.Source.SkillId == "ol:mo-quanyu" && fact.Source.BindingId == "singleton-slash-effects" && fact.Source.OwnerSeat == use.SourceSeat && fact.TargetIndex >= 1 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(fact.TargetSeat) && fact.TargetSeat != use.SourceSeat && _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program?.GameplayHash == fact.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashStartedEvent>().Count(e => e.FrameId == fact.FrameId && e.Source == fact.Source && e.GameplayHash == fact.GameplayHash && e.Original.UseId == use.Id && e.Original.ActionId == a.ActionId && (e.Mask & MoQuanyuBit(MoQuanyuEffect.Qingming)) != 0) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.FrameId && e.SkillId == fact.Source.SkillId && e.BindingId == fact.Source.BindingId && e.OwnerSeat == fact.Source.OwnerSeat && e.SkillInstanceId == fact.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuTargetAddedEvent>().Count(e => e.FrameId == fact.FrameId && e.UseId == use.Id) == 1;
    internal bool HasMoQuanyuAddedTargetTail(CardUseFrame use) => _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuTargetAddedEvent>().Any(e => IsMoQuanyuAddedTargetFact(use, e));
    internal bool ValidMoQuanyuSlash(ProgramSkillFrame f) => f.MoQuanyuSlash is { } r && f.SkillId == "ol:mo-quanyu" && f.TriggerId == "singleton-slash-effects" && f.InstructionIndex == 1 && r.Source == new CardConversionSource(f.SkillId, f.TriggerId, f.OwnerSeat, f.SkillInstanceId) && r.GameplayHash == f.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && r.Mask is> 0 and <= 63 && r.Stage == MoQuanyuSlashStage.AdditionalTarget && f.WindowContext is { } context && TryGetMoQuanyuSingletonUse(f.OwnerSeat, context, out var use, out _, out _) && use.MoQuanyuSoleUse == r.Original && use.MoQuanyuSlashPolicy is null && _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Original == r.Original && e.Mask == r.Mask && e.Entered == r.Entered) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashAppliedEvent>().Any(e => e.FrameId == f.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28912)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28912)]
    internal bool ContinueIssuedMoQuanyuSlash(ProgramSkillFrame f) => f.MoQuanyuSlash is not null && ValidMoQuanyuSlash(f);
}
