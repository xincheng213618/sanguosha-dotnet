# 卢植（普通移动版）

核查日期：2026-10-26。普通身份 `mobile:lu-zhi`；群、男、3体力；神话再临阴包，图鉴 `myth-yin`。技能注册81400/81401，武将及将池81499。立绘为[官网经典42200.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/42200.jpg)，建议键 `mobile-lu-zhi`。

## 当前来源

- [移动官网人物255](https://www.sanguosha.cn/pc/hero-detail-255.html)：当前普通身份正文明确明任开局摸一；贞良阳成本为双方体力差且至少一，阴在自己回合外使用或打出牌结算结束后按任的类别匹配，奖励一名角色摸一。
- [移动BWIKI卢植](https://wiki.biligame.com/msgs/卢植)，当前2026-01-10更新，[固定版本35837](https://wiki.biligame.com/msgs/index.php?title=卢植&oldid=35837)：普通经典身份与官网规则一致，列500将魂合成或招募的实际获取方式。与页面另列的谋卢植身份区分。

## 规则口径与复用

明任开局实际摸一张，再从手牌公开存入一张为任；结束阶段可用手牌交换任。直接复用 `draw`、`awaitOwnedCardMovement`、`selectOwnedCards`、`storeBoundHandInPublicPile` 与 `exchangePublicPileHand`。任归属于实际明任实例，贞良继续沿同一实际技能授予来源匹配公开任。

阳贞良在出牌阶段至多一次，选择攻击范围内其他角色，弃与任同颜色的X张HE牌，再造成一点原生伤害。X在实际选定目标时冻结为 `max(1, abs(自己HP - 目标HP))`。绝对差沿仓库现有谋卢植的体力差原生口径，不将“差”解释为有向减法；没有额外声称官网给出专项FAQ。随后弃牌移除白银狮子等产生的真实子结算不会重算已冻结成本，也不能把已付费用再付一次。

任和候选成本的即时颜色使用 `SuitColor(EffectiveSuit(owner, card))` 查询，冻结到本帧；不以受全局需求开关控制的 `CapturePhysicalCardColor` 稀疏声明事实查询代替即时规则判定，也不为本角色强行打开全局捕获。

公开任操作 `publicPileColorDamage` 新增可选固定 `costPolicy: hpDifferenceAtLeastOne`；省略仍沿经典固定弃一行为。移动策略的目标、HP与成本实体保存在原 `ProgramSkillFrame` 的类型化回执，以 `MoveProgramLogicalDiscard` 完成真实弃置，再沿原生移牌、回复/体力等子窗和原伤害返回。没有新opcode，也没有并行pending。

阴贞良仅自己的回合外、处于阴时可发动；接受后才转换。使用及打出分别订阅 `cardUseCompleted` 与 `cardResponseCompleted`，并检查原生完成父帧、类型化Dodge/Nullification实际使用返回，或真正打出完成且付款子窗已排空的返回。代提供的打出身份沿既有 `ResponseCompletionActorAction` 观察，不用进入弃牌堆代替打出完成。使用/打出牌的有效类别与当前任的基本/锦囊/装备类别比较；颜色不参与阴判据。接受阴的转换提交时冻结该次完成Action、原父窗口、任实体及类别，再沿标准目标选择和原生摸一继续，实际摸牌及得牌子窗付款不迁移到另一帧。

库内 `classic:mingren` 开局摸二；经典贞良阳固定弃一，阴按实际进入弃牌堆的同色牌。`mou-mobile` 明任也是摸二且为扣置任，谋贞良阴观察范围和摸牌量亦不同。不能完整引用这些同名技能；本实现只复用其适合的底层公开任、原生移牌、逻辑弃置、转换及正式结束窗口能力。

## focused检查与共享回归

- `MobileLuZhiChecks.OneCardOpeningPublicRenAndFrozenHpDifferencePaymentCold`：真实开局只摸一且任对全部观看者公开；体力较低时绝对差二及相等时至少一；同色实际实体付款、伤害顺序、出牌阶段次数和付款子窗Cold。
- `MobileLuZhiChecks.OutsideTurnNativeUseAndPlayedCompletedCategoryRewardCold`：真实Duel打出Slash、真实防御使用Dodge分别在对应原生完成窗口触发；付款观察窗前不提前触发；红任与黑基本牌按类别匹配，黑锦囊任与黑基本响应不匹配；令其他角色真实摸一和原响应/摸牌父帧Cold。

既有 `FengLinLuZhiChecks.ActualDiscardEntryBoundaries` 保留经典弃牌入口行为；既有公开堆投影及响应完成检查覆盖共享边界，不复制其矩阵。验证由集成任务串行运行，定向通过不代表全量通过。
