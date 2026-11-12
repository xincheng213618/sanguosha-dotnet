# 界步练师：版本与工程裁定

`boundary:bu-lian-shi` 的注册和可执行定义见[人物资料](../../../src/Characters/bu-lian-shi/boundary/Content/boundary-bu-lian-shi.character.json)、[技能规则](../../../src/Characters/bu-lian-shi/boundary/Skills/boundary-bu-lian-shi.rules.json)。规则依据见[来源档案](../sources/fenglin-twenty-ninth-775-source-2026-10-04.json)。

以下为现行实现的工程裁定。

这些选择不是官网 FAQ 或其他产品口径。对应规则来源保存在人物来源档案中。

## 安恤

- “两名角色”包含本人；互异、存活、在真实发动时各有一张合法可获得区域牌。正文没有限定牌区，本稿按 HEJ 合法集合。不透明他人 Hand 只公开牌位，不按隐藏 Basic/颜色/价值筛选，AI 只能读已发布的 public equipment/judgment 或自己的牌；弃牌保护沿成熟 Obtain 合法性处理。
- 获得顺序按真实命令提交的 pair 顺序冻结。本人手牌被选中沿已有同区语义保持实体在原 Hand，没有额外 CardsGained、Hand→Processing→Hand 或假的 movement。本人 E/J 与他人 HEJ 正常真实获得。装备规范化若转为 OutsideGame，已发生账单保留，实际 Delivered=false，仍算这次真实选择，后继不伪造已获得实体。
- 第一人成本孩子令来源失效、owner 死亡、赢家确定、任一原 pair 死亡或下一来源区无牌时，未付后继取消，已真实获得牌保留，实际 phase 额度已消费。已付孩子先结清。
- 展示牌取当前本人合法手牌，使用冻结有效花色；不限定为本次获得实体。原 pair 手牌数在公开展示后、6301 发行点查询并冻结；更早孩子改变手牌计入，不声明原 pair 提交时冻结。
- 原 pair 同手牌数时，技能持有人从两者选一。若展示后被更早子结算移走或原对象死，未支付赠牌取消、不改对象；付过赠牌后不按后来的手牌重定价。
- 原受赠者就是本人时，同 Hand 不移动、不制造赠牌获得，仍完成真实选择/展示；冻结非黑桃奖励可正常执行。黑桃没有奖励。非黑桃奖励实际摸牌数量可能为零，新 fact 按真实 ledger 数量记录，不发假得牌。
- 来源装备不可作为自己的获得候选，避免尚未发行的程序先支付自身源。已经付款后孩子导致来源失技不撤销账单；奖励等未付后继仍遵守明确资格取消。

## 追忆

- 限定额度按成熟 owner+skill+named usage 保存在 game scope，跨来源失效/重新获得保持；发行原对象在实际 6302 节点冻结。有效声明、实际 draw 数量、recover 请求都保留 exact receipt/fact，零实际 Draw 不重授额度。
- 初次 Ending 自身没有选到原合法受益者之前，被选对象死亡/来源失效则按旧可选触发已扣 usage 口径取消，不改选；同实例更晚 Ending 不重新发行。
- 死亡重复要求原 skill instance/state/hash 与 real OwnerDied 原 candidate 匹配。来源在死亡前已丢失或仅重新获得不同实例不补发死亡机会。原受益者此时不存活则不发布新目标、不返还限定额度。源码无来源/未发行的死亡没有“重新选择”默认。
- 已经开始 Draw/Recover 的 paid child 依靠发行身份与原 typed parent 排空；父回返不再查询当前资格并改选他人。受益者之后死亡，尚未进行的 Draw/Recover 沿成熟 dead-target 过滤跳过，已经发生的账单与 HP 不回滚。
- 恢复是实际 Recover1，上限截断；满血没有假 Recovery。死亡触发在赢家已确定之后沿成熟 OwnerDied 不发行默认取消。原 death 再发生不能重复同一发行。

## 追忆的原生死亡返回

The death-benefit operation retains the selected beneficiary and exact original death parent throughout paid child resolutions.

- The original dead owner's Death/Dying and original attack remain in place. Only the already-dead victim's exact proven original Dying is filtered from active resolution while the owning benefit program exists. Its parent window, candidate index, responder cursor, attempted bindings, turn and original attack identity are frozen and attested by one scalar fact hash.
- Actual Draw3 movement, short-deck actual counts, beneficiary gain observers and actual Recover1/replacement observers drain before the next instruction. Source death is expected in OwnerDied. Original skill instance and issuance are retained; the beneficiary cannot be substituted. A dead beneficiary cancels the remaining unpaid instruction, retaining already-issued cards and facts. Winner cancellation finishes paid descendants and the typed return.
- A normal configured damage observer may start real nested damage after the exact original benefit's movement/recovery. Damage preserves current source, amount, nature, redirection and chain propagation through the mature producer. The added admission requires the real paid subtree, not any ancestor with a receipt.
- HP-loss Dying, actual damage Dying, DyingEntering, SelfDyingResponse, physical/multi-material response Peach and existing bound/zero-entity Alcohol use preserve their actual actors, providers, materials and typed return. The original Dying is restored to active visibility when the owning root is popped, without rewriting it.
- A second live victim's Dying while another live rescue target is still pending remains rejected by the existing engine. The new filter does not hide live victims. The capability does not provide a generalized concurrent-Dying scheduler.
