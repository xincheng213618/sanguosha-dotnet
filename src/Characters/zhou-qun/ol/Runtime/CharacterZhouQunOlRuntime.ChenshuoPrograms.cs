#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouQunOlRuntime
{
    internal void RevealNextChenshuo(ProgramSkillFrame f)
    {
        BeginZhouQunMovement(ref f, ZhouQunStage.RevealChildren);
        var r = f.ZhouQun!;
        var card = _corePhysicalDrawFacadesOperations.DrawOneToProcessing(new(ChenshuoRevealReason));
        if (card is null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhouQun = r with { Repeat = false } });
        else
        {
            var shown = r.Shown!;
            var matched = global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind) == global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(shown.Kind) || card.Suit == shown.Suit || card.Rank == shown.Rank || ZhouQunNameLength(card.Kind) == shown.NameLength;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r with { Revealed = r.Revealed.Append(card.Id).ToArray(), Repeat = matched } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenshuoTopRevealedEvent(f.Id, r.Revealed.Count, card.Id, card.Kind, card.Suit, card.Rank, ZhouQunNameLength(card.Kind), matched));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.TriggerId!, f.OwnerSeat, "chenshuo-top", [_coreSnapshotValueProjectionOperations.ToSnapshot(card)]));
        }

        CompleteZhouQunMovement(f.Id);
    }
}
