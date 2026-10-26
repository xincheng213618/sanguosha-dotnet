# 黄盖·移动版

核查日期：2026-10-26。普通移动身份，吴、男、4体力，标准包；完整引用已注册的苦肉技能 `classic:kujin`，保持现有ID拼写，不另设同义技能。

来源：[移动官网11](https://www.sanguosha.cn/pc/hero-detail-11.html)、[移动BWIKI人物固定版本35810](https://wiki.biligame.com/msgs/index.php?title=黄盖&oldid=35810)（2026-01-10）、[普通苦肉固定版本20229](https://wiki.biligame.com/msgs/index.php?title=苦肉/黄盖&oldid=20229)（2022-12-07）。普通经典栏列出新建账号升级获得，不混用界版的弃牌费用、阶段限次或诈降。

当前普通苦肉在出牌阶段失去1点体力，然后摸2张牌，没有每个出牌阶段一次或每回合一次限制。`classic-kujin-skills.rules.json` 的 `lose-hp-and-draw` 是零手牌/目标输入、`usesPerTurn:null` 且没有 `usesPerPhase`，资格为实际体力至少1，节点严格先 `loseHp:1` 后 `draw:2`。因此可以在同一出牌阶段连续发动，也允许最后1点体力发动。

失去体力沿现有原生HP/濒死子结算与原帧返回。当前移动问答明确最后1HP发动时先完成濒死救援，被救回后才摸牌；同一费用不能在子返回时重复支付，也不是先摸牌再结算濒死。本次不新增生命值旁路、平行pending或额外死亡后摸牌规则。

既有 `HulaoRuleChecks.RealHulaoSetupTierFiveCompatibleRosterAndKurouColdReplay` 已使用同一 `classic:kujin` 进行真实失去1HP、从牌堆移动两张实体及cold；重复次数沿共用激活限次检查。本次为纯内容引用，不复制苦肉JSON、原生HP机制或新人物夹具。

普通经典立绘为[官网1100.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/1100.jpg)，在当前官网11列表第二张；首张30700为界版图，不能直接采用。`portraitKey` 为 `mobile-huang-gai`，本地目标名 `official-mobile-huang-gai.jpg`，图鉴分组 `standard`。
