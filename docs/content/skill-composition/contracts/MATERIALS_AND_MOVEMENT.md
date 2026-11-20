# Native materials and physical movement

Required when changing these mechanisms or their consumers. See the
[contract index](README.md) for related topics.

## Representations and registered entities

- Native card representations use `INativeCardRepresentations` and the original `CardUseFrame`: freeze the issued zero-entity face, retain physical printed faces separately from effective action appearance, and declare real carriers explicitly. Actual destruction/payment owners capture exact material departures from the physical journal; representation reads never reconstruct character proofs or authorize material movement. Existing claim and native-return protocols remain separate owners. A Card value without a registered zone has no physical-entity relation: `ICardZoneQueries.TryGetLocation` returns false, never an `OutsideGame` fallback; `GetLocation`, writes and payments remain strict.

- `CoreGameEngineFieldEquipmentRuntime.GetEquipmentWithPrintedEffects` reads the equipped entities whose printed effects apply. A current bone occupies its equipment slot as a bone. The same physical card contributes its printed effect when later equipped as an ordinary card. Kongwu's issued restriction independently gates the holder's equipment effects.

- Native Wooden Ox storage admission reads effective equipment when publishing and validating the action. The stored hand card moves face down. An optional transfer reads the effective Wooden Ox at its existing transfer boundary, after storage. Use of stored cards qualifies their original material separately, and the equipment departure resolves grain from the actual old and new locations.

- The playable-card query publishes an available declared card with the original hand. Otherwise the formal Wooden Ox rules append the owner's grain after the hand when the field contains an effective Wooden Ox. The field equipment owner reads the original equipment zone through `ICardZoneQueries`.

- Silver Lion departure starts from the original card and equipment location after the actual move. The original holder's typed equipment suppression applies at this boundary, alongside the existing health and armor conditions. Recovery belongs to the original movement continuation.
- Zixi's Qin placement drains post-instruction windows with `Program` on the paid placement frame. A direct Silver Lion HP child and a queued recovery replacement both return to that frame. The HP child inside a replacement returns to the replacement itself. The paid entity identifies the original armor holder independently of the seat receiving the Qin in Judgment.

## Claims, deliveries and retirement

- Actual damage-card claims are captured once on the original CardUseFrame by CardUseMaterialClaimRuntime from physical movement delivery. Extra-effect episodes do not copy claim ledgers; separate the persistent claim fact from current material-retirement destinations, and retain non-CardUse damage owners' own protocols.

- A damage-card claim may retain its skill invocation once the applied damage's physical cards reach the recipient's hand. The damage window grants that specific child permission to finish after skill loss. An empty claim grants no permission, and the owner's death still stops outstanding skill effects.

- Native material dispositions share `INativeMaterialClaims` and the original CardUse/Judgment frame. Keep accepted claims, issued transfers, conditional child reservations, non-Processing retained reads and actual deliveries distinct; only actual movements carry delivery sequences. Child reservations settle from the original physical entry before typed business returns. Read representation, relinquished retirement ownership and completion destination permission separately; reentered Processing must not be retired twice, and default damage claims must not gain Equipment/Judgment/Outside completion permission. Paid source-loss continuations reuse the original window's ProgramInvocations, independently of owner-death permission.

- Actual native material deliveries use `INativeMaterialClaims` and the original use or delayed Judgment frame's `MaterialClaims`. Capture real journal deliveries once; distinguish representation residency from the original owner's retirement right, which is relinquished after delivery even if that entity later reenters Processing. Keep accepted promises, child handoffs and producer retention separate from delivered acquisition; no character proof scan may replace this shared delivery fact.

- A post-use claim that waits for movement children retains the material delivery outcome from the completed original CardUse frame. A later discard of an already delivered entity leaves that original claim unavailable.

- Once physical Lightning starts resolving damage, the original Judgment frame records its first departure from the judgment zone. A completion claim can use the original native finish or death-cleanup discard entry while that entry survives. An earlier move to hand ends that Judgment's residency even if the same card later returns to the discard pile.

- A pair Pindian retains the source's original draw-pile reservation sequence on its own frame. Its unrevealed source and any replacement that returns the reserved card must still refer to that Processing entry. A read of the opponent's draw-pile top leaves it in the draw pile until payment. Simultaneous Pindian keeps its own reservation receipts, and paid reveal and result claims retain their own lifetimes.

- A pair Pindian returns the discard-pile entries actually delivered by its own cleanup with the native result binding. Each side has a separate receipt. A card obtained during an earlier window has no cleanup receipt even if a later discard moves the same physical card into the pile. A consumer rechecks the exact entry before acquiring it.

- Before publishing revealed Pindian materials, the pair or simultaneous contest records each physical Processing entry on its original frame. An already reserved card keeps the original reservation sequence. A reveal swap changes seat assignments and leaves the entry attached to the physical card. Zongshi and Shenduan compare their current eligible card with this entry before moving it. Their paid child continuations use their own settlement facts.

- A pair's ordinary result claims and native cleanup read those same reveal entries. The published choice retains its original claimant and result identities. Execution rechecks each physical entry before moving it. Declining or exhausting the choices leaves native cleanup responsible for the original entries still in Processing.

- Maximum-Slash claims use the original reveal entries in both pair and simultaneous comparisons. The initial freeze takes the highest effective rank among the eligible Slash entries at that time. Later loss of a frozen entry does not substitute a lower-ranked Slash. Simultaneous cleanup follows the group's final card assignment order and moves only its surviving original entries, including during the native death path.

- The pair policy queue used when a Maximum-Slash policy is live includes eligible source-side all-card claims, even when no Slash qualifies. It follows participant order and checks all-card, ordinary and Maximum-Slash policies within each participant. A source-side all-card policy with no eligible original entry does not add a choice to that queue. The mobile Zhiba challenge keeps its separately issued lord claim before the policy queue.

## Native-use material retirement

- Record actual non-Processing payment or destruction through `INativeMaterialClaims.CaptureRetirement` on the original `CardUseFrame.MaterialClaims`. A `Retirement` has no acquisition recipient and does not contribute to `WasClaimed` or claimed entities. A `Delivery` retains its acquisition and residency semantics. Ordinary payment into Processing issues no retirement receipt.
- Validate original material, issuance cutoff, payment source and actual journal movements at the shared write boundary. For direct payment into OutsideGame, a deck or a public pile, use that payment as the causal movement. For material in Processing, accept the first departure from the original entry and a real destruction immediately following that departure into DiscardPile. Duplicate records are idempotent. Conflicting or fabricated batches leave all rights unchanged.
- When a real child use starts with material already in Processing, retain the actual entry sequence from before issuance. A zero-entity use's physical payment remains a resource cost even when it shares the same Action. Physical lifetime writers supply the exact departure. The shared owner selects the nearest real use tied to that entry and issues no receipt when no native material matches. Later discards, responses or reentry into Processing cannot record a new retirement on a prior owner that relinquished the material.
- `HasRelinquishedMaterial` and `PreservesMaterialAtCompletion` keep the original owner from cleaning up that material again, including after reentry into Processing. For a retirement receipt, `CanReadMaterial` permits representation reads at the captured destination only. Payment, acquisition and movement require their own authorization.
- Native producers capture the exact outcome for common Slash payments, 吴景's real trick payment, selected foreign cards, 管亥's non-response public-pile payment and actual destruction of general cards, chariots, soul cards and ephemeral entities. The common completion, sequential trick, group, Amazing Grace, delayed-placement and material-eligibility paths read those facts. Issuance qualification, payment child windows and typed business returns stay with their producers.
- A resolved global attack's discard claim reads only real `UseFinished` entries produced by that group's native cleanup. Check the original discard entry before delivering each material. The claimant is the first eligible living holder other than the actual card user. Earlier native deliveries and later discards leave the original cleanup claim empty for those entities.

- Slash, global attack and ordinary-use cleanup share `DiscardFinishedUseMaterial` and `StoreFinishedUseDiscards`. They record actual `UseFinished` discard entries on the original `CardUseFrame` before its completion window. A claim checks that the matching entry is still current. A rule waiting for a child use can carry its exact entry back to the original program frame. Prior deliveries and materials still in their original Processing entry follow their own eligibility rules.
- First-category completed placement checks relinquishment on the native use whose frame and action match the opportunity. A completed response can use a different card's frame as its parent. Equipment eligibility follows its actual placement receipt.
- A paid equipment use whose slot is abolished before installation still owns the exit of its original Processing material. A native return checks the accepted use and entry at that exit. Equipment already installed follows the usual equipment-loss rules.

The shared receipt covers actual CardUse payment and destruction. Pure response
materials and delayed Judgment destruction keep their own scopes. Rule-specific
native issuance and business returns remain separate composition work, including
dependency direction in the rest of native card-use scheduling.

## Acquisition, Give and exact movement requests

- Whole discard-phase settlement uses `IActualDiscardRecoveryPhaseQueries.CurrentPhaseDiscards` to read genuine discards from the original physical journal after the actual phase's starting movement sequence. Freeze eligible discard-pile entries on the issuing program frame and use `StillAvailable` to recheck each original entry. Mobile Guzheng selects a discard from the phase owner's hand as its return. `ProgramStageRuntime` commits each paid stage on the original frame and clears its movement wait after the children settle. Mobile Guzheng drains recovery, character state, skill changes, HP changes, and then card movement. It offers the remaining entries when the return actually reached the phase owner's hand.

- If a discard-phase claim delivers cards one at a time, the original program frame keeps the next entry and actual deliveries through each gain child. Advance only after checking or delivering the corresponding entry. Complete the claim once after the remaining entries have been processed.

- A death cleanup claim reads the actual discard entries on the original DeathFrame. An optional source-zone filter applies to those movements. A later entry of the same card cannot restore eligibility for the original death claim.

- Discard and judgment provenance claims use the original movement entry at collection, candidate admission and execution. Reentering the discard pile gives the physical card a new entry and ends the previous opportunity.

- A delayed-trick conversion from discarded material checks the original discard entry at admission, target selection and native issuance. Once issued, the resulting use owns the material and resolves placement through the ordinary delayed-trick continuation.

- `claimMovedCards` with `discardPileReceived`, `movementDiscardOnly` and `perBatch` claims the matching entries of the original discard batch in one physical movement. Selection retains the original batch window, and execution rechecks each original entry at the discard pile after earlier effects such as HP payment.

- Synchronous true-discard capture can read the original typed movement source and fall back to the physical reason classifier. After the native producer returns, a claim window can also use a completed logical discard receipt with the same card, destination and movement sequence.

- A discard-phase color count prefers the effective suit recorded on the actual discard entry. For a movement without that receipt, read the card identity in the classified original owned zone.

- A self-discard reward identifies the actual executor on the completed logical discard invoice. Each counted movement belongs to the original batch and the required source zone.

- A red owned-loss receipt resolves the effective suit in the actual source zone during physical movement recording. The card snapshot keeps the printed face and the receipt carries the effective suit used for eligibility.

- An atomic native program discard can carry its executor, original owned source and effective suit sampled before movement onto the actual discard-pile entry. The atomic writer verifies the original producer, reason and selected owned materials. `NativeProgramDiscardSource` requests the original source for synchronous consumers such as the turn's first outside-phase discard. Effective suit is captured only when a suit consumer requires it. Discard provenance, neighbor first-discard capture and full discard-phase totals can read that entry before the native invoice closes. The phase totals count each qualifying discard occurrence by the current player. Redirected movements and separately staged discards follow their own settlement rules.

- Neighbor discard placement retains the original batch's first-discard fact and previous living seat. Offer and placement eligibility require a surviving original discard-pile entry. A later discard of the same card leaves the earlier batch recorded as the first.

- Keep logical acquisition collections separate from physical movement batches. Freeze acquired entities, original-hand witnesses and each required source time in shared receipts on the owning batch; characters consume those facts instead of rebuilding them with completion callbacks or private history. Typed replacement provenance stays at its native boundary, and acquisition grouping must not merge or discard ordinary movement windows. Captured invocation permission belongs to its original window and program, independently of skill possession and effectiveness; source-loss permission must not implicitly grant owner-death permission. Read-only card captures do not acquire temporary-material cleanup ownership; derived bindings preserve that ownership and private reads stay within their authorized participant scope. Read movement outcomes from original physical facts, separately from requested materials and logical discard semantics.

- Native Give facts belong to `NativeGivenCardRuntime` and the original journal, with scope interpreted by `NativeGiveScopeRuntime`. Declare logical giving and its actor in rule data; do not infer Give from movement zones, reason strings, gains or a character whitelist. Logical Give triggers declare perNativeGive on the original movement window; keep logical occurrences separate from physical completion indexes and derive capture demand from the declaration. Typed gain settlements keep their original producer contract; do not rebuild character ledgers or re-prove replacement issuance when reading delivered facts.

- Exact owned-card movement requests reuse the original OwnedCardSelection draft and typed SelectedCardPayment. Issue one request only after a full legal selection; distinguish accepted selection from actual delivery, including redirected, destroyed and retained outcomes. History-based card identities do not grant private face access or temporary-material cleanup ownership.

- A Slash target penalty records the physical entries for its selected payment on the original draft. Equipment departure can settle other cards in child batches, such as Wooden Ox grain. Those child movements settle under their own rules. Recast draws use the number of paid equipment cards.
