#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMouRuntime
{
    internal ProgramSkillFrame? MouLuSuSlotParent(EquipmentSlotNamedUseDraft slot) => slot.Intent == EquipmentSlotNamedUseIntent.Play && slot.OriginalParentId is { } id && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == id)is ProgramSkillFrame { MouLuSuAnniversary: { Stage: MouLuSuAnniversaryStage.Issuing, UseId: 0, SelectedAction: { } action, OriginalDecision: { } original } r } f && MouLuSuAnniversary.Valid(f) && slot.Source == action.ConversionSource && slot.EffectiveKind == action.PlayedCardKind && slot.Source.OwnerSeat == r.Other && slot.OriginalDecision.PromptId == original.PromptId && slot.PlayAction is not null && System.Text.Json.JsonSerializer.Serialize(slot.PlayAction) == System.Text.Json.JsonSerializer.Serialize(action) ? f : null;
    internal bool MouLuSuPaymentParent(long paymentId, long? parent, long? request, int cursor, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets)
    {
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == paymentId);
        return at > 0 && parent == request && _resolutionStackOperations._resolutionStack[at - 1] is ProgramSkillFrame { MouLuSuAnniversary: { Stage: MouLuSuAnniversaryStage.Issuing, UseId: 0, ActionId: 0, SelectedAction: { } a, OriginalDecision: { } original } r } f && f.Id == parent && f.InstructionIndex == cursor && MouLuSuAnniversary.Valid(f) && r.Other == source.OwnerSeat && a.ConversionSource == source && a.PlayedCardKind == kind && a.TargetSeats.SequenceEqual(targets) && original.PlayerSeat == r.Other && original.PromptId.Value > 0;
    }

    internal bool IsMouLuSuPaymentFrame(DrawFundedDistinctBasicFrame p) => p.Payment.Intent == DrawFundedDistinctBasicIntent.Play && MouLuSuPaymentParent(p.Id, p.Payment.ParentFrameId, p.Payment.RequestFrameId, p.Payment.Cursor, p.Payment.Source, p.Payment.EffectiveKind, p.TargetSeats);
    internal bool IsMouLuSuPaymentFrame(ChainedStateBasicFrame p) => p.Payment.Intent == ChainedStateBasicIntent.Play && MouLuSuPaymentParent(p.Id, p.Payment.ParentFrameId, p.Payment.RequestFrameId, p.Payment.Cursor, p.Payment.Source, p.Payment.EffectiveKind, p.TargetSeats);
    internal bool TryCancelMouLuSuPayment(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuSuAnniversary: { Stage: MouLuSuAnniversaryStage.Issuing, UseId: 0 } r } f || f.Id != id || !MouLuSuAnniversary.Valid(f))
            return false;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuSuAnniversary = r with { Stage = MouLuSuAnniversaryStage.UseChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        MouLuSuAnniversary.Continue(id);
        return true;
    }
}
