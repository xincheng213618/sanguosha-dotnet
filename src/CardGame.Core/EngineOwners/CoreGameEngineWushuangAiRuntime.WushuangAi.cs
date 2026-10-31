#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangAiRuntime
{
    internal void ContinueWushuangAiShopping(CharacterState player, WushuangIntermissionFrame supplied)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not WushuangIntermissionFrame frame || frame.Id != supplied.Id || frame.SeatCursor != player.Seat || frame.Acquisition is not null || player.IsHuman)
            throw new InvalidOperationException("Wushuang AI shopping lost its owning intermission.");
        // The limit belongs to the original window and survives acquisition
        // children. No cost or shop action is repeated when such a child returns.
        if (!frame.Opening && frame.AiShopActions >= 12)
        {
            _coreWushuangOperations.AdvanceWushuangShopSeat(frame);
            return;
        }

        var progress = _coreWushuangModeProjectionOperations.CreateWushuangSnapshot(player.Seat)!;
        var decision = _coreAiBrainsAiBrainsAccess._aiBrains[player.Seat].ChooseWushuangShop(_corePlayerViewProjectionOperations.CreateSnapshot(player.Seat), _coreWushuangModeProjectionOperations.CreateWushuangSnapshot(player.Seat)!, _contentRegistryOperations._contentRegistry);
        if (frame.Opening && decision.Action != WushuangShopAction.Buy)
            throw new InvalidOperationException("Wushuang AI must select one published opening tactic.");
        switch (decision.Action)
        {
            case WushuangShopAction.Buy:
            {
                var offer = progress.Offers.SingleOrDefault(candidate => candidate.Id == decision.OfferId);
                if (_coreWushuangOperations.ValidateWushuangPurchase(player.Seat, offer, decision.ReplaceSkillId)is { } error)
                    throw new InvalidOperationException("Wushuang AI chose an invalid purchase: " + error);
                _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame = frame with { AiShopActions = frame.AiShopActions + 1 });
                _coreWushuangOperations.BuyWushuangOffer(player.Seat, offer!, decision.ReplaceSkillId, frame.Opening);
                if (_coreWushuangAcquisitionOperations.TryBeginWushuangAcquisition(frame.Id, offer!))
                    return;
                if (frame.Opening)
                    _coreWushuangOperations.AdvanceWushuangShopSeat(frame);
                else
                    _coreWushuangOperations.CompleteWushuangShopAction(frame.Id);
                return;
            }

            case WushuangShopAction.Reroll:
                if (frame.Opening || progress.FreeRerolls == 0 && progress.Tokens < 1)
                    throw new InvalidOperationException("Wushuang AI cannot refresh an unpaid or opening page.");
                _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame = frame with { AiShopActions = frame.AiShopActions + 1 });
                if (progress.FreeRerolls > 0)
                    _coreWushuangTacticRulesOperations.ChangeWushuangFreeRerolls(player.Seat, -1, "refresh");
                else
                    _coreWushuangOperations.ChangeWushuangTokens(player.Seat, -1, "refresh");
                _coreWushuangOperations.RefreshWushuangOffers(player.Seat);
                _coreWushuangOperations.CompleteWushuangShopAction(frame.Id);
                return;
            case WushuangShopAction.Lock:
                _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame = frame with { AiShopActions = frame.AiShopActions + 1 });
                _coreWushuangOperations.RequireWushuangFeature.ToggleShopLock(player.Seat);
                _coreWushuangOperations.CompleteWushuangShopAction(frame.Id);
                return;
            case WushuangShopAction.Finish:
                _coreWushuangOperations.AdvanceWushuangShopSeat(frame);
                return;
            default:
                throw new InvalidOperationException("Unknown Wushuang AI shop action.");
        }
    }
}
