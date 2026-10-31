#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal void PrepareHuanpeiOriginalParent(HuanpeiPaidOrigin p)
    {
        if (p.Request.ParentSkillId is null || p.Request.Intent == HuanpeiIntent.WenyiPeach)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(p.Request.ParentFrameId!.Value);
        if (!HuanpeiRequestParentCurrent(p.Request) || f.HuanpeiReturn is not null)
            throw new InvalidOperationException("Huanpei cannot replace another native request lease.");
        f = f with
        {
            HuanpeiReturn = new(p, 0, 0)
        };
        if (p.Request.Intent == HuanpeiIntent.KeBiNengBasic)
        {
            var k = f.KeBiNengPingduan!;
            var a = new LegalAction(CoreNativeCardClassificationsRuntime.IsSlashCard(p.Kind) ? LegalActionKind.Slash : p.Kind == CardKind.Peach ? LegalActionKind.Peach : LegalActionKind.Alcohol, 0, p.Targets.Count == 0 ? null : p.Targets[0], "环佩：真实零实体基本牌", p.Kind, TargetSeats: p.Targets)
            {
                ConversionSource = p.Source
            };
            f = f with
            {
                KeBiNengPingduan = k with
                {
                    Stage = KeBiNengPingduanStage.BasicIssuing,
                    BasicActions = k.BasicActions.Append(a).ToArray(),
                    SelectedBasicAction = a
                }
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiKeActionAcceptedEvent(f.Id, f.InstructionIndex, k.Source, f.GameplayHash, p.ProgramFrameId, p.Kind));
        }
        else if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(f))
            f = f with
            {
                LvKuangLvXiang = f.LvKuangLvXiang!with
                {
                    Stage = "issuing",
                    IssuingIds = [],
                    IssuingKind = p.Kind,
                    IssuingProvider = p.Source.OwnerSeat
                }
            };
        else if (_characterWuJingOlOperations.IsWuJingHejiRequest(f))
            f = f with
            {
                WuJing = f.WuJing!with
                {
                    Stage = WuJingStage.IssuingUse,
                    Selection = new(p.Kind, false, false)
                    {
                        Conversions = [p.Source]
                    },
                    Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                    After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
                }
            };
        else if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(f))
        {
            var c = f.CaoChun!;
            f = f with
            {
                CaoChun = c with
                {
                    Stage = CaoChunStage.NativeSlash,
                    Slash = new(f.Id, c.Source, c.GameplayHash, c.InstructionIndex, c.ActualTurn, c.PhaseInstance, false, p.Targets.Single(), p.Kind)
                    {
                        ProviderSeat = p.Source.OwnerSeat,
                        Materials = [],
                        PaymentBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
                    }
                }
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f);
    }

    internal bool HasHuanpeiGenericBasic(ProgramSkillFrame f) => f.KeBiNengPingduan is { Stage: KeBiNengPingduanStage.BasicChoice } k && _characterKeBiNengOlOperations.ValidKeBiNeng(f) && HuanpeiSources(_coreMatchParticipantsPlayersAccess._players[k.ActorSeat]).Any(s => CanAdjustHuanpeiHand(_coreMatchParticipantsPlayersAccess._players[k.ActorSeat]) && HuanpeiNativePlayActions(_coreMatchParticipantsPlayersAccess._players[k.ActorSeat], s, true).Count > 0);
    internal bool HasAcceptedHuanpeiKeAction(ProgramSkillFrame f, LegalAction action)
    {
        if (f.HuanpeiReturn is not { Payment: { } p } || !ValidHuanpeiPayment(p) || p.Request.Intent != HuanpeiIntent.KeBiNengBasic || p.Request.ParentFrameId != f.Id || p.Request.Cursor != f.InstructionIndex || f.KeBiNengPingduan is not { } k || p.Source.OwnerSeat != k.ActorSeat || action.CardId != 0 || action.SelectableCardIds.Count != 0 || action.ConversionSource != p.Source || action.PlayedCardKind != p.Kind || !action.TargetSeats.SequenceEqual(p.Targets) || action.Kind != (CoreNativeCardClassificationsRuntime.IsSlashCard(p.Kind) ? LegalActionKind.Slash : p.Kind == CardKind.Peach ? LegalActionKind.Peach : LegalActionKind.Alcohol))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiKeActionAcceptedEvent>().Count(e => e == new HuanpeiKeActionAcceptedEvent(f.Id, f.InstructionIndex, k.Source, f.GameplayHash, p.ProgramFrameId, p.Kind)) == 1;
    }
}
