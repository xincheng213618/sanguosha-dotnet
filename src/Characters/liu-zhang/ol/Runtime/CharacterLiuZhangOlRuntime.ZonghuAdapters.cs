#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuZhangOlRuntime
{

    internal bool HasZonghuSpecializedPayment(ProgramSkillFrame f, CardUseFrame? use = null)
    {
        if (f.ZonghuReturn is not { } r || r.Origin is not { } p || !ValidZonghuPayment(p) || p.Need.OwnerFrameId != f.Id || p.Need.RequestFrameId != f.Id || p.Need.InstructionIndex != f.InstructionIndex || !CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuAttemptResolvedEvent>().Count(e => e.FrameId == p.AttemptId && e.Matched && e.NativeLegal) != 1)
            return false;
        if (use is not null && (use.ZonghuOrigin != p || r.UseId != use.Id || r.ActionId != use.Action?.ActionId || !IsIssuedZonghuUse(use)))
            return false;
        if (f.SunYuan is not null)
            return HasSayingZonghuPayment(f, use);
        if (f.Chanshuang is { } cs)
            return _characterLiYiOlOperations.TryGetChanshuangSlashRequest(f, out var actor, out var targets) && actor == p.Need.ActorSeat && targets.SequenceEqual([p.TargetSeat]) && cs.Stage is ChanshuangStage.SlashIssuing or ChanshuangStage.NativeUse or ChanshuangStage.NativeChildren && cs.Selection is { Materials.Count: 0, Sources: [var csSource] } selection && csSource == p.Source && selection.ActorSeat == p.Need.ActorSeat && selection.ProviderSeat == p.Need.ProviderSeat && selection.TargetSeat == p.TargetSeat && selection.Kind == p.OutputKind && (cs.Stage != ChanshuangStage.NativeChildren || r.Returned && cs.NativeReturned && cs.Native is { } csNative && csNative.UseId == r.UseId && csNative.ActionId == r.ActionId && csNative.Selection == selection && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is null && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuUseReturnedEvent>().Count(e => e.AttemptId == p.AttemptId && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ChanshuangSlashReturnedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == cs.Cursor && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1);
        if (f.WuJing is { } wu)
            return wu.Source == _characterWuJingOlOperations.WuJingSource(f) && wu.GameplayHash == f.GameplayHash && wu.Stage is WuJingStage.IssuingUse or WuJingStage.NativeUse or WuJingStage.NativeChildren && wu.TargetSeat == p.TargetSeat && f.OwnerSeat == p.Need.ActorSeat && wu.Selection is { CardIds.Count: 0, Conversions: [var source] } && source == p.Source && wu.Selection.Kind == p.OutputKind && (wu.Stage != WuJingStage.NativeChildren || r.Returned && wu.NativeCompleted && wu.NativeOrigin is { } native && native.UseId == r.UseId && native.ActionId == r.ActionId && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is null && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuUseReturnedEvent>().Count(e => e.AttemptId == p.AttemptId && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuJingHejiReturnedEvent>().Count(e => e.ProgramFrameId == f.Id && e.UseId == r.UseId) == 1);
        if (f.LvKuangLvXiang is { } qi)
            return qi.Source == CharacterLvKuangLvXiangOlRuntime.LvKuangLvXiangSource(f) && qi.GameplayHash == f.GameplayHash && qi.Stage is "issuing" or "native" && qi.Actor == p.Need.ActorSeat && qi.Target == p.TargetSeat && qi.IssuingProvider == p.Need.ProviderSeat && qi.IssuingIds.Count == 0 && qi.IssuingKind == p.OutputKind;
        return f.CaoChun is { Stage: CaoChunStage.NativeSlash, Slash: { } n } cc && cc.Source == CharacterCaoChunOlRuntime.CaoChunSource(f) && cc.GameplayHash == f.GameplayHash && n.ParentFrameId == f.Id && !n.Ending && n.Materials.Count == 0 && n.Kind == p.OutputKind && n.TargetSeat == p.TargetSeat && f.OwnerSeat == p.Need.ActorSeat && n.ProviderSeat == p.Need.ProviderSeat;
    }

    internal bool HasAcceptedZonghuKeAction(ProgramSkillFrame f, LegalAction a)
    {
        if (f.KeBiNengPingduan is not { } r || f.ZonghuKeAcceptedOrigin is not { } p || !ValidZonghuPayment(p) || p.Need.Intent != ZonghuIntent.GenericBasic || p.Need.OwnerFrameId != f.Id || p.Need.InstructionIndex != f.InstructionIndex || p.Need.ActorSeat != r.ActorSeat || p.Need.ProviderSeat != r.ActorSeat || a.CardId != 0 || a.SelectableCardIds.Count != 0 || a.ConversionSource != p.Source || a.PlayedCardKind != p.OutputKind || a.Kind != (CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind) ? LegalActionKind.Slash : p.OutputKind == CardKind.Peach ? LegalActionKind.Peach : LegalActionKind.Alcohol) || !a.TargetSeats.SequenceEqual(p.OutputKind == CardKind.Alcohol ? [] : new[] { p.TargetSeat }))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuKeActionAcceptedEvent>().Count(e => e == new ZonghuKeActionAcceptedEvent(f.Id, f.InstructionIndex, r.Source, f.GameplayHash, p.AttemptId, p.OutputKind, p.TargetSeat)) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuAttemptResolvedEvent>().Count(e => e.FrameId == p.AttemptId && e.Matched && e.NativeLegal) == 1;
    }

    internal bool HasSayingZonghuPayment(ProgramSkillFrame f, CardUseFrame? use = null)
    {
        if (f.SunYuan is not { } s || f.ZonghuReturn is not { } r || !ValidZonghuPayment(r.Origin) || !_characterSunYuanOlOperations.TryGetSayingSlashRequest(f, out var actor, out var targets) || r.Origin.Need.ActorSeat != actor || !targets.SequenceEqual([r.Origin.TargetSeat]) || r.Origin.Need.OwnerFrameId != f.Id || r.Origin.Need.RequestFrameId != f.Id || r.Origin.Need.InstructionIndex != f.InstructionIndex || s.Stage is not (SunYuanStage.SlashIssuing or SunYuanStage.NativeUse or SunYuanStage.NativeChildren) || s.Selection is not { Materials.Count: 0, Sources: [var source] } selection || source != r.Origin.Source || selection.ActorSeat != actor || selection.ProviderSeat != r.Origin.Need.ProviderSeat || selection.Kind != r.Origin.OutputKind || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuAttemptResolvedEvent>().Count(e => e.FrameId == r.Origin.AttemptId && e.Matched && e.NativeLegal) != 1)
            return false;
        if (use is not null && (use.ZonghuOrigin != r.Origin || r.UseId != use.Id || r.ActionId != use.Action?.ActionId || !IsIssuedZonghuUse(use)))
            return false;
        return s.Stage != SunYuanStage.NativeChildren || r.Returned && s.NativeReturned && s.Native is { } native && native.UseId == r.UseId && native.ActionId == r.ActionId && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is null && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SayingSlashReturnedEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1;
    }
}
