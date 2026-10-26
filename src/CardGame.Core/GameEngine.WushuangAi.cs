namespace CardGame.Core;

public sealed partial class GameEngine
{
    private void ContinueWushuangAiShopping(CharacterState player, WushuangIntermissionFrame supplied)
    {
        if (_resolutionStack.LastOrDefault() is not WushuangIntermissionFrame frame || frame.Id != supplied.Id ||
            frame.SeatCursor != player.Seat || frame.Acquisition is not null || player.IsHuman)
            throw new InvalidOperationException("Wushuang AI shopping lost its owning intermission.");
        // The limit belongs to the original window and survives acquisition
        // children. No cost or shop action is repeated when such a child returns.
        if (!frame.Opening && frame.AiShopActions >= 12)
        { AdvanceWushuangShopSeat(frame); return; }
        var progress = CreateWushuangSnapshot(player.Seat)!;
        var decision = _aiBrains[player.Seat].ChooseWushuangShop(CreateSnapshot(player.Seat),
            CreateWushuangSnapshot(player.Seat)!, _contentRegistry);
        if (frame.Opening && decision.Action != WushuangShopAction.Buy)
            throw new InvalidOperationException("Wushuang AI must select one published opening tactic.");
        switch (decision.Action)
        {
            case WushuangShopAction.Buy:
            {
                var offer = progress.Offers.SingleOrDefault(candidate => candidate.Id == decision.OfferId);
                if (ValidateWushuangPurchase(player.Seat, offer, decision.ReplaceSkillId) is { } error)
                    throw new InvalidOperationException("Wushuang AI chose an invalid purchase: " + error);
                ReplaceRuntimeFrame(frame.Id, frame = frame with { AiShopActions = frame.AiShopActions + 1 });
                BuyWushuangOffer(player.Seat, offer!, decision.ReplaceSkillId, frame.Opening);
                if (TryBeginWushuangAcquisition(frame.Id, offer!)) return;
                if (frame.Opening) AdvanceWushuangShopSeat(frame);
                else CompleteWushuangShopAction(frame.Id);
                return;
            }
            case WushuangShopAction.Reroll:
                if (frame.Opening || progress.FreeRerolls == 0 && progress.Tokens < 1)
                    throw new InvalidOperationException("Wushuang AI cannot refresh an unpaid or opening page.");
                ReplaceRuntimeFrame(frame.Id, frame = frame with { AiShopActions = frame.AiShopActions + 1 });
                if (progress.FreeRerolls > 0) ChangeWushuangFreeRerolls(player.Seat, -1, "refresh");
                else ChangeWushuangTokens(player.Seat, -1, "refresh");
                RefreshWushuangOffers(player.Seat);
                CompleteWushuangShopAction(frame.Id);
                return;
            case WushuangShopAction.Lock:
                ReplaceRuntimeFrame(frame.Id, frame = frame with { AiShopActions = frame.AiShopActions + 1 });
                RequireWushuangFeature.ToggleShopLock(player.Seat);
                CompleteWushuangShopAction(frame.Id);
                return;
            case WushuangShopAction.Finish:
                AdvanceWushuangShopSeat(frame);
                return;
            default:
                throw new InvalidOperationException("Unknown Wushuang AI shop action.");
        }
    }
}
